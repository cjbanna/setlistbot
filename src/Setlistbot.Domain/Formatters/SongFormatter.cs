namespace Setlistbot.Domain.Formatters
{
    public sealed class SongFormatter(Song song) : IFormatter
    {
        public string Format()
        {
            var formatter = song.SongTransition switch
            {
                SongTransition.Stop => new CombinedFormatter(
                    new LiteralFormatter(song.Name),
                    new SongTransitionFormatter(song.SongTransition)
                ),
                _ => new CombinedFormatter(
                    new LiteralFormatter(song.Name),
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
            Func<Song, IEnumerable<IFormatter>> getFormatters = (song) =>
                song.SongTransition switch
                {
                    SongTransition.Stop =>
                    [
                        new LiteralFormatter(song.Name),
                        new SongTransitionFormatter(song.SongTransition),
                        new SpaceFormatter(),
                    ],
                    _ =>
                    [
                        new LiteralFormatter(song.Name),
                        new SpaceFormatter(),
                        new SongTransitionFormatter(song.SongTransition),
                        new SpaceFormatter(),
                    ],
                };

            var formatters = songs.SelectMany<Song, IFormatter>(s => getFormatters(s));

            return formatters.Take(formatters.Count() - 2).Format();
        }
    }
}
