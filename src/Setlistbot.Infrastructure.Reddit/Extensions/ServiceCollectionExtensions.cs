using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Setlistbot.Infrastructure.Reddit.Options;

namespace Setlistbot.Infrastructure.Reddit.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ConfigKey = "Reddit";

        public static IServiceCollection AddReddit(this IServiceCollection services)
        {
            services.AddOptionsFromConfig<RedditOptions>(ConfigKey);

            services
                .AddScoped<IRedditClient, RedditClient>()
                .AddScoped<IRedditService, RedditService>();

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
