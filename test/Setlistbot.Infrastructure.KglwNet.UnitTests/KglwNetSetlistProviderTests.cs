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
            setlists.Should().NotBeNull();

            var setlist = setlists.Should().ContainSingle();
            setlist.Subject.Date.ToString("yyyy-MM-dd").Should().Be("2022-10-10");
            setlist.Subject.Location.Venue.Should().Be(Venue.From("Red Rocks Amphitheatre"));
            setlist.Subject.Location.City.Should().Be(City.From("Morrison"));
            setlist.Subject.Location.State.Should().Be(State.From("CO"));
            setlist.Subject.Location.Country.Should().Be(Country.From("USA"));
            setlist
                .Subject.Notes.Should()
                .Be(
                    "The intro to O.N.E. contained Straws in the Wind teases. The River contained Wah Wah teases and quotes throughout, as well as Crumbling Castle teases. The Land Before Timeland was played over the PA as the setbreak music, which incidentally served as the “debut” for the studio track. Rattlesnake contained teases and quotes from O.N.E., Automation, Honey, and Minimum Brain Size, and teases of Sleep Drifter. Honey contained teases of Sleep Drifter and Billabong Valley. The Reticent Raconteur through The Balrog featured Leah Senior (introduced as “The Balrog” and “the best singer”) providing the narration. Ambrose quoted “Happy Birthday, Lisa” (The Simpsons) while referencing Joey’s “Australian birthday” prior to The Grim Reaper. The Grim Reaper was then introduced as “some weird-ass Satanic rap”. Venusian 2 and Am I in Heaven? were on the printed setlist but were replaced with Planet B due to time constraints."
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
                                    song.Name.Value.Should().Be("Mars For The Rich");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(2);
                                    song.Name.Value.Should().Be("Hell");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(3);
                                    song.Name.Value.Should().Be("Magenta Mountain");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(4);
                                    song.Name.Value.Should().Be("Inner Cell");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(5);
                                    song.Name.Value.Should().Be("Loyalty");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(6);
                                    song.Name.Value.Should().Be("Horology");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(7);
                                    song.Name.Value.Should().Be("O.N.E.");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(8);
                                    song.Name.Value.Should().Be("Nuclear Fusion");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(9);
                                    song.Name.Value.Should().Be("All Is Known");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(10);
                                    song.Name.Value.Should().Be("Straws In The Wind");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(11);
                                    song.Name.Value.Should().Be("The Garden Goblin");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(12);
                                    song.Name.Value.Should().Be("The River");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(13);
                                    song.Name.Value.Should().Be("Magma");
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
                                    song.Name.Value.Should().Be("Rattlesnake");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(2);
                                    song.Name.Value.Should().Be("Automation");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(3);
                                    song.Name.Value.Should().Be("Honey");
                                    song.SongTransition.Should().Be(SongTransition.Segue);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(4);
                                    song.Name.Value.Should().Be("Sleep Drifter");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(5);
                                    song.Name.Value.Should().Be("Ataraxia");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(6);
                                    song.Name.Value.Should().Be("Evil Death Roll");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(7);
                                    song.Name.Value.Should().Be("Ice V");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(8);
                                    song.Name.Value.Should().Be("The Reticent Raconteur");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(9);
                                    song.Name.Value.Should().Be("The Lord of Lightning");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(10);
                                    song.Name.Value.Should().Be("The Balrog");
                                    song.SongTransition.Should().Be(SongTransition.Segue);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(11);
                                    song.Name.Value.Should().Be("Trapdoor");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(12);
                                    song.Name.Value.Should().Be("Hot Water");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(13);
                                    song.Name.Value.Should().Be("The Grim Reaper");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Position.Value.Should().Be(14);
                                    song.Name.Value.Should().Be("Planet B");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                }
                            );
                    }
                );
        }
    }
}
