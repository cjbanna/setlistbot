using Microsoft.Extensions.DependencyInjection;

namespace Setlistbot.Domain.Kglw.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedditKglw(this IServiceCollection services)
        {
            services.AddScoped<IReplyBuilder, RedditReplyBuilder>();
            return services;
        }

        public static IServiceCollection AddDiscordKglw(this IServiceCollection services)
        {
            services.AddScoped<IReplyBuilder, DiscordReplyBuilder>();
            return services;
        }
    }
}
