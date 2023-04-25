using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Setlistbot.Domain;
using Setlistbot.Infrastructure.ElGoose.Options;

namespace Setlistbot.Infrastructure.ElGoose.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ConfigKey = "ElGoose";

        public static IServiceCollection AddElGoose(this IServiceCollection services)
        {
            services.AddOptionsFromConfig<ElGooseOptions>(ConfigKey);

            services
                .AddScoped<IElGooseClient, ElGooseClient>()
                .AddScoped<ISetlistProvider, ElGooseSetlistProvider>();

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
