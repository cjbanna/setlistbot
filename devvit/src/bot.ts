import { parseDates } from './dates.ts';
import { buildReply } from './format.ts';
import { getSetlists, type Artist } from './setlists.ts';

/** Settings as Devvit returns them: unset, or whatever a moderator typed */
export type Settings = {
  artist?: string | string[];
  requireMention?: boolean;
  maxSetlists?: number;
  phishNetApiKey?: string;
};

/**
 * The reply markdown, or '' if the bot shouldn't reply.
 * `text` is a comment body, or a post title and body.
 */
export async function replyTo(
  text: string,
  author: string | undefined,
  appUsername: string,
  settings: Settings
): Promise<string> {
  // Devvit's docs don't say whether a select setting comes back as a string or a one-item array
  const artist = [settings.artist].flat()[0] as Artist | undefined;
  if (!artist) {
    return '';
  }
  // Bot replies contain dates. Replying to our own, or to the legacy
  // u/setlistbot (which replies to us) on a shared subreddit, would loop forever.
  const a = author?.toLowerCase();
  if (a === 'setlistbot' || a === appUsername.toLowerCase()) {
    return '';
  }
  // Matches both u/setlistbot and u/setlistbot-app
  if (settings.requireMention && !text.toLowerCase().includes('setlistbot')) {
    return '';
  }

  // The setting is free-form, so fall back to the default or round it to a usable cap
  const { maxSetlists } = settings;
  const max =
    typeof maxSetlists === 'number' && Number.isFinite(maxSetlists)
      ? Math.max(1, Math.floor(maxSetlists))
      : 25;
  // ponytail: only the first `max` dates are looked up (caps the parallel requests),
  // so a comment with many non-show dates can get fewer than `max` setlists.
  const dates = parseDates(text).slice(0, max);
  const setlists = await getSetlists(artist, dates, settings);
  return buildReply(setlists.slice(0, max));
}
