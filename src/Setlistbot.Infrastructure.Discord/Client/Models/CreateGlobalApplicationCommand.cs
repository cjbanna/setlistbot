using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Client.Models
{
    public class CreateGlobalApplicationCommand
    {
        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; init; } = string.Empty;

        [JsonProperty("type")]
        public ApplicationCommandType Type { get; init; }

        [JsonProperty("options")]
        public IEnumerable<ApplicationCommandOption>? Options { get; init; }
    }
}
