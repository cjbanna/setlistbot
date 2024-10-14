using CSharpFunctionalExtensions;
using Vogen;

namespace Setlistbot.Domain
{
    public sealed record Location(
        Maybe<Venue> Venue,
        City City,
        Maybe<State> State,
        Country Country
    );

    [ValueObject<string>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct State
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("State cannot be empty.")
                : Validation.Ok;
    }

    [ValueObject<string>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct Venue
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("Venue cannot be empty.")
                : Validation.Ok;
    }

    [ValueObject<string>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct City
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("City cannot be empty.")
                : Validation.Ok;
    }

    [ValueObject<string>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct Country
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("Country cannot be empty.")
                : Validation.Ok;
    }
}
