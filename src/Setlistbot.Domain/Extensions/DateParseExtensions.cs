using System.Text.RegularExpressions;

namespace Setlistbot.Domain.Extensions
{
    public static class DateParseExtensions
    {
        public static IEnumerable<DateTime> ParseDates(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Enumerable.Empty<DateTime>();
            }

            var dates = new List<DateTime>();

            var dateRegex = new Regex(@"\d{1,4}[- /.]\d{1,2}[- /.]\d{1,4}");
            foreach (Match match in dateRegex.Matches(input))
            {
                if (DateTime.TryParse(match.Value, out var date))
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
