using Vogen;

namespace Setlistbot.Domain
{
    public sealed class Set
    {
        private readonly Dictionary<SongPosition, Song> _songs = [];

        public SetName Name { get; private set; }
        public IReadOnlyList<Song> Songs => _songs.Values.OrderBy(s => s.Position).ToList();
        public TimeSpan Duration =>
            _songs.Aggregate(TimeSpan.Zero, (acc, s) => acc + s.Value.Duration);

        public Set(SetName name)
        {
            Name = name;
        }

        public Set(SetName name, IEnumerable<Song> songs)
        {
            Name = name;
            AddSongs(songs);
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
            _songs.Add(song.Position, song);
        }
    }

    [ValueObject<string>]
    public readonly partial struct SetName
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("Set name cannot be empty.")
                : Validation.Ok;
    }
}
