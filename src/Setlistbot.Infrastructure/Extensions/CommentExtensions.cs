using EnsureThat;
using Setlistbot.Domain.CommentAggregate;
using Setlistbot.Infrastructure.Data;

namespace Setlistbot.Infrastructure.Extensions
{
    public static class CommentExtensions
    {
        public static CommentEntity ToTableEntity(this Comment comment, string partitionKey)
        {
            Ensure.That(comment, nameof(comment)).IsNotNull();
            Ensure.That(partitionKey, nameof(partitionKey)).IsNotNullOrWhiteSpace();

            return new CommentEntity
            {
                PartitionKey = partitionKey,
                RowKey = comment.Id,
                Comment = comment.Body,
                Reply = comment.Reply,
                Author = comment.Author,
                Permalink = comment.Permalink
            };
        }

        public static Comment? ToDomain(this CommentEntity? entity)
        {
            return entity == null
                ? null
                : Comment.Hydrate(
                    entity.RowKey,
                    entity.Author,
                    entity.Comment,
                    entity.Permalink,
                    entity.PartitionKey,
                    entity.Reply
                );
        }
    }
}
