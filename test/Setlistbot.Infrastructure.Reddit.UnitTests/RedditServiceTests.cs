using System.Net;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Setlistbot.Domain;
using Setlistbot.Infrastructure.Reddit.Models;
using Setlistbot.Infrastructure.Reddit.Options;
using Xunit;

namespace Setlistbot.Infrastructure.Reddit.UnitTests
{
    public sealed class RedditServiceTestFixture
    {
        public Mock<ILogger<RedditService>> Logger { get; }
        public Mock<IRedditClient> Client { get; }
        public Mock<IOptions<RedditOptions>> Options { get; }
        public RedditService Service { get; }

        public RedditServiceTestFixture()
        {
            Logger = new Mock<ILogger<RedditService>>();
            Client = new Mock<IRedditClient>();
            Options = new Mock<IOptions<RedditOptions>>();
            Service = new RedditService(Logger.Object, Client.Object, Options.Object);
        }

        public RedditServiceTestFixture GivenValidOptions()
        {
            Options
                .Setup(o => o.Value)
                .Returns(
                    new RedditOptions
                    {
                        CommentsLimit = 10,
                        Username = "username",
                        Password = "password",
                        Secret = "secret",
                        Key = "key",
                    }
                );

            return this;
        }

        public RedditServiceTestFixture GivenAuthTokenSuccess()
        {
            Client
                .Setup(c =>
                    c.GetAuthToken(
                        It.IsAny<NonEmptyString>(),
                        It.IsAny<NonEmptyString>(),
                        It.IsAny<NonEmptyString>(),
                        It.IsAny<NonEmptyString>()
                    )
                )
                .ReturnsAsync(Result.Success(RedditToken.From("token")));
            return this;
        }

        public RedditServiceTestFixture GivenAuthTokenFailure()
        {
            Client
                .Setup(c =>
                    c.GetAuthToken(
                        It.IsAny<NonEmptyString>(),
                        It.IsAny<NonEmptyString>(),
                        It.IsAny<NonEmptyString>(),
                        It.IsAny<NonEmptyString>()
                    )
                )
                .ReturnsAsync(Result.Failure<RedditToken>("error"));
            return this;
        }

        public RedditServiceTestFixture GivenClientPostCommentSuccess()
        {
            Client
                .Setup(c =>
                    c.PostComment(
                        It.IsAny<RedditToken>(),
                        It.IsAny<NonEmptyString>(),
                        It.IsAny<NonEmptyString>()
                    )
                )
                .ReturnsAsync(
                    Result.Success<PostCommentResponse, HttpStatusCode>(new PostCommentResponse())
                );
            return this;
        }

        public RedditServiceTestFixture GivenClientGetCommentsSuccess()
        {
            Client
                .Setup(c =>
                    c.GetComments(
                        It.IsAny<RedditToken>(),
                        It.IsAny<Subreddit>(),
                        It.IsAny<Maybe<PositiveInt>>()
                    )
                )
                .ReturnsAsync(
                    Result.Success(
                        new SubredditCommentsResponse
                        {
                            Data = new SubredditCommentsResponseData
                            {
                                Children = new[]
                                {
                                    new SubredditComment()
                                    {
                                        Data = new SubredditCommentData
                                        {
                                            Id = "id",
                                            Author = "author",
                                            Body = "body",
                                            Permalink = "permalink",
                                        },
                                    },
                                },
                            },
                        }
                    )
                );
            return this;
        }

        public RedditServiceTestFixture GivenClientGetPostsSuccess()
        {
            Client
                .Setup(c => c.GetPosts(It.IsAny<RedditToken>(), It.IsAny<Subreddit>()))
                .ReturnsAsync(
                    Result.Success(
                        new SubredditPostsResponse
                        {
                            Data = new SubredditPostsResponseData
                            {
                                Children = new[]
                                {
                                    new SubredditPost()
                                    {
                                        Data = new SubredditPostData
                                        {
                                            Id = "id",
                                            Author = "author",
                                            Title = "title",
                                            SelfText = "selfText",
                                            Permalink = "permalink",
                                        },
                                    },
                                },
                            },
                        }
                    )
                );
            return this;
        }
    }

    public sealed class RedditServiceTests
    {
        [Fact]
        public async Task GetComments_WhenGetAuthTokenFails_ExpectNoComments()
        {
            // Arrange
            var fixture = new RedditServiceTestFixture();
            fixture.GivenValidOptions().GivenAuthTokenFailure();

            // Act
            var result = await fixture.Service.GetComments(Subreddit.From("subreddit"));

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetComments_WhenClientReturnsSuccess_ExpectComments()
        {
            // Arrange
            var fixture = new RedditServiceTestFixture();
            fixture.GivenValidOptions().GivenAuthTokenSuccess().GivenClientGetCommentsSuccess();

            // Act
            var result = await fixture.Service.GetComments(Subreddit.From("subreddit"));

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetPosts_WhenGetAuthTokenFails_ExpectNoPosts()
        {
            // Arrange
            var fixture = new RedditServiceTestFixture();
            fixture.GivenValidOptions().GivenAuthTokenFailure();

            // Act
            var result = await fixture.Service.GetPosts(Subreddit.From("subreddit"));

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPosts_WhenClientReturnsSuccess_ExpectPosts()
        {
            // Arrange
            var fixture = new RedditServiceTestFixture();
            fixture.GivenValidOptions().GivenAuthTokenSuccess().GivenClientGetPostsSuccess();

            // Act
            var result = await fixture.Service.GetPosts(Subreddit.From("subreddit"));

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task PostComment_WhenClientReturnsSuccess_ReturnsSuccess()
        {
            // Arrange
            var parent = NonEmptyString.From("parent");
            var text = NonEmptyString.From("text");

            var fixture = new RedditServiceTestFixture();
            fixture.GivenValidOptions().GivenAuthTokenSuccess().GivenClientPostCommentSuccess();

            // Act
            var result = await fixture.Service.PostComment(parent, text);

            // Assert
            result.Should().Succeed();
        }
    }
}
