using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Setlistbot.Application.Extensions;
using Setlistbot.Domain.Kglw.Extensions;
using Setlistbot.Domain.Phish.Extensions;
using Setlistbot.Function.Discord.Middleware;
using Setlistbot.Infrastructure.Discord;
using Setlistbot.Infrastructure.Discord.Extensions;
using Setlistbot.Infrastructure.KglwNet.Extensions;
using Setlistbot.Infrastructure.PhishNet.Extensions;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApp =>
    {
        workerApp.UseWhen<VerifyKeyMiddleware>(context =>
        {
            return context.FunctionDefinition.InputBindings.Values
                    .First(b => b.Type.EndsWith("Trigger"))
                    .Type == "httpTrigger";
        });
    })
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddUserSecrets<Program>();
    })
    .ConfigureServices(services =>
    {
        services
            .AddLogging()
            .AddDiscordBot()
            .AddDiscord()
            .AddDiscordKglw()
            .AddKglwNet()
            .AddDiscordPhish()
            .AddPhishNet();

        services.AddSingleton<ILoggerProvider>(
            (serviceProvider) =>
            {
                Log.Logger = new LoggerConfiguration().Enrich
                    .FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();
                return new SerilogLoggerProvider(Log.Logger, true);
            }
        );
    })
    .Build();

host.Services.GetRequiredService<IDiscordService>().RegisterApplicationCommands();

host.Run();
