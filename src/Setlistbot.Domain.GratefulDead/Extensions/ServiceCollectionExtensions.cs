using Microsoft.Extensions.DependencyInjection;

namespace Setlistbot.Domain.GratefulDead.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedditGratefulDead(this IServiceCollection services)
        {
            return services.AddScoped<IReplyBuilder, RedditReplyBuilder>();
        }

        public static IServiceCollection AddDiscordGratefulDead(this IServiceCollection services)
        {
            return services.AddScoped<IReplyBuilder, DiscordReplyBuilder>();
        }
    }
}
