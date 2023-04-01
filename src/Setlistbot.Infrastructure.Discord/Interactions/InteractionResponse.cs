using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public class InteractionResponse
    {
        [JsonProperty("type")]
        public InteractionCallbackType Type { get; init; }

        [JsonProperty("data")]
        public InteractionCallbackData? Data { get; init; }
    }
}
