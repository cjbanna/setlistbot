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
            Assert.Equal("kglw", actual);
        }

        [Fact]
        public void Build_WhenTwoSetShow_ExpectReply()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                new ArtistId("kglw"),
                new ArtistName("King Gizzard & the Lizard Wizard"),
                new DateOnly(2022, 10, 11),
                new Location(
                    new Venue("Red Rocks"),
                    new City("Morrison"),
                    new State("CO"),
                    new Country("USA")
                ),
                "show notes"
            );

            var set1 = new Set(new SetName("Set 1"));
            set1.AddSong(
                new Song(
                    new SongName("The Dripping Tap"),
                    1,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(new SongName("Gaia"), 2, SongTransition.Stop, TimeSpan.Zero, string.Empty)
            );
            set1.AddSong(
                new Song(
                    new SongName("Predator X"),
                    3,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("Organ Farmer"),
                    4,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("Pleura"),
                    5,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("Oddlife"),
                    6,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("Doom City"),
                    7,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("K.G.L.W. (Outro)"),
                    8,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("Boogiman Sam"),
                    9,
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("Sleepwalker"),
                    10,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("Sea of Trees"),
                    11,
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    new SongName("The Bitter Boogie"),
                    12,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            setlist.AddSet(set1);

            var set2 = new Set(new SetName("Set 2"));
            set2.AddSong(
                new Song(
                    new SongName("Perihelion"),
                    1,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("I'm in Your Mind"),
                    2,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("I'm Not in Your Mind"),
                    3,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Cellophane"),
                    4,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("I'm in Your Mind Fuzz"),
                    5,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Tezeta"),
                    6,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("A New World"),
                    7,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Altered Beast I"),
                    8,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Alter Me I"),
                    9,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Altered Beast II"),
                    10,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Alter Me II"),
                    11,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Altered Beast III"),
                    12,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Ambergris"),
                    13,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Muddy Water"),
                    14,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Iron Lung"),
                    15,
                    SongTransition.Stop,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Robot Stop"),
                    16,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Gamma Knife"),
                    17,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("People-Vultures"),
                    18,
                    SongTransition.Immediate,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Mr. Beat"),
                    19,
                    SongTransition.Segue,
                    TimeSpan.Zero,
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    new SongName("Iron Lung"),
                    20,
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
            var actual = builder.Build(new[] { setlist });

            // Assert
            var expected = TestDataHelper.GetTestData("TestData/2022-10-11-reply.md");
            Assert.Equal(expected, actual);
        }
    }
}
