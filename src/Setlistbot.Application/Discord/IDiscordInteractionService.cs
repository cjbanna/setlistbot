using CSharpFunctionalExtensions;
using Setlistbot.Infrastructure.Discord.Interactions;

namespace Setlistbot.Application.Discord
{
    public interface IDiscordInteractionService
    {
        Task<Maybe<InteractionResponse>> GetResponse(Interaction interaction);
    }
}
