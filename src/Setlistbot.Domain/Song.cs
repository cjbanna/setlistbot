using System.Diagnostics;
using EnsureThat;
using Vogen;

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

    [ValueObject<string>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct SongName
    {
        private static Validation Validate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? Validation.Invalid("Song name cannot be empty.")
                : Validation.Ok;
    }

    [ValueObject<int>(conversions: Conversions.TypeConverter | Conversions.NewtonsoftJson)]
    public readonly partial struct SongPosition
    {
        private static Validation Validate(int value) =>
            value > 0 ? Validation.Ok : Validation.Invalid("Song position must be greater than 0.");
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
