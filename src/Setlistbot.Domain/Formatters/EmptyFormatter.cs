namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Returns an empty string.
    /// </summary>
    public sealed class EmptyFormatter : IFormatter
    {
        public string Format() => string.Empty;
    }
}
