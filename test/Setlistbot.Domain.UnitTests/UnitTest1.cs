namespace Setlistbot.Domain.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void NewSong()
        {
            var song = new Song("foo", 1, ">", 100, "footnote");
            Assert.NotNull(song);
        }

        [Fact]
        public void WithFootnote()
        {
            var song = new Song("foo", 1, ">", 100, "");
            var newSong = song.WithFootnote("footnote");
            Assert.Equal("footnote", newSong.Footnote);
        }
    }
}
