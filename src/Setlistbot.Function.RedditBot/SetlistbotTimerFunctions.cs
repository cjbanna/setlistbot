using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Setlistbot.Application.Reddit;

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

        [Function("SetlistbotTimer")]
        [SuppressMessage(
            "Style",
            "IDE0060:Remove unused parameter",
            Justification = "The Azure functions library requires the TimerInfo parameter"
        )]
        public async Task Run([TimerTrigger("*/30 * * * * *")] MyInfo myTimer)
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

    public class MyInfo
    {
        public MyScheduleStatus? ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
