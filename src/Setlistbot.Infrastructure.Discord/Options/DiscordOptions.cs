namespace Setlistbot.Infrastructure.Discord.Options
{
    public class DiscordOptions
    {
        public string ApplicationId { get; init; } = string.Empty;
        public string BaseUrl { get; init; } = string.Empty;
        public string PublicKey { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
        public bool VerifyKey { get; init; } = true;
        public bool RegisterGlobalApplicationCommands { get; init; } = true;
    }
}
