namespace Setlistbot.Domain
{
    public interface ISetlistProvider
    {
        string ArtistId { get; }
        Task<IEnumerable<Setlist>> GetSetlists(DateOnly date);
    }
}
