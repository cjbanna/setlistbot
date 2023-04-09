using System.Text;

namespace Setlistbot.Domain.GratefulDead
{
    public class RedditReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "gd";

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
                var archiveOrgUrl = GetArchiveOrgUrl(setlist);
                var location = $"{setlist.Location.City}, {setlist.Location.State}";
                reply.Append(
                    $"[{setlist.Date.ToString("yyyy-MM-dd")}]({archiveOrgUrl}) {location} @ {setlist.Location.Venue}"
                );

                if (!string.IsNullOrWhiteSpace(setlist.SpotifyUrl))
                {
                    reply.Append($" | [Spotify]({setlist.SpotifyUrl})");
                }

                reply.AppendLine();
                reply.AppendLine();
            }

            return reply.ToString();
        }

        private string BuildSingleSetlistReply(Setlist setlist)
        {
            var reply = new StringBuilder();
            var date = setlist.Date.ToString("yyyy-MM-dd");
            var location = $"{setlist.Location.City}, {setlist.Location.State}";

            reply.Append($"# {date} {location} @ {setlist.Location.Venue}");
            reply.AppendLine();
            reply.AppendLine();

            foreach (var set in setlist.Sets)
            {
                if (!string.IsNullOrWhiteSpace(set.Name))
                {
                    reply.Append($"**{set.Name}:**  ");
                }

                foreach (var song in set.Songs)
                {
                    if (song.Transition == ">")
                    {
                        reply.Append($"{song.Name} > ");
                    }
                    else
                    {
                        reply.Append($"{song.Name}, ");
                    }
                }
                reply.Remove(reply.Length - 2, 1);
                reply.AppendLine();
                reply.AppendLine();
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
