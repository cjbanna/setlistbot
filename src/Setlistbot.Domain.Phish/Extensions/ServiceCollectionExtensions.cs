using Microsoft.Extensions.DependencyInjection;

namespace Setlistbot.Domain.Phish.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedditPhish(this IServiceCollection services)
        {
            services.AddScoped<IReplyBuilder, RedditReplyBuilder>();
            return services;
        }

        public static IServiceCollection AddDiscordPhish(this IServiceCollection services)
        {
            services.AddScoped<IReplyBuilder, DiscordReplyBuilder>();
            return services;
        }
    }
}
