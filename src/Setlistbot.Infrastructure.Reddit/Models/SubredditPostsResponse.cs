using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Reddit.Models
{
    public sealed class SubredditPostsResponse
    {
        [JsonPropertyName("data")]
        public SubredditPostsResponseData Data { get; set; } = new SubredditPostsResponseData();
    }

    public sealed class SubredditPostsResponseData
    {
        [JsonPropertyName("children")]
        public IEnumerable<SubredditPost> Children { get; set; } = new List<SubredditPost>();
    }

    public sealed class SubredditPost
    {
        [JsonPropertyName("data")]
        public SubredditPostData Data { get; set; } = new SubredditPostData();
    }

    public sealed class SubredditPostData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("selftext")]
        public string SelfText { get; set; } = string.Empty;

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; } = string.Empty;
    }
}
