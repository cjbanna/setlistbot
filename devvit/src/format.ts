import type { Setlist, Song } from './setlists.ts';

/** The reply markdown: one full setlist, or a linked list when there are several shows. */
export function buildReply(setlists: Setlist[]): string {
  const [first] = setlists;
  if (!first) {
    return '';
  }
  if (setlists.length === 1) {
    return formatSetlist(first) + first.links;
  }
  // A reply's shows are all by one artist, so they share a footer
  return (
    setlists.map((s) => `[${s.date}](${s.url}) @ ${s.location}\n\n`).join('') +
    (first.footer ?? '')
  );
}

const formatSetlist = (s: Setlist) =>
  `# ${s.date} @ ${s.location}\n\n` +
  s.sets
    .map((set) => `**${set.name}:** ${formatSongs(set.songs)}\n\n`)
    .join('');

const formatSongs = (songs: Song[]) =>
  songs
    .map((song, i) =>
      i === songs.length - 1
        ? song.name
        : song.name + (song.transition ? ` ${song.transition} ` : ', ')
    )
    .join('');
