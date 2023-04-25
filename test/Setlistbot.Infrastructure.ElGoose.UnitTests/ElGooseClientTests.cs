using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Setlistbot.Infrastructure.ElGoose.Options;

namespace Setlistbot.Infrastructure.ElGoose.UnitTests
{
    public class ElGooseClientTestFixture
    {
        public Mock<IOptions<ElGooseOptions>> Options { get; }
        public Mock<ILogger<ElGooseClient>> Logger { get; }
        public ElGooseClient ElGooseClient { get; }
        public HttpTest HttpTest { get; }

        public ElGooseClientTestFixture()
        {
            Options = new Mock<IOptions<ElGooseOptions>>();
            Options
                .SetupGet(o => o.Value)
                .Returns(new ElGooseOptions { BaseUrl = "https://elgoose.net/api/v1" });

            Logger = new Mock<ILogger<ElGooseClient>>();
            ElGooseClient = new ElGooseClient(Logger.Object, Options.Object);
            HttpTest = new HttpTest();
        }
    }

    public class ElGooseClientTests
    {
        [Fact]
        public async Task GetSetlistAsync_WhenNoError_ExpectSuccessResponse()
        {
            // Arrange
            var fixture = new ElGooseClientTestFixture();

            var setlistResponse = TestData.GetSetlistResponseTestData();
            fixture.HttpTest.RespondWithJson(setlistResponse);

            // Act
            var result = await fixture.ElGooseClient.GetSetlistAsync(new DateTime(2023, 04, 14));

            // Assert
            Assert.NotNull(result);

            fixture.HttpTest
                .ShouldHaveCalled("https://elgoose.net/api/v1/setlists/showdate/2023-04-14.json")
                .WithVerb(HttpMethod.Get)
                .Times(1);
        }
    }
}
