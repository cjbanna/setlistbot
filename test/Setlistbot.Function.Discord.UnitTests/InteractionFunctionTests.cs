using System.Net;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Setlistbot.Application.Discord;
using Setlistbot.Infrastructure.Discord.Interactions;

namespace Setlistbot.Function.Discord.UnitTests
{
    public class InteractionFunctionTestFixture
    {
        public Mock<ILogger<InteractionFunctions>> Logger { get; }
        public Mock<IDiscordInteractionService> DiscordInteractionService { get; }
        public Mock<FunctionContext> FunctionContext { get; }
        public InteractionFunctions InteractionFunction { get; }

        public InteractionFunctionTestFixture()
        {
            Logger = new Mock<ILogger<InteractionFunctions>>();
            DiscordInteractionService = new Mock<IDiscordInteractionService>();
            FunctionContext = new Mock<FunctionContext>();

            InteractionFunction = new InteractionFunctions(
                Logger.Object,
                DiscordInteractionService.Object
            );
        }
    }

    public class InteractionFunctionTests
    {
        [Fact]
        public async Task HandleInteraction_WhenPing_ExpectPong()
        {
            // Arrange
            var fixture = new InteractionFunctionTestFixture();

            var body = new { type = 1 };

            var bodyStream = new MemoryStream(
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body))
            );
            var httpRequest = new FakeHttpRequestData(fixture.FunctionContext.Object, bodyStream);

            // Simulate a pong response to a ping request
            fixture
                .DiscordInteractionService.Setup(d =>
                    d.GetResponse(
                        It.Is<Interaction>(i => i.InteractionType == InteractionType.Ping)
                    )
                )
                .ReturnsAsync(new InteractionResponse { Type = InteractionCallbackType.Pong });

            // Act
            var response = await fixture.InteractionFunction.HandleInteraction(httpRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response.Body.Position = 0;
            var responseBody = new StreamReader(response.Body).ReadToEnd();
            Assert.Equal("{\"type\":1}", responseBody);
            var contentTypeHeader = Assert.Single(response.Headers, h => h.Key == "Content-Type");
            Assert.Equal("application/json", contentTypeHeader.Value.First());
        }
    }
}
