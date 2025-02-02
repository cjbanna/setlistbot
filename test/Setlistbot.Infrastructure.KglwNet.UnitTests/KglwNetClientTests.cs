using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Setlistbot.Infrastructure.KglwNet.Options;

namespace Setlistbot.Infrastructure.KglwNet.UnitTests
{
    public sealed class KglwNetClientTestFixture
    {
        public Mock<IOptions<KglwNetOptions>> Options { get; }
        public Mock<ILogger<KglwNetClient>> Logger { get; }
        public KglwNetClient KglwNetClient { get; }
        public HttpTest HttpTest { get; }

        public KglwNetClientTestFixture()
        {
            Options = new Mock<IOptions<KglwNetOptions>>();
            Logger = new Mock<ILogger<KglwNetClient>>();
            KglwNetClient = new KglwNetClient(Logger.Object, Options.Object);
            HttpTest = new HttpTest();
        }

        public KglwNetClientTestFixture GivenBaseUrl(string baseUrl)
        {
            Options.Setup(o => o.Value).Returns(new KglwNetOptions { BaseUrl = baseUrl });
            return this;
        }
    }

    public sealed class KglwNetClientTests
    {
        [Fact]
        public async Task GetSetlistAsync_WhenNoError_ExpectSuccessResponse()
        {
            // Arrange
            var fixture = new KglwNetClientTestFixture().GivenBaseUrl(
                "https://kglw.songfishapp.com/api/v1"
            );

            var setlistResponse = TestData.GetSetlistResponseTestData();
            fixture.HttpTest.RespondWithJson(setlistResponse);

            // Act
            var result = await fixture.KglwNetClient.GetSetlistAsync(new DateOnly(2022, 10, 10));

            // Assert
            result.HasValue.Should().BeTrue();

            fixture
                .HttpTest.ShouldHaveCalled(
                    "https://kglw.songfishapp.com/api/v1/setlists/showdate/2022-10-10.json"
                )
                .WithVerb(HttpMethod.Get)
                .Times(1);
        }
    }
}
