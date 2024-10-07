using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Setlistbot.Infrastructure.PhishNet.Options;

namespace Setlistbot.Infrastructure.PhishNet.UnitTests
{
    public class PhishNetClientTestFixture
    {
        public Mock<IOptions<PhishNetOptions>> Options { get; }
        public Mock<ILogger<PhishNetClient>> Logger { get; }
        public PhishNetClient PhishNetClient { get; }
        public HttpTest HttpTest { get; }

        public PhishNetClientTestFixture()
        {
            Options = new Mock<IOptions<PhishNetOptions>>();
            Options
                .SetupGet(o => o.Value)
                .Returns(
                    new PhishNetOptions
                    {
                        ApiKey = "MyApiKey",
                        BaseUrl = "https://api.phish.net/v5",
                    }
                );

            Logger = new Mock<ILogger<PhishNetClient>>();
            PhishNetClient = new PhishNetClient(Logger.Object, Options.Object);
            HttpTest = new HttpTest();
        }
    }

    public class PhishNetClientTests
    {
        [Fact]
        public async Task GetSetlistAsync_WhenNoError_ExpectSuccessResponse()
        {
            // Arrange
            var fixture = new PhishNetClientTestFixture();

            var setlistResponse = TestData.GetSetlistResponseTestData();
            fixture.HttpTest.RespondWithJson(setlistResponse);

            // Act
            var result = await fixture.PhishNetClient.GetSetlistAsync(new DateTime(1997, 11, 22));

            // Assert
            Assert.NotNull(result);

            fixture
                .HttpTest.ShouldHaveCalled(
                    "https://api.phish.net/v5/setlists/showdate/1997-11-22.json?apikey=MyApiKey"
                )
                .WithVerb(HttpMethod.Get)
                .Times(1);
        }
    }
}
