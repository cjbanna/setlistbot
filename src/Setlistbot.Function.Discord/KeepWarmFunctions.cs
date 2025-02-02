using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Setlistbot.Function.Discord
{
    public sealed class KeepWarmFunctions
    {
        private readonly ILogger<KeepWarmFunctions> _logger;

        public KeepWarmFunctions(ILogger<KeepWarmFunctions> logger)
        {
            _logger = logger;
        }

        [Function("KeepWarm")]
        [SuppressMessage(
            "Style",
            "IDE0060:Remove unused parameter",
            Justification = "The Azure functions library requires the TimerInfo parameter"
        )]
        public void KeepWarm([TimerTrigger("0 */3 * * * *")] object timerInfo)
        {
            _logger.LogInformation("Keep warm function called");
        }
    }
}
