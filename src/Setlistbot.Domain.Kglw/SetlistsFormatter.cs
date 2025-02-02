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
}
