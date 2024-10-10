using EnsureThat;

namespace Setlistbot.Domain
{
    public readonly record struct PositiveTimeSpan
    {
        private readonly TimeSpan _value = TimeSpan.Zero;

        /// <summary>
        /// Record structs always have a default constructor, which is not allowed in this case.
        /// </summary>
        [Obsolete("Don't use the default constructor", true)]
        public PositiveTimeSpan() => throw new NotImplementedException();

        public PositiveTimeSpan(TimeSpan value) =>
            _value = TimeSpan.FromMilliseconds(
                EnsureArg.IsGte(value.TotalMilliseconds, 0, nameof(value))
            );

        public static implicit operator TimeSpan(PositiveTimeSpan value) => value._value;

        public static implicit operator PositiveTimeSpan(TimeSpan value) => new(value);
    }
}
