namespace Setlistbot.Domain.Kglw.UnitTests
{
    public class RedditReplyBuilderTests
    {
        [Fact]
        public void ArtistId_ExpectKglw()
        {
            // Arrange
            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.ArtistId;

            // Assert
            actual.Should().Be("kglw");
        }

        [Fact]
        public void Build_WhenTwoSetShow_ExpectReply()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                ArtistId.From("kglw"),
                ArtistName.From("King Gizzard & the Lizard Wizard"),
                new DateOnly(2022, 10, 11),
                new Location(
                    Venue.From("Red Rocks"),
                    City.From("Morrison"),
                    State.From("CO"),
                    Country.From("USA")
                ),
                "show notes"
            );

            var set1 = new Set(SetName.From("Set 1"));
            set1.AddSong(
                new Song(
                    SongName.From("The Dripping Tap"),
                    SongPosition.From(1),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Gaia"),
                    SongPosition.From(2),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Predator X"),
                    SongPosition.From(3),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Organ Farmer"),
                    SongPosition.From(4),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Pleura"),
                    SongPosition.From(5),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Oddlife"),
                    SongPosition.From(6),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Doom City"),
                    SongPosition.From(7),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("K.G.L.W. (Outro)"),
                    SongPosition.From(8),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Boogiman Sam"),
                    SongPosition.From(9),
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Sleepwalker"),
                    SongPosition.From(10),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Sea of Trees"),
                    SongPosition.From(11),
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("The Bitter Boogie"),
                    SongPosition.From(12),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            setlist.AddSet(set1);

            var set2 = new Set(SetName.From("Set 2"));
            set2.AddSong(
                new Song(
                    SongName.From("Perihelion"),
                    SongPosition.From(1),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("I'm in Your Mind"),
                    SongPosition.From(2),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("I'm Not in Your Mind"),
                    SongPosition.From(3),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Cellophane"),
                    SongPosition.From(4),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("I'm in Your Mind Fuzz"),
                    SongPosition.From(5),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Tezeta"),
                    SongPosition.From(6),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("A New World"),
                    SongPosition.From(7),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Altered Beast I"),
                    SongPosition.From(8),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Alter Me I"),
                    SongPosition.From(9),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Altered Beast II"),
                    SongPosition.From(10),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Alter Me II"),
                    SongPosition.From(11),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Altered Beast III"),
                    SongPosition.From(12),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Ambergris"),
                    SongPosition.From(13),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Muddy Water"),
                    SongPosition.From(14),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Iron Lung"),
                    SongPosition.From(15),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Robot Stop"),
                    SongPosition.From(16),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Gamma Knife"),
                    SongPosition.From(17),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("People-Vultures"),
                    SongPosition.From(18),
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Mr. Beat"),
                    SongPosition.From(19),
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Iron Lung"),
                    SongPosition.From(20),
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );

            setlist.AddSet(set2);

            var permalink = new Uri(
                "king-gizzard-the-lizard-wizard-october-10-2022-red-rocks-amphitheatre-morrison-co-usa.html",
                UriKind.Relative
            );
            setlist.AddPermalink(permalink);

            var builder = new RedditReplyBuilder();

            // Act
            var actual = builder.Build([setlist]);

            // Assert
            var expected = TestDataHelper.GetTestData("TestData/2022-10-11-reply.md");
            var expectedNormalized = expected.ReplaceLineEndings(string.Empty);
            var actualNormalized = actual.ReplaceLineEndings(string.Empty);
            actualNormalized.Should().Be(expectedNormalized);
        }
    }
}
