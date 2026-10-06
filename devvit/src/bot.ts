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
  // The bot's own replies contain dates. Replying to them would loop forever.
  if (author?.toLowerCase() === appUsername.toLowerCase()) return [];
  // Matches both u/setlistbot and u/setlistbot-app
  if (requireMention && !text.toLowerCase().includes('setlistbot')) return [];
  return parseDates(text);
}
