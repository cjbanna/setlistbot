namespace Setlistbot.Domain.Formatters
{
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
