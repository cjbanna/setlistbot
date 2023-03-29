namespace Setlistbot.Infrastructure.Discord
{
    public interface IDiscordService
    {
        Task RegisterApplicationCommands();
        bool VerifyInteraction(string publicKey, string signature, string timestamp, string body);
    }
}
