﻿using System.Diagnostics;
using EnsureThat;

namespace Setlistbot.Domain
{
    [DebuggerDisplay("{Position} {Name} {SongTransition}")]
    public class Song
    {
        public Song(
            SongName name,
            SongPosition position,
            SongTransition songTransition,
            TimeSpan duration,
            string footnote
        )
        {
            Ensure.That(duration, nameof(duration)).IsGte(TimeSpan.Zero);

            Name = name;
            Position = position;
            SongTransition = songTransition;
            Duration = duration;
            Footnote = footnote;
        }

        public SongPosition Position { get; }
        public SongName Name { get; }
        public SongTransition SongTransition { get; }
        public TimeSpan Duration { get; }
        public string Footnote { get; }
    }

    public record SongName(NonEmptyString Value)
    {
        public static implicit operator string(SongName songName) => songName.Value;
    }

    public record SongPosition : IComparable<SongPosition>
    {
        private readonly int _position = 0;

        public SongPosition(int position)
        {
            _position = EnsureArg.IsGt(position, 0, nameof(position));
        }

        public static implicit operator int(SongPosition position) => position._position;

        public static implicit operator SongPosition(int position) => new(position);

        public int CompareTo(SongPosition? other)
        {
            return other is null ? 1 : _position.CompareTo(other._position);
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
        public static string ToSymbol(this SongTransition songTransition) =>
            songTransition switch
            {
                SongTransition.Stop => ",",
                SongTransition.Immediate => ">",
                SongTransition.Segue => "->",
                _ => throw new System.NotImplementedException(),
            };

        public static SongTransition ToSongTransition(this string symbol) =>
            symbol.Trim() switch
            {
                ">" => SongTransition.Immediate,
                "->" => SongTransition.Segue,
                _ => SongTransition.Stop,
            };
    }
}
