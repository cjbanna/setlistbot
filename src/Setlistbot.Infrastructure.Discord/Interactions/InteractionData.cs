using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public sealed class InteractionData
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public InteractionType Type { get; init; } = default!;

        [JsonPropertyName("options")]
        public IEnumerable<InteractionOption> Options { get; init; } =
            Enumerable.Empty<InteractionOption>();
    }
}
