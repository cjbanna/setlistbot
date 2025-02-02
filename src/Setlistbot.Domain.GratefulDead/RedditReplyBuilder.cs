using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.GratefulDead
{
    public sealed class RedditReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "gd";

        public string Build(IEnumerable<Setlist> setlists)
        {
            var enumerable = setlists as Setlist[] ?? setlists.ToArray();
            IFormatter formatter = enumerable switch
            {
                { Length: 1 } => new SetlistFormatter(enumerable[0]),
                [..] => new SetlistsFormatter(enumerable),
                _ => new EmptyFormatter(),
            };
            return formatter.Format();
        }
    }
}
