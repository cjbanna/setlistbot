using Setlistbot.Domain.Formatters;
using Xunit;

namespace Setlistbot.Domain.UnitTests.Formatters
{
    public class LocationFormatterTests
    {
        [Fact]
        public void Format_WhenLocation_ExpectFormattedLocation()
        {
            // Arrange
            var location = new Location(
                new Venue("McNichols Arena"),
                new City("Denver"),
                new State("CO"),
                new Country("USA")
            );

            var formatter = new LocationFormatter(location);

            // Act
            var actual = formatter.Format();

            // Assert
            Assert.Equal("McNichols Arena, Denver, CO, USA", actual);
        }
    }
}
