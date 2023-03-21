using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Reddit.Models
{
    public sealed class SubredditCommentsResponse
    {
        [JsonProperty("data")]
        public SubredditCommentsResponseData Data { get; set; } =
            new SubredditCommentsResponseData();
    }

    public sealed class SubredditCommentsResponseData
    {
        [JsonProperty("children")]
        public IEnumerable<SubredditComment> Children { get; set; } = new List<SubredditComment>();
    }

    public sealed class SubredditComment
    {
        [JsonProperty("data")]
        public SubredditCommentData Data { get; set; } = new SubredditCommentData();
    }

    public sealed class SubredditCommentData
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("author")]
        public string Author { get; set; } = string.Empty;

        [JsonProperty("body")]
        public string Body { get; set; } = string.Empty;

        [JsonProperty("permalink")]
        public string Permalink { get; set; } = string.Empty;
    }
}
