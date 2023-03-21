using EnsureThat;
using Setlistbot.Domain.CommentAggregate;
using Setlistbot.Infrastructure.Data;
using Setlistbot.Infrastructure.Extensions;

namespace Setlistbot.Infrastructure.Repositories
{
    public sealed class CommentRepository : AzureTableRepository<CommentEntity>, ICommentRepository
    {
        private readonly string _subreddit;

        public CommentRepository(string subreddit, string connectionString, string tableName)
            : base(connectionString, tableName)
        {
            _subreddit = Ensure.String.IsNotNullOrWhiteSpace(subreddit, nameof(subreddit));
        }

        public async Task<Comment?> Get(string id)
        {
            Ensure.That(id, nameof(id)).IsNotNullOrWhiteSpace();

            var entity = await GetAsync(_subreddit, id);
            return entity.ToDomain();
        }

        public async Task Add(Comment comment)
        {
            var entity = comment.ToTableEntity(_subreddit);
            await AddAsync(entity);
        }

        public async Task Delete(Comment comment)
        {
            var entity = comment.ToTableEntity(_subreddit);
            await DeleteAsync(entity);
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            var comments = await GetAllAsync();
            return comments.Where(c => c != null).Select(c => c.ToDomain()!);
        }
    }
}
