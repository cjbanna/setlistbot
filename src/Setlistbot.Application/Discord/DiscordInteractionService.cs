using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Setlistbot.Infrastructure.Discord.Extensions;
using Setlistbot.Infrastructure.Discord.Interactions;
using Setlistbot.Infrastructure.Repositories;

namespace Setlistbot.Application.Discord
{
    public class DiscordInteractionService : IDiscordInteractionService
    {
        private readonly ILogger<DiscordInteractionService> _logger;
        private readonly ISetlistProviderFactory _setlistProviderFactory;
        private readonly IReplyBuilderFactory _replyBuilderFactory;
        private readonly IDiscordUsageRepository _discordUsageRepository;

        public DiscordInteractionService(
            ILogger<DiscordInteractionService> logger,
            ISetlistProviderFactory setlistProviderFactory,
            IReplyBuilderFactory replyBuilderFactory,
            IDiscordUsageRepository discordUsageRepository
        )
        {
            _logger = logger;
            _setlistProviderFactory = setlistProviderFactory;
            _replyBuilderFactory = replyBuilderFactory;
            _discordUsageRepository = discordUsageRepository;
        }

        public async Task<Maybe<InteractionResponse>> GetResponse(Interaction interaction)
        {
            try
            {
                if (interaction.InteractionType == InteractionType.Ping)
                {
                    return HandlePing();
                }

                if (
                    interaction is
                    { InteractionType: InteractionType.ApplicationCommand, Data.Name: "setlist" }
                )
                {
                    return await HandleSetlistCommand(interaction);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling interaction");
                return null;
            }

            return null;
        }

        private InteractionResponse HandlePing()
        {
            _logger.LogInformation("Ping handled");
            return new InteractionResponse { Type = InteractionCallbackType.Pong };
        }

        private async Task<Maybe<InteractionResponse>> HandleSetlistCommand(Interaction interaction)
        {
            _logger.LogInformation("Setlist command handled");

            await _discordUsageRepository.TrackUsageAsync(interaction);

            var artistId = interaction.GetOption("artist");
            var dateInput = interaction.GetOption("date");

            var setlistProvider = _setlistProviderFactory.Get(artistId);

            if (!DateOnly.TryParse(dateInput, out var date))
            {
                return new InteractionResponse
                {
                    Type = InteractionCallbackType.ChannelMessageWithSource,
                    Data = new InteractionCallbackData
                    {
                        Content =
                            $"Failed to parse date: '{dateInput}'. Try using mm/dd/yy or yyyy-mm-dd.",
                    },
                };
            }

            var setlists = await setlistProvider.GetSetlists(date);
            if (!setlists.Any())
            {
                return new InteractionResponse
                {
                    Type = InteractionCallbackType.ChannelMessageWithSource,
                    Data = new InteractionCallbackData
                    {
                        Content =
                            $"No setlist found for {artistId} on {date:yyyy-MM-dd}. Please try again with a different date or artist.",
                    },
                };
            }

            var replyBuilder = _replyBuilderFactory.Get(artistId);
            var reply = replyBuilder.Build(setlists);

            return new InteractionResponse
            {
                Type = InteractionCallbackType.ChannelMessageWithSource,
                Data = new InteractionCallbackData { Content = reply },
            };
        }
    }
}
