using DateOnly = System.DateOnly;

namespace Setlistbot.Domain.Kglw.UnitTests
{
    public class DiscordReplyBuilderTests
    {
        [Fact]
        public void ArtistId_ReturnsKglw()
        {
            // Arrange
            var builder = new DiscordReplyBuilder();

            // Act
            var result = builder.ArtistId;

            // Assert
            result.Should().Be("kglw");
        }

        [Fact]
        public void Build_WhenNoSetlists_ReturnsEmptyString()
        {
            // Arrange
            var builder = new DiscordReplyBuilder();

            // Act
            var result = builder.Build([]);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Build_WhenOnSetlist_ReturnsFormattedSetlist()
        {
            // Arrange
            var builder = new DiscordReplyBuilder();

            var artistId = ArtistId.From("kglw");
            var artistName = ArtistName.From("King Gizzard & the Lizard Wizard");
            var date = new DateOnly(2024, 8, 24);
            var venue = Venue.From("Jacobs Pavilion");
            var city = City.From("Cleveland");
            var state = State.From("OH");
            var country = Country.From("USA");
            var location = new Location(venue, city, state, country);
            var setlist = Setlist.NewSetlist(artistId, artistName, date, location, string.Empty);

            var song = new Song(
                SongName.From("Some Song"),
                SongPosition.From(1),
                SongTransition.Stop,
                TimeSpan.Zero,
                string.Empty
            );
            var set1 = new Set(SetName.From("Set 1"));
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Magma"),
                    Position = SongPosition.From(1),
                    SongTransition = SongTransition.Immediate,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Muddy Water"),
                    Position = SongPosition.From(2),
                    SongTransition = SongTransition.Stop,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Antarctica"),
                    Position = SongPosition.From(3),
                    SongTransition = SongTransition.Immediate,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Raw Feel"),
                    Position = SongPosition.From(4),
                    SongTransition = SongTransition.Stop,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Daily Blues"),
                    Position = SongPosition.From(5),
                    SongTransition = SongTransition.Segue,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Mr. Beat"),
                    Position = SongPosition.From(6),
                    SongTransition = SongTransition.Segue,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Ice V"),
                    Position = SongPosition.From(7),
                    SongTransition = SongTransition.Stop,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Le Risque"),
                    Position = SongPosition.From(8),
                    SongTransition = SongTransition.Stop,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Inner Cell"),
                    Position = SongPosition.From(9),
                    SongTransition = SongTransition.Immediate,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Loyalty"),
                    Position = SongPosition.From(10),
                    SongTransition = SongTransition.Immediate,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Horology"),
                    Position = SongPosition.From(11),
                    SongTransition = SongTransition.Stop,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Superbug"),
                    Position = SongPosition.From(12),
                    SongTransition = SongTransition.Stop,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Supercell"),
                    Position = SongPosition.From(13),
                    SongTransition = SongTransition.Immediate,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Predator X"),
                    Position = SongPosition.From(14),
                    SongTransition = SongTransition.Stop,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Self-Immolate"),
                    Position = SongPosition.From(15),
                    SongTransition = SongTransition.Immediate,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Flamethrower"),
                    Position = SongPosition.From(16),
                    SongTransition = SongTransition.Stop,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Dragon"),
                    Position = SongPosition.From(17),
                    SongTransition = SongTransition.Stop,
                }
            );
            set1.AddSong(
                song with
                {
                    Name = SongName.From("Gila Monster"),
                    Position = SongPosition.From(18),
                    SongTransition = SongTransition.Stop,
                }
            );

            setlist.AddSet(set1);

            setlist.AddPermalink(
                new Uri(
                    "king-gizzard-the-lizard-wizard-august-24-2024-jacobs-pavilion-cleveland-oh-usa.html",
                    UriKind.Relative
                )
            );

            // Act
            var actual = builder.Build([setlist]);

            // Assert
            var expected = TestDataHelper.GetTestData("TestData/2024-08-24-reply-discord.md");
            var expectedNormalized = expected.ReplaceLineEndings(string.Empty);
            var actualNormalized = actual.ReplaceLineEndings(string.Empty);
            actualNormalized.Should().Be(expectedNormalized);
        }
    }
}
