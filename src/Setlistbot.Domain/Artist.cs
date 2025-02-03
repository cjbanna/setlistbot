using Vogen;

namespace Setlistbot.Domain
{
    [ValueObject<string>]
    public readonly partial struct ArtistId
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("Artist ID cannot be empty.")
                : Validation.Ok;
    }

    [ValueObject<string>]
    public readonly partial struct ArtistName
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("Artist name cannot be empty.")
                : Validation.Ok;
    }
}
