namespace Setlistbot.Domain.Formatters
{
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
}
