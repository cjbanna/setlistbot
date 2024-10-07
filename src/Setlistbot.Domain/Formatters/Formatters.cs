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
        public string Format() => string.Concat(Enumerable.Repeat(Environment.NewLine, count));
    }

    /// <summary>
    /// Formats a space character a specified number of times.
    /// </summary>
    public sealed class SpaceFormatter(int count = 1) : IFormatter
    {
        public string Format() =>
            Enumerable
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
    /// Formats a Maybe as a string.
    /// </summary>
    public sealed class MaybeFormatter<T>(Maybe<T> maybe, string defaultValue = "") : IFormatter
        where T : notnull
    {
        public string Format() => maybe.Match(some => some.ToString()!, () => defaultValue);
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
    public sealed class SeparatedFormatter(IFormatter separator, IEnumerable<IFormatter> formatters)
        : IFormatter
    {
        public string Format() =>
            formatters
                .Zip(Enumerable.Repeat(separator, formatters.Count()))
                .SelectMany(z => new[] { z.First, z.Second })
                .Take(formatters.Count() - 2) // Trim off the last separator and space
                .Format();
    }

    /// <summary>
    /// Removes empty formatters from the list before formatting.
    /// </summary>
    public sealed class RemoveEmptyFormatter(IEnumerable<IFormatter> formatters) : IFormatter
    {
        public string Format() =>
            formatters.Where(f => !string.IsNullOrWhiteSpace(f.Format())).Format();
    }
}
