namespace Setlistbot.Infrastructure.KglwNet
{
    public interface IKglwNetClient
    {
        Task<SetlistResponse> GetSetlistAsync(DateTime date);
    }
}
