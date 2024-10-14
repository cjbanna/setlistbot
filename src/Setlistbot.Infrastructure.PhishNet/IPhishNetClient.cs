using CSharpFunctionalExtensions;

namespace Setlistbot.Infrastructure.PhishNet
{
    public interface IPhishNetClient
    {
        Task<Result<SetlistResponse>> GetSetlistAsync(DateOnly date);
    }
}
