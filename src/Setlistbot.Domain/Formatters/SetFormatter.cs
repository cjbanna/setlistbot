namespace Setlistbot.Domain.Formatters
{
    public sealed class SetFormatter(Set set) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new MarkdownBoldFormatter(new LiteralFormatter(set.Name)),
                new CombinedFormatter(
                    set.Songs.Select<Song, IFormatter>(s => new SongFormatter(s)).ToArray()
                ),
                new NewLineFormatter(2)
            ).Format();
    }
}
