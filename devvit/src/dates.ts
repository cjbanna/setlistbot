const datePattern = /\d{1,4}[- /.]\d{1,2}[- /.]\d{1,4}/g;

/**
 * Finds dates like 12/31/95 or 1995-12-31 in text, matching how the original
 * C# bot parsed them (.NET DateOnly.TryParse, en-US):
 * - a 3-4 digit first number means year-month-day, otherwise month/day/year
 * - two-digit years 00-49 are 20xx and 50-99 are 19xx
 */
export function parseDates(text: string): string[] {
  const dates = new Set<string>();
  for (const [match] of text.matchAll(datePattern)) {
    const [a, b, c] = match.split(/[- /.]/) as [string, string, string];
    const [y, m, d] = a.length > 2 ? [a, b, c] : [c, a, b];
    const year =
      y.length > 2 ? Number(y) : Number(y) + (Number(y) < 50 ? 2000 : 1900);
    const date = isoDate(year, Number(m), Number(d));
    if (date) dates.add(date);
  }
  return [...dates];
}

function isoDate(year: number, month: number, day: number): string | undefined {
  const daysInMonth =
    month >= 1 && month <= 12
      ? new Date(Date.UTC(year, month, 0)).getUTCDate()
      : undefined;
  if (year < 1 || !daysInMonth || day < 1 || day > daysInMonth)
    return undefined;
  const pad = (n: number, width: number) => String(n).padStart(width, '0');
  return `${pad(year, 4)}-${pad(month, 2)}-${pad(day, 2)}`;
}
