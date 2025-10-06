namespace Setlistbot.Domain.GratefulDead.UnitTests
{
    public sealed class RedditReplyBuilderTests
    {
        [Fact]
        public void ArtistId_ExpectGd()
        {
            // Arrange
            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.ArtistId;

            // Assert
            actual.Should().Be("gd");
        }

        [Fact]
        public void Build_WhenTwoSetShow_ExpectReply()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                ArtistId.From("gd"),
                ArtistName.From("The Grateful Dead"),
                new DateOnly(1977, 5, 8),
                new Location(
                    Venue.From("Barton Hall - Cornell University"),
                    City.From("Ithaca"),
                    State.From("NY"),
                    Country.From("USA")
                ),
                "show notes"
            );

            var set1 = new Set(SetName.From("Set 1"));
            var song = new Song(
                SongName.From("New Minglewood Blues"),
                SongPosition.From(1),
                SongTransition.Stop,
                TimeSpan.Zero,
                string.Empty
            );
            set1.AddSongs(
                [
                    song,
                    song with
                    {
                        Name = SongName.From("Loser"),
                        Position = SongPosition.From(2),
                    },
                    song with
                    {
                        Name = SongName.From("El Paso"),
                        Position = SongPosition.From(3),
                    },
                    song with
                    {
                        Name = SongName.From("They Love Each Other"),
                        Position = SongPosition.From(4),
                    },
                    song with
                    {
                        Name = SongName.From("Jack Straw"),
                        Position = SongPosition.From(5),
                    },
                    song with
                    {
                        Name = SongName.From("Deal"),
                        Position = SongPosition.From(6),
                    },
                    song with
                    {
                        Name = SongName.From("Lazy Lightnin'"),
                        Position = SongPosition.From(7),
                        SongTransition = SongTransition.Immediate,
                    },
                    song with
                    {
                        Name = SongName.From("Supplication"),
                        Position = SongPosition.From(8),
                    },
                    song with
                    {
                        Name = SongName.From("Brown Eyed Women"),
                        Position = SongPosition.From(9),
                    },
                    song with
                    {
                        Name = SongName.From("Mama Tried"),
                        Position = SongPosition.From(10),
                    },
                    song with
                    {
                        Name = SongName.From("Row Jimmy"),
                        Position = SongPosition.From(11),
                    },
                    song with
                    {
                        Name = SongName.From("Dancing In The Street"),
                        Position = SongPosition.From(12),
                    },
                ]
            );

            var set2 = new Set(SetName.From("Set 2"));
            set2.AddSongs(
                [
                    song with
                    {
                        Name = SongName.From("Scarlet Begonias"),
                        Position = SongPosition.From(1),
                        SongTransition = SongTransition.Immediate,
                    },
                    song with
                    {
                        Name = SongName.From("Fire On The Mountain"),
                        Position = SongPosition.From(2),
                    },
                    song with
                    {
                        Name = SongName.From("Estimated Prophet"),
                        Position = SongPosition.From(3),
                    },
                    song with
                    {
                        Name = SongName.From("Saint Stephen"),
                        Position = SongPosition.From(4),
                        SongTransition = SongTransition.Immediate,
                    },
                    song with
                    {
                        Name = SongName.From("Not Fade Away"),
                        Position = SongPosition.From(5),
                        SongTransition = SongTransition.Immediate,
                    },
                    song with
                    {
                        Name = SongName.From("Saint Stephen"),
                        Position = SongPosition.From(6),
                        SongTransition = SongTransition.Immediate,
                    },
                    song with
                    {
                        Name = SongName.From("Morning Dew"),
                        Position = SongPosition.From(7),
                    },
                ]
            );

            var encore = new Set(SetName.From("Encore"));
            encore.AddSongs(
                [
                    song with
                    {
                        Name = SongName.From("One More Saturday Night"),
                        Position = SongPosition.From(1),
                    },
                ]
            );

            setlist.AddSets([set1, set2, encore]);

            setlist.AddSpotifyUrl(new Uri("https://open.spotify.com/album/3T9UKU0jMIyrRD0PtKXqPJ"));

            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.Build([setlist]);

            // Assert
            var expected = TestDataHelper.GetTestData("TestData/1977-05-08-reply.md");
            var expectedNormalized = expected.ReplaceLineEndings(string.Empty);
            var actualNormalized = actual.ReplaceLineEndings(string.Empty);
            actualNormalized.Should().Be(expectedNormalized);
        }

        [Fact]
        public void Build_WhenMultipleShows_ExpectReply()
        {
            // Arrange
            var setlist1 = Setlist.NewSetlist(
                ArtistId.From("gd"),
                ArtistName.From("The Grateful Dead"),
                new DateOnly(1972, 9, 28),
                new Location(
                    Venue.From("Stanley Theater"),
                    City.From("Jersey City"),
                    State.From("NJ"),
                    Country.From("USA")
                ),
                "show notes"
            );

            var setlist2 = Setlist.NewSetlist(
                ArtistId.From("gd"),
                ArtistName.From("The Grateful Dead"),
                new DateOnly(1972, 12, 31),
                new Location(
                    Venue.From("Winterland Arena"),
                    City.From("San Francisco"),
                    State.From("CA"),
                    Country.From("USA")
                ),
                string.Empty
            );

            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.Build([setlist1, setlist2]);

            // Assert
            var expected = TestDataHelper.GetTestData("TestData/multiple-shows-reply.md");
            var expectedNormalized = expected.ReplaceLineEndings(string.Empty);
            var actualNormalized = actual.ReplaceLineEndings(string.Empty);
            actualNormalized.Should().Be(expectedNormalized);
        }
    }
}
