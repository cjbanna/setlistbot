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
    public class RedditServiceTestFixture
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

        public RedditServiceTestFixture GivenSuccessfulAuthToken()
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
    }

    public class RedditServiceTests
    {
        [Fact]
        public async Task PostComment_WhenClientReturnsSuccess_ReturnsSuccess()
        {
            // Arrange
            var parent = NonEmptyString.From("parent");
            var text = NonEmptyString.From("text");

            var fixture = new RedditServiceTestFixture();
            fixture.GivenValidOptions().GivenSuccessfulAuthToken().GivenClientPostCommentSuccess();

            // Act
            var result = await fixture.Service.PostComment(parent, text);

            // Assert
            result.Should().Succeed();
        }
    }
}
