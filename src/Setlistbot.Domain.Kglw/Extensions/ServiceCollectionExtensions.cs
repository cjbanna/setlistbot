using Microsoft.Extensions.DependencyInjection;

namespace Setlistbot.Domain.Kglw.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKglw(this IServiceCollection services)
        {
            services.AddScoped<IReplyBuilder, KglwReplyBuilder>();
            return services;
        }
    }
}
