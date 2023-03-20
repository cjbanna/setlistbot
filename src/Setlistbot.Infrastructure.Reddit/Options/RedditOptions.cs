namespace Setlistbot.Infrastructure.Reddit.Options
{
    public sealed class RedditOptions
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int CommentsLimit { get; set; }
    }
}
