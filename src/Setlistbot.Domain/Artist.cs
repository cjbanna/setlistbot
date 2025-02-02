using Vogen;

namespace Setlistbot.Domain
{
    [ValueObject<string>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct ArtistId
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("Artist ID cannot be empty.")
                : Validation.Ok;
    }

    [ValueObject<string>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct ArtistName
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("Artist name cannot be empty.")
                : Validation.Ok;
    }
}
