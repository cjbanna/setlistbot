using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public sealed class InteractionCallbackData
    {
        [JsonProperty("content")]
        public string Content { get; init; } = string.Empty;
    }
}
