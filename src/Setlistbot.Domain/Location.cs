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
            City = Ensure.String.IsNotNullOrWhiteSpace(city, nameof(city));
            State = Ensure.String.IsNotNullOrWhiteSpace(state, nameof(state));
            Country = Ensure.String.IsNotNullOrWhiteSpace(country, nameof(country));
        }
    }
}
