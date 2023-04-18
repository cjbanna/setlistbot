using Azure;
using Azure.Data.Tables;

namespace Setlistbot.Infrastructure.Data
{
    public sealed class DiscordUsageEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Id { get; set; } = string.Empty;
        public string ApplicationId { get; set; } = string.Empty;
        public string GuildId { get; set; } = string.Empty;
        public int IteractionType { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}
