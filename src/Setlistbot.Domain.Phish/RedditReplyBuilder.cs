using System.Text;

namespace Setlistbot.Domain.Phish
{
    public class RedditReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "phish";

        public string Build(IEnumerable<Setlist> setlists)
        {
            return setlists.Count() == 1
                ? BuildSingleSetlistReply(setlists.First())
                : BuildMultipleSetlistReply(setlists);
        }

        private string BuildMultipleSetlistReply(IEnumerable<Setlist> setlists)
        {
            var reply = new StringBuilder();

            foreach (var setlist in setlists.OrderBy(s => s.Date))
            {
                var date = setlist.Date.ToString("yyyy-MM-dd");
                var location = $"{setlist.Location.City}, {setlist.Location.State}";

                reply.Append(
                    $"# {date} {location} {setlist.Location.Venue}, {setlist.Location.City}, {setlist.Location.State}, {setlist.Location.Country}"
                );
                reply.AppendLine();
                reply.AppendFormat(
                    "[phish.net]({0}) | [phish.in]({1}) | [phishtracks]({2})",
                    GetPhishNetUrl(setlist),
                    GetPhishinUrl(setlist),
                    GetPhishTracksUrl(setlist)
                );
                reply.AppendLine();
                reply.AppendLine();
            }

            reply.Append("> _data provided by [phish.net](http://phish.net)_");

            return reply.ToString();
        }

        public string BuildSingleSetlistReply(Setlist setlist)
        {
            var reply = new StringBuilder();
            reply.Append(
                $"# {setlist.Date.ToString("yyyy-MM-dd")} @ {setlist.Location.Venue}, {setlist.Location.City},"
            );

            if (!string.IsNullOrWhiteSpace(setlist.Location.State))
            {
                reply.Append($" {setlist.Location.State},");
            }

            if (!string.IsNullOrWhiteSpace(setlist.Location.Country))
            {
                reply.Append($" {setlist.Location.Country}");
            }

            if (reply.ToString().EndsWith(','))
            {
                reply.Remove(reply.Length - 1, 1);
            }

            reply.AppendLine();
            reply.AppendLine();

            foreach (var set in setlist.Sets)
            {
                reply.AppendFormat(@"**{0}**: ", set.Name);
                foreach (var song in set.Songs.OrderBy(s => s.Position))
                {
                    if (song.Transition == ",")
                    {
                        reply.Append($"{song.Name}{song.Transition} ");
                    }
                    else
                    {
                        reply.Append($"{song.Name} {song.Transition} ");
                    }
                }

                reply.Remove(reply.Length - 2, 1);
                reply.AppendLine();
                reply.AppendLine();
            }

            reply.AppendFormat(
                "[phish.net]({0}) | [phish.in]({1}) | [phishtracks]({2})",
                GetPhishNetUrl(setlist),
                GetPhishinUrl(setlist),
                GetPhishTracksUrl(setlist)
            );
            reply.AppendLine();
            reply.AppendLine();
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
