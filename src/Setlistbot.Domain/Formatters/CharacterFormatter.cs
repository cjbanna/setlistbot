namespace Setlistbot.Domain.Formatters
{
    /// <summary>
    /// Formats a character as-is.
    /// </summary>
    public sealed class CharacterFormatter(char c) : IFormatter
    {
        public string Format() => c.ToString();
    }
}
