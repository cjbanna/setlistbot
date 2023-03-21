namespace Setlistbot.Domain.CommentAggregate
{
    public interface ICommentRepository
    {
        Task<Comment?> Get(string id);
        Task<IEnumerable<Comment>> GetAll();
        Task Add(Comment comment);
        Task Delete(Comment comment);
    }
}
