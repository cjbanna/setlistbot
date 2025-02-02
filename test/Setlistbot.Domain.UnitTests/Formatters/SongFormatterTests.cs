using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.UnitTests.Formatters
{
    public sealed class SongFormatterTests
    {
        [Fact]
        public void Format_WhenSongName_ExpectSongName()
        {
            // Arrange
            var songName = SongName.From("Song Name");

            var formatter = new LiteralFormatter(songName.Value);

            // Act
            var actual = formatter.Format();

            // Assert
            actual.Should().Be("Song Name");
        }

        [Fact]
        public void Format_WhenSong_ExpectFormattedSong()
        {
            // Arrange
            var song = new Song(
                SongName.From("Song Name"),
                SongPosition.From(1),
                SongTransition.Stop,
                TimeSpan.Zero,
                string.Empty
            );

            var formatter = new SongFormatter(song);

            // Act
            var actual = formatter.Format();

            // Assert
            actual.Should().Be("Song Name,");
        }
    }
}
