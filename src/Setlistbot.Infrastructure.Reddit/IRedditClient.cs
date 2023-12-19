using Setlistbot.Infrastructure.Reddit.Models;

namespace Setlistbot.Infrastructure.Reddit
{
    public interface IRedditClient
    {
        Task<string?> GetAuthToken(string username, string password, string key, string secret);
        Task<SubredditCommentsResponse?> GetComments(
            string token,
            string subreddit,
            int? limit = default
        );
        Task<PostCommentResponse?> PostComment(string token, string parent, string text);
        Task<SubredditPostsResponse?> GetPosts(string token, string subreddit);
    }
}
