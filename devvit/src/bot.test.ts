import assert from 'node:assert/strict';
import { test } from 'node:test';
import { datesToReplyTo } from './bot.ts';

test('ignores the bot’s own comments', () => {
  assert.deepEqual(
    datesToReplyTo(
      '# 1977-05-08 @ Ithaca',
      'Setlistbot-App',
      'setlistbot-app',
      false
    ),
    []
  );
});

test('replies to any comment with a date when no mention is required', () => {
  assert.deepEqual(
    datesToReplyTo('5/8/77 was great', 'someone', 'setlistbot-app', false),
    ['1977-05-08']
  );
});

test('requires a mention when configured', () => {
  assert.deepEqual(
    datesToReplyTo('5/8/77 was great', 'someone', 'setlistbot-app', true),
    []
  );
  assert.deepEqual(
    datesToReplyTo('u/SetlistBot 5/8/77', 'someone', 'setlistbot-app', true),
    ['1977-05-08']
  );
  assert.deepEqual(
    datesToReplyTo(
      'u/setlistbot-app 5/8/77',
      'someone',
      'setlistbot-app',
      true
    ),
    ['1977-05-08']
  );
});

test('needs at least one date', () => {
  assert.deepEqual(
    datesToReplyTo('u/setlistbot hi', 'someone', 'setlistbot-app', true),
    []
  );
});
