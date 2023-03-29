using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public class InteractionCallbackData
    {
        [JsonProperty("content")]
        public string Content { get; init; } = string.Empty;
    }
}
