using System.Text;

namespace Setlistbot.Domain.Phish
{
    public class DiscordReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "phish";

        public string Build(IEnumerable<Setlist> setlists)
        {
            var enumerable = setlists as Setlist[] ?? setlists.ToArray();
            if (!enumerable.Any())
            {
                return string.Empty;
            }

            var setlist = enumerable.First();
            var reply = new StringBuilder();
            var date = setlist.Date.ToString("yyyy-MM-dd");
            var location = $"{setlist.Location.City}, {setlist.Location.State}";

            reply.Append("**Phish**");
            reply.Append(Environment.NewLine);
            reply.Append(Environment.NewLine);
            reply.Append($"{date} {location} @ {setlist.Location.Venue}");
            reply.Append(Environment.NewLine);
            reply.Append(Environment.NewLine);

            foreach (var set in setlist.Sets)
            {
                reply.Append($"**{set.Name}**: ");
                foreach (var song in set.Songs.OrderBy(s => s.Position))
                {
                    if (song.SongTransition == SongTransition.Stop)
                    {
                        reply.Append($"{song.Name}{song.SongTransition} ");
                    }
                    else
                    {
                        reply.Append($"{song.Name} {song.SongTransition} ");
                    }
                }
                reply.Remove(reply.Length - 1, 1);
                reply.Append(Environment.NewLine);
                reply.Append(Environment.NewLine);
            }

            reply.Append("> _data provided by [phish.net](http://phish.net)_");

            return reply.ToString();
        }

        private string GetPhishTracksUrl(Setlist setlist)
        {
            return $"http://phishtracks.com/shows/{setlist.Date:yyyy-MM-dd}";
        }

        private string GetPhishinUrl(Setlist setlist)
        {
            return $"http://phish.in/{setlist.Date}";
        }

        private string GetPhishNetUrl(Setlist setlist)
        {
            return $"http://phish.net/setlists/?d={setlist.Date:yyyy-MM-dd}";
        }
    }
}
