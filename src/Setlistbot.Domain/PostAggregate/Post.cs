using EnsureThat;
using Setlistbot.Domain.Extensions;

namespace Setlistbot.Domain.PostAggregate
{
    public sealed class Post
    {
        public string Id { get; private set; } = string.Empty;
        public string Author { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string SelfText { get; private set; } = string.Empty;
        public string Permalink { get; private set; } = string.Empty;
        public string ArtistId { get; private set; } = string.Empty;
        public string Reply { get; private set; } = string.Empty;

        public string ParentId => $"t3_{Id}";

        public IReadOnlyCollection<DateOnly> Dates =>
            Title.ParseDates().Concat(SelfText.ParseDates()).ToList().AsReadOnly();

        private Post() { }

        public Post(
            string id,
            string author,
            string title,
            string selfText,
            string permalink,
            string artistId,
            string reply
        )
        {
            Id = Ensure.String.IsNotNullOrWhiteSpace(id, nameof(id));
            Author = Ensure.String.IsNotNullOrWhiteSpace(author, nameof(author));
            Title = Ensure.String.IsNotNullOrWhiteSpace(title, nameof(title));
            SelfText = selfText;
            Permalink = Ensure.String.IsNotNullOrWhiteSpace(permalink, nameof(permalink));
            ArtistId = Ensure.String.IsNotNullOrWhiteSpace(artistId, nameof(artistId));
            Reply = reply;
        }

        public static Post NewPost(
            string id,
            string author,
            string title,
            string selfText,
            string permalink,
            string artistId
        ) => new(id, author, title, selfText, permalink, artistId, string.Empty);

        public bool HasMentionOf(string text)
        {
            return Title.Contains(text, StringComparison.CurrentCultureIgnoreCase)
                || SelfText.Contains(text, StringComparison.CurrentCultureIgnoreCase);
        }

        public void SetReply(string reply) =>
            Reply = Ensure.String.IsNotNullOrWhiteSpace(reply, nameof(reply));
    }
}
