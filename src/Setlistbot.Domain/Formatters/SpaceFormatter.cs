using System.Text;

namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a space character a specified number of times.
    /// </summary>
    public sealed class SpaceFormatter(int count = 1) : IFormatter
    {
        public string Format() =>
            count == 1
                ? " "
                : Enumerable
                    .Repeat(" ", count)
                    .Aggregate(new StringBuilder(), (acc, s) => acc.Append(s))
                    .ToString();
    }
}
