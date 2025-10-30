using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddSingleton<TableServiceClient>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<AzureTableOptions>>().Value;
                return new TableServiceClient(options.ConnectionString);
            });

            const string commentsTableName = "comments";
            const string postsTableName = "posts";

            services
                .AddScoped<ICommentRepository>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<AzureTableOptions>>().Value;
                    return new CommentRepository(
                        subreddit,
                        options.ConnectionString,
                        commentsTableName
                    );
                })
                .AddScoped<IPostRepository>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<AzureTableOptions>>().Value;
                    return new PostRepository(subreddit, options.ConnectionString, postsTableName);
                })
                .AddScoped<IDiscordUsageRepository>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<AzureTableOptions>>().Value;
                    return new DiscordUsageRepository(
                        options.ConnectionString,
                        "discordusage",
                        sp.GetRequiredService<ILogger<DiscordUsageRepository>>()
                    );
                })
                .AddHostedService(sp => new TableInitializer(
                    sp.GetRequiredService<TableServiceClient>(),
                    new[] { commentsTableName, postsTableName, "discordusage" }
                ));

            return services;
        }

        public static IServiceCollection AddDiscordUsageInfrastructure(
            this IServiceCollection services
        )
        {
            services.AddOptionsFromConfig<AzureTableOptions>(ConfigKey);

            services.AddSingleton<TableServiceClient>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<AzureTableOptions>>().Value;
                return new TableServiceClient(options.ConnectionString);
            });

            services
                .AddScoped<IDiscordUsageRepository>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<AzureTableOptions>>().Value;
                    return new DiscordUsageRepository(
                        options.ConnectionString,
                        "discordusage",
                        sp.GetRequiredService<ILogger<DiscordUsageRepository>>()
                    );
                })
                .AddHostedService(sp => new TableInitializer(
                    sp.GetRequiredService<TableServiceClient>(),
                    new[] { "discordusage" }
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

        private sealed class TableInitializer : IHostedService
        {
            private readonly TableServiceClient _serviceClient;
            private readonly IEnumerable<string> _tableNames;

            public TableInitializer(
                TableServiceClient serviceClient,
                IEnumerable<string> tableNames
            )
            {
                _serviceClient = serviceClient;
                _tableNames = tableNames;
            }

            public Task StartAsync(CancellationToken cancellationToken)
            {
                foreach (var tableName in _tableNames)
                {
                    _serviceClient.CreateTableIfNotExists(tableName);
                }
                return Task.CompletedTask;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
