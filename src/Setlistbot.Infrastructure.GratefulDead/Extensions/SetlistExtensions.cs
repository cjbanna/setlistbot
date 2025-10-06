using CSharpFunctionalExtensions;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.GratefulDead.Extensions
{
    public static class SetlistExtensions
    {
        public static Domain.Setlist ToSetlist(this Setlist gdSetlist)
        {
            var split = gdSetlist.Location.Split(',');
            var city = split[0].Trim();
            var state = split.Length > 1 ? split[1].Trim() : string.Empty;
            var country = state.Length > 2 ? split[1].Trim() : "USA";

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
