using CSharpFunctionalExtensions;

namespace Setlistbot.Infrastructure.KglwNet
{
    public interface IKglwNetClient
    {
        Task<Maybe<SetlistResponse>> GetSetlistAsync(DateOnly date);
    }
}
