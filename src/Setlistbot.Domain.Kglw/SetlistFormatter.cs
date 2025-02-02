using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Kglw
{
    public sealed class SetlistFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new CombinedFormatter(
                new Formatters.SetlistFormatter(setlist),
                new AttributionFormatter(setlist)
            ).Format();
    }
}
