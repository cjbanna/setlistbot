import assert from 'node:assert/strict';
import { test, type TestContext } from 'node:test';
import { reddit, redis, runWithContext, settings } from '@devvit/web/server';
import { app } from './app.ts';

/**
 * Mocks Devvit for a Grateful Dead subreddit, whose setlists are bundled so no
 * fetch is needed, and records the Redis and Reddit calls in order.
 */
function mockDevvit(
  t: TestContext,
  extraSettings: Record<string, unknown> = {}
) {
  const calls: string[] = [];
  const values: Record<string, unknown> = { artist: 'gd', ...extraSettings };
  t.mock.method(settings, 'get', async (key: string) => values[key]);
  t.mock.method(redis, 'incrBy', async () => {
    calls.push('incrBy');
    return 1;
  });
  t.mock.method(reddit, 'submitComment', async () => {
    calls.push('submitComment');
  });
  return calls;
}

const comment = (body = '5/8/77') =>
  runWithContext({ appSlug: 'setlistbot-app' } as never, async () =>
    app.request('/internal/triggers/on-comment-create', {
      method: 'POST',
      body: JSON.stringify({
        comment: { id: 't1_abc', body },
        author: { name: 'someone' },
      }),
    })
  );

test('a failed lookup fails the trigger without recording a reply', async (t) => {
  const calls = mockDevvit(t, { artist: 'kglw' });
  t.mock.method(
    globalThis,
    'fetch',
    async () => new Response('', { status: 503 })
  );

  const response = await comment();

  assert.equal(response.status, 500);
  assert.deepEqual(calls, []);
});

test('a comment with no show acks without replying', async (t) => {
  const calls = mockDevvit(t);

  const response = await comment('1/1/99');

  assert.equal(response.status, 200);
  assert.deepEqual(calls, []);
});

test('records the reply just before posting it', async (t) => {
  const calls = mockDevvit(t);

  const response = await comment();

  assert.equal(response.status, 200);
  assert.deepEqual(calls, ['incrBy', 'submitComment']);
});

test('a failed reply record fails the trigger without posting', async (t) => {
  const calls = mockDevvit(t);
  t.mock.method(redis, 'incrBy', async () => {
    throw new Error('Redis unavailable');
  });

  const response = await comment();

  assert.equal(response.status, 500);
  assert.deepEqual(calls, []);
});

test('a failed post still acks the trigger', async (t) => {
  mockDevvit(t);
  t.mock.method(reddit, 'submitComment', async () => {
    throw new Error('Reddit unavailable');
  });
  t.mock.method(console, 'error', () => {});

  const response = await comment();

  assert.equal(response.status, 200);
});
