using System.Diagnostics;
using EnsureThat;

namespace Setlistbot.Domain
{
    [DebuggerDisplay("{Position} {Name} {SongTransition}")]
    public sealed record Song(
        SongName Name,
        SongPosition Position,
        SongTransition SongTransition,
        PositiveTimeSpan Duration,
        string Footnote
    );

    public sealed record SongName(NonEmptyString Value)
    {
        public static implicit operator string(SongName songName) => songName.Value;

        public static implicit operator SongName(string songName) =>
            new(new NonEmptyString(songName));
    }

    public readonly struct SongPosition : IComparable<SongPosition>
    {
        private readonly int _position = 0;

        [Obsolete("Don't use the default constructor", true)]
        public SongPosition() => throw new NotImplementedException();

        public SongPosition(int position)
        {
            _position = EnsureArg.IsGt(position, 0, nameof(position));
        }

        public static implicit operator int(SongPosition position) => position._position;

        public static implicit operator SongPosition(int position) => new(position);

        public int CompareTo(SongPosition other)
        {
            return _position.CompareTo(other._position);
        }
    }

    public enum SongTransition
    {
        Stop,
        Immediate,
        Segue,
    }

    public static class SongTransitionExtensions
    {
        public static SongTransition ToSongTransition(this string symbol) =>
            symbol.Trim() switch
            {
                ">" => SongTransition.Immediate,
                "->" => SongTransition.Segue,
                _ => SongTransition.Stop,
            };
    }
}
