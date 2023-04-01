using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public class InteractionData
    {
        [JsonProperty("id")]
        public string Id { get; init; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        [JsonProperty("type")]
        public InteractionType Type { get; init; } = default!;

        [JsonProperty("options")]
        public IEnumerable<InteractionOption> Options { get; init; } =
            Enumerable.Empty<InteractionOption>();
    }
}
