using Setlistbot.Domain.CommentAggregate;
using Setlistbot.Domain.PostAggregate;

namespace Setlistbot.Infrastructure.Reddit
{
    public interface IRedditService
    {
        Task<IEnumerable<Comment>> GetComments(string subreddit);
        Task<IEnumerable<Post>> GetPosts(string subreddit);
        Task<bool> PostComment(string parent, string text);
    }
}
