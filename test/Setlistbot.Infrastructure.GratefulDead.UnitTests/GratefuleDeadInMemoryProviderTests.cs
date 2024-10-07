namespace Setlistbot.Infrastructure.GratefulDead.UnitTests
{
    public class GratefuleDeadInMemoryProviderTests
    {
        [Fact]
        public async Task GetSetlists_WhenShowExists_ExpectSetlist()
        {
            // Arrange
            var provider = new GratefulDeadInMemoryProvider();

            // Act
            var setlists = await provider.GetSetlists(new DateTime(1977, 5, 8));

            // Assert
            var setlist = Assert.Single(setlists);
            Assert.Equal("gd", setlist.ArtistId);
            Assert.NotNull(setlist.Location);
            Assert.Equal("Ithaca", setlist.Location.City);
            Assert.Equal("NY", setlist.Location.State);
            Assert.Equal("USA", setlist.Location.Country);
            Assert.Equal("Barton Hall - Cornell University", setlist.Location.Venue);
            Assert.Equal(new DateTime(1977, 5, 8), setlist.Date);
            Assert.NotEmpty(setlist.SpotifyUrl);
            Assert.Collection(
                setlist.Sets,
                set =>
                {
                    Assert.Equal("Set 1", set.Name);
                    Assert.Collection(
                        set.Songs,
                        song =>
                        {
                            Assert.Equal("New Minglewood Blues", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Loser", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("El Paso", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("They Love Each Other", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Jack Straw", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Deal", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Lazy Lightnin'", song.Name);
                            Assert.Equal(">", song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Supplication", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Brown Eyed Women", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Mama Tried", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Row Jimmy", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Dancing In The Street", song.Name);
                            Assert.Empty(song.SongTransition);
                        }
                    );
                },
                set =>
                {
                    Assert.Equal("Set 2", set.Name);
                    Assert.Collection(
                        set.Songs,
                        song =>
                        {
                            Assert.Equal("Scarlet Begonias", song.Name);
                            Assert.Equal(">", song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Fire On The Mountain", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Estimated Prophet", song.Name);
                            Assert.Empty(song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Saint Stephen", song.Name);
                            Assert.Equal(">", song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Not Fade Away", song.Name);
                            Assert.Equal(">", song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Saint Stephen", song.Name);
                            Assert.Equal(">", song.SongTransition);
                        },
                        song =>
                        {
                            Assert.Equal("Morning Dew", song.Name);
                            Assert.Empty(song.SongTransition);
                        }
                    );
                },
                encore =>
                {
                    Assert.Equal("Encore", encore.Name);
                    Assert.Collection(
                        encore.Songs,
                        song =>
                        {
                            Assert.Equal("One More Saturday Night", song.Name);
                            Assert.Empty(song.SongTransition);
                        }
                    );
                }
            );
        }
    }
}
