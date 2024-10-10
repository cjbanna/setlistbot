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

    public sealed record SetName(NonEmptyString Value)
    {
        public static implicit operator string(SetName setName) => setName.Value;

        public static implicit operator SetName(string setName) => new(new NonEmptyString(setName));
    }
}
