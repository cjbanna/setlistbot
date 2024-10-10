using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Domain;
using Setlistbot.Domain.CommentAggregate;
using Setlistbot.Domain.PostAggregate;
using Setlistbot.Infrastructure.Reddit.Options;

namespace Setlistbot.Infrastructure.Reddit
{
    public sealed class RedditService : IRedditService
    {
        private readonly IRedditClient _client;
        private readonly ILogger<RedditService> _logger;
        private readonly RedditOptions _redditOptions;

        public RedditService(
            IRedditClient client,
            ILogger<RedditService> logger,
            IOptions<RedditOptions> redditOptions
        )
        {
            _client = client;
            _logger = logger;
            _redditOptions = redditOptions.Value;
        }

        public async Task<IEnumerable<Comment>> GetComments(Subreddit subreddit)
        {
            try
            {
                return await GetAuthToken()
                    .Map(token => GetComments(subreddit, token))
                    .GetValueOrDefault(() => []);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get comments");
                return [];
            }
        }

        private async Task<IEnumerable<Comment>> GetComments(Subreddit subreddit, RedditToken token)
        {
            return await _client
                .GetComments(token, subreddit, new PositiveInt(_redditOptions.CommentsLimit))
                .Map(response =>
                    response.Data.Children.Select(c =>
                        Comment.NewComment(
                            c.Data.Id,
                            c.Data.Author,
                            c.Data.Body,
                            c.Data.Permalink,
                            subreddit
                        )
                    )
                )
                .GetValueOrDefault(() => []);
        }

        public async Task<IEnumerable<Post>> GetPosts(Subreddit subreddit)
        {
            try
            {
                return await GetAuthToken()
                    .Map(token => GetPosts(subreddit, token))
                    .GetValueOrDefault(() => []);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get posts");
                return [];
            }
        }

        private async Task<IEnumerable<Post>> GetPosts(Subreddit subreddit, RedditToken token)
        {
            return await _client
                .GetPosts(token, subreddit)
                .Map(response =>
                    response.Data.Children.Select(c =>
                        Post.NewPost(
                            c.Data.Id,
                            c.Data.Author,
                            c.Data.Title,
                            c.Data.SelfText,
                            c.Data.Permalink,
                            subreddit
                        )
                    )
                )
                .GetValueOrDefault(() => []);
        }

        public async Task<bool> PostComment(NonEmptyString parent, NonEmptyString text)
        {
            try
            {
                return await GetAuthToken()
                    .Map(token => PostComment(parent, text, token))
                    .GetValueOrDefault(() => false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to post comment");
            }

            return false;
        }

        private async Task<bool> PostComment(
            NonEmptyString parent,
            NonEmptyString text,
            RedditToken token
        ) =>
            await _client
                .PostComment(token, parent, text)
                .Map(response => response is not null)
                .GetValueOrDefault(() => false);

        private async Task<Maybe<RedditToken>> GetAuthToken() =>
            await _client.GetAuthToken(
                _redditOptions.Username,
                _redditOptions.Password,
                _redditOptions.Key,
                _redditOptions.Secret
            );
    }
}
