namespace Setlistbot.Domain
{
    public interface IReplyBuilder
    {
        string ArtistId { get; }
        string Build(IEnumerable<Setlist> setlists);
    }
}
