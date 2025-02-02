using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public sealed class AttributionFormatter : IFormatter
    {
        public string Format() =>
            new MarkdownQuoteFormatter(
                new MarkdownItalicFormatter(
                    new CombinedFormatter(
                        new LiteralFormatter("data provided by"),
                        new SpaceFormatter(),
                        new MarkdownLinkFormatter(
                            new Uri("https://phish.net"),
                            new LiteralFormatter("phish.net")
                        )
                    )
                )
            ).Format();
    }
}
