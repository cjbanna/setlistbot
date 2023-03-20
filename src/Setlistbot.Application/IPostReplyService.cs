using Setlistbot.Domain.PostAggregate;

namespace Setlistbot.Application
{
    public interface IPostReplyService
    {
        Task Reply(Post post);
    }
}
