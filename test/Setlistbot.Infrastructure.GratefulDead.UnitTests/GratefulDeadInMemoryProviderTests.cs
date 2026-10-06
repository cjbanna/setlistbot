using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.GratefulDead.UnitTests
{
    public sealed class GratefulDeadInMemoryProviderTests
    {
        [Theory]
        [InlineData(1972, 4, 7, "London", "", "England")]
        [InlineData(1990, 3, 21, "Hamilton", "Ontario", "Canada")]
        [InlineData(1970, 7, 3, "Calgary", "AB", "Canada")]
        [InlineData(1968, 2, 22, "North Shore, Lake Tahoe", "CA", "USA")]
        [InlineData(1969, 2, 4, "Omaha", "NE", "USA")]
        [InlineData(1969, 4, 15, "Omaha", "NE", "USA")]
        [InlineData(1966, 7, 29, "Vancouver", "BC", "Canada")]
        [InlineData(1966, 7, 30, "Vancouver", "BC", "Canada")]
        [InlineData(1967, 7, 31, "Toronto", "ON", "Canada")]
        [InlineData(1967, 8, 4, "Toronto", "ON", "Canada")]
        [InlineData(1969, 7, 8, "Toronto", "ON", "Canada")]
        public async Task GetSetlists_ParsesLocation(
            int year,
            int month,
            int day,
            string city,
            string state,
            string country
        )
        {
            var provider = new GratefulDeadInMemoryProvider();

            var setlists = await provider.GetSetlists(new DateOnly(year, month, day));

            var location = Assert.Single(setlists).Location;
            Assert.Equal(City.From(city), location.City);
            Assert.Equal(state, location.State.HasValue ? location.State.Value.Value : "");
            Assert.Equal(Country.From(country), location.Country);
        }

        [Fact]
        public async Task GetSetlists_WhenShowExists_ExpectSetlist()
        {
            // Arrange
            var provider = new GratefulDeadInMemoryProvider();

            // Act
            var setlists = await provider.GetSetlists(new DateOnly(1977, 5, 8));

            // Assert
            var setlist = Assert.Single(setlists);
            Assert.Equal(ArtistId.From("gd"), setlist.ArtistId);
            Assert.NotNull(setlist.Location);
            Assert.Equal(City.From("Ithaca"), setlist.Location.City);
            Assert.Equal(State.From("NY"), setlist.Location.State);
            Assert.Equal(Country.From("USA"), setlist.Location.Country);
            Assert.Equal(Venue.From("Barton Hall - Cornell University"), setlist.Location.Venue);
            Assert.Equal(new DateOnly(1977, 5, 8), setlist.Date);
            Assert.NotNull(setlist.SpotifyUrl.Value);
            Assert.NotEmpty(setlist.SpotifyUrl.Value);
            Assert.Equal(3, setlist.Sets.Count);

            // Verify Set 1
            var set1 = setlist.Sets[0];
            Assert.Equal("Set 1", set1.Name.Value);
            Assert.Equal(12, set1.Songs.Count);
            Assert.Equal("New Minglewood Blues", set1.Songs[0].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[0].SongTransition);
            Assert.Equal("Loser", set1.Songs[1].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[1].SongTransition);
            Assert.Equal("El Paso", set1.Songs[2].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[2].SongTransition);
            Assert.Equal("They Love Each Other", set1.Songs[3].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[3].SongTransition);
            Assert.Equal("Jack Straw", set1.Songs[4].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[4].SongTransition);
            Assert.Equal("Deal", set1.Songs[5].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[5].SongTransition);
            Assert.Equal("Lazy Lightnin'", set1.Songs[6].Name.Value);
            Assert.Equal(SongTransition.Immediate, set1.Songs[6].SongTransition);
            Assert.Equal("Supplication", set1.Songs[7].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[7].SongTransition);
            Assert.Equal("Brown Eyed Women", set1.Songs[8].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[8].SongTransition);
            Assert.Equal("Mama Tried", set1.Songs[9].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[9].SongTransition);
            Assert.Equal("Row Jimmy", set1.Songs[10].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[10].SongTransition);
            Assert.Equal("Dancing In The Street", set1.Songs[11].Name.Value);
            Assert.Equal(SongTransition.Stop, set1.Songs[11].SongTransition);

            // Verify Set 2
            var set2 = setlist.Sets[1];
            Assert.Equal("Set 2", set2.Name.Value);
            Assert.Equal(7, set2.Songs.Count);
            Assert.Equal("Scarlet Begonias", set2.Songs[0].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[0].SongTransition);
            Assert.Equal("Fire On The Mountain", set2.Songs[1].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[1].SongTransition);
            Assert.Equal("Estimated Prophet", set2.Songs[2].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[2].SongTransition);
            Assert.Equal("Saint Stephen", set2.Songs[3].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[3].SongTransition);
            Assert.Equal("Not Fade Away", set2.Songs[4].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[4].SongTransition);
            Assert.Equal("Saint Stephen", set2.Songs[5].Name.Value);
            Assert.Equal(SongTransition.Immediate, set2.Songs[5].SongTransition);
            Assert.Equal("Morning Dew", set2.Songs[6].Name.Value);
            Assert.Equal(SongTransition.Stop, set2.Songs[6].SongTransition);

            // Verify Encore
            var encore = setlist.Sets[2];
            Assert.Equal("Encore", encore.Name.Value);
            Assert.Single(encore.Songs);
            Assert.Equal("One More Saturday Night", encore.Songs[0].Name.Value);
            Assert.Equal(SongTransition.Stop, encore.Songs[0].SongTransition);
        }
    }
}
