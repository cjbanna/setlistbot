using System.Net;
using CSharpFunctionalExtensions;
using Setlistbot.Domain;
using Setlistbot.Infrastructure.Reddit.Models;

namespace Setlistbot.Infrastructure.Reddit
{
    public interface IRedditClient
    {
        Task<Result<RedditToken>> GetAuthToken(
            NonEmptyString username,
            NonEmptyString password,
            NonEmptyString key,
            NonEmptyString secret
        );
        Task<Result<SubredditCommentsResponse>> GetComments(
            RedditToken token,
            Subreddit subreddit,
            Maybe<PositiveInt> limit = default
        );
        Task<Result<PostCommentResponse, HttpStatusCode>> PostComment(
            RedditToken token,
            NonEmptyString parent,
            NonEmptyString text
        );
        Task<Result<SubredditPostsResponse>> GetPosts(RedditToken token, Subreddit subreddit);
    }
}
