using Azure.Data.Tables;
using EnsureThat;

namespace Setlistbot.Infrastructure.Data
{
    public abstract class AzureTableRepository<T> : IAzureTableRepository<T>
        where T : class, ITableEntity, new()
    {
        protected readonly TableClient _client;

        protected abstract string TableName { get; }

        public AzureTableRepository(string connectionString)
        {
            Ensure.That(connectionString, nameof(connectionString)).IsNotNullOrWhiteSpace();

            _client = new TableClient(connectionString, TableName);
        }

        public async Task<IEnumerable<T>> GetAsync(string partitionKey)
        {
            Ensure.That(partitionKey, nameof(partitionKey)).IsNotNullOrWhiteSpace();

            var filter = TableClient.CreateQueryFilter($"PartitionKey eq {partitionKey}");

            var results = new List<T>();

            await foreach (var page in _client.QueryAsync<T>(filter).AsPages())
            {
                results.AddRange(page.Values);
            }

            return results;
        }

        public async Task<T?> GetAsync(string partitionKey, string rowKey)
        {
            var filter = TableClient.CreateQueryFilter(
                $"PartitionKey eq {partitionKey} and RowKey eq {rowKey}"
            );

            var results = new List<T>();

            await foreach (var page in _client.QueryAsync<T>(filter).AsPages())
            {
                results.AddRange(page.Values);
            }

            return results.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var results = new List<T>();

            await foreach (var page in _client.QueryAsync<T>().AsPages())
            {
                results.AddRange(page.Values);
            }

            return results;
        }

        public async Task DeleteAsync(T entity)
        {
            Ensure.That(entity, nameof(entity)).IsNotNull();

            await _client.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
        }

        public async Task AddAsync(T entity)
        {
            await _client.UpsertEntityAsync(entity);
        }

        public async Task AddOrUpdateAsync(T entity)
        {
            await _client.UpsertEntityAsync(entity);
        }
    }
}
