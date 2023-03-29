using Flurl.Http;

namespace Setlistbot.Infrastructure.Discord.Client
{
    public static class FlurlExtensions
    {
        public static IFlurlRequest WithDiscordHeaders(this string url, string token)
        {
            return url.WithHeader("Authorization", $"Bot {token}")
                .WithHeader("User-Agent", "Setlistbot");
        }
    }
}
