using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Kglw
{
    public sealed class AttributionFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new MarkdownQuoteFormatter(
                new MarkdownItalicFormatter(
                    new CombinedFormatter(
                        new LiteralFormatter("data provided by"),
                        new SpaceFormatter(),
                        new KglwNetLinkFormatter(setlist, new LiteralFormatter("kglw.net"))
                    )
                )
            ).Format();
    }
}
