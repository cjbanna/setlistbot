namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a formatter as a Markdown link.
    /// </summary>
    /// <param name="uri">The URI</param>
    /// <param name="textFormatter">The formatter that provides the link text</param>
    public sealed class MarkdownLinkFormatter(Uri uri, IFormatter textFormatter) : IFormatter
    {
        public string Format() => $"[{textFormatter.Format()}]({uri})";
    }
}
