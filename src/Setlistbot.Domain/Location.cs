using EnsureThat;

namespace Setlistbot.Domain
{
    public class Location
    {
        public string Venue { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }

        public Location(string venue, string city, string state, string country)
        {
            // Sometimes the exact venue is not known
            Venue = venue;
            // Shows played outside USA may not have a state
            State = state;
            City = Ensure.String.IsNotNullOrWhiteSpace(city, nameof(city));
            Country = Ensure.String.IsNotNullOrWhiteSpace(country, nameof(country));
        }
    }
}
