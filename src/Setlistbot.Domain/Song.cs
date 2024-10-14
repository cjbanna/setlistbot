using System.Diagnostics;
using EnsureThat;
using StronglyTypedPrimitives;
using StronglyTypedPrimitives.Attributes;

namespace Setlistbot.Domain
{
    [DebuggerDisplay("{Position} {Name} {SongTransition}")]
    public sealed record Song(
        SongName Name,
        SongPosition Position,
        SongTransition SongTransition,
        TimeSpan Duration,
        string Footnote
    )
    {
        public TimeSpan Duration { get; } =
            TimeSpan.FromMilliseconds(
                EnsureArg.IsGte(Duration.TotalMilliseconds, 0, nameof(Duration))
            );
    }

    [StronglyTyped(Template.String)]
    public readonly partial struct SongName
    {
        public SongName(NonEmptyString value) => _value = value;
    }

    [StronglyTyped(Template.Int)]
    public readonly partial struct SongPosition
    {
        public SongPosition(int value) => _value = EnsureArg.IsGt(value, 0, nameof(value));
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
