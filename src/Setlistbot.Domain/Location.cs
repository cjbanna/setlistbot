using CSharpFunctionalExtensions;

namespace Setlistbot.Domain
{
    public sealed record Location(
        Maybe<Venue> Venue,
        City City,
        Maybe<State> State,
        Country Country
    );

    public sealed record State(NonEmptyString Value)
    {
        public static implicit operator string(State state) => state.Value;

        public static implicit operator State(string state) => new(new NonEmptyString(state));
    }

    public sealed record Venue(NonEmptyString Value)
    {
        public static implicit operator string(Venue venue) => venue.Value;

        public static implicit operator Venue(string venue) => new(new NonEmptyString(venue));
    }

    public sealed record City(NonEmptyString Value)
    {
        public static implicit operator string(City city) => city.Value;

        public static implicit operator City(string city) => new(new NonEmptyString(city));
    }

    public sealed record Country(NonEmptyString Value)
    {
        public static implicit operator string(Country country) => country.Value;

        public static implicit operator Country(string country) => new(new NonEmptyString(country));
    }
}
