using EnsureThat;

namespace Setlistbot.Domain
{
    public class Set
    {
        private readonly List<Song> _songs = new();

        public SetName Name { get; private set; }
        public IReadOnlyList<Song> Songs => _songs.OrderBy(s => s.Position).ToList();
        public TimeSpan Duration => _songs.Aggregate(TimeSpan.Zero, (acc, s) => acc + s.Duration);

        public Set(SetName name)
        {
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
            if (Songs.Any(s => s.Position == song.Position))
            {
                throw new InvalidOperationException($"Position {song.Position} must be unique");
            }

            _songs.Add(song);
        }
    }

    public record SetName : StringNotNullOrWhiteSpace
    {
        public SetName(string value)
            : base(value) { }
    }
}
