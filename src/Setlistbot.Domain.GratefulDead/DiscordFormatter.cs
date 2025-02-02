using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.GratefulDead
{
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
}
