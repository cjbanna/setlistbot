using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.GratefulDead
{
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
}
