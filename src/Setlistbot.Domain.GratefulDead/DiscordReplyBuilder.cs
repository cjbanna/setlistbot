using System.Text;

namespace Setlistbot.Domain.GratefulDead
{
    public class DiscordReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "gd";

        public string Build(IEnumerable<Setlist> setlists)
        {
            if (setlists == null || !setlists.Any())
            {
                return string.Empty;
            }

            var setlist = setlists.First();
            var reply = new StringBuilder();
            var date = setlist.Date.ToString("yyyy-MM-dd");
            var location = $"{setlist.Location.City}, {setlist.Location.State}";

            reply.Append("**Grateful Dead**");
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
                    if (song.SongTransition.Trim() == ">")
                    {
                        reply.Append($"{song.Name} > ");
                    }
                    else
                    {
                        reply.Append($"{song.Name}, ");
                    }
                }
                reply.Remove(reply.Length - 2, 1);
                reply.Append(Environment.NewLine);
                reply.Append(Environment.NewLine);
            }

            var archiveOrgUrl = GetArchiveOrgUrl(setlist);
            reply.Append($"[archive.org]({archiveOrgUrl})");

            if (!string.IsNullOrWhiteSpace(setlist.SpotifyUrl))
            {
                reply.Append($" | [Spotify]({setlist.SpotifyUrl})");
            }

            return reply.ToString();
        }

        private string GetArchiveOrgUrl(Setlist setlist)
        {
            return $"https://archive.org/details/GratefulDead?query=date:{setlist.Date.ToString("yyyy-MM-dd")}";
        }
    }
}
