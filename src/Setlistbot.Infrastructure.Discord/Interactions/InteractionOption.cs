using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public sealed class InteractionOption
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public ApplicationCommandOptionType Type { get; init; } = default!;

        [JsonPropertyName("value")]
        public object Value { get; init; } = default!;
    }
}
