using Azure;
using Azure.Data.Tables;

namespace Setlistbot.Infrastructure.Data
{
    public sealed class PostEntity : ITableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string SelfText { get; set; } = string.Empty;
        public string Reply { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Permalink { get; set; } = string.Empty;
        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
