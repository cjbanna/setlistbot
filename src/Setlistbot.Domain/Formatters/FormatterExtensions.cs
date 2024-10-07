using System.Text;

namespace Setlistbot.Domain.Formatters
{
    public static class FormatterExtensions
    {
        public static string Format(this IEnumerable<IFormatter> formatters)
        {
            return formatters
                .Select(f => f.Format())
                .Aggregate(new StringBuilder(), (acc, s) => acc.Append(s))
                .ToString();
        }
    }
}
