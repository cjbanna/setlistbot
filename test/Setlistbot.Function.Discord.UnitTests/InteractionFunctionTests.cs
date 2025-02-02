using System.Net;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Setlistbot.Application.Discord;
using Setlistbot.Infrastructure.Discord.Interactions;

namespace Setlistbot.Function.Discord.UnitTests
{
    public sealed class InteractionFunctionTestFixture
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

    public sealed class InteractionFunctionTests
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
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body.Position = 0;
            var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            responseBody.Should().Be("{\"type\":1}");
            response
                .Headers.Should()
                .ContainSingle(h => h.Key == "Content-Type")
                .Which.Value.Should()
                .Contain("application/json");
        }
    }
}
