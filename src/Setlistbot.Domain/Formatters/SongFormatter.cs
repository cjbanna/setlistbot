namespace Setlistbot.Domain.Formatters
{
    public sealed class SongFormatter(Song song) : IFormatter
    {
        public string Format()
        {
            var transitionFormatter = new SongTransitionFormatter(song.SongTransition);
            var transition = transitionFormatter.Format();
            return song.SongTransition switch
            {
                SongTransition.Immediate => $"{song.Name}{transition}",
                _ => $"{song.Name} {transition}",
            };
        }
    }
}
