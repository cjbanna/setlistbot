using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Kglw
{
    public sealed class KglwNetLinkFormatter(Setlist setlist, IFormatter textFormatter) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(
                new Uri($"https://kglw.net/setlists/{setlist.Permalink}"),
                textFormatter
            ).Format();
    }
}
