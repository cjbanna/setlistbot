using EnsureThat;
using Setlistbot.Domain.Extensions;

namespace Setlistbot.Domain.CommentAggregate
{
    public sealed class Comment
    {
        private List<DateOnly> _dates = [];

        public string Id { get; private set; } = string.Empty;
        public string Author { get; private set; } = string.Empty;
        public string Body { get; private set; } = string.Empty;
        public string Permalink { get; private set; } = string.Empty;
        public string ArtistId { get; private set; } = string.Empty;
        public string Reply { get; private set; } = string.Empty;

        public string ParentId => $"t1_{Id}";

        public IReadOnlyCollection<DateOnly> Dates
        {
            get
            {
                _dates ??= Body.ParseDates().ToList();
                return _dates.AsReadOnly();
            }
        }

        private Comment() { }

        public Comment(
            string id,
            string author,
            string body,
            string permalink,
            string artistId,
            string reply
        )
        {
            Ensure.String.IsNotNullOrWhiteSpace(id, nameof(id));
            Ensure.String.IsNotNullOrEmpty(body, nameof(body));
            Ensure.String.IsNotNullOrWhiteSpace(permalink, nameof(permalink));
            Ensure.String.IsNotNullOrWhiteSpace(author, nameof(author));
            Ensure.String.IsNotEmptyOrWhiteSpace(artistId, nameof(artistId));
            Ensure.String.IsNotNullOrWhiteSpace(reply, nameof(reply));

            Id = id;
            Author = author;
            Body = body;
            Permalink = permalink;
            ArtistId = artistId;
            Reply = reply;
        }

        public static Comment NewComment(
            string id,
            string author,
            string body,
            string permalink,
            string artistId
        )
        {
            Ensure.String.IsNotNullOrWhiteSpace(id, nameof(id));
            Ensure.String.IsNotNullOrEmpty(body, nameof(body));
            Ensure.String.IsNotNullOrWhiteSpace(permalink, nameof(permalink));
            Ensure.String.IsNotNullOrWhiteSpace(author, nameof(author));
            Ensure.String.IsNotEmptyOrWhiteSpace(artistId, nameof(artistId));

            return new Comment()
            {
                Id = id,
                Author = author,
                Body = body,
                Permalink = permalink,
                ArtistId = artistId,
                Reply = string.Empty,
            };
        }

        /// <summary>
        /// Returns true if text is mentioned in the comment
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool HasMentionOf(string text)
        {
            return text != null && Body.Contains(text, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Sets the reply value
        /// </summary>
        /// <param name="reply">The raw string value that was used in the reply</param>
        public void SetReply(string reply)
        {
            Ensure.That(reply, nameof(reply)).IsNotNullOrWhiteSpace();

            Reply = reply;
        }
    }
}
