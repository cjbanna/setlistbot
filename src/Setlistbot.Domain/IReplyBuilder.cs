namespace Setlistbot.Domain
{
    public interface IReplyBuilder
    {
        string ArtistId { get; }
        string Build(IList<Setlist> setlists, int maxSetlists);
        string Build(Setlist setlist);
    }
}
