using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Setlistbot.Application.Options;

namespace Setlistbot.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ConfigKey = "Bot";

        public static IServiceCollection AddBot(this IServiceCollection services)
        {
            services.AddOptionsFromConfig<BotOptions>(ConfigKey);

            services
                .AddScoped<ICommentReplyService, CommentReplyService>()
                .AddScoped<IPostReplyService, PostReplyService>()
                .AddScoped<IRedditSetlistbot, RedditSetlistbot>()
                .AddScoped<IReplyBuilderFactory, ReplyBuilderFactory>()
                .AddScoped<ISetlistProviderFactory, SetlistProviderFactory>();

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
