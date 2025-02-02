using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.GratefulDead
{
    public sealed class ArchiveOrgLinkFormatter(Setlist setlist) : IFormatter
    {
        public string Format() =>
            new MarkdownLinkFormatter(
                new Uri(
                    $"https://archive.org/details/GratefulDead?query=date:{setlist.Date:yyyy-MM-dd}"
                ),
                new LiteralFormatter("archive.org")
            ).Format();
    }
}
