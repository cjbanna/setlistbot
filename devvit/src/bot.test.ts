import assert from 'node:assert/strict';
import { test } from 'node:test';
import { replyTo, type Settings } from './bot.ts';

// Grateful Dead setlists are bundled, so these replies need no network
const reply = (text: string, settings: Settings = {}, author = 'someone') =>
  replyTo(text, author, 'setlistbot-app', { artist: 'gd', ...settings });

test('does nothing until an artist is set', async () => {
  assert.equal(await reply('5/8/77', { artist: undefined }), '');
});

test('accepts the artist setting as a one-item array', async () => {
  assert.match(await reply('5/8/77', { artist: ['gd'] }), /^# 1977-05-08/);
});

test('ignores the bot’s own comments', async () => {
  assert.equal(await reply('# 1977-05-08 @ Ithaca', {}, 'Setlistbot-App'), '');
});

test('ignores the legacy u/setlistbot so the two bots can’t loop', async () => {
  assert.equal(await reply('# 1977-05-08 @ Ithaca', {}, 'SetlistBot'), '');
  assert.equal(
    await reply(
      'u/setlistbot-app 5/8/77',
      { requireMention: true },
      'SetlistBot'
    ),
    ''
  );
});

test('replies to any comment with a date when no mention is required', async () => {
  assert.match(await reply('5/8/77 was great'), /^# 1977-05-08/);
});

test('requires a mention when configured', async () => {
  const requireMention = true;
  assert.equal(await reply('5/8/77 was great', { requireMention }), '');
  assert.match(
    await reply('u/SetlistBot 5/8/77', { requireMention }),
    /^# 1977-05-08/
  );
  assert.match(
    await reply('u/setlistbot-app 5/8/77', { requireMention }),
    /^# 1977-05-08/
  );
});

test('needs at least one show date', async () => {
  assert.equal(await reply('u/setlistbot hi'), '');
  assert.equal(await reply('1/1/99'), '');
});

// One show is a full setlist; several are a list with a line per show
const shows = (markdown: string) =>
  markdown.startsWith('# ') ? 1 : (markdown.match(/^\[/gm) ?? []).length;

for (const [maxSetlists, expected] of [
  [undefined, 4],
  [2, 2],
  [2.9, 2],
  [0, 1],
  [-1, 1],
  [Number.NaN, 4],
  ['3', 4], // not a number, so the default of 25
] as const) {
  test(`maxSetlists ${String(maxSetlists)} replies with ${expected} shows`, async () => {
    const markdown = await reply('5/8/77 4/7/72 3/21/90 2/22/68', {
      maxSetlists: maxSetlists as number,
    });
    assert.equal(shows(markdown), expected);
  });
}
