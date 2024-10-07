using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Kglw
{
    public class RedditReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "kglw";

        public string Build(IEnumerable<Setlist> setlists)
        {
            var enumerable = setlists as Setlist[] ?? setlists.ToArray();
            IFormatter formatter = enumerable switch
            {
                [] => new EmptyFormatter(),
                { Length: 1 } => new KglwSetlistFormatter(enumerable[0]),
                [..] => new KglwMultipleSetlistFormatter(enumerable),
            };
            return formatter.Format();
        }
    }
}
