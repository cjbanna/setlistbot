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
            Assert.Equal("Red Rocks Amphitheatre", setlist.Location.Venue.Value);
            Assert.Equal("Morrison", setlist.Location.City);
            Assert.Equal("CO", setlist.Location.State.Value);
            Assert.Equal("USA", setlist.Location.Country);
            Assert.Equal(
                "The intro to O.N.E. contained Straws in the Wind teases. The River contained Wah Wah teases and quotes throughout, as well as Crumbling Castle teases. The Land Before Timeland was played over the PA as the setbreak music, which incidentally served as the “debut” for the studio track. Rattlesnake contained teases and quotes from O.N.E., Automation, Honey, and Minimum Brain Size, and teases of Sleep Drifter. Honey contained teases of Sleep Drifter and Billabong Valley. The Reticent Raconteur through The Balrog featured Leah Senior (introduced as “The Balrog” and “the best singer”) providing the narration. Ambrose quoted “Happy Birthday, Lisa” (The Simpsons) while referencing Joey’s “Australian birthday” prior to The Grim Reaper. The Grim Reaper was then introduced as “some weird-ass Satanic rap”. Venusian 2 and Am I in Heaven? were on the printed setlist but were replaced with Planet B due to time constraints.",
                setlist.Notes
            );

            Assert.Collection(
                setlist.Sets,
                set => Assert.Equal("Set 1", set.Name),
                set => Assert.Equal("Set 2", set.Name)
            );

            var set1 = setlist.Sets.First(s => s.Name == "Set 1");
            Assert.Collection(
                set1.Songs,
                song =>
                {
                    Assert.Equal<SongPosition>(1, song.Position);
                    Assert.Equal("Mars For The Rich", song.Name);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(2, song.Position);
                    Assert.Equal("Hell", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(3, song.Position);
                    Assert.Equal("Magenta Mountain", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(4, song.Position);
                    Assert.Equal("Inner Cell", song.Name);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(5, song.Position);
                    Assert.Equal("Loyalty", song.Name);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(6, song.Position);
                    Assert.Equal("Horology", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(7, song.Position);
                    Assert.Equal("O.N.E.", song.Name);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(8, song.Position);
                    Assert.Equal("Nuclear Fusion", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(9, song.Position);
                    Assert.Equal("All Is Known", song.Name);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(10, song.Position);
                    Assert.Equal("Straws In The Wind", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(11, song.Position);
                    Assert.Equal("The Garden Goblin", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(12, song.Position);
                    Assert.Equal("The River", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(13, song.Position);
                    Assert.Equal("Magma", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                }
            );

            var set2 = setlist.Sets.First(s => s.Name == "Set 2");
            Assert.Collection(
                set2.Songs,
                song =>
                {
                    Assert.Equal<SongPosition>(1, song.Position);
                    Assert.Equal("Rattlesnake", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(2, song.Position);
                    Assert.Equal("Automation", song.Name);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(3, song.Position);
                    Assert.Equal("Honey", song.Name);
                    Assert.Equal(SongTransition.Segue, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(4, song.Position);
                    Assert.Equal("Sleep Drifter", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(5, song.Position);
                    Assert.Equal("Ataraxia", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(6, song.Position);
                    Assert.Equal("Evil Death Roll", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(7, song.Position);
                    Assert.Equal("Ice V", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(8, song.Position);
                    Assert.Equal("The Reticent Raconteur", song.Name);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(9, song.Position);
                    Assert.Equal("The Lord of Lightning", song.Name);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(10, song.Position);
                    Assert.Equal("The Balrog", song.Name);
                    Assert.Equal(SongTransition.Segue, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(11, song.Position);
                    Assert.Equal("Trapdoor", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(12, song.Position);
                    Assert.Equal("Hot Water", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(13, song.Position);
                    Assert.Equal("The Grim Reaper", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal<SongPosition>(14, song.Position);
                    Assert.Equal("Planet B", song.Name);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                }
            );
        }
    }
}
