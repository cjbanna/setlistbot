using EnsureThat;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Setlistbot.Infrastructure.Reddit.Models;

namespace Setlistbot.Infrastructure.Reddit
{
    public sealed class RedditClient : IRedditClient
    {
        private readonly ILogger<RedditClient> _logger;

        public RedditClient(ILogger<RedditClient> logger)
        {
            _logger = Ensure.Any.IsNotNull(logger, nameof(logger));
        }

        /// <summary>
        /// Gets a reddit auth token
        /// </summary>
        /// <returns>An auth token if successful</returns>
        public async Task<string?> GetAuthToken(
            string username,
            string password,
            string key,
            string secret
        )
        {
            Ensure.That(username, nameof(username)).IsNotNullOrWhiteSpace();
            Ensure.That(password, nameof(password)).IsNotNullOrWhiteSpace();
            Ensure.That(key, nameof(key)).IsNotNullOrWhiteSpace();
            Ensure.That(secret, nameof(secret)).IsNotNullOrWhiteSpace();

            var token = default(string);

            try
            {
                var request = new
                {
                    grant_type = "password",
                    username,
                    password
                };

                var response = await "https://www.reddit.com/api/v1/access_token"
                    .WithHeader("User-Agent", "setlistbot")
                    .WithBasicAuth(key, secret)
                    .PostUrlEncodedAsync(request)
                    .ReceiveJson<AuthTokenResponse>();

                token = response?.AccessToken;
            }
            catch (FlurlHttpException ex)
            {
                var content = await ex.GetResponseStringAsync();
                _logger.LogError(ex, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get auth token");
            }

            return token;
        }

        /// <summary>
        /// Gets the last 25 comments for a subreddit
        /// </summary>
        /// <param name="subreddit">The name of the subreddit</param>
        /// <param name="limit">The number of comments to take</param>
        /// <returns>The deserialized response from the API if successful</returns>
        public async Task<SubredditCommentsResponse?> GetComments(
            string subreddit,
            int? limit = default
        )
        {
            Ensure.That(subreddit, nameof(subreddit)).IsNotNullOrWhiteSpace();

            if (limit.HasValue)
            {
                Ensure.That(limit.Value, nameof(limit)).IsGt(0);
            }

            var response = default(SubredditCommentsResponse);

            try
            {
                // 25 is the default limit if no 'limit' query parameter is supplied
                var url = $"https://www.reddit.com/r/{subreddit}/comments.json";

                if (limit.HasValue)
                {
                    url += $"?limit={limit}";
                }

                response = await url.WithHeader("User-Agent", "setlistbot")
                    .GetJsonAsync<SubredditCommentsResponse>();
            }
            catch (FlurlHttpException ex)
            {
                var content = await ex.GetResponseStringAsync();
                _logger.LogError(ex, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get comments for subreddit [{subreddit}]");
            }

            return response;
        }

        /// <summary>
        /// Gets the last 25 posts for a subreddit
        /// </summary>
        /// <param name="token"></param>
        /// <param name="subreddit"></param>
        /// <returns></returns>
        public async Task<SubredditPostsResponse?> GetPosts(string token, string subreddit)
        {
            Ensure.That(subreddit, nameof(subreddit)).IsNotNullOrWhiteSpace();

            var response = default(SubredditPostsResponse);

            try
            {
                var url = $"https://oauth.reddit.com/r/{subreddit}/new";

                response = await url.WithHeader("User-Agent", "setlistbot")
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<SubredditPostsResponse>();
            }
            catch (FlurlHttpException ex)
            {
                var content = await ex.GetResponseStringAsync();
                _logger.LogError(ex, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get posts for subreddit [{subreddit}]");
            }

            return response;
        }

        /// <summary>
        /// Posts a comment
        /// </summary>
        /// <param name="token">A valid auth token</param>
        /// <param name="parent">The comment's parent</param>
        /// <param name="text">The comment text, markdown supported</param>
        /// <returns></returns>
        public async Task<PostCommentResponse?> PostComment(
            string token,
            string parent,
            string text
        )
        {
            var response = default(PostCommentResponse);

            try
            {
                var data = new
                {
                    api_type = "json",
                    thing_id = parent,
                    text
                };

                var flurlResponse = await "https://oauth.reddit.com/api/comment"
                    .WithHeader("User-Agent", "setlistbot")
                    .WithOAuthBearerToken(token)
                    .PostUrlEncodedAsync(data);

                response = await flurlResponse.GetJsonAsync<PostCommentResponse>();
            }
            catch (FlurlHttpException ex)
            {
                var content = await ex.GetResponseStringAsync();
                _logger.LogError(ex, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to post comment [{parent}] [{text}]");
            }

            return response;
        }
    }
}
