using EnsureThat;

namespace Setlistbot.Domain
{
    public readonly record struct PositiveInt
    {
        private readonly int _value = 0;

        public PositiveInt(int value) => _value = EnsureArg.IsGte(value, 0, nameof(value));

        public static implicit operator int(PositiveInt value) => value._value;

        public static implicit operator PositiveInt(int value) => new(value);
    }
}
