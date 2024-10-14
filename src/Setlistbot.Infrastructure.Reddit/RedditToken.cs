using Vogen;

namespace Setlistbot.Infrastructure.Reddit
{
    [ValueObject<string>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct RedditToken
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("Value cannot be null or whitespace.")
                : Validation.Ok;
    }
}
