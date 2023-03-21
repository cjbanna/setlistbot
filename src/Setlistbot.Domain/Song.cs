using EnsureThat;
using System.Diagnostics;

namespace Setlistbot.Domain
{
    [DebuggerDisplay("{Position} {Name} {Transition}")]
    public class Song
    {
        public Song(string name, int position, string transition, int duration, string footnote)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();
            Ensure.That(position, nameof(position)).IsGt(0);
            Ensure.That(duration, nameof(duration)).IsGte(0);
            Ensure.That(transition, nameof(transition)).IsNotNull();

            Name = name;
            Position = position;
            Transition = transition.Trim();
            Duration = duration;
            Footnote = footnote;
        }

        public int Position { get; }
        public string Name { get; }
        public string Transition { get; }
        public int Duration { get; }
        public string Footnote { get; }
    }
}
