using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public class InteractionOption
    {
        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        [JsonProperty("type")]
        public ApplicationCommandOptionType Type { get; init; } = default!;

        [JsonProperty("value")]
        public object Value { get; init; } = default!;
    }
}
