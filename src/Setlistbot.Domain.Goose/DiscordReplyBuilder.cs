namespace Setlistbot.Domain.Goose
{
    public class DiscordReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "goose";

        public string Build(IEnumerable<Setlist> setlists)
        {
            throw new NotImplementedException();
        }
    }
}
