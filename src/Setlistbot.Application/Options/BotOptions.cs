namespace Setlistbot.Application.Options
{
    public sealed class BotOptions
    {
        public string ArtistId { get; set; } = string.Empty;
        public string Subreddit { get; set; } = string.Empty;
        public int MaxSetlistCount { get; set; }
        public bool RequireMention { get; set; }
    }
}
