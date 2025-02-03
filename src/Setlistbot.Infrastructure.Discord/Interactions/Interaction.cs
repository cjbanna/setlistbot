using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Discord.Interactions
{
    public sealed class Interaction
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("application_id")]
        public string ApplicationId { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public InteractionType InteractionType { get; set; } = default!;

        [JsonPropertyName("data")]
        public InteractionData? Data { get; set; }

        [JsonPropertyName("guild_id")]
        public string GuildId { get; set; } = string.Empty;
    }
}
