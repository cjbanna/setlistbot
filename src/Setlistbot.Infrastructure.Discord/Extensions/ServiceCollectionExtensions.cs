using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Setlistbot.Infrastructure.Discord.Client;
using Setlistbot.Infrastructure.Discord.Options;

namespace Setlistbot.Infrastructure.Discord.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ConfigKey = "Discord";

        public static IServiceCollection AddDiscord(this IServiceCollection services)
        {
            services.AddOptionsFromConfig<DiscordOptions>(ConfigKey);

            services
                .AddScoped<IDiscordClient, DiscordClient>()
                .AddScoped<IDiscordService, DiscordService>();

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
