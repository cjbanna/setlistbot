using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Reddit.Models
{
    public sealed class AuthTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonProperty("expires_in")]
        public int ExpiresInMinutes { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; } = string.Empty;
    }
}
