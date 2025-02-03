using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.KglwNet
{
    public sealed class SetlistResponse
    {
        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public SetlistResponseData[] Data { get; set; } = new SetlistResponseData[0];
    }

    public sealed class SetlistResponseData
    {
        [JsonPropertyName("show_id")]
        public int ShowId { get; set; }

        [JsonPropertyName("showdate")]
        public string ShowDate { get; set; } = string.Empty;

        [JsonPropertyName("artist")]
        public string ArtistName { get; set; } = string.Empty;

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("songname")]
        public string Song { get; set; } = string.Empty;

        [JsonPropertyName("footnote")]
        public string Footnote { get; set; } = string.Empty;

        [JsonPropertyName("settype")]
        public string SetType { get; set; } = string.Empty;

        [JsonPropertyName("setnumber")]
        public string SetNumber { get; set; } = string.Empty;

        [JsonPropertyName("transition")]
        public string Transition { get; set; } = string.Empty;

        [JsonPropertyName("venuename")]
        public string Venue { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("shownotes")]
        public string ShowNotes { get; set; } = string.Empty;

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; } = string.Empty;
    }
}
