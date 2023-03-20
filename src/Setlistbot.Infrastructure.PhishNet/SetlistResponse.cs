using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.PhishNet
{
    public class SetlistResponse
    {
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; } = string.Empty;

        [JsonProperty("data")]
        public SetlistResponseData[] Data { get; set; } = new SetlistResponseData[0];
    }

    public class SetlistResponseData
    {
        [JsonProperty("showid")]
        public int ShowId { get; set; }

        [JsonProperty("showdate")]
        public string ShowDate { get; set; } = string.Empty;

        [JsonProperty("artist_name")]
        public string ArtistName { get; set; } = string.Empty;

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("song")]
        public string Song { get; set; } = string.Empty;

        [JsonProperty("footnote")]
        public string Footnote { get; set; } = string.Empty;

        [JsonProperty("set")]
        public string Set { get; set; } = string.Empty;

        [JsonProperty("trans_mark")]
        public string Transition { get; set; } = string.Empty;

        [JsonProperty("venue")]
        public string Venue { get; set; } = string.Empty;

        [JsonProperty("city")]
        public string City { get; set; } = string.Empty;

        [JsonProperty("state")]
        public string State { get; set; } = string.Empty;

        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        [JsonProperty("setlistnotes")]
        public string SetlistNotes { get; set; } = string.Empty;
    }
}
