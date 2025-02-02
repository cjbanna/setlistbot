using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.GratefulDead
{
    public sealed class SpotifyLinkFormatter(string spotifyUri) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(
                new Uri(spotifyUri),
                new LiteralFormatter("Spotify")
            ).Format();
    }
}
