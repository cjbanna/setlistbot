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
                new ArtistId("phish"),
                new ArtistName("Phish"),
                new DateOnly(1997, 11, 17),
                new Location(
                    new Venue("McNichols Arena"),
                    new City("Denver"),
                    new State("CO"),
                    new Country("USA")
                ),
                "show notes"
            );

            var set1 = new Set(new SetName("Set 1"));
            set1.AddSong(
                new Song(
                    new SongName("Tweezer"),
                    1,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(new SongName("Reba"), 2, SongTransition.Stop, TimeSpan.Zero, string.Empty)
            );
            set1.AddSong(
                new Song(
                    new SongName("Train Song"),
                    3,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("Ghost"),
                    4,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(new SongName("Fire"), 5, SongTransition.Stop, TimeSpan.Zero, string.Empty)
            );
            setlist.AddSet(set1);

            var set2 = new Set(new SetName("Set 2"));
            set2.AddSong(
                new Song(
                    new SongName("Down with Disease"),
                    1,
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Olivia's Pool"),
                    2,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Johnny B. Goode"),
                    3,
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Jesus Just Left Chicago"),
                    4,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("When the Circus Comes"),
                    5,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("You Enjoy Myself"),
                    6,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            setlist.AddSet(set2);

            var encore = new Set(new SetName("Encore"));
            encore.AddSong(
                new Song(
                    new SongName("Character Zero"),
                    1,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
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
