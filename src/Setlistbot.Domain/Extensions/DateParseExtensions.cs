using System.Text.RegularExpressions;

namespace Setlistbot.Domain.Extensions
{
    public static partial class DateParseExtensions
    {
        public static IEnumerable<DateOnly> ParseDates(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return [];
            }

            var dates = new List<DateOnly>();

            var dateRegex = DatesRegex();
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

        [GeneratedRegex(@"\d{1,4}[- /.]\d{1,2}[- /.]\d{1,4}")]
        private static partial Regex DatesRegex();
    }
}
