using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public sealed class LinksFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new SeparatedFormatter(
                new CombinedFormatter(
                    new SpaceFormatter(),
                    new CharacterFormatter('|'),
                    new SpaceFormatter()
                ),
                new IFormatter[]
                {
                    new PhishNetLinkFormatter(setlist.Date, new LiteralFormatter("phish.net")),
                    new PhishInLinkFormatter(setlist.Date, new LiteralFormatter("phish.in")),
                    new PhishTracksLinkFormatter(setlist.Date, new LiteralFormatter("phishtracks")),
                }
            ).Format();
    }
}
