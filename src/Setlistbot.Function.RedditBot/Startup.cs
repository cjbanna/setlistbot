using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Extensions.Logging;
using Setlistbot.Application.Extensions;
using Setlistbot.Application.Options;
using Setlistbot.Domain.Kglw.Extensions;
using Setlistbot.Domain.Phish.Extensions;
using Setlistbot.Infrastructure.Extensions;
using Setlistbot.Infrastructure.KglwNet.Extensions;
using Setlistbot.Infrastructure.PhishNet.Extensions;
using Setlistbot.Infrastructure.Reddit.Extensions;
using System;

[assembly: FunctionsStartup(typeof(Setlistbot.Function.RedditBot.Startup))]

namespace Setlistbot.Function.RedditBot
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddLogging()
                .AddOptions()
                // Application
                .AddRedditBot();

            var subreddit = GetSubreddit(builder);

            // Domain
            builder.Services
                .AddRedditPhish()
                .AddRedditKglw()
                // Infrastructure
                .AddInfrastructure(subreddit)
                .AddReddit()
                .AddKglwNet()
                .AddPhishNet();

            builder.Services.AddSingleton<ILoggerProvider>(
                (serviceProvider) =>
                {
                    Log.Logger = new LoggerConfiguration().Enrich
                        .FromLogContext()
                        .WriteTo.Console()
                        .CreateLogger();
                    return new SerilogLoggerProvider(Log.Logger, true);
                }
            );
        }

        private static string GetSubreddit(IFunctionsHostBuilder builder)
        {
            var botOptions = builder.Services
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
