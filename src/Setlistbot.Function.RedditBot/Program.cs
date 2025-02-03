using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Extensions.Logging;
using Setlistbot.Application.Extensions;
using Setlistbot.Application.Options;
using Setlistbot.Domain.GratefulDead.Extensions;
using Setlistbot.Domain.Kglw.Extensions;
using Setlistbot.Domain.Phish.Extensions;
using Setlistbot.Infrastructure.Extensions;
using Setlistbot.Infrastructure.GratefulDead.Extensions;
using Setlistbot.Infrastructure.KglwNet.Extensions;
using Setlistbot.Infrastructure.PhishNet.Extensions;
using Setlistbot.Infrastructure.Reddit.Extensions;

namespace Setlistbot.Function.RedditBot
{
    public sealed class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddUserSecrets<Program>();
                })
                .ConfigureServices(services =>
                {
                    services
                        .AddLogging()
                        .AddOptions()
                        // Application
                        .AddRedditBot();

                    var subreddit = GetSubreddit(services);

                    // Domain
                    services
                        .AddRedditPhish()
                        .AddRedditKglw()
                        .AddRedditGratefulDead()
                        // Infrastructure
                        .AddInfrastructure(subreddit)
                        .AddReddit()
                        .AddKglwNet()
                        .AddPhishNet()
                        .AddGratefulDeadInMemory();

                    services.AddSingleton<ILoggerProvider>(_ =>
                    {
                        Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .CreateLogger();
                        return new SerilogLoggerProvider(Log.Logger, true);
                    });
                })
                .Build();

            host.Run();
        }

        static string GetSubreddit(IServiceCollection services)
        {
            var botOptions = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<BotOptions>>();
            if (botOptions == null)
            {
                throw new Exception("BotOptions not found");
            }

            var subreddit = botOptions.Value.Subreddit;
            if (string.IsNullOrEmpty(subreddit))
            {
                throw new Exception("BotOptions:Subreddit cannot be null or empty");
            }

            return subreddit;
        }
    }
}
