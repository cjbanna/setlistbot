using CSharpFunctionalExtensions;
using StronglyTypedPrimitives.Attributes;

namespace Setlistbot.Domain
{
    public sealed record Location(
        Maybe<Venue> Venue,
        City City,
        Maybe<State> State,
        Country Country
    );

    [StronglyTyped(Template.String)]
    public readonly partial struct State
    {
        public State(NonEmptyString value) => _value = value;
    }

    [StronglyTyped(Template.String)]
    public readonly partial struct Venue
    {
        public Venue(NonEmptyString value) => _value = value;
    }

    [StronglyTyped(Template.String)]
    public readonly partial struct City
    {
        public City(NonEmptyString value) => _value = value;
    }

    [StronglyTyped(Template.String)]
    public readonly partial struct Country
    {
        public Country(NonEmptyString value) => _value = value;
    }
}
