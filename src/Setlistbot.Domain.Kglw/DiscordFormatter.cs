using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Kglw
{
    public sealed class DiscordFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new MarkdownBoldFormatter(new LiteralFormatter(setlist.ArtistName.Value)),
                new NewLineFormatter(2),
                new SetlistHeaderFormatter(setlist),
                new NewLineFormatter(2),
                new SetsFormatter(setlist.Sets),
                new NewLineFormatter(2),
                new AttributionFormatter(setlist)
            ).Format();
    }
}
