using Microsoft.Extensions.DependencyInjection;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.GratefulDead.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGratefulDeadInMemory(this IServiceCollection services)
        {
            return services.AddSingleton<ISetlistProvider, GratefulDeadInMemoryProvider>();
        }
    }
}
