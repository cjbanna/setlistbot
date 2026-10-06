import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import { test, type TestContext } from 'node:test';
import { reddit, redis, runWithContext, settings } from '@devvit/web/server';
import { app } from './app.ts';

/** Mocks Devvit for a kglw subreddit, and records the Redis and Reddit calls in order. */
function mockDevvit(t: TestContext) {
  const calls: string[] = [];
  t.mock.method(settings, 'get', async (key: string) =>
    key === 'artist' ? 'kglw' : undefined
  );
  t.mock.method(redis, 'incrBy', async () => {
    calls.push('incrBy');
    return 1;
  });
  t.mock.method(reddit, 'submitComment', async () => {
    calls.push('submitComment');
  });
  return calls;
}

const comment = () =>
  runWithContext({ appSlug: 'setlistbot-app' } as never, async () =>
    app.request('/internal/triggers/on-comment-create', {
      method: 'POST',
      body: JSON.stringify({
        comment: { id: 't1_abc', body: '2022-10-10' },
        author: { name: 'someone' },
      }),
    })
  );

test('a failed lookup fails the trigger without recording a reply', async (t) => {
  const calls = mockDevvit(t);
  t.mock.method(
    globalThis,
    'fetch',
    async () => new Response('', { status: 503 })
  );

  const response = await comment();

  assert.equal(response.status, 500);
  assert.deepEqual(calls, []);
});

test('records the reply just before posting it', async (t) => {
  const calls = mockDevvit(t);
  const body = readFileSync(
    new URL(
      '../../test/Setlistbot.Infrastructure.KglwNet.UnitTests/KglwNetResponses/2022-10-10-setlist-response.json',
      import.meta.url
    )
  );
  t.mock.method(globalThis, 'fetch', async () => new Response(body));

  const response = await comment();

  assert.equal(response.status, 200);
  assert.deepEqual(calls, ['incrBy', 'submitComment']);
});
