using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Setlistbot.Infrastructure.KglwNet.Options;

namespace Setlistbot.Infrastructure.KglwNet.UnitTests
{
    public class KglwNetClientTestFixture
    {
        public Mock<IOptions<KglwNetOptions>> Options { get; }
        public Mock<ILogger<KglwNetClient>> Logger { get; }
        public KglwNetClient KglwNetClient { get; }
        public HttpTest HttpTest { get; }

        public KglwNetClientTestFixture()
        {
            Options = new Mock<IOptions<KglwNetOptions>>();
            Options
                .SetupGet(o => o.Value)
                .Returns(new KglwNetOptions { BaseUrl = "https://kglw.songfishapp.com/api/v1" });

            Logger = new Mock<ILogger<KglwNetClient>>();
            KglwNetClient = new KglwNetClient(Logger.Object, Options.Object);
            HttpTest = new HttpTest();
        }
    }

    public class KglwNetClientTests
    {
        [Fact]
        public async Task GetSetlistAsync_WhenNoError_ExpectSuccessResponse()
        {
            // Arrange
            var fixture = new KglwNetClientTestFixture();

            var setlistResponse = TestData.GetSetlistResponseTestData();
            fixture.HttpTest.RespondWithJson(setlistResponse);

            // Act
            var result = await fixture.KglwNetClient.GetSetlistAsync(new DateOnly(2022, 10, 10));

            // Assert
            Assert.True(result.HasValue);

            fixture
                .HttpTest.ShouldHaveCalled(
                    "https://kglw.songfishapp.com/api/v1/setlists/showdate/2022-10-10.json"
                )
                .WithVerb(HttpMethod.Get)
                .Times(1);
        }
    }
}
