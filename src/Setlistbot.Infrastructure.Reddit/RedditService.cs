using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            _client = Ensure.Any.IsNotNull(client, nameof(client));
            _logger = Ensure.Any.IsNotNull(logger, nameof(logger));
            Ensure.That(redditOptions, nameof(redditOptions)).IsNotNull();
            _redditOptions = Ensure.Any.IsNotNull(redditOptions.Value, nameof(redditOptions.Value));
        }

        public async Task<IEnumerable<Comment>> GetComments(string subreddit)
        {
            Ensure.That(subreddit, nameof(subreddit)).IsNotNullOrWhiteSpace();

            try
            {
                var token = await GetAuthToken();
                if (token == null)
                {
                    return Enumerable.Empty<Comment>();
                }

                var response = await _client.GetComments(
                    token,
                    subreddit,
                    _redditOptions.CommentsLimit
                );
                if (response != null)
                {
                    return response.Data.Children
                        .Where(c => c != null)
                        .Select(
                            c =>
                                Comment.NewComment(
                                    c.Data.Id,
                                    c.Data.Author,
                                    c.Data.Body,
                                    c.Data.Permalink,
                                    subreddit
                                )
                        );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get comments");
            }

            return Enumerable.Empty<Comment>();
        }

        public async Task<IEnumerable<Post>> GetPosts(string subreddit)
        {
            Ensure.That(subreddit, nameof(subreddit)).IsNotNullOrWhiteSpace();

            try
            {
                var token = await GetAuthToken();
                if (token == null)
                {
                    return Enumerable.Empty<Post>();
                }

                var response = await _client.GetPosts(token, subreddit);
                if (response != null)
                {
                    return response.Data.Children
                        .Where(c => c != null)
                        .Select(
                            c =>
                                Post.NewPost(
                                    c.Data.Id,
                                    c.Data.Author,
                                    c.Data.Title,
                                    c.Data.SelfText,
                                    c.Data.Permalink,
                                    subreddit
                                )
                        );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get posts");
            }

            return Enumerable.Empty<Post>();
        }

        public async Task<bool> PostComment(string parent, string text)
        {
            Ensure.That(parent, nameof(parent)).IsNotNullOrWhiteSpace();
            Ensure.That(text, nameof(text)).IsNotNullOrWhiteSpace();

            try
            {
                var token = await GetAuthToken();
                if (token == null)
                {
                    return false;
                }

                var response = await _client.PostComment(token, parent, text);
                return response != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to post comment");
            }

            return false;
        }

        private async Task<string?> GetAuthToken()
        {
            return await _client.GetAuthToken(
                _redditOptions.Username,
                _redditOptions.Password,
                _redditOptions.Key,
                _redditOptions.Secret
            );
        }
    }
}
