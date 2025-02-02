using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.KglwNet
{
    public sealed class SetlistResponse
    {
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; } = string.Empty;

        [JsonProperty("data")]
        public SetlistResponseData[] Data { get; set; } = new SetlistResponseData[0];
    }

    public sealed class SetlistResponseData
    {
        [JsonProperty("show_id")]
        public int ShowId { get; set; }

        [JsonProperty("showdate")]
        public string ShowDate { get; set; } = string.Empty;

        [JsonProperty("artist")]
        public string ArtistName { get; set; } = string.Empty;

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("songname")]
        public string Song { get; set; } = string.Empty;

        [JsonProperty("footnote")]
        public string Footnote { get; set; } = string.Empty;

        [JsonProperty("settype")]
        public string SetType { get; set; } = string.Empty;

        [JsonProperty("setnumber")]
        public int SetNumber { get; set; }

        [JsonProperty("transition")]
        public string Transition { get; set; } = string.Empty;

        [JsonProperty("venuename")]
        public string Venue { get; set; } = string.Empty;

        [JsonProperty("city")]
        public string City { get; set; } = string.Empty;

        [JsonProperty("state")]
        public string State { get; set; } = string.Empty;

        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        [JsonProperty("shownotes")]
        public string ShowNotes { get; set; } = string.Empty;

        [JsonProperty("permalink")]
        public string Permalink { get; set; } = string.Empty;
    }
}
