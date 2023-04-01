using Setlistbot.Domain.CommentAggregate;

namespace Setlistbot.Application.Reddit
{
    public interface ICommentReplyService
    {
        Task Reply(Comment comment);
    }
}
