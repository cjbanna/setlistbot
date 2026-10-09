import gdShows from '../../src/Setlistbot.Infrastructure.GratefulDead/gd-shows.json' with { type: 'json' };

export type Artist = 'phish' | 'gd' | 'kglw';

export type Song = { name: string; transition: '' | '>' | '->' };

export type Setlist = {
  date: string;
  /** "Venue, City, State, Country" with missing parts left out */
  location: string;
  sets: { name: string; songs: Song[] }[];
  /** Where the date links to when listing several shows */
  url: string;
  /** Appended after a single full setlist */
  links: string;
  /** Appended after a list of several shows */
  footer?: string;
};

/** `keys` holds the API keys for the artists whose setlist APIs need one */
export async function getSetlists(
  artist: Artist,
  dates: string[],
  keys: { phishNetApiKey?: string } = {}
): Promise<Setlist[]> {
  const lookup = {
    phish: (date: string) => getPhishSetlists(date, keys.phishNetApiKey ?? ''),
    kglw: getKglwSetlists,
    gd: async (date: string) => getGratefulDeadSetlists(date),
  }[artist];
  return (await Promise.all(dates.map(lookup))).flat();
}

type PhishNetRow = {
  showdate: string;
  artist_name: string;
  position: number | string;
  song: string;
  set: string;
  trans_mark: string;
  venue: string;
  city: string;
  state: string;
  country: string;
};

const phishSetNames: Record<string, string> = {
  '1': 'Set 1',
  '2': 'Set 2',
  '3': 'Set 3',
  '4': 'Set 4',
  e: 'Encore',
  e2: 'Encore 2',
  e3: 'Encore 3',
};

const phishNetCredit = '> _data provided by [phish.net](https://phish.net/)_';

async function getPhishSetlists(date: string, apiKey: string) {
  // phish.net reports API errors (bad key, rate limit) in an HTTP 200 body
  const { error, error_message, data } = await getJson<{
    error: boolean | number;
    error_message?: string;
    data: PhishNetRow[];
  }>(
    `https://api.phish.net/v5/setlists/showdate/${date}.json?apikey=${encodeURIComponent(apiKey)}`
  );
  // Code 11 is "no matching data", i.e. no show that day
  if (error === 11) {
    return [];
  }
  if (error) {
    throw new Error(`phish.net error: ${error_message ?? error}`);
  }
  return toSetlists(
    data
      .filter((r) => r.artist_name === 'Phish')
      .map((r) => ({
        date: r.showdate,
        location: joinLocation(r.venue, r.city, r.state, r.country),
        url: `https://phish.net/setlists/?d=${r.showdate}`,
        set: phishSetNames[r.set] ?? 'Set',
        position: Number(r.position),
        song: r.song,
        transition: r.trans_mark,
      }))
  ).map((s) => ({
    ...s,
    links: `[phish.net](${s.url}) | [phish.in](https://phish.in/${s.date}) | [phishtracks](https://phishtracks.com/shows/${s.date})\n\n${phishNetCredit}`,
    footer: phishNetCredit,
  }));
}

type KglwNetRow = {
  showdate: string;
  artist: string;
  position: number | string;
  songname: string;
  settype: string;
  setnumber: string;
  transition: string;
  venuename: string;
  city: string;
  state: string;
  country: string;
  permalink: string;
};

async function getKglwSetlists(date: string) {
  const { data } = await getJson<{ data: KglwNetRow[] }>(
    `https://kglw.net/api/v2/setlists/showdate/${date}.json`
  );
  return toSetlists(
    data
      .filter((r) => r.artist === 'King Gizzard & the Lizard Wizard')
      .map((r) => {
        const set = `${r.settype} ${r.setnumber}`;
        return {
          date: r.showdate,
          location: joinLocation(r.venuename, r.city, r.state, r.country),
          url: `https://kglw.net/setlists/${r.permalink}`,
          set: /one set/i.test(set) ? 'One Set' : set,
          position: Number(r.position),
          song: r.songname,
          transition: r.transition,
        };
      })
  ).map((s) => ({
    ...s,
    links: `> _data provided by [kglw.net](${s.url})_`,
  }));
}

type GdShow = {
  showDate: string;
  venue: string;
  location: string;
  spotifyUrl: string;
  sets: { name: string; songs: { name: string; segue: boolean }[] }[];
};

const gdShowsByDate = Map.groupBy(gdShows as GdShow[], (s) =>
  s.showDate.slice(0, 10)
);

const usStates = new Set(
  'AL AK AZ AR CA CO CT DE DC FL GA HI ID IL IN IA KS KY LA ME MD MA MI MN MS MO MT NE NV NH NJ NM NY NC ND OH OK OR PA RI SC SD TN TX UT VT VA WA WV WI WY'.split(
    ' '
  )
);

function getGratefulDeadSetlists(date: string): Setlist[] {
  return (gdShowsByDate.get(date) ?? []).map((show) => {
    // US shows are "City, ST". Everything else ends with the country, e.g.
    // "London, England" or "Hamilton, Ontario, Canada".
    const parts = show.location.split(',').map((p) => p.trim());
    const isUs = parts.length === 1 || usStates.has(parts.at(-1) ?? '');
    const url = `https://archive.org/details/GratefulDead?query=date:${date}`;
    return {
      date,
      location: joinLocation(show.venue, ...parts, isUs ? 'USA' : ''),
      url,
      links:
        `[archive.org](${url})` +
        (show.spotifyUrl ? ` | [Spotify](${show.spotifyUrl})` : ''),
      sets: show.sets.map((set) => ({
        name: set.name,
        songs: set.songs.map((song) => ({
          name: song.name,
          transition: song.segue ? '>' : '',
        })),
      })),
    };
  });
}

type Row = {
  date: string;
  location: string;
  url: string;
  set: string;
  position: number;
  song: string;
  transition: string | null;
};

/** Groups one-row-per-song API data into shows, then sets, ordered like the C# bot did. */
function toSetlists(rows: Row[]): Omit<Setlist, 'links'>[] {
  const shows = Map.groupBy(rows, (r) => `${r.date}|${r.location}|${r.url}`);
  return [...shows.values()]
    .map((show) => ({
      date: show[0]!.date,
      location: show[0]!.location,
      url: show[0]!.url,
      sets: [...Map.groupBy(show, (r) => r.set)].map(([name, songs]) => ({
        name,
        songs: songs
          .sort((a, b) => a.position - b.position)
          .map((r) => ({
            name: r.song,
            transition: toTransition(r.transition),
          })),
      })),
    }))
    .sort((a, b) => a.date.localeCompare(b.date));
}

// APIs can send null for empty fields
const toTransition = (mark: string | null): Song['transition'] =>
  mark?.trim() === '>' ? '>' : mark?.trim() === '->' ? '->' : '';

const joinLocation = (...parts: (string | null)[]) =>
  parts.filter((p) => p?.trim()).join(', ');

async function getJson<T>(url: string): Promise<T> {
  const response = await fetch(url);
  // Don't put the URL in the error: the phish.net one contains the API key
  if (!response.ok) {
    throw new Error(`${new URL(url).host} returned ${response.status}`);
  }
  return (await response.json()) as T;
}
