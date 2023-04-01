using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Client.Models
{
    public class ApplicationCommandOptionChoice
    {
        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        [JsonProperty("value")]
        public string Value { get; init; } = string.Empty;
    }
}
