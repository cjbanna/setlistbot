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
}
