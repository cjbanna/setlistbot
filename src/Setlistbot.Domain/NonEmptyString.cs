using EnsureThat;

namespace Setlistbot.Domain
{
    /// <summary>
    /// A non-empty string.
    /// </summary>
    public record struct NonEmptyString
    {
        private readonly string _value = string.Empty;

        /// <summary>
        /// Record structs always have a default constructor, which is not allowed in this case.
        /// </summary>
        [Obsolete("Don't use the default constructor", true)]
        public NonEmptyString() => throw new NotImplementedException();

        public NonEmptyString(string value) =>
            _value = EnsureArg.IsNotNullOrWhiteSpace(value, nameof(value));

        public static implicit operator string(NonEmptyString value) => value._value;

        public static implicit operator NonEmptyString(string value) => new(value);
    }
}
