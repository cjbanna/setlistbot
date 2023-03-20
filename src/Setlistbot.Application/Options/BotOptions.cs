namespace Setlistbot.Application.Options
{
    public class BotOptions
    {
        public string ArtistId { get; set; } = string.Empty;
        public string Subreddit { get; set; } = string.Empty;
        public int MaxSetlistCount { get; set; }
        public bool RequireMention { get; set; }
    }
}
