namespace Setlistbot.Domain.Formatters
{
    public sealed class SetlistHeaderFormatter(Setlist setlist) : IFormatter
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
}
