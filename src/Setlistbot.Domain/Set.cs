using EnsureThat;

namespace Setlistbot.Domain
{
    public class Set
    {
        private readonly List<Song> _songs = new();

        public string Name { get; private set; }
        public IReadOnlyList<Song> Songs => _songs.OrderBy(s => s.Position).ToList().AsReadOnly();
        public int Duration => _songs.Sum(s => s.Duration);

        public Set(string name)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();
            Name = name;
        }

        public void AddSongs(IEnumerable<Song> songs)
        {
            foreach (var song in songs)
            {
                AddSong(song);
            }
        }

        public void AddSong(Song song)
        {
            Ensure.That(song, nameof(song)).IsNotNull();

            if (Songs.Any(s => s.Position == song.Position))
            {
                throw new InvalidOperationException($"Position {song.Position} must be unique");
            }

            _songs.Add(song);
        }

        // A method that returns a new Set with the songs in a random order
        public Set Shuffle()
        {
            var random = new Random();
            var shuffledSongs = _songs.OrderBy(s => random.Next()).ToList();
            var shuffledSet = new Set(Name);
            shuffledSet.AddSongs(shuffledSongs);
            return shuffledSet;
        }
    }
}
