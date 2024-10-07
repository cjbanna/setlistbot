namespace Setlistbot.Domain.Formatters
{
    public sealed class SetlistFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new MarkdownHeaderFormatter(new SetlistHeaderFormatter(setlist)),
                new CombinedFormatter(
                    setlist.Sets.Select<Set, IFormatter>(s => new SetFormatter(s)).ToArray()
                )
            ).Format();
    }
}
