using Setlistbot.Domain.Formatters;
using Xunit;

namespace Setlistbot.Domain.UnitTests.Formatters
{
    public class SongFormatterTests
    {
        [Fact]
        public void Format_WhenSongName_ExpectSongName()
        {
            // Arrange
            var songName = new SongName("Song Name");

            var formatter = new LiteralFormatter(songName);

            // Act
            var actual = formatter.Format();

            // Assert
            Assert.Equal("Song Name", actual);
        }

        [Fact]
        public void Format_WhenSong_ExpectFormattedSong()
        {
            // Arrange
            var song = new Song(
                new SongName("Song Name"),
                1,
                SongTransition.Stop,
                TimeSpan.Zero,
                string.Empty
            );

            var formatter = new SongFormatter(song);

            // Act
            var actual = formatter.Format();

            // Assert
            Assert.Equal("Song Name,", actual);
        }
    }
}
