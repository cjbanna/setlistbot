using CSharpFunctionalExtensions;

namespace Setlistbot.Domain
{
    public record Location(Maybe<Venue> Venue, City City, Maybe<State> State, Country Country);

    public record State(NonEmptyString Value)
    {
        public static implicit operator string(State state) => state.Value;
    }

    public record Venue(NonEmptyString Value)
    {
        public static implicit operator string(Venue venue) => venue.Value;
    }

    public record City(NonEmptyString Value)
    {
        public static implicit operator string(City city) => city.Value;
    }

    public record Country(NonEmptyString Value)
    {
        public static implicit operator string(Country country) => country.Value;
    }
}
