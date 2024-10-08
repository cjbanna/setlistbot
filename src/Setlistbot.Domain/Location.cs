using CSharpFunctionalExtensions;

namespace Setlistbot.Domain
{
    public record Location(Maybe<Venue> Venue, City City, Maybe<State> State, Country Country);

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
