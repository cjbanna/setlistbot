using System.ComponentModel;
using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public sealed class PhishMultipleSetlistFormatter(IEnumerable<Setlist> setlists) : IFormatter
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
                .Append<IFormatter>(new PhishAttributionFormatter())
                .Format();
    }

    public sealed class PhishSetlistFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new SetlistFormatter(setlist),
                new PhishLinksFormatter(setlist),
                new NewLineFormatter(2),
                new PhishAttributionFormatter()
            ).Format();
    }

    public sealed class PhishAttributionFormatter : IFormatter
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

    public sealed class PhishLinksFormatter(Setlist setlist) : IFormatter
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
            new MarkdownLinkFormatter(PhishUri.PhishIn(date), textFormatter).Format();
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
            new Uri($"http://phish.net/setlists/?d={date:yyyy-MM-dd}");

        public static Uri PhishIn(DateOnly date) => new Uri($"http://phish.in/{date}");

        public static Uri PhishTracks(DateOnly date) =>
            new Uri($"http://phishtracks.com/shows/{date:yyyy-MM-dd}");
    }
}
