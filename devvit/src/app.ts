import { context, reddit, redis, settings } from '@devvit/web/server';
import type {
  OnCommentCreateRequest,
  OnPostCreateRequest,
  TriggerResponse,
} from '@devvit/web/shared';
import { Hono } from 'hono';
import { replyTo, type Settings } from './bot.ts';

export const app = new Hono();

app.post('/internal/triggers/on-comment-create', async (c) => {
  const { comment, author } = await c.req.json<OnCommentCreateRequest>();
  if (comment) {
    await reply(comment.id, comment.body, author?.name);
  }
  return c.json<TriggerResponse>({});
});

app.post('/internal/triggers/on-post-create', async (c) => {
  const { post, author } = await c.req.json<OnPostCreateRequest>();
  if (post) {
    await reply(post.id, `${post.title}\n${post.selftext}`, author?.name);
  }
  return c.json<TriggerResponse>({});
});

async function reply(
  thingId: string,
  text: string,
  author: string | undefined
) {
  // A failed lookup throws before the reply is recorded, so the trigger fails
  // instead of the comment being marked as replied to.
  const markdown = await replyTo(
    text,
    author,
    context.appSlug,
    await settings.getAll<Settings>()
  );
  if (!markdown) {
    return;
  }

  // Record the reply before posting so a redelivered event can't post twice
  if ((await redis.incrBy(`replied:${thingId}`, 1)) > 1) {
    return;
  }
  try {
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
