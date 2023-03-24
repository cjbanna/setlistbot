using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Setlistbot.Application;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Setlistbot.Function.RedditBot
{
    public class SetlistbotTimer
    {
        private readonly ILogger<SetlistbotTimer> _logger;
        private readonly IRedditSetlistbot _bot;

        public SetlistbotTimer(ILogger<SetlistbotTimer> log, IRedditSetlistbot bot)
        {
            _logger = log;
            _bot = bot;
        }

        [FunctionName("SetlistbotTimer")]
        [SuppressMessage(
            "Style",
            "IDE0060:Remove unused parameter",
            Justification = "The Azure functions library requires the TimerInfo parameter"
        )]
        public async Task Run([TimerTrigger("*/30 * * * * *")] TimerInfo timerInfo)
        {
            try
            {
                _logger.LogInformation(
                    "SetlistbotTimer function executed at: {Now}",
                    DateTime.UtcNow
                );

                await _bot.ReplyToMentions();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SetlistbotTimer function.");
            }
        }
    }
}
