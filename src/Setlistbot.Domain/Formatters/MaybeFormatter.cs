using CSharpFunctionalExtensions;

namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a Maybe. If the Maybe is a Some, the inner value is formatted. If the Maybe is a None, the default
    /// formatter is used.
    /// </summary>
    public sealed class MaybeFormatter<T> : IFormatter
        where T : notnull
    {
        private readonly Maybe<T> _maybe;
        private readonly Func<IFormatter> _formatterProvider;
        private readonly IFormatter? _defaultFormatter;

        /// <summary>
        /// Formats a Maybe. If the Maybe is a Some, the inner value is formatted. If the Maybe is a None, the default
        /// formatter is used.
        /// </summary>
        public MaybeFormatter(
            Maybe<T> maybe,
            IFormatter formatter,
            IFormatter? defaultFormatter = null
        )
        {
            _maybe = maybe;
            _formatterProvider = () => formatter;
            _defaultFormatter = defaultFormatter;
        }

        public MaybeFormatter(Maybe<T> maybe, Func<IFormatter> formatterProvider)
        {
            _maybe = maybe;
            _formatterProvider = formatterProvider;
        }

        public string Format() =>
            _maybe.Match(
                _ => _formatterProvider().Format(),
                (_defaultFormatter ?? new EmptyFormatter()).Format
            );
    }
}
