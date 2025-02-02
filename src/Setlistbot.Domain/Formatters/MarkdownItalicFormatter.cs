namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a formatter as a Markdown italic.
    /// </summary>
    public sealed class MarkdownItalicFormatter(IFormatter formatter) : IFormatter
    {
        public string Format() => $"_{formatter.Format()}_";
    }
}
