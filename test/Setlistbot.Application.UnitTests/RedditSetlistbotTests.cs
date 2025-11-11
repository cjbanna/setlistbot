using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Setlistbot.Application.Options;
using Setlistbot.Application.Reddit;
using Setlistbot.Domain;
using Setlistbot.Domain.CommentAggregate;
using Setlistbot.Domain.PostAggregate;
using Setlistbot.Infrastructure.Reddit;
using Setlistbot.Infrastructure.Reddit.Options;
using Xunit;

namespace Setlistbot.Application.UnitTests
{
    public sealed class RedditSetlistbotTestFixture
    {
        public Mock<ILogger<RedditSetlistbot>> Logger { get; }
        public Mock<IRedditService> RedditService { get; }
        public Mock<ICommentRepository> CommentRepository { get; }
        public Mock<IPostRepository> PostRepository { get; }
        public Mock<IReplyBuilderFactory> ReplyBuilderFactory { get; }
        public Mock<ISetlistProviderFactory> SetlistProviderFactory { get; }
        public Mock<IOptions<BotOptions>> BotOptions { get; }
        public Mock<IOptions<RedditOptions>> RedditOptions { get; }
        public Mock<IReplyBuilder> ReplyBuilder { get; }
        public Mock<ISetlistProvider> SetlistProvider { get; }

        public RedditSetlistbotTestFixture()
        {
            Logger = new Mock<ILogger<RedditSetlistbot>>();
            RedditService = new Mock<IRedditService>();
            CommentRepository = new Mock<ICommentRepository>();
            PostRepository = new Mock<IPostRepository>();
            ReplyBuilderFactory = new Mock<IReplyBuilderFactory>();
            SetlistProviderFactory = new Mock<ISetlistProviderFactory>();
            BotOptions = new Mock<IOptions<BotOptions>>();
            RedditOptions = new Mock<IOptions<RedditOptions>>();
            ReplyBuilder = new Mock<IReplyBuilder>();
            SetlistProvider = new Mock<ISetlistProvider>();
        }

        public RedditSetlistbotTestFixture WithValidOptions()
        {
            var botOptions = new BotOptions
            {
                ArtistId = "phish",
                Subreddit = "phish",
                MaxSetlistCount = 3,
                RequireMention = false,
            };

            var redditOptions = new RedditOptions
            {
                Username = "testbot",
                Password = "password",
                Key = "key",
                Secret = "secret",
                CommentsLimit = 10,
            };

            BotOptions.Setup(o => o.Value).Returns(botOptions);
            RedditOptions.Setup(o => o.Value).Returns(redditOptions);

            SetlistProviderFactory.Setup(f => f.Get("phish")).Returns(SetlistProvider.Object);
            ReplyBuilderFactory.Setup(f => f.Get("phish")).Returns(ReplyBuilder.Object);

            return this;
        }

        public RedditSetlistbot CreateRedditSetlistbot()
        {
            return new RedditSetlistbot(
                Logger.Object,
                RedditService.Object,
                CommentRepository.Object,
                PostRepository.Object,
                ReplyBuilderFactory.Object,
                SetlistProviderFactory.Object,
                BotOptions.Object,
                RedditOptions.Object
            );
        }
    }

