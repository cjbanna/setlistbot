using Microsoft.Extensions.Logging;
using Moq;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.KglwNet.UnitTests
{
    public sealed class KglwNetSetlistProviderTestFixture
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

    public sealed class KglwNetSetlistProviderTests
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
            Assert.Equal(Venue.From("Red Rocks Amphitheatre"), setlist.Location.Venue);
            Assert.Equal(City.From("Morrison"), setlist.Location.City);
            Assert.Equal(State.From("CO"), setlist.Location.State);
            Assert.Equal(Country.From("USA"), setlist.Location.Country);
            Assert.Equal(
                "The intro to O.N.E. contained Straws in the Wind teases. The River contained Wah Wah teases and quotes throughout, as well as Crumbling Castle teases. The Land Before Timeland was played over the PA as the setbreak music, which incidentally served as the “debut” for the studio track. Rattlesnake contained teases and quotes from O.N.E., Automation, Honey, and Minimum Brain Size, and teases of Sleep Drifter. Honey contained teases of Sleep Drifter and Billabong Valley. The Reticent Raconteur through The Balrog featured Leah Senior (introduced as “The Balrog” and “the best singer”) providing the narration. Ambrose quoted “Happy Birthday, Lisa” (The Simpsons) while referencing Joey’s “Australian birthday” prior to The Grim Reaper. The Grim Reaper was then introduced as “some weird-ass Satanic rap”. Venusian 2 and Am I in Heaven? were on the printed setlist but were replaced with Planet B due to time constraints.",
                setlist.Notes
            );

            // Verify Set 1
            Assert.Equal(2, setlist.Sets.Count);
            var set1 = setlist.Sets[0];
            Assert.Equal("Set 1", set1.Name.Value);
            Assert.Equal(13, set1.Songs.Count);
            Assert.Equal(1, set1.Songs[0].Position.Value);
            Assert.Equal("Mars For The Rich", set1.Songs[0].Name.Value);
            Assert.Equal(SongTransition.Immediate, set1.Songs[0].SongTransition);
            Assert.Equal(2, set1.Songs[1].Position.Value);
            Assert.Equal("Hell", set1.Songs[1].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[1].SongTransition);
            Assert.Equal(3, set1.Songs[2].Position.Value);
            Assert.Equal("Magenta Mountain", set1.Songs[2].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[2].SongTransition);
            Assert.Equal(4, set1.Songs[3].Position.Value);
            Assert.Equal("Inner Cell", set1.Songs[3].Name.Value);
            Assert.Equal(SongTransition.Immediate, set1.Songs[3].SongTransition);
            Assert.Equal(5, set1.Songs[4].Position.Value);
            Assert.Equal("Loyalty", set1.Songs[4].Name.Value);
            Assert.Equal(SongTransition.Immediate, set1.Songs[4].SongTransition);
            Assert.Equal(6, set1.Songs[5].Position.Value);
            Assert.Equal("Horology", set1.Songs[5].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[5].SongTransition);
            Assert.Equal(7, set1.Songs[6].Position.Value);
            Assert.Equal("O.N.E.", set1.Songs[6].Name.Value);
            Assert.Equal(SongTransition.Immediate, set1.Songs[6].SongTransition);
            Assert.Equal(8, set1.Songs[7].Position.Value);
            Assert.Equal("Nuclear Fusion", set1.Songs[7].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[7].SongTransition);
            Assert.Equal(9, set1.Songs[8].Position.Value);
            Assert.Equal("All Is Known", set1.Songs[8].Name.Value);
            Assert.Equal(SongTransition.Immediate, set1.Songs[8].SongTransition);
            Assert.Equal(10, set1.Songs[9].Position.Value);
            Assert.Equal("Straws In The Wind", set1.Songs[9].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[9].SongTransition);
            Assert.Equal(11, set1.Songs[10].Position.Value);
            Assert.Equal("The Garden Goblin", set1.Songs[10].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[10].SongTransition);
            Assert.Equal(12, set1.Songs[11].Position.Value);
            Assert.Equal("The River", set1.Songs[11].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[11].SongTransition);
            Assert.Equal(13, set1.Songs[12].Position.Value);
            Assert.Equal("Magma", set1.Songs[12].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[12].SongTransition);

            // Verify Set 2
            var set2 = setlist.Sets[1];
            Assert.Equal("Set 2", set2.Name.Value);
            Assert.Equal(14, set2.Songs.Count);
            Assert.Equal(1, set2.Songs[0].Position.Value);
            Assert.Equal("Rattlesnake", set2.Songs[0].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[0].SongTransition);
            Assert.Equal(2, set2.Songs[1].Position.Value);
            Assert.Equal("Automation", set2.Songs[1].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[1].SongTransition);
            Assert.Equal(3, set2.Songs[2].Position.Value);
            Assert.Equal("Honey", set2.Songs[2].Name.Value);
            Assert.Equal(SongTransition.Segue, set2.Songs[2].SongTransition);
            Assert.Equal(4, set2.Songs[3].Position.Value);
            Assert.Equal("Sleep Drifter", set2.Songs[3].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[3].SongTransition);
            Assert.Equal(5, set2.Songs[4].Position.Value);
            Assert.Equal("Ataraxia", set2.Songs[4].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[4].SongTransition);
            Assert.Equal(6, set2.Songs[5].Position.Value);
            Assert.Equal("Evil Death Roll", set2.Songs[5].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[5].SongTransition);
            Assert.Equal(7, set2.Songs[6].Position.Value);
            Assert.Equal("Ice V", set2.Songs[6].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[6].SongTransition);
            Assert.Equal(8, set2.Songs[7].Position.Value);
            Assert.Equal("The Reticent Raconteur", set2.Songs[7].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[7].SongTransition);
            Assert.Equal(9, set2.Songs[8].Position.Value);
            Assert.Equal("The Lord of Lightning", set2.Songs[8].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[8].SongTransition);
            Assert.Equal(10, set2.Songs[9].Position.Value);
            Assert.Equal("The Balrog", set2.Songs[9].Name.Value);
            Assert.Equal(SongTransition.Segue, set2.Songs[9].SongTransition);
            Assert.Equal(11, set2.Songs[10].Position.Value);
            Assert.Equal("Trapdoor", set2.Songs[10].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[10].SongTransition);
            Assert.Equal(12, set2.Songs[11].Position.Value);
            Assert.Equal("Hot Water", set2.Songs[11].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[11].SongTransition);
            Assert.Equal(13, set2.Songs[12].Position.Value);
            Assert.Equal("The Grim Reaper", set2.Songs[12].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[12].SongTransition);
            Assert.Equal(14, set2.Songs[13].Position.Value);
            Assert.Equal("Planet B", set2.Songs[13].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[13].SongTransition);
        }
    }
}
