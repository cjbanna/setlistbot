using domain = Setlistbot.Domain;

namespace Setlistbot.Infrastructure.GratefulDead.Extensions
{
    public static class SetlistExtensions
    {
        public static domain.Setlist ToSetlist(this Setlist gdSetlist)
        {
            var split = gdSetlist.Location.Split(',');
            var city = split[0].Trim();
            var state = split.Length > 1 ? split[1].Trim() : string.Empty;
            var country = state.Length > 2 ? split[1].Trim() : "USA";

            var location = new domain.Location(gdSetlist.Venue, city, state, country);
            var setlist = domain.Setlist.NewSetlist(
                "gd",
                "Grateful Dead",
                gdSetlist.ShowDate,
                location,
                string.Empty
            );

            if (!string.IsNullOrEmpty(gdSetlist.SpotifyUrl))
            {
                setlist.AddSpotifyUrl(new Uri(gdSetlist.SpotifyUrl));
            }

            foreach (var gdSet in gdSetlist.Sets)
            {
                var set = new domain.Set(gdSet.Name);
                var position = 1;
                foreach (var gdSong in gdSet.Songs)
                {
                    var transition = gdSong.Segue ? ">" : string.Empty;
                    var song = new domain.Song(
                        gdSong.Name,
                        position,
                        transition,
                        duration: 0,
                        footnote: string.Empty
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
