using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Setlistbot.Domain;
using Setlistbot.Infrastructure.KglwNet.Options;

namespace Setlistbot.Infrastructure.KglwNet.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ConfigKey = "KglwNet";

        public static IServiceCollection AddKglwNet(this IServiceCollection services)
        {
            services.AddOptionsFromConfig<KglwNetOptions>(ConfigKey);

            services
                .AddScoped<IKglwNetClient, KglwNetClient>()
                .AddScoped<ISetlistProvider, KglwNetSetlistProvider>();

            return services;
        }

        private static IServiceCollection AddOptionsFromConfig<T>(
            this IServiceCollection collection,
            string? key = null
        )
            where T : class
        {
            key ??= typeof(T).Name;

            collection
                .AddOptions<T>()
                .Configure<IConfiguration>(
                    (settings, configuration) => configuration.GetSection(key).Bind(settings)
                );

            return collection;
        }
    }
}
