using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Application.Options;
using Setlistbot.Domain;
using Setlistbot.Domain.CommentAggregate;
using Setlistbot.Infrastructure.Reddit;
using Setlistbot.Infrastructure.Reddit.Options;

namespace Setlistbot.Application
{
    public class CommentReplyService : ICommentReplyService
    {
        private readonly ILogger<CommentReplyService> _logger;
        private readonly ICommentRepository _commentRepository;
        private readonly IReplyBuilderFactory _replyBuilderFactory;
        private readonly IReplyBuilder _replyBuilder;
        private readonly ISetlistProviderFactory _setlistProviderFactory;
        private readonly ISetlistProvider _setlistProvider;
        private readonly IRedditService _redditService;
        private readonly RedditOptions _redditOptions;
        private readonly BotOptions _botOptions;

        public CommentReplyService(
            ILogger<CommentReplyService> logger,
            ICommentRepository commentRepository,
            IReplyBuilderFactory replyBuilderFactory,
            ISetlistProviderFactory setlistProviderFactory,
            IRedditService redditService,
            IOptions<RedditOptions> redditOptions,
            IOptions<BotOptions> botOptions
        )
        {
            _logger = Ensure.Any.IsNotNull(logger, nameof(logger));
            _commentRepository = Ensure.Any.IsNotNull(commentRepository, nameof(commentRepository));
            _replyBuilderFactory = Ensure.Any.IsNotNull(
                replyBuilderFactory,
                nameof(replyBuilderFactory)
            );
            _setlistProviderFactory = Ensure.Any.IsNotNull(
                setlistProviderFactory,
                nameof(setlistProviderFactory)
            );
            _redditService = Ensure.Any.IsNotNull(redditService, nameof(redditService));
            Ensure.That(redditOptions, nameof(redditOptions)).IsNotNull();
            _redditOptions = Ensure.Any.IsNotNull(redditOptions.Value, nameof(redditOptions.Value));
            Ensure.That(botOptions, nameof(botOptions)).IsNotNull();
            _botOptions = Ensure.Any.IsNotNull(botOptions.Value, nameof(botOptions.Value));

            Ensure
                .That(
                    _botOptions.ArtistId,
                    nameof(_botOptions.ArtistId),
                    options => options.WithMessage($"BotOptions:ArtistId is required")
                )
                .IsNotNullOrWhiteSpace();

            var setlistProvider = _setlistProviderFactory.Get(_botOptions.ArtistId);

            _setlistProvider = Ensure.Any.IsNotNull(
                setlistProvider,
                nameof(setlistProvider),
                options =>
                    options.WithMessage(
                        $"There is no {nameof(ISetlistProvider)} configured for artist id: {_botOptions.ArtistId}"
                    )
            );

            var replyBuilder = _replyBuilderFactory.Get(_botOptions.ArtistId);

            _replyBuilder = Ensure.Any.IsNotNull(
                replyBuilder,
                nameof(replyBuilder),
                options =>
                    options.WithMessage(
                        $"There is no {nameof(IReplyBuilder)} registered for artist id: {_botOptions.ArtistId}"
                    )
            );
        }

        public async Task Reply(Comment comment)
        {
            Ensure.Any.IsNotNull(comment, nameof(comment));

            var shouldReply = await ShouldReply(comment);
            if (!shouldReply)
            {
                return;
            }

            _logger.LogInformation("Replying to comment id: {CommentId}", comment.Id);

            try
            {
                var setlists = await GetSetlists(comment);
                var reply = BuildReply(setlists);
                if (reply.Length > 0)
                {
                    comment.SetReply(reply);

                    _logger.LogInformation("Saving comment id: {CommentId}", comment.Id);

                    // save comment before replying to prevent repeated replies
                    await _commentRepository.Add(comment);

                    _logger.LogInformation("Posting reply to comment id: {CommentId}", comment.Id);

                    var parent = comment.ParentId;
                    var posted = await _redditService.PostComment(parent, reply);
                    if (!posted)
                    {
                        await _commentRepository.Delete(comment);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reply to comment id: {CommentId}", comment.Id);
            }
        }

        private async Task<bool> ShouldReply(Comment comment)
        {
            var mentioned =
                !_botOptions.RequireMention || comment.HasMentionOf(_redditOptions.Username);

            var hasDates = comment.Dates.Any();

            var isBotReply = string.Equals(
                comment.Author,
                _redditOptions.Username,
                StringComparison.CurrentCultureIgnoreCase
            );

            var reply = await _commentRepository.Get(comment.Id);
            var alreadyReplied = reply != null;

            return mentioned && hasDates && !isBotReply && !alreadyReplied;
        }

        private string BuildReply(List<Setlist> setlists)
        {
            var reply = string.Empty;

            if (setlists.Count > 1)
            {
                reply = _replyBuilder.Build(setlists, _botOptions.MaxSetlistCount);
            }
            else if (setlists.Count == 1)
            {
                reply = _replyBuilder.Build(setlists.First());
            }

            return reply;
        }

        private async Task<List<Setlist>> GetSetlists(Comment comment)
        {
            var setlists = new List<Setlist>();
            foreach (var date in comment.Dates)
            {
                var setlistsForDate = await _setlistProvider.GetSetlists(date);
                setlists.AddRange(setlistsForDate);
            }

            return setlists;
        }
    }
}
