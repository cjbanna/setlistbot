﻿using EnsureThat;
using Setlistbot.Domain.PostAggregate;
using Setlistbot.Infrastructure.Data;
using Setlistbot.Infrastructure.Extensions;

namespace Setlistbot.Infrastructure.Repositories
{
    public sealed class PostRepository : AzureTableRepository<PostEntity>, IPostRepository
    {
        private readonly string _subreddit;

        public PostRepository(string subreddit, string connectionString, string tableName)
            : base(connectionString, tableName)
        {
            _subreddit = Ensure.String.IsNotNullOrWhiteSpace(subreddit, nameof(subreddit));
        }

        public async Task<Post?> Get(string id)
        {
            Ensure.That(id, nameof(id)).IsNotNullOrWhiteSpace();

            var entity = await GetAsync(_subreddit, id);
            return entity.ToDomain();
        }

        public async Task Add(Post post)
        {
            var entity = post.ToTableEntity(_subreddit);
            await AddAsync(entity);
        }

        public async Task Delete(Post post)
        {
            var entity = post.ToTableEntity(_subreddit);
            await DeleteAsync(entity);
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            var posts = await GetAllAsync();
            return posts.Where(c => c != null).Select(c => c.ToDomain()!);
        }
    }
}
