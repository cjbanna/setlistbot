using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.GratefulDead
{
    public sealed class SetlistsFormatter(IEnumerable<Setlist> setlists) : IFormatter
    {
        public string Format() =>
            setlists
                .Select(setlist => new CombinedFormatter(
                    new ArchiveOrgLinkFormatter(setlist, linkAsDate: true),
                    new SpaceFormatter(),
                    new CharacterFormatter('@'),
                    new SpaceFormatter(),
                    new LocationFormatter(setlist.Location),
                    new NewLineFormatter(2)
                ))
                .Format();
    }
}
