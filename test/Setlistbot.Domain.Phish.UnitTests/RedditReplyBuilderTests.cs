namespace Setlistbot.Domain.Phish.UnitTests
{
    public class RedditReplyBuilderTests
    {
        [Fact]
        public void ArtistId_ExpectPhish()
        {
            // Arrange
            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.ArtistId;

            // Assert
            Assert.Equal("phish", actual);
        }

        [Fact]
        public void Build_WhenTwoSetShow_ExpectReply()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                "phish",
                "Phish",
                new DateTime(1997, 11, 17),
                new Location("McNichols Arena", "Denver", "CO", "USA"),
                "show notes"
            );

            var set1 = new Set("Set 1");
            set1.AddSong(new Song("Tweezer", 1, ",", 0, string.Empty));
            set1.AddSong(new Song("Reba", 2, ",", 0, string.Empty));
            set1.AddSong(new Song("Train Song", 3, ",", 0, string.Empty));
            set1.AddSong(new Song("Ghost", 4, ">", 0, string.Empty));
            set1.AddSong(new Song("Fire", 5, ",", 0, string.Empty));
            setlist.AddSet(set1);

            var set2 = new Set("Set 2");
            set2.AddSong(new Song("Down with Disease", 1, "->", 0, string.Empty));
            set2.AddSong(new Song("Olivia's Pool", 2, ">", 0, string.Empty));
            set2.AddSong(new Song("Johnny B. Goode", 3, "->", 0, string.Empty));
            set2.AddSong(new Song("Jesus Just Left Chicago", 4, ",", 0, string.Empty));
            set2.AddSong(new Song("When the Cricus Comes", 5, ",", 0, string.Empty));
            set2.AddSong(new Song("You Enjoy Myself", 6, ",", 0, string.Empty));
            setlist.AddSet(set2);

            var encore = new Set("Encore");
            encore.AddSong(new Song("Character Zero", 1, ",", 0, string.Empty));
            setlist.AddSet(encore);

            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.Build(new[] { setlist });

            // Assert
            var expected = TestDataHelper.GetTestData("TestData/1997-11-17-reply.md");
            Assert.Equal(expected, actual);
        }
    }
}
