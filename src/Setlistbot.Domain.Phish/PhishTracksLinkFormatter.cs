using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public sealed class PhishTracksLinkFormatter(DateOnly date, IFormatter textFormatter)
        : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(PhishUri.PhishTracks(date), textFormatter).Format();
    }
}
