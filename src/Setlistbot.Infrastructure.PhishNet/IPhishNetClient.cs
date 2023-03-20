namespace Setlistbot.Infrastructure.PhishNet
{
    public interface IPhishNetClient
    {
        Task<SetlistResponse> GetSetlistAsync(DateTime date);
    }
}
