import { parseDates } from './dates.ts';

/**
 * The dates to reply with setlists for, or none if the bot shouldn't reply.
 * `text` is a comment body, or a post title and body.
 */
export function datesToReplyTo(
  text: string,
  author: string | undefined,
  appUsername: string,
  requireMention: boolean
): string[] {
  // Bot replies contain dates. Replying to our own, or to the legacy
  // u/setlistbot (which replies to us) on a shared subreddit, would loop forever.
  const a = author?.toLowerCase();
  if (a === 'setlistbot' || a === appUsername.toLowerCase()) {
    return [];
  }
  // Matches both u/setlistbot and u/setlistbot-app
  if (requireMention && !text.toLowerCase().includes('setlistbot')) {
    return [];
  }
  return parseDates(text);
}
