using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public sealed class PhishInLinkFormatter(DateOnly date, IFormatter textFormatter) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(PhishUri.PhishIn(date), textFormatter).Format();
    }
}
