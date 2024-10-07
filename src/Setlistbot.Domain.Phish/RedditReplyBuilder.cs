using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Phish
{
    public class RedditReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "phish";

        public string Build(IEnumerable<Setlist> setlists)
        {
            var enumerable = setlists as Setlist[] ?? setlists.ToArray();
            IFormatter formatter = enumerable switch
            {
                [] => new EmptyFormatter(),
                { Length: 1 } => new PhishSetlistFormatter(enumerable[0]),
                [..] => new PhishMultipleSetlistFormatter(enumerable),
            };
            return formatter.Format();
        }
    }
}
