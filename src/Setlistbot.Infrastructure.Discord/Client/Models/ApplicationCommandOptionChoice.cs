using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Discord.Client.Models
{
    public sealed class ApplicationCommandOptionChoice
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("value")]
        public string Value { get; init; } = string.Empty;
    }
}
