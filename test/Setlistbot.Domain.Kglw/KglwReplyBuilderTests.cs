namespace Setlistbot.Domain.Kglw.UnitTests
{
    public class KglwReplyBuilderTests
    {
        [Fact]
        public void ArtistId_ExpectKglw()
        {
            // Arrange
            var builder = new KglwReplyBuilder();

            // Act
            var actual = builder.ArtistId;

            // Assert
            Assert.Equal("kglw", actual);
        }

        [Fact]
        public void Build_WhenTwoSetShow_ExpectReply()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                "kglw",
                "King Gizzard & the Lizard Wizard",
                new DateTime(2022, 10, 11),
                new Location("Red Rocks", "Morrison", "CO", "USA"),
                "show notes"
            );

            var set1 = new Set("Set 1");
            set1.AddSong(new Song("The Dripping Tap", 1, ",", 0, string.Empty));
            set1.AddSong(new Song("Gaia", 2, ",", 0, string.Empty));
            set1.AddSong(new Song("Predator X", 3, ",", 0, string.Empty));
            set1.AddSong(new Song("Organ Farmer", 4, ",", 0, string.Empty));
            set1.AddSong(new Song("Pleura", 5, ",", 0, string.Empty));
            set1.AddSong(new Song("Oddlife", 6, ">", 0, string.Empty));
            set1.AddSong(new Song("Doom City", 7, ",", 0, string.Empty));
            set1.AddSong(new Song("K.G.L.W. (Outro)", 8, ",", 0, string.Empty));
            set1.AddSong(new Song("Boogiman Sam", 9, "->", 0, string.Empty));
            set1.AddSong(new Song("Sleepwalker", 10, ",", 0, string.Empty));
            set1.AddSong(new Song("Sea of Trees", 11, "->", 0, string.Empty));
            set1.AddSong(new Song("The Bitter Boogie", 12, ",", 0, string.Empty));
            setlist.AddSet(set1);

            var set2 = new Set("Set 2");
            set2.AddSong(new Song("Perihelion", 1, ">", 0, string.Empty));
            set2.AddSong(new Song("I'm in Your Mind", 2, ">", 0, string.Empty));
            set2.AddSong(new Song("I'm Not in Your Mind", 3, ">", 0, string.Empty));
            set2.AddSong(new Song("Cellophane", 4, ">", 0, string.Empty));
            set2.AddSong(new Song("I'm in Your Mind Fuzz", 5, ",", 0, string.Empty));
            set2.AddSong(new Song("Tezeta", 6, ",", 0, string.Empty));
            set2.AddSong(new Song("A New World", 7, ">", 0, string.Empty));
            set2.AddSong(new Song("Altered Beast I", 8, ">", 0, string.Empty));
            set2.AddSong(new Song("Alter Me I", 9, ">", 0, string.Empty));
            set2.AddSong(new Song("Altered Beast II", 10, ">", 0, string.Empty));
            set2.AddSong(new Song("Alter Me II", 11, ">", 0, string.Empty));
            set2.AddSong(new Song("Altered Beast III", 12, ">", 0, string.Empty));
            set2.AddSong(new Song("Ambergris", 13, ",", 0, string.Empty));
            set2.AddSong(new Song("Muddy Water", 14, ",", 0, string.Empty));
            set2.AddSong(new Song("Iron Lung", 15, ",", 0, string.Empty));
            set2.AddSong(new Song("Robot Stop", 16, ">", 0, string.Empty));
            set2.AddSong(new Song("Gamma Knife", 17, ">", 0, string.Empty));
            set2.AddSong(new Song("People-Vultures", 18, ">", 0, string.Empty));
            set2.AddSong(new Song("Mr. Beat", 19, "->", 0, string.Empty));
            set2.AddSong(new Song("Iron Lung", 20, ",", 0, string.Empty));

            setlist.AddSet(set2);

            var builder = new KglwReplyBuilder();

            // Act
            var actual = builder.Build(setlist);

            // Assert
            var expected = TestDataHelper.GetTestData("TestData/2022-10-11-reply.md");
            Assert.Equal(expected, actual);
        }
    }
}
