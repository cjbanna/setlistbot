using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Setlistbot.Application.Discord;
using Setlistbot.Infrastructure.Discord.Interactions;

namespace Setlistbot.Function.Discord
{
    public sealed class InteractionFunctions
    {
        private readonly ILogger<InteractionFunctions> _logger;
        private readonly IDiscordInteractionService _discordInteractionService;

        public InteractionFunctions(
            ILogger<InteractionFunctions> logger,
            IDiscordInteractionService discordInteractionService
        )
        {
            _logger = logger;
            _discordInteractionService = discordInteractionService;
        }

        [Function("HandleInteraction")]
        public async Task<HttpResponseData> HandleInteraction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "interactions")]
                HttpRequestData httpRequest
        )
        {
            try
            {
                var interaction = await httpRequest.ReadFromJsonAsync<Interaction>();
                if (interaction is null)
                {
                    throw new Exception($"Could not parse interaction from HTTP request body");
                }

                var interactionResponse = await _discordInteractionService.GetResponse(interaction);

                return await interactionResponse.Match(
                    async some => await GetHttpResponse(some),
                    () => throw new Exception("No interaction response returned")
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to handle interaction");
                httpRequest.Body.Seek(0, SeekOrigin.Begin);
                var body = await httpRequest.ReadAsStringAsync();
                _logger.LogInformation("HTTP body: {Body}", body);
                return httpRequest.CreateResponse(HttpStatusCode.InternalServerError);
            }

            async Task<HttpResponseData> GetHttpResponse(InteractionResponse response)
            {
                var httpResponse = httpRequest.CreateResponse(HttpStatusCode.OK);
                await httpResponse.WriteAsJsonAsync(response);
                return httpResponse;
            }
        }
    }
}
