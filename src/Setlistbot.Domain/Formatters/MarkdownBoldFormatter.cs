namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a formatter as a Markdown bold.
    /// </summary>
    public sealed class MarkdownBoldFormatter(IFormatter formatter) : IFormatter
    {
        public string Format() => $"**{formatter.Format()}**";
    }
}
