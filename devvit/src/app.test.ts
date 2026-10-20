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
  t.mock.method(settings, 'getAll', async () => values);
  t.mock.method(redis, 'set', async () => {
    calls.push('set');
    return 'OK';
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

test('claims the reply with a 30-day expiry before posting it', async (t) => {
  t.mock.timers.enable({
    apis: ['Date'],
    now: new Date('2026-10-20T00:00:00Z'),
  });
  const calls = mockDevvit(t);
  const claim = t.mock.method(redis, 'set');

  const response = await comment();

  assert.equal(response.status, 200);
  assert.deepEqual(claim.mock.calls[0]?.arguments, [
    'replied:t1_abc',
    '1',
    { nx: true, expiration: new Date('2026-11-19T00:00:00Z') },
  ]);
  assert.deepEqual(calls, ['set', 'submitComment']);
});

test('an existing reply claim skips posting', async (t) => {
  const calls = mockDevvit(t);
  t.mock.method(redis, 'set', async () => {
    calls.push('set');
    return '';
  });

  const response = await comment();

  assert.equal(response.status, 200);
  assert.deepEqual(calls, ['set']);
});

test('post triggers also claim the reply before posting it', async (t) => {
  const calls = mockDevvit(t);
  const claim = t.mock.method(redis, 'set');

  const response = await runWithContext(
    { appSlug: 'setlistbot-app' } as never,
    async () =>
      app.request('/internal/triggers/on-post-create', {
        method: 'POST',
        body: JSON.stringify({
          post: { id: 't3_abc', title: '5/8/77', selftext: '' },
          author: { name: 'someone' },
        }),
      })
  );

  assert.equal(response.status, 200);
  assert.equal(claim.mock.calls[0]?.arguments[0], 'replied:t3_abc');
  assert.deepEqual(calls, ['set', 'submitComment']);
});

test('reads settings in one call', async (t) => {
  mockDevvit(t);
  const getAll = t.mock.method(settings, 'getAll', async () => ({
    artist: 'gd',
  }));

  await comment();

  assert.equal(getAll.mock.callCount(), 1);
});

test('a failed reply record fails the trigger without posting', async (t) => {
  const calls = mockDevvit(t);
  t.mock.method(redis, 'set', async () => {
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
