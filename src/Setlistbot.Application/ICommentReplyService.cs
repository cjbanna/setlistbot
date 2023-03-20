using Setlistbot.Domain.CommentAggregate;

namespace Setlistbot.Application
{
    public interface ICommentReplyService
    {
        Task Reply(Comment comment);
    }
}
