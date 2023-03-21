﻿using Azure.Data.Tables;

namespace Setlistbot.Infrastructure.Data
{
    public interface IAzureTableRepository<T>
        where T : class, ITableEntity, new()
    {
        Task<IEnumerable<T>> GetAsync(string partitionKey);
        Task<T?> GetAsync(string partitionKey, string rowKey);
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(T entity);
        Task AddAsync(T entity);
        Task AddOrUpdateAsync(T entity);
    }
}
