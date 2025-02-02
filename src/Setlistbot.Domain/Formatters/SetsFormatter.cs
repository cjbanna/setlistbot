namespace Setlistbot.Domain.Formatters
{
    public sealed class SetsFormatter(IEnumerable<Set> sets) : IFormatter
    {
        public string Format() =>
            sets.Select<Set, IFormatter>(s => new CombinedFormatter(
                    new SetFormatter(s),
                    new NewLineFormatter(2)
                ))
                .ToArray()
                .Format();
    }
}
