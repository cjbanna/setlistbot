using Microsoft.Extensions.DependencyInjection;
using Setlistbot.Domain;

namespace Setlistbot.Application
{
    public class SetlistProviderFactory : ISetlistProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SetlistProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISetlistProvider Get(string artistId)
        {
            var setlistProviders = _serviceProvider.GetServices<ISetlistProvider>();
            return setlistProviders.FirstOrDefault(p => p.ArtistId == artistId)
                ?? throw new Exception($"Could not resolve IServiceProvider for {artistId}");
        }
    }
}
