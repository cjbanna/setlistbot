using Microsoft.Extensions.Logging;
using Moq;

namespace Setlistbot.Infrastructure.PhishNet.UnitTests
{
    public class PhishNetSetlistProviderTestFixture
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

    public class PhishNetSetlistProviderTests
    {
        [Fact]
        public async Task GetSetlistsAsync_WhenSetlistResponse_ExpectSetlist()
        {
            // Arrange
            var fixture = new PhishNetSetlistProviderTestFixture();
            var date = new DateTime(1997, 11, 22);

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

            Assert.Collection(
                setlist.Sets,
                set => Assert.Equal("Set 1", set.Name),
                set => Assert.Equal("Set 2", set.Name),
                set => Assert.Equal("Encore", set.Name)
            );

            var set1 = setlist.Sets.First(s => s.Name == "Set 1");
            Assert.Collection(
                set1.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position);
                    Assert.Equal("Mike's Song", song.Name);
                    Assert.Equal("->", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position);
                    Assert.Equal("I Am Hydrogen", song.Name);
                    Assert.Equal(">", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(3, song.Position);
                    Assert.Equal("Weekapaug Groove", song.Name);
                    Assert.Equal(",", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(4, song.Position);
                    Assert.Equal("Harry Hood", song.Name);
                    Assert.Equal(">", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(5, song.Position);
                    Assert.Equal("Train Song", song.Name);
                    Assert.Equal(",", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(6, song.Position);
                    Assert.Equal("Billy Breathes", song.Name);
                    Assert.Equal(",", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(7, song.Position);
                    Assert.Equal("Frankenstein", song.Name);
                    Assert.Equal(">", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(8, song.Position);
                    Assert.Equal("Izabella", song.Name);
                    Assert.Equal("", song.SongTransition);
                }
            );

            var set2 = setlist.Sets.First(s => s.Name == "Set 2");
            Assert.Collection(
                set2.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position);
                    Assert.Equal("Halley's Comet", song.Name);
                    Assert.Equal(">", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position);
                    Assert.Equal("Tweezer", song.Name);
                    Assert.Equal(">", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(3, song.Position);
                    Assert.Equal("Black-Eyed Katy", song.Name);
                    Assert.Equal(">", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(4, song.Position);
                    Assert.Equal("Piper", song.Name);
                    Assert.Equal(">", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(5, song.Position);
                    Assert.Equal("Run Like an Antelope", song.Name);
                    Assert.Equal("", song.SongTransition);
                    Assert.Equal("Lyric changed to \"Michael Esquandolas.\"", song.Footnote);
                }
            );

            var encore = setlist.Sets.First(s => s.Name == "Encore");
            Assert.Collection(
                encore.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position);
                    Assert.Equal("Bouncing Around the Room", song.Name);
                    Assert.Equal(">", song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position);
                    Assert.Equal("Tweezer Reprise", song.Name);
                    Assert.Equal("", song.SongTransition);
                }
            );
        }
    }
}
