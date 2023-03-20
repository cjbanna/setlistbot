using Microsoft.Extensions.DependencyInjection;

namespace Setlistbot.Domain.Phish.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPhish(this IServiceCollection services)
        {
            services.AddScoped<IReplyBuilder, PhishReplyBuilder>();
            return services;
        }
    }
}
