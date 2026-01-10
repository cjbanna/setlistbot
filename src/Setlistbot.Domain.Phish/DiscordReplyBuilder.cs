namespace Setlistbot.Domain.Phish
{
    public sealed class DiscordReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "phish";

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
