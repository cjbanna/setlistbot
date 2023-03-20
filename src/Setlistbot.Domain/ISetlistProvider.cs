namespace Setlistbot.Domain
{
    public interface ISetlistProvider
    {
        Task<IEnumerable<Setlist>> GetSetlistsAsync(DateTime date);
    }
}
