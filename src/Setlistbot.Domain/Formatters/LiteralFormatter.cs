namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a string as-is.
    /// </summary>
    public sealed class LiteralFormatter(string s) : IFormatter
    {
        public string Format() => s;
    }
}
