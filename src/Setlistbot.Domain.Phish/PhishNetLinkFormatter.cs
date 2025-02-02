using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public sealed class PhishNetLinkFormatter(DateOnly date, IFormatter textFormatter) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(PhishUri.PhishNet(date), textFormatter).Format();
    }
}
