using System.Text;

namespace Setlistbot.Domain.GratefulDead
{
    public sealed class DiscordReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "gd";

        public string Build(IEnumerable<Setlist> setlists)
        {
            if (!setlists.Any())
            {
                return string.Empty;
            }

            var setlist = setlists.First();
            return new DiscordFormatter(setlist).Format();
        }
    }
}
