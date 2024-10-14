using Microsoft.Extensions.Logging;
using Moq;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.KglwNet.UnitTests
{
    public class KglwNetSetlistProviderTestFixture
    {
        public Mock<ILogger<KglwNetSetlistProvider>> Logger { get; }

        public Mock<IKglwNetClient> KglwNetClient { get; }

        public KglwNetSetlistProvider KglwNetSetlistProvider { get; }

        public KglwNetSetlistProviderTestFixture()
        {
            Logger = new Mock<ILogger<KglwNetSetlistProvider>>();
            KglwNetClient = new Mock<IKglwNetClient>();
            KglwNetSetlistProvider = new KglwNetSetlistProvider(
                Logger.Object,
                KglwNetClient.Object
            );
        }
    }

    public class KglwNetSetlistProviderTests
    {
        [Fact]
        public async Task GetSetlistsAsync_WhenSetlistResponse_ExpectSetlist()
        {
            // Arrange
            var fixture = new KglwNetSetlistProviderTestFixture();
            var date = new DateOnly(2020, 10, 10);

            var setlistResponse = TestData.GetSetlistResponseTestData();
            fixture.KglwNetClient.Setup(x => x.GetSetlistAsync(date)).ReturnsAsync(setlistResponse);

            // Act
            var setlists = await fixture.KglwNetSetlistProvider.GetSetlists(date);

            // Assert
            Assert.NotNull(setlists);

            var setlist = Assert.Single(setlists);
            Assert.Equal("2022-10-10", setlist.Date.ToString("yyyy-MM-dd"));
            Assert.Equal(Venue.From("Red Rocks Amphitheatre"), setlist.Location.Venue.Value);
            Assert.Equal(City.From("Morrison"), setlist.Location.City);
            Assert.Equal(State.From("CO"), setlist.Location.State.Value);
            Assert.Equal(Country.From("USA"), setlist.Location.Country);
            Assert.Equal(
                "The intro to O.N.E. contained Straws in the Wind teases. The River contained Wah Wah teases and quotes throughout, as well as Crumbling Castle teases. The Land Before Timeland was played over the PA as the setbreak music, which incidentally served as the “debut” for the studio track. Rattlesnake contained teases and quotes from O.N.E., Automation, Honey, and Minimum Brain Size, and teases of Sleep Drifter. Honey contained teases of Sleep Drifter and Billabong Valley. The Reticent Raconteur through The Balrog featured Leah Senior (introduced as “The Balrog” and “the best singer”) providing the narration. Ambrose quoted “Happy Birthday, Lisa” (The Simpsons) while referencing Joey’s “Australian birthday” prior to The Grim Reaper. The Grim Reaper was then introduced as “some weird-ass Satanic rap”. Venusian 2 and Am I in Heaven? were on the printed setlist but were replaced with Planet B due to time constraints.",
                setlist.Notes
            );

            Assert.Collection(
                setlist.Sets,
                set => Assert.Equal("Set 1", set.Name.Value),
                set => Assert.Equal("Set 2", set.Name.Value)
            );

            var set1 = setlist.Sets.First(s => s.Name == "Set 1");
            Assert.Collection(
                set1.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position.Value);
                    Assert.Equal("Mars For The Rich", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position.Value);
                    Assert.Equal("Hell", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(3, song.Position.Value);
                    Assert.Equal("Magenta Mountain", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(4, song.Position.Value);
                    Assert.Equal("Inner Cell", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(5, song.Position.Value);
                    Assert.Equal("Loyalty", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(6, song.Position.Value);
                    Assert.Equal("Horology", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(7, song.Position.Value);
                    Assert.Equal("O.N.E.", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(8, song.Position.Value);
                    Assert.Equal("Nuclear Fusion", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(9, song.Position.Value);
                    Assert.Equal("All Is Known", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(10, song.Position.Value);
                    Assert.Equal("Straws In The Wind", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(11, song.Position.Value);
                    Assert.Equal("The Garden Goblin", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(12, song.Position.Value);
                    Assert.Equal("The River", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(13, song.Position.Value);
                    Assert.Equal("Magma", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                }
            );

            var set2 = setlist.Sets.First(s => s.Name == "Set 2");
            Assert.Collection(
                set2.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position.Value);
                    Assert.Equal("Rattlesnake", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position.Value);
                    Assert.Equal("Automation", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(3, song.Position.Value);
                    Assert.Equal("Honey", song.Name.Value);
                    Assert.Equal(SongTransition.Segue, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(4, song.Position.Value);
                    Assert.Equal("Sleep Drifter", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(5, song.Position.Value);
                    Assert.Equal("Ataraxia", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(6, song.Position.Value);
                    Assert.Equal("Evil Death Roll", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(7, song.Position.Value);
                    Assert.Equal("Ice V", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(8, song.Position.Value);
                    Assert.Equal("The Reticent Raconteur", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(9, song.Position.Value);
                    Assert.Equal("The Lord of Lightning", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(10, song.Position.Value);
                    Assert.Equal("The Balrog", song.Name.Value);
                    Assert.Equal(SongTransition.Segue, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(11, song.Position.Value);
                    Assert.Equal("Trapdoor", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(12, song.Position.Value);
                    Assert.Equal("Hot Water", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(13, song.Position.Value);
                    Assert.Equal("The Grim Reaper", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(14, song.Position.Value);
                    Assert.Equal("Planet B", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                }
            );
        }
    }
}
