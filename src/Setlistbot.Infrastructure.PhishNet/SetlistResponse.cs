using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.PhishNet
{
    public sealed class SetlistResponse
    {
        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public SetlistResponseData[] Data { get; set; } = [];
    }

    public sealed class SetlistResponseData
    {
        [JsonPropertyName("showid")]
        public string ShowId { get; set; }

        [JsonPropertyName("showdate")]
        public string ShowDate { get; set; } = string.Empty;

        [JsonPropertyName("artist_name")]
        public string ArtistName { get; set; } = string.Empty;

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("song")]
        public string Song { get; set; } = string.Empty;

        [JsonPropertyName("footnote")]
        public string Footnote { get; set; } = string.Empty;

        [JsonPropertyName("set")]
        public string Set { get; set; } = string.Empty;

        [JsonPropertyName("trans_mark")]
        public string Transition { get; set; } = string.Empty;

        [JsonPropertyName("venue")]
        public string Venue { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("setlistnotes")]
        public string SetlistNotes { get; set; } = string.Empty;
    }
}
