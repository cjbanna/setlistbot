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
            actual.Should().Be("phish");
        }

        [Fact]
        public void Build_WhenTwoSetShow_ExpectReply()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                new DateOnly(1997, 11, 17),
                new Location(
                    Venue.From("McNichols Arena"),
                    City.From("Denver"),
                    State.From("CO"),
                    Country.From("USA")
                ),
                "show notes"
            );

            var set1 = new Set(SetName.From("Set 1"));
            set1.AddSong(
                new Song(
                    SongName.From("Tweezer"),
                    SongPosition.From(1),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Reba"),
                    SongPosition.From(2),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Train Song"),
                    SongPosition.From(3),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Ghost"),
                    SongPosition.From(4),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Fire"),
                    SongPosition.From(5),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            setlist.AddSet(set1);

            var set2 = new Set(SetName.From("Set 2"));
            set2.AddSong(
                new Song(
                    SongName.From("Down with Disease"),
                    SongPosition.From(1),
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Olivia's Pool"),
                    SongPosition.From(2),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Johnny B. Goode"),
                    SongPosition.From(3),
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Jesus Just Left Chicago"),
                    SongPosition.From(4),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("When the Circus Comes"),
                    SongPosition.From(5),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("You Enjoy Myself"),
                    SongPosition.From(6),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            setlist.AddSet(set2);

            var encore = new Set(SetName.From("Encore"));
            encore.AddSong(
                new Song(
                    SongName.From("Character Zero"),
                    SongPosition.From(1),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            setlist.AddSet(encore);

            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.Build([setlist]);

            // Assert
            var expected = TestDataHelper.GetTestData("TestData/1997-11-17-reply.md");
            actual.Should().Be(expected);
        }
    }
}
