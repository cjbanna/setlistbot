using System.Text.RegularExpressions;

namespace Setlistbot.Domain.Extensions
{
    public static class DateParseExtensions
    {
        public static IEnumerable<DateOnly> ParseDates(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Enumerable.Empty<DateOnly>();
            }

            var dates = new List<DateOnly>();

            var dateRegex = new Regex(@"\d{1,4}[- /.]\d{1,2}[- /.]\d{1,4}");
            foreach (Match match in dateRegex.Matches(input))
            {
                if (DateOnly.TryParse(match.Value, out var date))
                {
                    if (!dates.Contains(date))
                    {
                        dates.Add(date);
                    }
                }
            }

            return dates;
        }
    }
}
