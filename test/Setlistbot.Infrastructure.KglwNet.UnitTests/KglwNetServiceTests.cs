using Microsoft.Extensions.Logging;
using Moq;

namespace Setlistbot.Infrastructure.KglwNet.UnitTests
{
    public class KglwNetServiceTestFixture
    {
        public Mock<ILogger<KglwNetService>> Logger { get; }

        public Mock<IKglwNetClient> KglwNetClient { get; }

        public KglwNetService KglwNetService { get; }

        public KglwNetServiceTestFixture()
        {
            Logger = new Mock<ILogger<KglwNetService>>();
            KglwNetClient = new Mock<IKglwNetClient>();
            KglwNetService = new KglwNetService(Logger.Object, KglwNetClient.Object);
        }
    }

    public class KglwNetServiceTests
    {
        [Fact]
        public async Task GetSetlistsAsync_WhenSetlistResponse_ExpectSetlist()
        {
            // Arrange
            var fixture = new KglwNetServiceTestFixture();
            var date = new DateTime(2020, 10, 10);

            var setlistResponse = TestData.GetSetlistResponseTestData();
            fixture.KglwNetClient.Setup(x => x.GetSetlistAsync(date)).ReturnsAsync(setlistResponse);

            // Act
            var setlists = await fixture.KglwNetService.GetSetlistsAsync(date);

            // Assert
            Assert.NotNull(setlists);

            var setlist = Assert.Single(setlists);
            Assert.Equal("2022-10-10", setlist.Date.ToString("yyyy-MM-dd"));
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
                    Assert.Equal(1, song.Position);
                    Assert.Equal("Mars For The Rich", song.Name);
                    Assert.Equal(">", song.Transition);
                }
            );
        }
    }
}
