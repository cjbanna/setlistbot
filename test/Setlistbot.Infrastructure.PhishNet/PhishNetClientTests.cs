using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Setlistbot.Infrastructure.PhishNet.Options;

namespace Setlistbot.Infrastructure.PhishNet.UnitTests
{
    public sealed class PhishNetClientTestFixture
    {
        public Mock<IOptions<PhishNetOptions>> Options { get; }
        public Mock<ILogger<PhishNetClient>> Logger { get; }
        public PhishNetClient PhishNetClient { get; }
        public HttpTest HttpTest { get; }

        public PhishNetClientTestFixture()
        {
            Options = new Mock<IOptions<PhishNetOptions>>();
            Logger = new Mock<ILogger<PhishNetClient>>();
            PhishNetClient = new PhishNetClient(Logger.Object, Options.Object);
            HttpTest = new HttpTest();
        }

        public PhishNetClientTestFixture GivenValidOptions()
        {
            Options
                .SetupGet(x => x.Value)
                .Returns(
                    new PhishNetOptions
                    {
                        ApiKey = "MyApiKey",
                        BaseUrl = "https://api.phish.net/v5",
                    }
                );

            return this;
        }
    }

    public sealed class PhishNetClientTests
    {
        [Fact]
        public async Task GetSetlistAsync_WhenNoError_ExpectSuccessResponse()
        {
            // Arrange
            var fixture = new PhishNetClientTestFixture().GivenValidOptions();

            var setlistResponse = TestData.GetSetlistResponseTestData();
            fixture.HttpTest.RespondWithJson(setlistResponse);

            // Act
            var result = await fixture.PhishNetClient.GetSetlistAsync(new DateOnly(1997, 11, 22));

            // Assert
            result.Should().Succeed();

            fixture
                .HttpTest.ShouldHaveCalled(
                    "https://api.phish.net/v5/setlists/showdate/1997-11-22.json?apikey=MyApiKey"
                )
                .WithVerb(HttpMethod.Get)
                .Times(1);
        }
    }
}
