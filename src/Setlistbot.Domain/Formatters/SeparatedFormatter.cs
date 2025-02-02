namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a list of formatters with a separator.
    /// </summary>
    /// <example>
    /// Using a comma separator: [formatter1, formatter2, formatter3] => "one,two,three"
    /// </example>
    public sealed class SeparatedFormatter(IFormatter separator, IEnumerable<IFormatter> formatters)
        : IFormatter
    {
        public string Format()
        {
            var count = formatters.Count();
            return formatters
                .Zip(Enumerable.Repeat(separator, count))
                .SelectMany(z => new[] { z.First, z.Second })
                .Take(count * 2 - 1) // Trim the last separator
                .Format();
        }
    }
}
