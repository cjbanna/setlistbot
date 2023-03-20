using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.Reddit.Models
{
    public sealed class PostCommentResponse
    {
        [JsonProperty("json")]
        public PostCommentResponseJson Json { get; set; } = new PostCommentResponseJson();
    }

    public sealed class PostCommentResponseJson
    {
        [JsonProperty("data")]
        public PostCommentResponseData Data { get; set; } = new PostCommentResponseData();
    }

    public sealed class PostCommentResponseData
    {
        [JsonProperty("things")]
        public IEnumerable<Thing> Things { get; set; } = new List<Thing>();
    }

    public sealed class Thing
    {
        [JsonProperty("data")]
        public ThingData Data { get; set; } = new ThingData();
    }

    public sealed class ThingData
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("permalink")]
        public string Permalink { get; set; } = string.Empty;
    }
}
