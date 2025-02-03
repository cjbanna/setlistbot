using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Setlistbot.Application.Discord;
using Setlistbot.Infrastructure.Discord.Interactions;
using WorkerHttpFake;

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

            var json = JsonSerializer.Serialize(body);
            var httpRequest = new HttpRequestDataBuilder().WithBody(json).Build();

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
            responseBody.Should().Be("{\"type\":1,\"data\":null}");
            response
                .Headers.Should()
                .ContainSingle(h => h.Key == "Content-Type")
                .Which.Value.Should()
                .Contain("application/json; charset=utf-8");
        }
    }
}
