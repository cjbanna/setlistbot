using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Reddit.Models
{
    public sealed class SubredditPostsResponse
    {
        [JsonProperty("data")]
        public SubredditPostsResponseData Data { get; set; } = new SubredditPostsResponseData();
    }

    public sealed class SubredditPostsResponseData
    {
        [JsonProperty("children")]
        public IEnumerable<SubredditPost> Children { get; set; } = new List<SubredditPost>();
    }

    public sealed class SubredditPost
    {
        [JsonProperty("data")]
        public SubredditPostData Data { get; set; } = new SubredditPostData();
    }

    public sealed class SubredditPostData
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("author")]
        public string Author { get; set; } = string.Empty;

        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("selftext")]
        public string SelfText { get; set; } = string.Empty;

        [JsonProperty("permalink")]
        public string Permalink { get; set; } = string.Empty;
    }
}
