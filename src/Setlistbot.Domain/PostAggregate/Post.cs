using EnsureThat;
using Setlistbot.Domain.Extensions;

namespace Setlistbot.Domain.PostAggregate
{
    public sealed class Post
    {
        private List<DateTime> _dates = null!;

        public string Id { get; private set; } = string.Empty;
        public string Author { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string SelfText { get; private set; } = string.Empty;
        public string Permalink { get; private set; } = string.Empty;
        public string ArtistId { get; private set; } = string.Empty;
        public string Reply { get; private set; } = string.Empty;

        public string ParentId => $"t3_{Id}";

        public IReadOnlyCollection<DateTime> Dates
        {
            get
            {
                _dates ??= Title.ParseDates().Concat(SelfText.ParseDates()).ToList();
                return _dates.AsReadOnly();
            }
        }

        private Post() { }

        public static Post NewPost(
            string id,
            string author,
            string title,
            string selfText,
            string permalink,
            string artistId
        )
        {
            Ensure.String.IsNotNullOrWhiteSpace(id, nameof(id));
            Ensure.String.IsNotNullOrWhiteSpace(author, nameof(author));
            Ensure.String.IsNotNullOrWhiteSpace(title, nameof(title));
            Ensure.String.IsNotNullOrWhiteSpace(permalink, nameof(permalink));
            Ensure.String.IsNotEmptyOrWhiteSpace(artistId, nameof(artistId));

            return new Post()
            {
                Id = id,
                Author = author,
                Title = title,
                SelfText = selfText,
                Permalink = permalink,
                ArtistId = artistId,
                Reply = string.Empty
            };
        }

        public static Post Hydrate(
            string id,
            string author,
            string title,
            string selfText,
            string permalink,
            string artistId,
            string reply
        )
        {
            Ensure.String.IsNotNullOrWhiteSpace(id, nameof(id));
            Ensure.String.IsNotNullOrWhiteSpace(author, nameof(author));
            Ensure.String.IsNotNullOrWhiteSpace(title, nameof(title));
            Ensure.String.IsNotNullOrWhiteSpace(permalink, nameof(permalink));
            Ensure.String.IsNotNullOrWhiteSpace(artistId, nameof(artistId));
            Ensure.String.IsNotNullOrWhiteSpace(reply, nameof(reply));

            return new Post()
            {
                Id = id,
                Author = author,
                Title = title,
                SelfText = selfText,
                Permalink = permalink,
                ArtistId = artistId,
                Reply = reply
            };
        }

        public bool HasMentionOf(string text)
        {
            return text != null
                && (
                    Title.Contains(text, StringComparison.CurrentCultureIgnoreCase)
                    || SelfText.Contains(text, StringComparison.CurrentCultureIgnoreCase)
                );
        }

        public void SetReply(string reply)
        {
            Ensure.That(reply, nameof(reply)).IsNotNullOrWhiteSpace();

            Reply = reply;
        }
    }
}
