namespace Setlistbot.Domain.PostAggregate
{
    public interface IPostRepository
    {
        Task<Post?> Get(string id);

        Task<IEnumerable<Post>> GetAll();

        Task Add(Post post);

        Task Delete(Post post);
    }
}
