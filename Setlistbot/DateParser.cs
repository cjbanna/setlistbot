using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Setlistbot
{
    public class DateParser
    {
        public List<DateTime> ParseDates(string input)
        {
            List<DateTime> dates = new List<DateTime>();

            Regex dateRegex = new Regex(@"\d{1,4}[- /.]\d{1,2}[- /.]\d{1,4}");

            DateTime date = default(DateTime);
            foreach (Match match in dateRegex.Matches(input))
            {
                if (DateTime.TryParse(match.Value, out date))
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
