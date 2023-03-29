using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Infrastructure.Discord.Client.Models;
using Setlistbot.Infrastructure.Discord.Options;

namespace Setlistbot.Infrastructure.Discord.Client
{
    public class DiscordClient : IDiscordClient
    {
        private readonly ILogger<DiscordClient> _logger;
        private readonly DiscordOptions _options;

        public DiscordClient(ILogger<DiscordClient> logger, IOptions<DiscordOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task<IEnumerable<ApplicationCommand>> CreateGlobalApplicationCommands(
            IEnumerable<CreateGlobalApplicationCommand> commands
        )
        {
            try
            {
                var url = Url.Combine(
                    _options.BaseUrl,
                    "applications",
                    _options.ApplicationId,
                    "commands"
                );

                return await url.WithDiscordHeaders(_options.Token)
                    .PutJsonAsync(commands)
                    .ReceiveJson<IEnumerable<ApplicationCommand>>();
            }
            catch (FlurlHttpException ex)
            {
                var body = await ex.GetResponseStringAsync();
                _logger.LogError(ex, body);
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationCommand>> GetGlobalApplicationCommands()
        {
            try
            {
                var url = Url.Combine(
                    _options.BaseUrl,
                    "applications",
                    _options.ApplicationId,
                    "commands"
                );

                return await url.WithDiscordHeaders(_options.Token)
                    .GetAsync()
                    .ReceiveJson<IEnumerable<ApplicationCommand>>();
            }
            catch (FlurlHttpException ex)
            {
                var body = await ex.GetResponseStringAsync();
                _logger.LogError(ex, body);
                throw;
            }
        }
    }
}
