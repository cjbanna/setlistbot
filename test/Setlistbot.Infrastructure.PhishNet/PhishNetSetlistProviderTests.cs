using Microsoft.Extensions.Logging;
using Moq;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.PhishNet.UnitTests
{
    public sealed class PhishNetSetlistProviderTestFixture
    {
        public Mock<ILogger<PhishNetSetlistProvider>> Logger { get; }

        public Mock<IPhishNetClient> PhishNetClient { get; }

        public PhishNetSetlistProvider PhishNetSetlistProvider { get; }

        public PhishNetSetlistProviderTestFixture()
        {
            Logger = new Mock<ILogger<PhishNetSetlistProvider>>();
            PhishNetClient = new Mock<IPhishNetClient>();
            PhishNetSetlistProvider = new PhishNetSetlistProvider(
                Logger.Object,
                PhishNetClient.Object
            );
        }
    }

    public sealed class PhishNetSetlistProviderTests
    {
        [Fact]
        public async Task GetSetlistsAsync_WhenSetlistResponse_ExpectSetlist()
        {
            // Arrange
            var fixture = new PhishNetSetlistProviderTestFixture();
            var date = new DateOnly(1997, 11, 22);

            var setlistResponse = TestData.GetSetlistResponseTestData();
            fixture
                .PhishNetClient.Setup(x => x.GetSetlistAsync(date))
                .ReturnsAsync(setlistResponse);

            // Act
            var setlists = await fixture.PhishNetSetlistProvider.GetSetlists(date);

            // Assert
            Assert.NotNull(setlists);

            var setlist = Assert.Single(setlists);
            Assert.Equal("1997-11-22", setlist.Date.ToString("yyyy-MM-dd"));
            Assert.Equal(
                "Mike&#39;s Song and Tweezer both contained BEK teases, with the ones in Tweezer taking place well before the segue into BEK. Fans of stage banter will want to seek out the second set for Trey&rsquo;s humorous response to the crowd&rsquo;s Destiny Unbound chant before Halley&rsquo;s. The &quot;Marco Esquandolas&quot; lyric in Antelope was changed to &quot;Michael Esquandolas.&quot;&nbsp;This show was released as part of the&nbsp;<em>Hampton/Winston-Salem &#39;97</em>&nbsp;box&nbsp;set.",
                setlist.Notes
            );

            // Verify Set 1
            Assert.Equal(3, setlist.Sets.Count);
            var set1 = setlist.Sets[0];
            Assert.Equal("Set 1", set1.Name.Value);
            Assert.Equal(8, set1.Songs.Count);
            Assert.Equal(1, set1.Songs[0].Position.Value);
            Assert.Equal("Mike's Song", set1.Songs[0].Name.Value);
            Assert.Equal(SongTransition.Segue, set1.Songs[0].SongTransition);
            Assert.Equal(2, set1.Songs[1].Position.Value);
            Assert.Equal("I Am Hydrogen", set1.Songs[1].Name.Value);
            Assert.Equal(SongTransition.Immediate, set1.Songs[1].SongTransition);
            Assert.Equal(3, set1.Songs[2].Position.Value);
            Assert.Equal("Weekapaug Groove", set1.Songs[2].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[2].SongTransition);
            Assert.Equal(4, set1.Songs[3].Position.Value);
            Assert.Equal("Harry Hood", set1.Songs[3].Name.Value);
            Assert.Equal(SongTransition.Immediate, set1.Songs[3].SongTransition);
            Assert.Equal(5, set1.Songs[4].Position.Value);
            Assert.Equal("Train Song", set1.Songs[4].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[4].SongTransition);
            Assert.Equal(6, set1.Songs[5].Position.Value);
            Assert.Equal("Billy Breathes", set1.Songs[5].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[5].SongTransition);
            Assert.Equal(7, set1.Songs[6].Position.Value);
            Assert.Equal("Frankenstein", set1.Songs[6].Name.Value);
            Assert.Equal(SongTransition.Immediate, set1.Songs[6].SongTransition);
            Assert.Equal(8, set1.Songs[7].Position.Value);
            Assert.Equal("Izabella", set1.Songs[7].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[7].SongTransition);

            // Verify Set 2
            var set2 = setlist.Sets[1];
            Assert.Equal("Set 2", set2.Name.Value);
            Assert.Equal(5, set2.Songs.Count);
            Assert.Equal(1, set2.Songs[0].Position.Value);
            Assert.Equal("Halley's Comet", set2.Songs[0].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[0].SongTransition);
            Assert.Equal(2, set2.Songs[1].Position.Value);
            Assert.Equal("Tweezer", set2.Songs[1].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[1].SongTransition);
            Assert.Equal(3, set2.Songs[2].Position.Value);
            Assert.Equal("Black-Eyed Katy", set2.Songs[2].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[2].SongTransition);
            Assert.Equal(4, set2.Songs[3].Position.Value);
            Assert.Equal("Piper", set2.Songs[3].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[3].SongTransition);
            Assert.Equal(5, set2.Songs[4].Position.Value);
            Assert.Equal("Run Like an Antelope", set2.Songs[4].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[4].SongTransition);
            Assert.Equal("Lyric changed to \"Michael Esquandolas.\"", set2.Songs[4].Footnote);

            // Verify Encore
            var encore = setlist.Sets[2];
            Assert.Equal("Encore", encore.Name.Value);
            Assert.Equal(2, encore.Songs.Count);
            Assert.Equal(1, encore.Songs[0].Position.Value);
            Assert.Equal("Bouncing Around the Room", encore.Songs[0].Name.Value);
            Assert.Equal(SongTransition.Immediate, encore.Songs[0].SongTransition);
            Assert.Equal(2, encore.Songs[1].Position.Value);
            Assert.Equal("Tweezer Reprise", encore.Songs[1].Name.Value);
            Assert.Equal(SongTransition.Stop, encore.Songs[1].SongTransition);
        }
    }
}
