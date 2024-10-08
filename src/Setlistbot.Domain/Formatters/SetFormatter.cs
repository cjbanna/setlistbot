namespace Setlistbot.Domain.Formatters
{
    public sealed class SetFormatter(Set set) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new MarkdownBoldFormatter(
                    new CombinedFormatter(
                        new LiteralFormatter(set.Name),
                        new CharacterFormatter(':')
                    )
                ),
                new SpaceFormatter(),
                new SongsFormatter(set.Songs)
            ).Format();
    }

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
