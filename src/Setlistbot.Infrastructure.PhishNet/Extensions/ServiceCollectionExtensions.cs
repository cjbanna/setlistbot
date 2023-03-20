using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Setlistbot.Infrastructure.Phish.Options;

namespace Setlistbot.Infrastructure.PhishNet.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ConfigKey = "PhishNet";

        public static void AddPhishNet(this IServiceCollection services)
        {
            services.AddOptionsFromConfig<PhishNetOptions>(ConfigKey);

            services.AddScoped<IPhishNetClient, PhishNetClient>();
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
                    (settings, configuration) =>
                    {
                        configuration.GetSection(key).Bind(settings);
                    }
                );

            return collection;
        }
    }
}
