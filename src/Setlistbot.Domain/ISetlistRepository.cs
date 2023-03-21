namespace Setlistbot.Domain
{
    public interface ISetlistRepository
    {
        Task<Setlist> GetByDateAsync(int year, int month, int day);
    }
}
