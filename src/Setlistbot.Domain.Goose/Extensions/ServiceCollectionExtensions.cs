using Microsoft.Extensions.DependencyInjection;

namespace Setlistbot.Domain.Goose.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedditGoose(this IServiceCollection services)
        {
            return services.AddScoped<IReplyBuilder, RedditReplyBuilder>();
        }

        public static IServiceCollection AddDiscordGoose(this IServiceCollection services)
        {
            return services.AddScoped<IReplyBuilder, DiscordReplyBuilder>();
        }
    }
}
