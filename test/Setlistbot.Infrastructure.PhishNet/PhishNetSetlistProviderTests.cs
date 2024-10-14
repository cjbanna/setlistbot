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
            setlists.Should().NotBeNull();

            var setlist = setlists.Should().ContainSingle();
            setlist.Subject.Date.ToString("yyyy-MM-dd").Should().Be("1997-11-22");
            setlist
                .Subject.Notes.Should()
                .Be(
                    "Mike&#39;s Song and Tweezer both contained BEK teases, with the ones in Tweezer taking place well before the segue into BEK. Fans of stage banter will want to seek out the second set for Trey&rsquo;s humorous response to the crowd&rsquo;s Destiny Unbound chant before Halley&rsquo;s. The &quot;Marco Esquandolas&quot; lyric in Antelope was changed to &quot;Michael Esquandolas.&quot;&nbsp;This show was released as part of the&nbsp;<em>Hampton/Winston-Salem &#39;97</em>&nbsp;box&nbsp;set."
                );

            setlist
                .Subject.Sets.Should()
                .SatisfyRespectively(
                    set =>
                    {
                        set.Name.Value.Should().Be("Set 1");
                        set.Songs.Should()
                            .SatisfyRespectively(
                                song =>
                                {
                                    song.Position.Value.Should().Be(1);
                                    song.Name.Value.Should().Be("Mike's Song");
                                    song.SongTransition.Should().Be(SongTransition.Segue);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(2);
                                    song.Name.Value.Should().Be("I Am Hydrogen");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(3);
                                    song.Name.Value.Should().Be("Weekapaug Groove");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(4);
                                    song.Name.Value.Should().Be("Harry Hood");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(5);
                                    song.Name.Value.Should().Be("Train Song");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(6);
                                    song.Name.Value.Should().Be("Billy Breathes");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(7);
                                    song.Name.Value.Should().Be("Frankenstein");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(8);
                                    song.Name.Value.Should().Be("Izabella");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                }
                            );
                    },
                    set =>
                    {
                        set.Name.Value.Should().Be("Set 2");
                        set.Songs.Should()
                            .SatisfyRespectively(
                                song =>
                                {
                                    song.Position.Value.Should().Be(1);
                                    song.Name.Value.Should().Be("Halley's Comet");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(2);
                                    song.Name.Value.Should().Be("Tweezer");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(3);
                                    song.Name.Value.Should().Be("Black-Eyed Katy");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(4);
                                    song.Name.Value.Should().Be("Piper");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(5);
                                    song.Name.Value.Should().Be("Run Like an Antelope");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                    song.Footnote.Should()
                                        .Be("Lyric changed to \"Michael Esquandolas.\"");
                                }
                            );
                    },
                    set =>
                    {
                        set.Name.Value.Should().Be("Encore");
                        set.Songs.Should()
                            .SatisfyRespectively(
                                song =>
                                {
                                    song.Position.Value.Should().Be(1);
                                    song.Name.Value.Should().Be("Bouncing Around the Room");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(2);
                                    song.Name.Value.Should().Be("Tweezer Reprise");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                }
                            );
                    }
                );
        }
    }
}
