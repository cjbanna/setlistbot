using CSharpFunctionalExtensions;

namespace Setlistbot.Domain
{
    public record Location(Maybe<Venue> Venue, City City, Maybe<State> State, Country Country)
    {
        // Sometimes the exact venue is not known
        public Maybe<Venue> Venue { get; private set; } = Venue;
        public City City { get; private set; } = City;

        // Shows played outside USA may not have a state
        public Maybe<State> State { get; private set; } = State;
        public Country Country { get; private set; } = Country;
    }

    public record State : StringNotNullOrWhiteSpace
    {
        public State(string value)
            : base(value) { }
    }

    public record Venue : StringNotNullOrWhiteSpace
    {
        public Venue(string value)
            : base(value) { }
    }

    public record City : StringNotNullOrWhiteSpace
    {
        public City(string value)
            : base(value) { }
    }

    public record Country : StringNotNullOrWhiteSpace
    {
        public Country(string value)
            : base(value) { }
    }
}
