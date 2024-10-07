using CSharpFunctionalExtensions;
using EnsureThat;

namespace Setlistbot.Domain
{
    public sealed class Setlist
    {
        private readonly List<Set> _sets = new();

        public IReadOnlyList<Set> Sets => _sets.AsReadOnly();

        public ArtistId ArtistId { get; private set; } = default!;
        public ArtistName ArtistName { get; private set; } = default!;
        public DateOnly Date { get; private set; }
        public Location Location { get; private set; } = default!;
        public TimeSpan Duration => _sets.Aggregate(TimeSpan.Zero, (acc, s) => acc + s.Duration);
        public string Notes { get; private set; } = string.Empty;
        public Maybe<string> SpotifyUrl { get; private set; }
        public Maybe<string> Permalink { get; private set; }

        private Setlist() { }

        public static Setlist NewSetlist(
            ArtistId artistId,
            ArtistName artistName,
            DateOnly showDate,
            Location location,
            string notes
        )
        {
            Ensure.That(location, nameof(location)).IsNotNull();
            Ensure.That(notes, nameof(notes)).IsNotNull();

            return new Setlist
            {
                ArtistId = artistId,
                ArtistName = artistName,
                Date = showDate,
                Location = location,
                Notes = notes,
            };
        }

        public void AddSet(Set set)
        {
            Ensure.That(set, nameof(set)).IsNotNull();

            if (
                _sets.Any(s =>
                    string.Equals(s.Name, set.Name, StringComparison.CurrentCultureIgnoreCase)
                )
            )
            {
                throw new InvalidOperationException($"Set name '{set.Name}' must be unique");
            }

            _sets.Add(set);
        }

        public void AddSpotifyUrl(Uri url)
        {
            Ensure.That(url, nameof(url)).IsNotNull();
            SpotifyUrl = url.ToString();
        }

        public void AddPermalink(Uri url)
        {
            Ensure.That(url, nameof(url)).IsNotNull();
            Permalink = url.ToString();
        }
    }
}
