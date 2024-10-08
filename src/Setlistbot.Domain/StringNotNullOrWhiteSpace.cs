using EnsureThat;

namespace Setlistbot.Domain
{
    public abstract record StringNotNullOrWhiteSpace
    {
        private readonly string _value = string.Empty;

        protected StringNotNullOrWhiteSpace(string value)
        {
            _value = EnsureArg.IsNotNullOrWhiteSpace(value, nameof(value));
        }

        public static implicit operator string(StringNotNullOrWhiteSpace value) => value._value;
    }
}
