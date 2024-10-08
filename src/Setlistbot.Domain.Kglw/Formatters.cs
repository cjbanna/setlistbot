using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Kglw
{
    public sealed class SetlistsFormatter(IEnumerable<Setlist> setlists) : IFormatter
    {
        public string Format() =>
            setlists
                .Select(setlist => new CombinedFormatter(
                    new KglwNetLinkFormatter(setlist, new YearMonthDayFormatter(setlist.Date)),
                    new SpaceFormatter(),
                    new CharacterFormatter('@'),
                    new SpaceFormatter(),
                    new LocationFormatter(setlist.Location),
                    new NewLineFormatter(2)
                ))
                .Format();
    }

    public sealed class SetlistFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new Formatters.SetlistFormatter(setlist),
                new AttributionFormatter(setlist)
            ).Format();
    }

    public sealed class DiscordFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new MarkdownBoldFormatter(new LiteralFormatter("King Gizzard & The Lizard Wizard")),
                new NewLineFormatter(2),
                new SetlistHeaderFormatter(setlist),
                new NewLineFormatter(2),
                new CombinedFormatter(
                    setlist.Sets.Select<Set, IFormatter>(s => new SetFormatter(s)).ToArray()
                ),
                new NewLineFormatter(2),
                new AttributionFormatter(setlist)
            ).Format();
    }

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

    public sealed class KglwNetLinkFormatter(Setlist setlist, IFormatter textFormatter) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(KglwUri.KglwNet(setlist), textFormatter).Format();
    }

    public static class KglwUri
    {
        public static Uri KglwNet(Setlist setlist) =>
            new($"https://kglw.net/setlists/{setlist.Permalink}");
    }
}
