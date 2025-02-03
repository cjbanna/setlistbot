using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Discord.Client.Models
{
    public sealed class ApplicationCommandOption
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public ApplicationCommandOptionType Type { get; init; }

        [JsonPropertyName("required")]
        public bool Required { get; init; }

        [JsonPropertyName("choices")]
        public IEnumerable<ApplicationCommandOptionChoice>? Choices { get; init; }
    }
}
