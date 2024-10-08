namespace Setlistbot.Domain.Formatters
{
    public sealed class SetlistFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new MarkdownHeaderFormatter(new SetlistHeaderFormatter(setlist)),
                new NewLineFormatter(2),
                new SetsFormatter(setlist.Sets)
            ).Format();
    }
}
