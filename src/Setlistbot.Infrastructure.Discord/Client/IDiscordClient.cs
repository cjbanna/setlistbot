using Setlistbot.Infrastructure.Discord.Client.Models;

namespace Setlistbot.Infrastructure.Discord.Client
{
    public interface IDiscordClient
    {
        Task<IEnumerable<ApplicationCommand>> CreateGlobalApplicationCommands(
            IEnumerable<CreateGlobalApplicationCommand> commands
        );

        Task<IEnumerable<ApplicationCommand>> GetGlobalApplicationCommands();
    }
}
