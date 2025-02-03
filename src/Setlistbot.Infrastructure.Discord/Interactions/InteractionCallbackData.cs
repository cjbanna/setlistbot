using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public sealed class InteractionCallbackData
    {
        [JsonPropertyName("content")]
        public string Content { get; init; } = string.Empty;
    }
}
