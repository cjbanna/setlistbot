using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Application.Options;
using Setlistbot.Infrastructure.Reddit;

namespace Setlistbot.Application.Reddit
{
    public class RedditSetlistbot : IRedditSetlistbot
    {
        private readonly ILogger<RedditSetlistbot> _logger;
        private readonly IRedditService _redditService;
        private readonly ICommentReplyService _commentReplyService;
        private readonly IPostReplyService _postReplyService;
        private readonly BotOptions _botOptions;

        public RedditSetlistbot(
            ILogger<RedditSetlistbot> logger,
            IRedditService redditService,
            ICommentReplyService commentService,
            IPostReplyService postService,
            IOptions<BotOptions> botOptions
        )
        {
            _logger = Ensure.Any.IsNotNull(logger, nameof(logger));
            _redditService = Ensure.Any.IsNotNull(redditService, nameof(redditService));
            _commentReplyService = Ensure.Any.IsNotNull(commentService, nameof(commentService));
            _postReplyService = Ensure.Any.IsNotNull(postService, nameof(postService));
            Ensure.That(botOptions, nameof(botOptions)).IsNotNull();
            _botOptions = Ensure.Any.IsNotNull(botOptions.Value, nameof(botOptions.Value));
        }

        public async Task ReplyToMentions()
        {
            _logger.LogInformation("Checking for mentions");

            try
            {
                await ReplyToComments();
                await ReplyToPosts();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check for mentions");
            }

            _logger.LogInformation("Finished checking for mentions");
        }

        private async Task ReplyToComments()
        {
            var comments = await _redditService.GetComments(_botOptions.Subreddit);

            foreach (var comment in comments)
            {
                await _commentReplyService.Reply(comment);
            }
        }

        private async Task ReplyToPosts()
        {
            var posts = await _redditService.GetPosts(_botOptions.Subreddit);

            foreach (var post in posts)
            {
                await _postReplyService.Reply(post);
            }
        }
    }
}
