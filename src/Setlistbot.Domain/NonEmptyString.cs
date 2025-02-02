using Vogen;

namespace Setlistbot.Domain
{
    [ValueObject<string>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct NonEmptyString
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("String cannot be empty.")
                : Validation.Ok;
    }
}
