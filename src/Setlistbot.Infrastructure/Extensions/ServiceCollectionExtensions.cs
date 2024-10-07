using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Domain.CommentAggregate;
using Setlistbot.Domain.PostAggregate;
using Setlistbot.Infrastructure.Options;
using Setlistbot.Infrastructure.Repositories;

namespace Setlistbot.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ConfigKey = "AzureTable";

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string subreddit
        )
        {
            services.AddOptionsFromConfig<AzureTableOptions>(ConfigKey);

            var options = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<AzureTableOptions>>()
                .Value;

            var commentsTableName = "comments";
            var postsTableName = "posts";

            // Ensure the tables exist
            var serviceClient = new TableServiceClient(options.ConnectionString);
            serviceClient.CreateTableIfNotExists(commentsTableName);
            serviceClient.CreateTableIfNotExists(postsTableName);

            services
                .AddScoped<ICommentRepository>(_ => new CommentRepository(
                    subreddit,
                    options.ConnectionString,
                    commentsTableName
                ))
                .AddScoped<IPostRepository>(_ => new PostRepository(
                    subreddit,
                    options.ConnectionString,
                    postsTableName
                ))
                .AddScoped<IDiscordUsageRepository>(provider => new DiscordUsageRepository(
                    options.ConnectionString,
                    "discordusage",
                    provider.GetRequiredService<ILogger<DiscordUsageRepository>>()
                ));

            return services;
        }

        public static IServiceCollection AddDiscordUsageInfrastructure(
            this IServiceCollection services
        )
        {
            services.AddOptionsFromConfig<AzureTableOptions>(ConfigKey);

            var options = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<AzureTableOptions>>()
                .Value;

            services.AddScoped<IDiscordUsageRepository>(provider => new DiscordUsageRepository(
                options.ConnectionString,
                "discordusage",
                provider.GetRequiredService<ILogger<DiscordUsageRepository>>()
            ));

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
