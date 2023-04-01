using Setlistbot.Infrastructure.Discord.Interactions;

namespace Setlistbot.Application.Discord
{
    public interface IDiscordInteractionService
    {
        Task<InteractionResponse?> GetResponse(Interaction interaction);
    }
}
