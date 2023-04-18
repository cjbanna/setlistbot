using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public class Interaction
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; } = string.Empty;

        [JsonProperty("type")]
        public InteractionType InteractionType { get; set; } = default!;

        [JsonProperty("data")]
        public InteractionData? Data { get; set; }

        [JsonProperty("guild_id")]
        public string GuildId { get; set; } = string.Empty;
    }
}
