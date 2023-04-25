using System.Text;

namespace Setlistbot.Domain.Goose
{
    public class RedditReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "goose";

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
                var kglwNetUrl = GetElGooseUrl(setlist);
                reply.Append(
                    $"[{setlist.Date.ToString("yyyy-MM-dd")}]({kglwNetUrl}) @ {setlist.Location.Venue}, {setlist.Location.City}, {setlist.Location.State}, {setlist.Location.Country}"
                );

                reply.Append(Environment.NewLine);
                reply.Append(Environment.NewLine);
            }

            return reply.ToString();
        }

        private string BuildSingleSetlistReply(Setlist setlist)
        {
            if (setlist == default)
            {
                return string.Empty;
            }

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

            reply.Append(Environment.NewLine);
            reply.Append(Environment.NewLine);

            foreach (var set in setlist.Sets)
            {
                if (!string.IsNullOrWhiteSpace(set.Name))
                {
                    reply.Append($"**{set.Name}:**  ");
                }

                foreach (var song in set.Songs)
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
                reply.Append(Environment.NewLine);
                reply.Append(Environment.NewLine);
            }

            reply.Append($"> _data provided by [elgoose.net](https://elgoose.net)_");

            return reply.ToString();
        }

        private string GetElGooseUrl(Setlist setlist)
        {
            return $"http://elgoose.net/?d={setlist.Date.ToString("yyyy-MM-dd")}";
        }
    }
}
