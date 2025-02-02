namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a DateTime as a year-month-day string.
    /// </summary>
    public sealed class YearMonthDayFormatter(DateOnly date) : IFormatter
    {
        public string Format() => date.ToString("yyyy-MM-dd");
    }
}
