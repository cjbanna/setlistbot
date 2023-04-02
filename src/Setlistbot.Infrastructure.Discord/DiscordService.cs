using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Infrastructure.Discord.Client;
using Setlistbot.Infrastructure.Discord.Client.Models;
using Setlistbot.Infrastructure.Discord.Options;
using System.Text;
using DiscordRest = Discord.Rest;

namespace Setlistbot.Infrastructure.Discord
{
    public class DiscordService : IDiscordService
    {
        private readonly ILogger<DiscordService> _logger;
        private readonly IDiscordClient _discordClient;
        private readonly DiscordOptions _options;

        public DiscordService(
            ILogger<DiscordService> logger,
            IDiscordClient discordClient,
            IOptions<DiscordOptions> options
        )
        {
            _logger = logger;
            _discordClient = discordClient;
            _options = options.Value;
        }

        public async Task RegisterApplicationCommands()
        {
            if (!_options.RegisterGlobalApplicationCommands)
            {
                _logger.LogInformation("Registering global application commands is disabled.");
                return;
            }

            _logger.LogInformation("Registering global application commands");

            var commands = new[]
            {
                new CreateGlobalApplicationCommand
                {
                    Name = "setlist",
                    Description = "Gets a setlist by band and date",
                    Type = ApplicationCommandType.ChatInput,
                    Options = new List<ApplicationCommandOption>
                    {
                        new ApplicationCommandOption
                        {
                            Name = "artist",
                            Description = "The artist name",
                            Type = ApplicationCommandOptionType.String,
                            Required = true,
                            Choices = new List<ApplicationCommandOptionChoice>
                            {
                                new ApplicationCommandOptionChoice
                                {
                                    Name = "Phish",
                                    Value = "phish"
                                },
                                new ApplicationCommandOptionChoice
                                {
                                    Name = "King Gizzard and the Lizard Wizard",
                                    Value = "kglw"
                                }
                            }
                        },
                        new ApplicationCommandOption
                        {
                            Name = "date",
                            Description = "The date of the show",
                            Type = ApplicationCommandOptionType.String,
                            Required = true
                        }
                    }
                }
            };

            var response = await _discordClient.CreateGlobalApplicationCommands(commands);
            _logger.LogInformation("Registered application command {command}", response);
        }

        public bool VerifyInteraction(
            string publicKey,
            string signature,
            string timestamp,
            string body
        )
        {
            _logger.LogInformation(
                $"Verifying interaction with signature '{signature}' and timestamp '{timestamp}' and body '{body}'"
            );

            var client = new DiscordRest.DiscordRestClient();

            return client.IsValidHttpInteraction(
                publicKey,
                signature,
                timestamp,
                Encoding.UTF8.GetBytes(body)
            );
        }
    }
}
