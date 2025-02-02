namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a formatter as a Markdown quote.
    /// </summary>
    public sealed class MarkdownQuoteFormatter(IFormatter formatter) : IFormatter
    {
        public string Format() => $"> {formatter.Format()}";
    }
}
