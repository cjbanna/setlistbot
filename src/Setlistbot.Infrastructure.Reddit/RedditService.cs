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
        private readonly IOptions<RedditOptions> _redditOptions;

        public RedditService(
            ILogger<RedditService> logger,
            IRedditClient client,
            IOptions<RedditOptions> redditOptions
        )
        {
            _client = client;
            _logger = logger;
            _redditOptions = redditOptions;
        }

        public async Task<IEnumerable<Comment>> GetComments(Subreddit subreddit) =>
            await GetAuthToken()
                .Map(token => GetComments(subreddit, token))
                .GetValueOrDefault(() => []);

        private async Task<IEnumerable<Comment>> GetComments(
            Subreddit subreddit,
            RedditToken token
        ) =>
            await _client
                .GetComments(token, subreddit, new PositiveInt(_redditOptions.Value.CommentsLimit))
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

        public async Task<IEnumerable<Post>> GetPosts(Subreddit subreddit) =>
            await GetAuthToken()
                .Map(token => GetPosts(subreddit, token))
                .GetValueOrDefault(() => []);

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

        public async Task<Result> PostComment(NonEmptyString parent, NonEmptyString text)
        {
            return await GetAuthToken()
                .Map(async token => await _client.PostComment(token, parent, text))
                .OnSuccessTry(_ => Result.Success());
        }

        private async Task<Result<RedditToken>> GetAuthToken() =>
            await _client.GetAuthToken(
                _redditOptions.Value.Username,
                _redditOptions.Value.Password,
                _redditOptions.Value.Key,
                _redditOptions.Value.Secret
            );
    }
}
