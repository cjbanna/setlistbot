using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public sealed class SetlistsFormatter(IEnumerable<Setlist> setlists) : IFormatter
    {
        public string Format() =>
            setlists
                .Select(setlist => new CombinedFormatter(
                    new PhishNetLinkFormatter(
                        setlist.Date,
                        new YearMonthDayFormatter(setlist.Date)
                    ),
                    new SpaceFormatter(),
                    new CharacterFormatter('@'),
                    new SpaceFormatter(),
                    new LocationFormatter(setlist.Location),
                    new NewLineFormatter(2)
                ))
                .Append<IFormatter>(new AttributionFormatter())
                .Format();
    }

    public sealed class SetlistFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new Formatters.SetlistFormatter(setlist),
                new LinksFormatter(setlist),
                new NewLineFormatter(2),
                new AttributionFormatter()
            ).Format();
    }

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

    public sealed class LinksFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new SeparatedFormatter(
                new CombinedFormatter(
                    new SpaceFormatter(),
                    new CharacterFormatter('|'),
                    new SpaceFormatter()
                ),
                new IFormatter[]
                {
                    new PhishNetLinkFormatter(setlist.Date, new LiteralFormatter("phish.net")),
                    new PhishInLinkFormatter(setlist.Date, new LiteralFormatter("phish.in")),
                    new PhishTracksLinkFormatter(setlist.Date, new LiteralFormatter("phishtracks")),
                }
            ).Format();
    }

    public sealed class PhishTracksLinkFormatter(DateOnly date, IFormatter textFormatter)
        : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(PhishUri.PhishTracks(date), textFormatter).Format();
    }

    public sealed class PhishInLinkFormatter(DateOnly date, IFormatter textFormatter) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(PhishUri.PhishIn(date), textFormatter).Format();
    }

    public sealed class PhishNetLinkFormatter(DateOnly date, IFormatter textFormatter) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(PhishUri.PhishNet(date), textFormatter).Format();
    }

    public static class PhishUri
    {
        public static Uri PhishNet(DateOnly date) =>
            new Uri($"https://phish.net/setlists/?d={date:yyyy-MM-dd}");

        public static Uri PhishIn(DateOnly date) => new Uri($"https://phish.in/{date:yyyy-MM-dd}");

        public static Uri PhishTracks(DateOnly date) =>
            new Uri($"https://phishtracks.com/shows/{date:yyyy-MM-dd}");
    }
}
