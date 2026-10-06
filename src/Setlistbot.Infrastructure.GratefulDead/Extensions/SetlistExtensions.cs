using CSharpFunctionalExtensions;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.GratefulDead.Extensions
{
    public static class SetlistExtensions
    {
        private static readonly HashSet<string> UsStates =
        [
            .. "AL AK AZ AR CA CO CT DE DC FL GA HI ID IL IN IA KS KY LA ME MD MA MI MN MS MO MT NE NV NH NJ NM NY NC ND OH OK OR PA RI SC SD TN TX UT VT VA WA WV WI WY".Split(
                ' '
            ),
        ];

        public static Domain.Setlist ToSetlist(this Setlist gdSetlist)
        {
            // US shows are "City, ST". Everything else ends with the country, e.g.
            // "London, England" or "Hamilton, Ontario, Canada".
            var parts = gdSetlist.Location.Split(',', StringSplitOptions.TrimEntries);
            string city,
                state,
                country;
            if (parts.Length == 1)
            {
                (city, state, country) = (parts[0], string.Empty, "USA");
            }
            else if (UsStates.Contains(parts[^1]))
            {
                (city, state, country) = (string.Join(", ", parts[..^1]), parts[^1], "USA");
            }
            else
            {
                (city, state, country) = (parts[0], string.Join(", ", parts[1..^1]), parts[^1]);
            }

            var location = new Location(
                string.IsNullOrWhiteSpace(gdSetlist.Venue)
                    ? Maybe.None
                    : Maybe.From(Venue.From(gdSetlist.Venue)),
                City.From(city),
                string.IsNullOrWhiteSpace(state) ? Maybe.None : Maybe.From(State.From(state)),
                Country.From(country)
            );

            var setlist = Domain.Setlist.NewSetlist(
                ArtistId.From("gd"),
                ArtistName.From("Grateful Dead"),
                DateOnly.FromDateTime(gdSetlist.ShowDate),
                location,
                string.Empty
            );

            if (!string.IsNullOrEmpty(gdSetlist.SpotifyUrl))
            {
                setlist.AddSpotifyUrl(new Uri(gdSetlist.SpotifyUrl));
            }

            foreach (var gdSet in gdSetlist.Sets)
            {
                var set = new Domain.Set(SetName.From(gdSet.Name));
                var position = 1;
                foreach (var gdSong in gdSet.Songs)
                {
                    var transition = gdSong.Segue ? ">" : string.Empty;
                    var song = new Domain.Song(
                        SongName.From(gdSong.Name),
                        SongPosition.From(position),
                        transition.ToSongTransition(),
                        TimeSpan.Zero,
                        string.Empty
                    );
                    set.AddSong(song);
                    position++;
                }
                setlist.AddSet(set);
            }

            return setlist;
        }
    }
}
