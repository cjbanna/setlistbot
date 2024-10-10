using CSharpFunctionalExtensions;

namespace Setlistbot.Domain.PostAggregate
{
    public interface IPostRepository
    {
        Task<Maybe<Post>> Get(NonEmptyString id);

        Task<IEnumerable<Post>> GetAll();

        Task Add(Post post);

        Task Delete(Post post);
    }
}
