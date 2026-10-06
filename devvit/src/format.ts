import type { Artist, Setlist, Song } from './setlists.ts';

const phishNetCredit = '> _data provided by [phish.net](https://phish.net/)_';
const phishNetUrl = (s: Setlist) => `https://phish.net/setlists/?d=${s.date}`;
const kglwNetUrl = (s: Setlist) =>
  `https://kglw.net/setlists/${s.permalink ?? ''}`;
const archiveUrl = (s: Setlist) =>
  `https://archive.org/details/GratefulDead?query=date:${s.date}`;

const artists: Record<
  Artist,
  {
    /** Appended after a single full setlist */
    links: (s: Setlist) => string;
    /** Where the date links to when listing several shows */
    url: (s: Setlist) => string;
    /** Appended after a list of several shows */
    footer: string;
  }
> = {
  phish: {
    links: (s) =>
      `[phish.net](${phishNetUrl(s)}) | [phish.in](https://phish.in/${s.date}) | [phishtracks](https://phishtracks.com/shows/${s.date})\n\n${phishNetCredit}`,
    url: phishNetUrl,
    footer: phishNetCredit,
  },
  kglw: {
    links: (s) => `> _data provided by [kglw.net](${kglwNetUrl(s)})_`,
    url: kglwNetUrl,
    footer: '',
  },
  gd: {
    links: (s) =>
      `[archive.org](${archiveUrl(s)})` +
      (s.spotifyUrl ? ` | [Spotify](${s.spotifyUrl})` : ''),
    url: archiveUrl,
    footer: '',
  },
};

/** The reply markdown: one full setlist, or a linked list when there are several shows. */
export function buildReply(artist: Artist, setlists: Setlist[]): string {
  const { links, url, footer } = artists[artist];
  const [first] = setlists;
  if (!first) return '';
  if (setlists.length === 1) return formatSetlist(first) + links(first);
  return (
    setlists.map((s) => `[${s.date}](${url(s)}) @ ${s.location}\n\n`).join('') +
    footer
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
