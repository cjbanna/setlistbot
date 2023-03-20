using EnsureThat;
using Setlistbot.Domain.PostAggregate;
using Setlistbot.Infrastructure.Data;
using Setlistbot.Infrastructure.Extensions;

namespace Setlistbot.Infrastructure.Reddit
{
    public sealed class PostRepository : AzureTableRepository<PostEntity>, IPostRepository
    {
        private readonly string _subreddit;

        protected override string TableName => "posts";

        public PostRepository(string subreddit, string connectionString)
            : base(connectionString)
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
            return posts.Select(c => c.ToDomain());
        }
    }
}
