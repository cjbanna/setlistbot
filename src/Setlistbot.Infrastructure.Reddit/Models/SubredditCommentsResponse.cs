using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Reddit.Models
{
    public sealed class SubredditCommentsResponse
    {
        [JsonPropertyName("data")]
        public SubredditCommentsResponseData Data { get; set; } =
            new SubredditCommentsResponseData();
    }

    public sealed class SubredditCommentsResponseData
    {
        [JsonPropertyName("children")]
        public IEnumerable<SubredditComment> Children { get; set; } = new List<SubredditComment>();
    }

    public sealed class SubredditComment
    {
        [JsonPropertyName("data")]
        public SubredditCommentData Data { get; set; } = new SubredditCommentData();
    }

    public sealed class SubredditCommentData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; } = string.Empty;
    }
}
