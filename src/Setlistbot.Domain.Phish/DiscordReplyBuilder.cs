using System.Text;

namespace Setlistbot.Domain.Phish
{
    public class DiscordReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "phish";

        public string Build(IEnumerable<Setlist> setlists, int maxSetlists = 20)
        {
            return string.Empty;
        }

        public string Build(Setlist setlist)
        {
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
                    if (song.Transition.Trim() == ",")
                    {
                        reply.Append($"{song.Name}{song.Transition.Trim()} ");
                    }
                    else
                    {
                        reply.Append($"{song.Name} {song.Transition.Trim()} ");
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
            return string.Format(
                "http://phishtracks.com/shows/{0}",
                setlist.Date.ToString("yyyy-MM-dd")
            );
        }

        private string GetPhishinUrl(Setlist setlist)
        {
            return string.Format("http://phish.in/{0}", setlist.Date);
        }

        private string GetPhishNetUrl(Setlist setlist)
        {
            return string.Format(
                "http://phish.net/setlists/?d={0}",
                setlist.Date.ToString("yyyy-MM-dd")
            );
        }
    }
}
