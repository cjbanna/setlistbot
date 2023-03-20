using Setlistbot.Domain;

namespace Setlistbot.Application
{
    public interface ISetlistProviderFactory
    {
        ISetlistProvider Get(string artistId);
    }
}
