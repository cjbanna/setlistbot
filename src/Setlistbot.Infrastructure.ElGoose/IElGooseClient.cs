namespace Setlistbot.Infrastructure.ElGoose
{
    public interface IElGooseClient
    {
        Task<SetlistResponse> GetSetlistAsync(DateTime date);
    }
}
