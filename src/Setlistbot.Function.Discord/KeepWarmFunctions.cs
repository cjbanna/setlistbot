using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Setlistbot.Function.Discord
{
    public class KeepWarmFunctions
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
        public void KeepWarm([TimerTrigger("* */3 * * * *")] object timerInfo)
        {
            _logger.LogInformation("Keep warm function called");
        }
    }
}
