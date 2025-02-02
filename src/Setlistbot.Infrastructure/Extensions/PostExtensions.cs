using EnsureThat;
using Setlistbot.Domain.PostAggregate;
using Setlistbot.Infrastructure.Data;

namespace Setlistbot.Infrastructure.Extensions
{
    public static class PostExtensions
    {
        public static PostEntity ToTableEntity(this Post post, string partitionKey)
        {
            Ensure.That(post, nameof(post)).IsNotNull();
            Ensure.That(partitionKey, nameof(partitionKey)).IsNotNullOrWhiteSpace();

            return new PostEntity
            {
                PartitionKey = partitionKey,
                RowKey = post.Id,
                Title = post.Title,
                SelfText = post.SelfText,
                Reply = post.Reply,
                Author = post.Author,
                Permalink = post.Permalink,
            };
        }

        public static Post? ToDomain(this PostEntity? entity)
        {
            return entity == null
                ? null
                : Post.Hydrate(
                    entity.RowKey,
                    entity.Author,
                    entity.Title,
                    entity.SelfText,
                    entity.Permalink,
                    entity.PartitionKey,
                    entity.Reply
                );
        }
    }
}
