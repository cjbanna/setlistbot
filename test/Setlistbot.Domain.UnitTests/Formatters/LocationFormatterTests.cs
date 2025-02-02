using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.UnitTests.Formatters
{
    public sealed class LocationFormatterTests
    {
        [Fact]
        public void Format_WhenLocation_ExpectFormattedLocation()
        {
            // Arrange
            var location = new Location(
                Venue.From("McNichols Arena"),
                City.From("Denver"),
                State.From("CO"),
                Country.From("USA")
            );

            var formatter = new LocationFormatter(location);

            // Act
            var actual = formatter.Format();

            // Assert
            actual.Should().Be("McNichols Arena, Denver, CO, USA");
        }
    }
}
