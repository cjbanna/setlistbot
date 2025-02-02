using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Client.Models
{
    public sealed class ApplicationCommandOption
    {
        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; init; } = string.Empty;

        [JsonProperty("type")]
        public ApplicationCommandOptionType Type { get; init; }

        [JsonProperty("required")]
        public bool Required { get; init; }

        [JsonProperty("choices")]
        public IEnumerable<ApplicationCommandOptionChoice>? Choices { get; init; }
    }
}
