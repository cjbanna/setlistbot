namespace Setlistbot.Domain.Formatters
{
    public sealed class SetFormatter(Set set) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new MarkdownBoldFormatter(
                    new CombinedFormatter(
                        new LiteralFormatter(set.Name.Value),
                        new CharacterFormatter(':')
                    )
                ),
                new SpaceFormatter(),
                new SongsFormatter(set.Songs)
            ).Format();
    }
}
