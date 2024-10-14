using Microsoft.Extensions.Logging;
using Moq;
using Setlistbot.Domain;

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

            Assert.Collection(
                setlist.Sets,
                set => Assert.Equal("Set 1", set.Name.Value),
                set => Assert.Equal("Set 2", set.Name.Value),
                set => Assert.Equal("Encore", set.Name.Value)
            );

            var set1 = setlist.Sets.First(s => s.Name == "Set 1");
            Assert.Collection(
                set1.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position.Value);
                    Assert.Equal("Mike's Song", song.Name.Value);
                    Assert.Equal(SongTransition.Segue, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position.Value);
                    Assert.Equal("I Am Hydrogen", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(3, song.Position.Value);
                    Assert.Equal("Weekapaug Groove", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(4, song.Position.Value);
                    Assert.Equal("Harry Hood", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(5, song.Position.Value);
                    Assert.Equal("Train Song", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(6, song.Position.Value);
                    Assert.Equal("Billy Breathes", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(7, song.Position.Value);
                    Assert.Equal("Frankenstein", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(8, song.Position.Value);
                    Assert.Equal("Izabella", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                }
            );

            var set2 = setlist.Sets.First(s => s.Name == "Set 2");
            Assert.Collection(
                set2.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position.Value);
                    Assert.Equal("Halley's Comet", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position.Value);
                    Assert.Equal("Tweezer", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(3, song.Position.Value);
                    Assert.Equal("Black-Eyed Katy", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(4, song.Position.Value);
                    Assert.Equal("Piper", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(5, song.Position.Value);
                    Assert.Equal("Run Like an Antelope", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                    Assert.Equal("Lyric changed to \"Michael Esquandolas.\"", song.Footnote);
                }
            );

            var encore = setlist.Sets.First(s => s.Name == "Encore");
            Assert.Collection(
                encore.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position.Value);
                    Assert.Equal("Bouncing Around the Room", song.Name.Value);
                    Assert.Equal(SongTransition.Immediate, song.SongTransition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position.Value);
                    Assert.Equal("Tweezer Reprise", song.Name.Value);
                    Assert.Equal(SongTransition.Stop, song.SongTransition);
                }
            );
        }
    }
}
