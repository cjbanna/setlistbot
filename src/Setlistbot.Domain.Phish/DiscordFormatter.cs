using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public sealed class DiscordFormatter(Setlist setlist) : IFormatter
    {
        private static readonly Uri PhishNetUri = new("http://phish.net");

        public string Format() =>
            new CombinedFormatter(
                new MarkdownBoldFormatter(new LiteralFormatter("Phish")),
                new NewLineFormatter(2),
                new SetlistHeaderFormatter(setlist),
                new NewLineFormatter(2),
                new SetsFormatter(setlist.Sets),
                new NewLineFormatter(2),
                new MarkdownQuoteFormatter(
                    new MarkdownItalicFormatter(
                        new CombinedFormatter(
                            new LiteralFormatter("data provided by "),
                            new MarkdownLinkFormatter(
                                PhishNetUri,
                                new LiteralFormatter("phish.net")
                            )
                        )
                    )
                )
            ).Format();
    }
}
