using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Infrastructure.Discord;
using Setlistbot.Infrastructure.Discord.Options;

namespace Setlistbot.Function.Discord.Middleware
{
    // Interpreted from https://github.com/discord/discord-interactions-js/blob/main/src/index.ts#L163
    public class VerifyKeyMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<VerifyKeyMiddleware> _logger;
        private readonly IDiscordService _discordService;
        private readonly DiscordOptions _options;

        public VerifyKeyMiddleware(
            ILogger<VerifyKeyMiddleware> logger,
            IDiscordService discordService,
            IOptions<DiscordOptions> options
        )
        {
            _logger = logger;
            _discordService = discordService;
            _options = options.Value;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            // Disable key verification for local development
            if (!_options.VerifyKey)
            {
                await next(context);
                return;
            }

            var request = await context.GetHttpRequestDataAsync();
            if (request != null)
            {
                try
                {
                    var invocationResult = context.GetInvocationResult();

                    if (
                        !request.Headers.TryGetValues("X-Signature-Ed25519", out var signatures)
                        || !request.Headers.TryGetValues(
                            "X-Signature-Timestamp",
                            out var timestamps
                        )
                    )
                    {
                        invocationResult.Value = request.CreateResponse(
                            HttpStatusCode.Unauthorized
                        );
                        return;
                    }

                    var signature = signatures.First();
                    var timestamp = timestamps.First();

                    request.Body.Seek(0, SeekOrigin.Begin);

                    var body = await request.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(body))
                    {
                        _logger.LogInformation("No body on request");
                        invocationResult.Value = request.CreateResponse(HttpStatusCode.BadRequest);
                        return;
                    }

                    var validInteraction = _discordService.VerifyInteraction(
                        _options.PublicKey,
                        signature,
                        timestamp,
                        body
                    );

                    if (!validInteraction)
                    {
                        var response = request.CreateResponse(HttpStatusCode.Unauthorized);
                        await response.WriteStringAsync("Invalid signature");
                        invocationResult.Value = response;
                        return;
                    }
                }
                finally
                {
                    // Always rewind the request body stream, so the next middleware can read it.
                    request.Body.Seek(0, SeekOrigin.Begin);
                }
            }
            await next(context);
        }
    }
}
