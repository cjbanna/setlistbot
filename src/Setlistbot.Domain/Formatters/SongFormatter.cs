namespace Setlistbot.Domain.Formatters
{
    public sealed class SongFormatter(Song song) : IFormatter
    {
        public string Format()
        {
            var formatter = song.SongTransition switch
            {
                SongTransition.Stop => new CombinedFormatter(
                    new LiteralFormatter(song.Name.Value),
                    new SongTransitionFormatter(song.SongTransition)
                ),
                _ => new CombinedFormatter(
                    new LiteralFormatter(song.Name.Value),
                    new SpaceFormatter(),
                    new SongTransitionFormatter(song.SongTransition)
                ),
            };

            return formatter.Format();
        }
    }

    public sealed class SongsFormatter(IEnumerable<Song> songs) : IFormatter
    {
        public string Format()
        {
            var formatters = songs.SelectMany(s =>
                new IFormatter[]
                {
                    new LiteralFormatter(s.Name.Value),
                    new SongTransitionSuffixFormatter(s.SongTransition),
                }
            );
            // Trim the trailing transition formatter
            return formatters.Take(formatters.Count() - 1).Format();
        }
    }
}
