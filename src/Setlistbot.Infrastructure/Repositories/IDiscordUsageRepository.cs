using Setlistbot.Infrastructure.Discord.Interactions;

namespace Setlistbot.Infrastructure.Repositories
{
    public interface IDiscordUsageRepository
    {
        Task TrackUsageAsync(Interaction interaction);
    }
}
