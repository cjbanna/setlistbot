using CSharpFunctionalExtensions;
using Setlistbot.Domain;
using Setlistbot.Infrastructure.Reddit.Models;

namespace Setlistbot.Infrastructure.Reddit
{
    public interface IRedditClient
    {
        Task<Maybe<RedditToken>> GetAuthToken(
            NonEmptyString username,
            NonEmptyString password,
            NonEmptyString key,
            NonEmptyString secret
        );
        Task<Maybe<SubredditCommentsResponse>> GetComments(
            RedditToken token,
            Subreddit subreddit,
            Maybe<PositiveInt> limit = default
        );
        Task<Maybe<PostCommentResponse>> PostComment(
            RedditToken token,
            NonEmptyString parent,
            NonEmptyString text
        );
        Task<Maybe<SubredditPostsResponse>> GetPosts(RedditToken token, Subreddit subreddit);
    }
}
