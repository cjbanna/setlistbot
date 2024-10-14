using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.GratefulDead.UnitTests
{
    public class GratefulDeadInMemoryProviderTests
    {
        [Fact]
        public async Task GetSetlists_WhenShowExists_ExpectSetlist()
        {
            // Arrange
            var provider = new GratefulDeadInMemoryProvider();

            // Act
            var setlists = await provider.GetSetlists(new DateOnly(1977, 5, 8));

            // Assert
            var setlist = setlists.Should().ContainSingle();
            setlist.Subject.ArtistId.Should().Be(ArtistId.From("gd"));
            setlist.Subject.Location.Should().NotBeNull();
            setlist.Subject.Location.City.Should().Be(City.From("Ithaca"));
            setlist.Subject.Location.State.Should().Be(State.From("NY"));
            setlist.Subject.Location.Country.Should().Be(Country.From("USA"));
            setlist
                .Subject.Location.Venue.Should()
                .Be(Venue.From("Barton Hall - Cornell University"));
            setlist.Subject.Date.Should().Be(new DateOnly(1977, 5, 8));
            setlist.Subject.SpotifyUrl.Value.Should().NotBeNullOrEmpty();
            setlist.Subject.Sets.Should().HaveCount(3);

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
                                    song.Name.Value.Should().Be("New Minglewood Blues");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Loser");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("El Paso");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("They Love Each Other");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Jack Straw");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Deal");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Lazy Lightnin'");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Supplication");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Brown Eyed Women");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Mama Tried");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Row Jimmy");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Dancing In The Street");
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
                                    song.Name.Value.Should().Be("Scarlet Begonias");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Fire On The Mountain");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Estimated Prophet");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Saint Stephen");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Not Fade Away");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Saint Stephen");
                                    song.SongTransition.Should().Be(SongTransition.Immediate);
                                },
                                song =>
                                {
                                    song.Name.Value.Should().Be("Morning Dew");
                                    song.SongTransition.Should().Be(SongTransition.Stop);
                                }
                            );
                    },
                    encore =>
                    {
                        encore.Name.Value.Should().Be("Encore");
                        encore
                            .Songs.Should()
                            .SatisfyRespectively(song =>
                            {
                                song.Name.Value.Should().Be("One More Saturday Night");
                                song.SongTransition.Should().Be(SongTransition.Stop);
                            });
                    }
                );
        }
    }
}
