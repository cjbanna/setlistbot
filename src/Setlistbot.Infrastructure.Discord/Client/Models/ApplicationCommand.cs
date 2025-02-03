using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Discord.Client.Models
{
    public sealed class ApplicationCommand
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public ApplicationCommandType Type { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; init; } = string.Empty;
    }
}
