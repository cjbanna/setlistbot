using CSharpFunctionalExtensions;

namespace Setlistbot.Domain.Formatters
{
    public sealed class LocationFormatter(Location location) : IFormatter
    {
        // Missing parts are skipped so they don't leave a dangling ", "
        public string Format() =>
            string.Join(
                ", ",
                new[]
                {
                    location.Venue.Map(v => v.Value).GetValueOrDefault(),
                    location.City.Value,
                    location.State.Map(s => s.Value).GetValueOrDefault(),
                    location.Country.Value,
                }.Where(s => !string.IsNullOrEmpty(s))
            );
    }
}
