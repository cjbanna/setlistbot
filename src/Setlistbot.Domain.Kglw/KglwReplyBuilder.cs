using System.Text;

namespace Setlistbot.Domain.Kglw
{
    public class KglwReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "kglw";

        public string Build(IList<Setlist> setlists, int maxSetlists)
        {
            var reply = new StringBuilder();

            foreach (var setlist in setlists.Take(maxSetlists).OrderBy(s => s.Date))
            {
                var archiveOrgUrl = GetArchiveOrgUrl(setlist);
                reply.Append(
                    $"[{setlist.Date.ToString("yyyy-MM-dd")}]({archiveOrgUrl}) @ {setlist.Location.Venue}, {setlist.Location.City}, {setlist.Location.State}, {setlist.Location.Country}"
                );

                reply.AppendLine();
                reply.AppendLine();
            }

            return reply.ToString();
        }

        public string Build(Setlist setlist)
        {
            if (setlist == default)
            {
                return string.Empty;
            }

            var reply = new StringBuilder();
            reply.Append(
                $"# {setlist.Date.ToString("yyyy-MM-dd")} {setlist.Location} @ {setlist.Location.Venue}, {setlist.Location.City}, {setlist.Location.State}, {setlist.Location.Country}"
            );
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
                    reply.Append($"{song.Name} {song.Transition} ");
                }

                reply.Remove(reply.Length - 1, 1);
                reply.AppendLine();
                reply.AppendLine();
            }

            var archiveOrgUrl = GetArchiveOrgUrl(setlist);
            reply.Append($"[archive.org]({archiveOrgUrl})");

            return reply.ToString();
        }

        private string GetArchiveOrgUrl(Setlist setlist)
        {
            return $"https://archive.org/details/KingGizzardAndTheLizardWizard?query=date:{setlist.Date.ToString("yyyy-MM-dd")}";
        }
    }
}
