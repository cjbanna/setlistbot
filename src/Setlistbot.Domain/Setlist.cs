using EnsureThat;

namespace Setlistbot.Domain
{
    public sealed class Setlist
    {
        private readonly List<Set> _sets = new();

        public IReadOnlyList<Set> Sets => _sets.AsReadOnly();

        public string ArtistId { get; private set; } = string.Empty;
        public string ArtistName { get; private set; } = string.Empty;
        public DateTime Date { get; private set; }
        public Location Location { get; private set; } = null!;
        public int Duration => Sets.Sum(s => s.Duration);
        public string Notes { get; private set; } = string.Empty;
        public string SpotifyUrl { get; private set; } = string.Empty;

        private Setlist() { }

        public static Setlist NewSetlist(
            string artistId,
            string artistName,
            DateTime showDate,
            Location location,
            string notes
        )
        {
            Ensure.That(artistId, nameof(artistId)).IsNotNullOrWhiteSpace();
            Ensure.That(artistName, nameof(artistName)).IsNotNullOrWhiteSpace();
            Ensure.That(location, nameof(location)).IsNotNull();
            Ensure.That(notes, nameof(notes)).IsNotNull();

            return new Setlist
            {
                ArtistId = artistId,
                ArtistName = artistName,
                Date = showDate,
                Location = location,
                Notes = notes
            };
        }

        public void AddSet(Set set)
        {
            Ensure.That(set, nameof(set)).IsNotNull();

            if (
                _sets.Any(
                    s => string.Equals(s.Name, set.Name, StringComparison.CurrentCultureIgnoreCase)
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
    }
}
