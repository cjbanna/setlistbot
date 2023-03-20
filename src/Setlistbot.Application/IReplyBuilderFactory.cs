using Setlistbot.Domain;

namespace Setlistbot.Application
{
    public interface IReplyBuilderFactory
    {
        IReplyBuilder Get(string artistId);
    }
}
