using EnsureThat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Application.Options;
using Setlistbot.Domain;
using Setlistbot.Domain.PostAggregate;
using Setlistbot.Infrastructure.Reddit;
using Setlistbot.Infrastructure.Reddit.Options;

namespace Setlistbot.Application.Reddit
{
    public sealed class PostReplyService : IPostReplyService
    {
        private readonly ILogger<PostReplyService> _logger;
        private readonly IPostRepository _postRepository;
        private readonly IReplyBuilderFactory _replyBuilderFactory;
        private readonly IReplyBuilder _replyBuilder;
        private readonly ISetlistProviderFactory _setlistProviderFactory;
        private readonly ISetlistProvider _setlistProvider;
        private readonly IRedditService _redditService;
        private readonly RedditOptions _redditOptions;
        private readonly BotOptions _botOptions;

        public PostReplyService(
            ILogger<PostReplyService> logger,
            IPostRepository postRepository,
            IReplyBuilderFactory replyBuilderFactory,
            ISetlistProviderFactory setlistProviderFactory,
            IRedditService redditService,
            IOptions<RedditOptions> redditOptions,
            IOptions<BotOptions> botOptions
        )
        {
            _logger = Ensure.Any.IsNotNull(logger, nameof(logger));
            _postRepository = Ensure.Any.IsNotNull(postRepository, nameof(postRepository));
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

        public async Task Reply(Post post)
        {
            Ensure.Any.IsNotNull(post, nameof(post));

            var shouldReply = await ShouldReply(post);
            if (!shouldReply)
            {
                return;
            }

            _logger.LogInformation("Replying to post id: {PostId}", post.Id);

            try
            {
                var setlists = await GetSetlists(post);
                var reply = BuildReply(setlists);
                if (reply.Length > 0)
                {
                    post.SetReply(reply);

                    _logger.LogInformation("Saving post id: {PostId}", post.Id);

                    // save comment before replying to prevent repeated replies
                    await _postRepository.Add(post);

                    _logger.LogInformation("Posting reply to post id {PostId}", post.Id);

                    var posted = await _redditService.PostComment(post.ParentId, reply);
                    if (!posted)
                    {
                        await _postRepository.Delete(post);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reply to post id: {PostId}", post.Id);
            }
        }

        private async Task<bool> ShouldReply(Post post)
        {
            var mentioned =
                !_botOptions.RequireMention || post.HasMentionOf(_redditOptions.Username);

            return mentioned && post.Dates.Any() && await _postRepository.Get(post.Id) == null;
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

        private async Task<List<Setlist>> GetSetlists(Post post)
        {
            var setlists = new List<Setlist>();

            foreach (var date in post.Dates)
            {
                var setlistsForDate = await _setlistProvider.GetSetlists(date);
                setlists.AddRange(setlistsForDate);
            }

            return setlists;
        }
    }
}
