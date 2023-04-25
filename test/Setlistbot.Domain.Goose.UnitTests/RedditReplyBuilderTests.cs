namespace Setlistbot.Domain.Goose.UnitTests
{
    public class RedditReplyBuilderTests
    {
        [Fact]
        public void ArtistId_ExpectGoose()
        {
            // Arrange
            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.ArtistId;

            // Assert
            Assert.Equal("goose", actual);
        }

        [Fact]
        public void Build_WhenTwoSetShow_ExpectReply()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                "goose",
                "Goose",
                new DateTime(2023, 04, 14),
                new Location("The Salt Shed", "Chicago", "IL", "USA"),
                "show notes"
            );

            var set1 = new Set("Set 1");
            set1.AddSong(new Song("All I Need", 1, ",", 0, string.Empty));
            set1.AddSong(new Song("The Whales", 2, ",", 0, string.Empty));
            set1.AddSong(new Song("Earthling or Alien?", 3, ",", 0, string.Empty));
            set1.AddSong(new Song("Jive I", 4, ">", 0, string.Empty));
            set1.AddSong(new Song("Jive Lee", 5, ",", 0, string.Empty));
            set1.AddSong(new Song("Everything Must Go", 6, ",", 0, string.Empty));
            set1.AddSong(new Song("Thatch", 7, ",", 0, string.Empty));
            setlist.AddSet(set1);

            var set2 = new Set("Set 2");
            set2.AddSong(new Song("Hungersite", 1, ">", 0, string.Empty));
            set2.AddSong(new Song("Into The Myst", 2, ">", 0, string.Empty));
            set2.AddSong(new Song("Red Bird", 3, ",", 0, string.Empty));
            set2.AddSong(new Song("Seekers On The Ridge Pt. 1", 4, ">", 0, string.Empty));
            set2.AddSong(new Song("Seekers On The Ridge Pt. 2", 5, ",", 0, string.Empty));
            set2.AddSong(new Song("Madhuvan", 6, ",", 0, string.Empty));
            setlist.AddSet(set2);

            var encore = new Set("Encore");
            encore.AddSong(new Song("Tomorrow Never Knows", 1, "", 0, string.Empty));
            setlist.AddSet(encore);

            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.Build(new[] { setlist });

            // Assert
            var expected = TestDataHelper.GetTestData("TestData/2023-04-14-reply.md");
            Assert.Equal(expected, actual);
        }
    }
}
