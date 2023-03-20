using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Setlistbot.Application.Extensions;
using Setlistbot.Domain.Kglw.Extensions;
using Setlistbot.Domain.Phish.Extensions;
using Setlistbot.Infrastructure.KglwNet.Extensions;
using Setlistbot.Infrastructure.PhishNet.Extensions;
using Setlistbot.Infrastructure.Reddit.Extensions;

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
                .AddBot()
                // Domain
                .AddPhish()
                .AddKglw()
                // Infrastructure
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
    }
}
