namespace Setlistbot.Domain.Formatters
{
    public sealed class SongTransitionFormatter(SongTransition songTransition) : IFormatter
    {
        public string Format() =>
            songTransition switch
            {
                SongTransition.Stop => ",",
                SongTransition.Immediate => ">",
                SongTransition.Segue => "->",
                _ => throw new NotImplementedException(),
            };
    }
}
