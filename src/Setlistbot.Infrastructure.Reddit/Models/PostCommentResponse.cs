using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.Reddit.Models
{
    public sealed class PostCommentResponse
    {
        [JsonPropertyName("json")]
        public PostCommentResponseJson Json { get; set; } = new PostCommentResponseJson();
    }

    public sealed class PostCommentResponseJson
    {
        [JsonPropertyName("data")]
        public PostCommentResponseData Data { get; set; } = new PostCommentResponseData();
    }

    public sealed class PostCommentResponseData
    {
        [JsonPropertyName("things")]
        public IEnumerable<Thing> Things { get; set; } = new List<Thing>();
    }

    public sealed class Thing
    {
        [JsonPropertyName("data")]
        public ThingData Data { get; set; } = new ThingData();
    }

    public sealed class ThingData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; } = string.Empty;
    }
}
