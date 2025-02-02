using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public sealed class SetlistFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new Formatters.SetlistFormatter(setlist),
                new LinksFormatter(setlist),
                new NewLineFormatter(2),
                new AttributionFormatter()
            ).Format();
    }
}
