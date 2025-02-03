using Setlistbot.Domain.Extensions;

namespace Setlistbot.Domain.UnitTests.Extensions
{
    public class DateParseExtensionsTests
    {
        [Theory]
        [InlineData("This is a date: 2023-10-01")]
        [InlineData("Another date: 2023/10/01")]
        [InlineData("Date with dots: 10.1.2023")]
        [InlineData("Date with mixed delimiters: 2023-10/01")]
        [InlineData("Date with spaces: 2023 10 01")]
        [InlineData("Date with 2 digit year: 10-1-23")]
        public void ParseDates_ValidDate_ReturnsDateOnly(string input)
        {
            // Arrange
            var expectedDate = new DateOnly(2023, 10, 1);

            // Act
            var result = input.ParseDates();

            // Assert
            Assert.Single(result);
            Assert.Contains(expectedDate, result);
        }

        [Fact]
        public void ParseDates_MultipleDates_ReturnsUniqueDates()
        {
            // Arrange
            var input = "Dates: 2023-10-01, 2023-10-01, 2023-10-02";
            var expectedDates = new List<DateOnly>
            {
                new DateOnly(2023, 10, 1),
                new DateOnly(2023, 10, 2),
            };

            // Act
            var result = input.ParseDates();

            // Assert
            Assert.Equal(expectedDates.Count, result.Count());
            Assert.Contains(expectedDates[0], result);
            Assert.Contains(expectedDates[1], result);
        }

        [Fact]
        public void ParseDates_NoDates_ReturnsEmptyCollection()
        {
            // Arrange
            var input = "No dates here.";

            // Act
            var result = input.ParseDates();

            // Assert
            Assert.Empty(result);
        }
    }
}
