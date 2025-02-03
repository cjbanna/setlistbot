using Vogen;

namespace Setlistbot.Domain
{
    [ValueObject<string>]
    public readonly partial struct NonEmptyString
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("String cannot be empty.")
                : Validation.Ok;
    }
}
