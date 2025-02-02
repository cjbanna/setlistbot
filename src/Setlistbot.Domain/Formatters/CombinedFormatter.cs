namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a list of formatters in order.
    /// </summary>
    public sealed class CombinedFormatter(params IFormatter[] formatters) : IFormatter
    {
        public string Format() => formatters.Format();
    }
}
