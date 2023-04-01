using Setlistbot.Domain.PostAggregate;

namespace Setlistbot.Application.Reddit
{
    public interface IPostReplyService
    {
        Task Reply(Post post);
    }
}
