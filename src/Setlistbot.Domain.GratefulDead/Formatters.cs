using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.GratefulDead
{
    public sealed class SetlistsFormatter(IEnumerable<Setlist> setlists) : IFormatter
    {
        public string Format() =>
            setlists
                .Select(setlist => new CombinedFormatter(
                    new ArchiveOrgLinkFormatter(setlist),
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
                new ArchiveOrgLinkFormatter(setlist),
                new MaybeFormatter<string>(
                    setlist.SpotifyUrl,
                    () =>
                        new CombinedFormatter(
                            new SpaceFormatter(),
                            new CharacterFormatter('|'),
                            new SpaceFormatter(),
                            new SpotifyLinkFormatter(setlist.SpotifyUrl.Value)
                        )
                )
            ).Format();
    }

    public sealed class HeaderFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new YearMonthDayFormatter(setlist.Date),
                new SpaceFormatter(),
                new CharacterFormatter('@'),
                new SpaceFormatter(),
                new LocationFormatter(setlist.Location)
            ).Format();
    }

    public sealed class DiscordFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new MarkdownBoldFormatter(new LiteralFormatter("Grateful Dead")),
                new NewLineFormatter(2),
                new SetlistHeaderFormatter(setlist),
                new NewLineFormatter(2),
                new SetsFormatter(setlist.Sets),
                new NewLineFormatter(2),
                new ArchiveOrgLinkFormatter(setlist),
                new MaybeFormatter<string>(
                    setlist.SpotifyUrl,
                    new CombinedFormatter(
                        new SpaceFormatter(),
                        new CharacterFormatter('|'),
                        new SpotifyLinkFormatter(setlist.SpotifyUrl.Value)
                    )
                )
            ).Format();
    }

    public sealed class ArchiveOrgLinkFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(
                Uris.ArchiveOrg(setlist),
                new LiteralFormatter("archive.org")
            ).Format();
    }

    public sealed class SpotifyLinkFormatter(string spotifyUri) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(
                new Uri(spotifyUri),
                new LiteralFormatter("Spotify")
            ).Format();
    }

    public static class Uris
    {
        public static Uri ArchiveOrg(Setlist setlist) =>
            new($"https://archive.org/details/GratefulDead?query=date:{setlist.Date:yyyy-MM-dd}");
    }
}
