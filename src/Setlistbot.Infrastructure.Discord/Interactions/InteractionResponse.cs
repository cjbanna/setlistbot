using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public sealed class InteractionResponse
    {
        [JsonPropertyName("type")]
        public InteractionCallbackType Type { get; init; }

        [JsonPropertyName("data")]
        public InteractionCallbackData? Data { get; init; }
    }
}
