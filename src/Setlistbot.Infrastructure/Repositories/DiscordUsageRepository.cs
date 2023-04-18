using EnsureThat;
using Microsoft.Extensions.Logging;
using Setlistbot.Infrastructure.Data;
using Setlistbot.Infrastructure.Discord.Interactions;
using Setlistbot.Infrastructure.Extensions;

namespace Setlistbot.Infrastructure.Repositories
{
    public class DiscordUsageRepository
        : AzureTableRepository<DiscordUsageEntity>,
            IDiscordUsageRepository
    {
        private readonly ILogger<DiscordUsageRepository> _logger;

        public DiscordUsageRepository(
            string connectionString,
            string tableName,
            ILogger<DiscordUsageRepository> logger
        )
            : base(connectionString, tableName)
        {
            _logger = Ensure.Any.IsNotNull(logger, nameof(logger));
        }

        public async Task TrackUsageAsync(Interaction interaction)
        {
            try
            {
                var entity = interaction.ToTableEntity();
                await AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to track discord usage");
                // Don't throw, fail and move on
            }
        }
    }
}
