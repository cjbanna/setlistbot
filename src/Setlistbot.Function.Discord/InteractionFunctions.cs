using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Setlistbot.Application.Discord;
using Setlistbot.Function.Discord.Extensions;
using Setlistbot.Infrastructure.Discord.Interactions;
using System.Net;

namespace Setlistbot.Function.Discord
{
    public class InteractionFunctions
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
                var interaction = await httpRequest.DeserializeJsonBodyAsync<Interaction>();
                if (interaction == null)
                {
                    throw new Exception($"Could not parse interaction from HTTP request body");
                }

                var interactionResponse = await _discordInteractionService.GetResponse(interaction);
                if (interactionResponse == null)
                {
                    throw new Exception("No interaction response returned");
                }

                var httpResponse = httpRequest.CreateResponse(HttpStatusCode.OK);
                await httpResponse.SerializeJsonBodyAsync(interactionResponse);
                return httpResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to handle interaction");
                httpRequest.Body.Seek(0, SeekOrigin.Begin);
                var body = await httpRequest.ReadAsStringAsync();
                _logger.LogInformation("HTTP body: {Body}", body);
                return httpRequest.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
