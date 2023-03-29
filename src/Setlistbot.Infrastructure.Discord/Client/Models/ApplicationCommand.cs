using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Client.Models
{
    public class ApplicationCommand
    {
        [JsonProperty("id")]
        public string Id { get; init; } = string.Empty;

        [JsonProperty("type")]
        public ApplicationCommandType Type { get; init; }

        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; init; } = string.Empty;
    }
}
