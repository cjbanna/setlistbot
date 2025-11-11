using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Setlistbot.Domain.CommentAggregate;
using Setlistbot.Domain.PostAggregate;
using Setlistbot.Infrastructure.Extensions;
using Setlistbot.Infrastructure.Repositories;
using Xunit;

namespace Setlistbot.Infrastructure.UnitTests
{
    public sealed class ServiceCollectionExtensionsTests
    {
        private static ServiceProvider BuildProvider(Action<IServiceCollection> register)
        {
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["AzureTable:ConnectionString"] = "UseDevelopmentStorage=true;",
                    }
                )
                .Build();

            services.AddSingleton<IConfiguration>(config);

            register(services);

            return services.BuildServiceProvider();
        }

        [Fact]
        public void AddInfrastructure_WhenAdded_ShouldRegisterRepositoriesAndInitializer()
        {
            using var provider = BuildProvider(s => s.AddInfrastructure("testsubreddit"));

            Assert.NotNull(provider.GetRequiredService<ICommentRepository>());
            Assert.NotNull(provider.GetRequiredService<IPostRepository>());
            Assert.NotNull(provider.GetRequiredService<IDiscordUsageRepository>());
            Assert.NotNull(provider.GetRequiredService<TableServiceClient>());

            var hostedServices = provider.GetServices<IHostedService>();
            Assert.Single(hostedServices, h => h.GetType().Name == "TableInitializer");
        }

        [Fact]
        public void AddDiscordUsageInfrastructure_WhenAdded_ShouldRegisterRepositoryAndInitializer()
        {
            using var provider = BuildProvider(s => s.AddDiscordUsageInfrastructure());

            Assert.NotNull(provider.GetRequiredService<IDiscordUsageRepository>());
            Assert.NotNull(provider.GetRequiredService<TableServiceClient>());

            var hostedServices = provider.GetServices<IHostedService>();
            Assert.Single(hostedServices, h => h.GetType().Name == "TableInitializer");
        }
    }
}
