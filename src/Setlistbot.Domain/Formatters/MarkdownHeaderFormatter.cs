namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a formatter as a Markdown header.
    /// </summary>
    public sealed class MarkdownHeaderFormatter(IFormatter formatter) : IFormatter
    {
        public string Format() => $"# {formatter.Format()}";
    }
}
