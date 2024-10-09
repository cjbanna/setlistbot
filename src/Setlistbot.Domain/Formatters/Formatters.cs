using System.Text;
using CSharpFunctionalExtensions;

namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Returns an empty string.
    /// </summary>
    public sealed class EmptyFormatter : IFormatter
    {
        public string Format() => string.Empty;
    }

    /// <summary>
    /// Formats a character as-is.
    /// </summary>
    public sealed class CharacterFormatter(char c) : IFormatter
    {
        public string Format() => c.ToString();
    }

    /// <summary>
    /// Formats a string as-is.
    /// </summary>
    public sealed class LiteralFormatter(string s) : IFormatter
    {
        public string Format() => s;
    }

    /// <summary>
    /// Formats a new line character a specified number of times.
    /// </summary>
    public sealed class NewLineFormatter(int count = 1) : IFormatter
    {
        public string Format() =>
            count == 1
                ? Environment.NewLine
                : string.Concat(Enumerable.Repeat(Environment.NewLine, count));
    }

    /// <summary>
    /// Formats a space character a specified number of times.
    /// </summary>
    public sealed class SpaceFormatter(int count = 1) : IFormatter
    {
        public string Format() =>
            count == 1
                ? " "
                : Enumerable
                    .Repeat(" ", count)
                    .Aggregate(new StringBuilder(), (acc, s) => acc.Append(s))
                    .ToString();
    }

    /// <summary>
    /// Formats a DateTime as a year-month-day string.
    /// </summary>
    public sealed class YearMonthDayFormatter(DateOnly date) : IFormatter
    {
        public string Format() => date.ToString("yyyy-MM-dd");
    }

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

    /// <summary>
    /// Formats a formatter as a Markdown bold.
    /// </summary>
    public sealed class MarkdownBoldFormatter(IFormatter formatter) : IFormatter
    {
        public string Format() => $"**{formatter.Format()}**";
    }

    /// <summary>
    /// Formats a formatter as a Markdown link.
    /// </summary>
    /// <param name="uri">The URI</param>
    /// <param name="textFormatter">The formatter that provides the link text</param>
    public sealed class MarkdownLinkFormatter(Uri uri, IFormatter textFormatter) : IFormatter
    {
        public string Format() => $"[{textFormatter.Format()}]({uri})";
    }

    /// <summary>
    /// Formats a formatter as a Markdown italic.
    /// </summary>
    public sealed class MarkdownItalicFormatter(IFormatter formatter) : IFormatter
    {
        public string Format() => $"_{formatter.Format()}_";
    }

    /// <summary>
    /// Formats a formatter as a Markdown quote.
    /// </summary>
    public sealed class MarkdownQuoteFormatter(IFormatter formatter) : IFormatter
    {
        public string Format() => $"> {formatter.Format()}";
    }

    /// <summary>
    /// Formats a formatter as a Markdown header.
    /// </summary>
    public sealed class MarkdownHeaderFormatter(IFormatter formatter) : IFormatter
    {
        public string Format() => $"# {formatter.Format()}";
    }

    /// <summary>
    /// Formats a list of formatters in order.
    /// </summary>
    public sealed class CombinedFormatter(params IFormatter[] formatters) : IFormatter
    {
        public string Format() => formatters.Format();
    }

    /// <summary>
    /// Formats a list of formatters with a separator.
    /// </summary>
    /// <example>
    /// Using a comma separator: [formatter1, formatter2, formatter3] => "one,two,three"
    /// </example>
    public sealed class SeparatedFormatter(IFormatter separator, IEnumerable<IFormatter> formatters)
        : IFormatter
    {
        public string Format()
        {
            var count = formatters.Count();
            return formatters
                .Zip(Enumerable.Repeat(separator, count))
                .SelectMany(z => new[] { z.First, z.Second })
                .Take(count * 2 - 1) // Trim the last separator
                .Format();
        }
    }
}
