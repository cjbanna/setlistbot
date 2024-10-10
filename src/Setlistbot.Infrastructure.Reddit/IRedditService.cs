using Setlistbot.Domain;
using Setlistbot.Domain.CommentAggregate;
using Setlistbot.Domain.PostAggregate;

namespace Setlistbot.Infrastructure.Reddit
{
    public interface IRedditService
    {
        Task<IEnumerable<Comment>> GetComments(Subreddit subreddit);
        Task<IEnumerable<Post>> GetPosts(Subreddit subreddit);
        Task<bool> PostComment(NonEmptyString parent, NonEmptyString text);
    }
}
