using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Discord.Client.Models
{
    public sealed class CreateGlobalApplicationCommand
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public ApplicationCommandType Type { get; init; }

        [JsonPropertyName("options")]
        public IEnumerable<ApplicationCommandOption>? Options { get; init; }
    }
}