    public sealed class RedditSetlistbotTests
    {
        [Fact]
        public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RedditSetlistbot(
                    null!,
                    fixture.RedditService.Object,
                    fixture.CommentRepository.Object,
                    fixture.PostRepository.Object,
                    fixture.ReplyBuilderFactory.Object,
                    fixture.SetlistProviderFactory.Object,
                    fixture.BotOptions.Object,
                    fixture.RedditOptions.Object
                )
            );

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenRedditServiceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RedditSetlistbot(
                    fixture.Logger.Object,
                    null!,
                    fixture.CommentRepository.Object,
                    fixture.PostRepository.Object,
                    fixture.ReplyBuilderFactory.Object,
                    fixture.SetlistProviderFactory.Object,
                    fixture.BotOptions.Object,
                    fixture.RedditOptions.Object
                )
            );

            Assert.Equal("redditService", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenCommentRepositoryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RedditSetlistbot(
                    fixture.Logger.Object,
                    fixture.RedditService.Object,
                    null!,
                    fixture.PostRepository.Object,
                    fixture.ReplyBuilderFactory.Object,
                    fixture.SetlistProviderFactory.Object,
                    fixture.BotOptions.Object,
                    fixture.RedditOptions.Object
                )
            );

            Assert.Equal("commentRepository", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenPostRepositoryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RedditSetlistbot(
                    fixture.Logger.Object,
                    fixture.RedditService.Object,
                    fixture.CommentRepository.Object,
                    null!,
                    fixture.ReplyBuilderFactory.Object,
                    fixture.SetlistProviderFactory.Object,
                    fixture.BotOptions.Object,
                    fixture.RedditOptions.Object
                )
            );

            Assert.Equal("postRepository", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenReplyBuilderFactoryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RedditSetlistbot(
                    fixture.Logger.Object,
                    fixture.RedditService.Object,
                    fixture.CommentRepository.Object,
                    fixture.PostRepository.Object,
                    null!,
                    fixture.SetlistProviderFactory.Object,
                    fixture.BotOptions.Object,
                    fixture.RedditOptions.Object
                )
            );

            Assert.Equal("replyBuilderFactory", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenSetlistProviderFactoryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RedditSetlistbot(
                    fixture.Logger.Object,
                    fixture.RedditService.Object,
                    fixture.CommentRepository.Object,
                    fixture.PostRepository.Object,
                    fixture.ReplyBuilderFactory.Object,
                    null!,
                    fixture.BotOptions.Object,
                    fixture.RedditOptions.Object
                )
            );

            Assert.Equal("setlistProviderFactory", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenBotOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RedditSetlistbot(
                    fixture.Logger.Object,
                    fixture.RedditService.Object,
                    fixture.CommentRepository.Object,
                    fixture.PostRepository.Object,
                    fixture.ReplyBuilderFactory.Object,
                    fixture.SetlistProviderFactory.Object,
                    null!,
                    fixture.RedditOptions.Object
                )
            );

            Assert.Equal("botOptions", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenRedditOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new RedditSetlistbot(
                    fixture.Logger.Object,
                    fixture.RedditService.Object,
                    fixture.CommentRepository.Object,
                    fixture.PostRepository.Object,
                    fixture.ReplyBuilderFactory.Object,
                    fixture.SetlistProviderFactory.Object,
                    fixture.BotOptions.Object,
                    null!
                )
            );

            Assert.Equal("redditOptions", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenBotOptionsValueIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture();
            fixture.BotOptions.Setup(o => o.Value).Returns((BotOptions)null!);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                fixture.CreateRedditSetlistbot()
            );

            Assert.Equal("Value", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenRedditOptionsValueIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture();
            fixture.BotOptions.Setup(o => o.Value).Returns(new BotOptions { ArtistId = "phish" });
            fixture.RedditOptions.Setup(o => o.Value).Returns((RedditOptions)null!);
            fixture
                .SetlistProviderFactory.Setup(f => f.Get("phish"))
                .Returns(fixture.SetlistProvider.Object);
            fixture
                .ReplyBuilderFactory.Setup(f => f.Get("phish"))
                .Returns(fixture.ReplyBuilder.Object);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                fixture.CreateRedditSetlistbot()
            );

            Assert.Equal("Value", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenArtistIdIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture();
            var botOptions = new BotOptions { ArtistId = "" };
            var redditOptions = new RedditOptions();
            fixture.BotOptions.Setup(o => o.Value).Returns(botOptions);
            fixture.RedditOptions.Setup(o => o.Value).Returns(redditOptions);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                fixture.CreateRedditSetlistbot()
            );

            Assert.Contains("ArtistId", exception.Message);
            Assert.Contains("required", exception.Message);
        }

        [Fact]
        public void Constructor_WhenSetlistProviderNotFound_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture();
            var botOptions = new BotOptions { ArtistId = "unknown" };
            var redditOptions = new RedditOptions();

            fixture.BotOptions.Setup(o => o.Value).Returns(botOptions);
            fixture.RedditOptions.Setup(o => o.Value).Returns(redditOptions);
            fixture
                .SetlistProviderFactory.Setup(f => f.Get("unknown"))
                .Returns((ISetlistProvider)null!);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                fixture.CreateRedditSetlistbot()
            );

            Assert.Contains(
                "There is no ISetlistProvider configured for artist id: unknown",
                exception.Message
            );
        }

        [Fact]
        public void Constructor_WhenReplyBuilderNotFound_ThrowsArgumentNullException()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture();
            var botOptions = new BotOptions { ArtistId = "unknown" };
            var redditOptions = new RedditOptions();

            fixture.BotOptions.Setup(o => o.Value).Returns(botOptions);
            fixture.RedditOptions.Setup(o => o.Value).Returns(redditOptions);
            fixture
                .SetlistProviderFactory.Setup(f => f.Get("unknown"))
                .Returns(fixture.SetlistProvider.Object);
            fixture.ReplyBuilderFactory.Setup(f => f.Get("unknown")).Returns((IReplyBuilder)null!);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                fixture.CreateRedditSetlistbot()
            );

            Assert.Contains(
                "There is no IReplyBuilder registered for artist id: unknown",
                exception.Message
            );
        }

        [Fact]
        public void Constructor_WhenValidDependencies_CreatesInstanceSuccessfully()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();

            // Act
            var bot = fixture.CreateRedditSetlistbot();

            // Assert
            Assert.NotNull(bot);
        }

        [Fact]
        public async Task ReplyToMentions_WhenCalled_CallsReplyToCommentsAndReplyToPosts()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment>());
            fixture
                .RedditService.Setup(r => r.GetPosts(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Post>());

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.RedditService.Verify(r => r.GetComments(It.IsAny<Subreddit>()), Times.Once);
            fixture.RedditService.Verify(r => r.GetPosts(It.IsAny<Subreddit>()), Times.Once);
        }

        [Fact]
        public async Task ReplyToMentions_WhenExceptionThrown_LogsError()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var exception = new Exception("Test exception");
            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ThrowsAsync(exception);

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.Logger.Verify(
                l =>
                    l.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>(
                            (v, t) => v.ToString()!.Contains("Failed to check for mentions")
                        ),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                    ),
                Times.Once
            );
        }

        [Fact]
        public async Task ReplyToComments_WhenCommentHasNoMention_DoesNotReply()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            fixture
                .BotOptions.Setup(o => o.Value)
                .Returns(
                    new BotOptions
                    {
                        ArtistId = "phish",
                        RequireMention = true,
                        Subreddit = "phish",
                    }
                );

            var bot = fixture.CreateRedditSetlistbot();

            var comment = Comment.NewComment(
                "123",
                "author",
                "Some comment with dates 1995-12-31",
                "/r/phish/comment",
                "phish"
            );
            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment> { comment });

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.CommentRepository.Verify(r => r.Add(It.IsAny<Comment>()), Times.Never);
            fixture.RedditService.Verify(
                r => r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ReplyToComments_WhenCommentHasNoDates_DoesNotReply()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var comment = Comment.NewComment(
                "123",
                "author",
                "Some comment without dates",
                "/r/phish/comment",
                "phish"
            );
            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment> { comment });

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.CommentRepository.Verify(r => r.Add(It.IsAny<Comment>()), Times.Never);
            fixture.RedditService.Verify(
                r => r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ReplyToComments_WhenCommentIsByBot_DoesNotReply()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var comment = Comment.NewComment(
                "123",
                "testbot",
                "Some comment with dates 1995-12-31",
                "/r/phish/comment",
                "phish"
            );
            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment> { comment });

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.CommentRepository.Verify(r => r.Add(It.IsAny<Comment>()), Times.Never);
            fixture.RedditService.Verify(
                r => r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ReplyToComments_WhenAlreadyReplied_DoesNotReply()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var comment = Comment.NewComment(
                "123",
                "author",
                "Some comment with dates 1995-12-31",
                "/r/phish/comment",
                "phish"
            );
            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment> { comment });
            fixture
                .CommentRepository.Setup(r => r.Get(It.IsAny<NonEmptyString>()))
                .ReturnsAsync(comment);

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.CommentRepository.Verify(r => r.Add(It.IsAny<Comment>()), Times.Never);
            fixture.RedditService.Verify(
                r => r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ReplyToComments_WhenValidComment_RepliesSuccessfully()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var date = new DateOnly(1995, 12, 31);
            var location = new Location(
                Venue.From("Madison Square Garden"),
                City.From("New York"),
                State.From("NY"),
                Country.From("USA")
            );
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                date,
                location,
                "Test setlist"
            );

            var comment = Comment.NewComment(
                "123",
                "author",
                "Some comment with dates 1995-12-31",
                "/r/phish/comment",
                "phish"
            );

            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment> { comment });
            fixture
                .CommentRepository.Setup(r => r.Get(It.IsAny<NonEmptyString>()))
                .ReturnsAsync(Maybe<Comment>.None);
            fixture
                .SetlistProvider.Setup(p => p.GetSetlists(date))
                .ReturnsAsync(new List<Setlist> { setlist });
            fixture
                .ReplyBuilder.Setup(b => b.Build(It.IsAny<IEnumerable<Setlist>>()))
                .Returns("Test reply");
            fixture
                .RedditService.Setup(r =>
                    r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>())
                )
                .ReturnsAsync(Result.Success());

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.CommentRepository.Verify(r => r.Add(It.IsAny<Comment>()), Times.Once);
            fixture.RedditService.Verify(
                r => r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>()),
                Times.Once
            );
        }

        [Fact]
        public async Task ReplyToComments_WhenReplyBuildingReturnsEmptyString_DoesNotPostReply()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var date = new DateOnly(1995, 12, 31);
            var comment = Comment.NewComment(
                "123",
                "author",
                "Some comment with dates 1995-12-31",
                "/r/phish/comment",
                "phish"
            );

            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment> { comment });
            fixture
                .CommentRepository.Setup(r => r.Get(It.IsAny<NonEmptyString>()))
                .ReturnsAsync(Maybe<Comment>.None);
            fixture
                .SetlistProvider.Setup(p => p.GetSetlists(date))
                .ReturnsAsync(new List<Setlist>());
            fixture.ReplyBuilder.Setup(b => b.Build(It.IsAny<IEnumerable<Setlist>>())).Returns("");

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.CommentRepository.Verify(r => r.Add(It.IsAny<Comment>()), Times.Never);
            fixture.RedditService.Verify(
                r => r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ReplyToComments_WhenPostCommentFails_LogsWarning()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var date = new DateOnly(1995, 12, 31);
            var location = new Location(
                Venue.From("Madison Square Garden"),
                City.From("New York"),
                State.From("NY"),
                Country.From("USA")
            );
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                date,
                location,
                "Test setlist"
            );

            var comment = Comment.NewComment(
                "123",
                "author",
                "Some comment with dates 1995-12-31",
                "/r/phish/comment",
                "phish"
            );

            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment> { comment });
            fixture
                .CommentRepository.Setup(r => r.Get(It.IsAny<NonEmptyString>()))
                .ReturnsAsync(Maybe<Comment>.None);
            fixture
                .SetlistProvider.Setup(p => p.GetSetlists(date))
                .ReturnsAsync(new List<Setlist> { setlist });
            fixture
                .ReplyBuilder.Setup(b => b.Build(It.IsAny<IEnumerable<Setlist>>()))
                .Returns("Test reply");
            fixture
                .RedditService.Setup(r =>
                    r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>())
                )
                .ReturnsAsync(Result.Failure("Failed to post"));

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.CommentRepository.Verify(r => r.Add(It.IsAny<Comment>()), Times.Once);

            fixture.Logger.Verify(
                l =>
                    l.Log(
                        LogLevel.Warning,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>(
                            (v, t) => v.ToString()!.Contains("Failed to post reply to comment id")
                        ),
                        It.IsAny<Exception?>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                    ),
                Times.Once
            );
        }

        [Fact]
        public async Task ReplyToPosts_WhenValidPost_RepliesSuccessfully()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var date = new DateOnly(1995, 12, 31);
            var location = new Location(
                Venue.From("Madison Square Garden"),
                City.From("New York"),
                State.From("NY"),
                Country.From("USA")
            );
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                date,
                location,
                "Test setlist"
            );

            var post = Post.NewPost(
                "456",
                "author",
                "Post title with date 1995-12-31",
                "Post text",
                "/r/phish/post",
                "phish"
            );

            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment>());
            fixture
                .RedditService.Setup(r => r.GetPosts(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Post> { post });
            fixture
                .PostRepository.Setup(r => r.Get(It.IsAny<NonEmptyString>()))
                .ReturnsAsync(Maybe<Post>.None);
            fixture
                .SetlistProvider.Setup(p => p.GetSetlists(date))
                .ReturnsAsync(new List<Setlist> { setlist });
            fixture
                .ReplyBuilder.Setup(b => b.Build(It.IsAny<IEnumerable<Setlist>>()))
                .Returns("Test reply");
            fixture
                .RedditService.Setup(r =>
                    r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>())
                )
                .ReturnsAsync(Result.Success());

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.PostRepository.Verify(r => r.Add(It.IsAny<Post>()), Times.Once);
            fixture.RedditService.Verify(
                r => r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>()),
                Times.Once
            );
        }

        [Fact]
        public async Task ReplyToPosts_WhenPostHasNoDates_DoesNotReply()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var post = Post.NewPost(
                "456",
                "author",
                "Post title without dates",
                "Post text",
                "/r/phish/post",
                "phish"
            );

            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment>());
            fixture
                .RedditService.Setup(r => r.GetPosts(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Post> { post });

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.PostRepository.Verify(r => r.Add(It.IsAny<Post>()), Times.Never);
            fixture.RedditService.Verify(
                r => r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ReplyToPosts_WhenAlreadyReplied_DoesNotReply()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var post = Post.NewPost(
                "456",
                "author",
                "Post title with date 1995-12-31",
                "Post text",
                "/r/phish/post",
                "phish"
            );

            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment>());
            fixture
                .RedditService.Setup(r => r.GetPosts(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Post> { post });
            fixture.PostRepository.Setup(r => r.Get(It.IsAny<NonEmptyString>())).ReturnsAsync(post);

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.PostRepository.Verify(r => r.Add(It.IsAny<Post>()), Times.Never);
            fixture.RedditService.Verify(
                r => r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>()),
                Times.Never
            );
        }

        [Fact]
        public async Task BuildReply_WhenSetlistsExceedMaxCount_LimitsToMaxCount()
        {
            // Arrange
            var fixture = new RedditSetlistbotTestFixture().WithValidOptions();
            var bot = fixture.CreateRedditSetlistbot();

            var date = new DateOnly(1995, 12, 31);
            var location = new Location(
                Venue.From("Madison Square Garden"),
                City.From("New York"),
                State.From("NY"),
                Country.From("USA")
            );

            var setlists = new List<Setlist>();
            for (var i = 0; i < 5; i++)
            {
                setlists.Add(
                    Setlist.NewSetlist(
                        ArtistId.From("phish"),
                        ArtistName.From("Phish"),
                        date.AddDays(i),
                        location,
                        $"Test setlist {i}"
                    )
                );
            }

            var comment = Comment.NewComment(
                "123",
                "author",
                "Comment with multiple dates 1995-12-31, 1996-01-01, 1996-01-02, 1996-01-03, 1996-01-04",
                "/r/phish/comment",
                "phish"
            );

            fixture
                .RedditService.Setup(r => r.GetComments(It.IsAny<Subreddit>()))
                .ReturnsAsync(new List<Comment> { comment });
            fixture
                .CommentRepository.Setup(r => r.Get(It.IsAny<NonEmptyString>()))
                .ReturnsAsync(Maybe<Comment>.None);
            fixture
                .SetlistProvider.Setup(p => p.GetSetlists(It.IsAny<DateOnly>()))
                .ReturnsAsync(setlists);
            fixture
                .ReplyBuilder.Setup(b => b.Build(It.Is<IEnumerable<Setlist>>(s => s.Count() == 3)))
                .Returns("Limited reply");
            fixture
                .RedditService.Setup(r =>
                    r.PostComment(It.IsAny<NonEmptyString>(), It.IsAny<NonEmptyString>())
                )
                .ReturnsAsync(Result.Success());

            // Act
            await bot.ReplyToMentions();

            // Assert
            fixture.ReplyBuilder.Verify(
                b => b.Build(It.Is<IEnumerable<Setlist>>(s => s.Count() == 3)),
                Times.AtLeastOnce
            );
        }
    }
}
