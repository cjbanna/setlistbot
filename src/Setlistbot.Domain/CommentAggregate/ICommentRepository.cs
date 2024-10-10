using CSharpFunctionalExtensions;

namespace Setlistbot.Domain.CommentAggregate
{
    public interface ICommentRepository
    {
        Task<Maybe<Comment>> Get(NonEmptyString id);
        Task<IEnumerable<Comment>> GetAll();
        Task Add(Comment comment);
        Task Delete(Comment comment);
    }
}
