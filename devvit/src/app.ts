import { context, reddit, redis, settings } from '@devvit/web/server';
import type {
  OnCommentCreateRequest,
  OnPostCreateRequest,
  TriggerResponse,
} from '@devvit/web/shared';
import { Hono } from 'hono';
import { datesToReplyTo } from './bot.ts';
import { buildReply } from './format.ts';
import { getSetlists, type Artist } from './setlists.ts';

export const app = new Hono();

app.post('/internal/triggers/on-comment-create', async (c) => {
  const { comment, author } = await c.req.json<OnCommentCreateRequest>();
  if (comment) await reply(comment.id, comment.body, author?.name);
  return c.json<TriggerResponse>({});
});

app.post('/internal/triggers/on-post-create', async (c) => {
  const { post, author } = await c.req.json<OnPostCreateRequest>();
  if (post)
    await reply(post.id, `${post.title}\n${post.selftext}`, author?.name);
  return c.json<TriggerResponse>({});
});

async function reply(
  thingId: string,
  text: string,
  author: string | undefined
) {
  const [artistSetting, requireMention, maxSetlists, phishNetApiKey] =
    await Promise.all([
      settings.get<string | string[]>('artist'),
      settings.get<boolean>('requireMention'),
      settings.get<number>('maxSetlists'),
      settings.get<string>('phishNetApiKey'),
    ]);
  // Devvit's docs don't say whether a select setting comes back as a string or a one-item array
  const artist = [artistSetting].flat()[0] as Artist | undefined;
  if (!artist) return;

  const dates = datesToReplyTo(
    text,
    author,
    context.appSlug,
    requireMention ?? false
  );
  if (dates.length === 0) return;

  const max = maxSetlists ?? 25;
  // ponytail: only the first `max` dates are looked up (caps the parallel requests),
  // so a comment with many non-show dates can get fewer than `max` setlists.
  // A failed lookup throws before the reply is recorded, so the trigger fails
  // instead of the comment being marked as replied to.
  const setlists = await getSetlists(
    artist,
    dates.slice(0, max),
    phishNetApiKey ?? ''
  );
  const markdown = buildReply(artist, setlists.slice(0, max));
  if (!markdown) return;

  try {
    // Record the reply before posting so a redelivered event can't post twice
    if ((await redis.incrBy(`replied:${thingId}`, 1)) > 1) return;
    await reddit.submitComment({
      id: thingId as `t1_${string}` | `t3_${string}`,
      text: markdown,
      runAs: 'APP',
    });
  } catch (error) {
    // Don't fail the trigger: a failed post may still have gone through
    console.error(`Failed to reply to ${thingId}`, error);
  }
}
