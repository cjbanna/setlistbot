using Microsoft.Extensions.Logging;
using Moq;

namespace Setlistbot.Infrastructure.ElGoose.UnitTests
{
    public class ElGooseSetlistProviderTestFixture
    {
        public Mock<ILogger<ElGooseSetlistProvider>> Logger { get; }

        public Mock<IElGooseClient> ElGooseClient { get; }

        public ElGooseSetlistProvider ElGooseSetlistProvider { get; }

        public ElGooseSetlistProviderTestFixture()
        {
            Logger = new Mock<ILogger<ElGooseSetlistProvider>>();
            ElGooseClient = new Mock<IElGooseClient>();
            ElGooseSetlistProvider = new ElGooseSetlistProvider(
                Logger.Object,
                ElGooseClient.Object
            );
        }
    }

    public class ElGooseSetlistProviderTests
    {
        [Fact]
        public async Task GetSetlistsAsync_WhenSetlistResponse_ExpectSetlist()
        {
            // Arrange
            var fixture = new ElGooseSetlistProviderTestFixture();
            var date = new DateTime(2023, 4, 14);

            var setlistResponse = TestData.GetSetlistResponseTestData();
            fixture.ElGooseClient.Setup(x => x.GetSetlistAsync(date)).ReturnsAsync(setlistResponse);

            // Act
            var setlists = await fixture.ElGooseSetlistProvider.GetSetlists(date);

            // Assert
            Assert.NotNull(setlists);

            var setlist = Assert.Single(setlists);
            Assert.Equal("2023-04-14", setlist.Date.ToString("yyyy-MM-dd"));
            Assert.Equal("The Salt Shed", setlist.Location.Venue);
            Assert.Equal("Chicago", setlist.Location.City);
            Assert.Equal("IL", setlist.Location.State);
            Assert.Equal("USA", setlist.Location.Country);
            Assert.Equal(
                "The band left the stage during Madhuvan and let the drone play through the encore break.",
                setlist.Notes
            );

            Assert.Collection(
                setlist.Sets,
                set => Assert.Equal("Set 1", set.Name),
                set => Assert.Equal("Set 2", set.Name),
                encore => Assert.Equal("Encore", encore.Name)
            );

            var set1 = setlist.Sets.First(s => s.Name == "Set 1");
            Assert.Collection(
                set1.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position);
                    Assert.Equal("All I Need", song.Name);
                    Assert.Equal(",", song.Transition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position);
                    Assert.Equal("The Whales", song.Name);
                    Assert.Equal(",", song.Transition);
                },
                song =>
                {
                    Assert.Equal(3, song.Position);
                    Assert.Equal("Earthling or Alien?", song.Name);
                    Assert.Equal(",", song.Transition);
                },
                song =>
                {
                    Assert.Equal(4, song.Position);
                    Assert.Equal("Jive I", song.Name);
                    Assert.Equal(">", song.Transition);
                },
                song =>
                {
                    Assert.Equal(5, song.Position);
                    Assert.Equal("Jive Lee", song.Name);
                    Assert.Equal(",", song.Transition);
                },
                song =>
                {
                    Assert.Equal(6, song.Position);
                    Assert.Equal("Everything Must Go", song.Name);
                    Assert.Equal(",", song.Transition);
                },
                song =>
                {
                    Assert.Equal(7, song.Position);
                    Assert.Equal("Thatch", song.Name);
                    Assert.Equal("", song.Transition);
                }
            );

            var set2 = setlist.Sets.First(s => s.Name == "Set 2");
            Assert.Collection(
                set2.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position);
                    Assert.Equal("Hungersite", song.Name);
                    Assert.Equal(">", song.Transition);
                },
                song =>
                {
                    Assert.Equal(2, song.Position);
                    Assert.Equal("Into The Myst", song.Name);
                    Assert.Equal(">", song.Transition);
                },
                song =>
                {
                    Assert.Equal(3, song.Position);
                    Assert.Equal("Red Bird", song.Name);
                    Assert.Equal(",", song.Transition);
                },
                song =>
                {
                    Assert.Equal(4, song.Position);
                    Assert.Equal("Seekers On The Ridge Pt. 1", song.Name);
                    Assert.Equal(">", song.Transition);
                },
                song =>
                {
                    Assert.Equal(5, song.Position);
                    Assert.Equal("Seekers On The Ridge Pt. 2", song.Name);
                    Assert.Equal(",", song.Transition);
                },
                song =>
                {
                    Assert.Equal(6, song.Position);
                    Assert.Equal("Madhuvan", song.Name);
                    Assert.Equal("", song.Transition);
                }
            );

            var encore = setlist.Sets.First(s => s.Name == "Encore");
            Assert.Collection(
                encore.Songs,
                song =>
                {
                    Assert.Equal(1, song.Position);
                    Assert.Equal("Tomorrow Never Knows", song.Name);
                    Assert.Equal("", song.Transition);
                }
            );
        }
    }
}
