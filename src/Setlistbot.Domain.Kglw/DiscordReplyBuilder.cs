using System.Text;

namespace Setlistbot.Domain.Kglw
{
    public class DiscordReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "kglw";

        public string Build(IEnumerable<Setlist> setlists, int maxSetlists)
        {
            return string.Empty;
        }

        public string Build(Setlist setlist)
        {
            if (setlist == default)
            {
                return string.Empty;
            }

            var reply = new StringBuilder();
            reply.Append("**King Gizzard & the Lizard Wizard**");
            reply.Append(Environment.NewLine);
            reply.Append(Environment.NewLine);
            reply.Append(
                $"{setlist.Date.ToString("yyyy-MM-dd")} @ {setlist.Location.Venue}, {setlist.Location.City},"
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

            reply.Append("> _data provided by [kglw.net](http://kglw.net)_");

            return reply.ToString();
        }
    }
}
