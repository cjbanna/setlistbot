using CSharpFunctionalExtensions;

namespace Setlistbot.Infrastructure.PhishNet
{
    public interface IPhishNetClient
    {
        Task<Maybe<SetlistResponse>> GetSetlistAsync(DateOnly date);
    }
}
