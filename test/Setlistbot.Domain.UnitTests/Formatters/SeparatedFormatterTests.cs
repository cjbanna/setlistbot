using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.UnitTests.Formatters
{
    public sealed class SeparatedFormatterTests
    {
        [Fact]
        public void Format_WhenHasNoFormatters_ExpectEmptyString()
        {
            // Arrange
            var formatter = new SeparatedFormatter(
                new CharacterFormatter(','),
                Array.Empty<IFormatter>()
            );

            // Act
            var actual = formatter.Format();

            // Assert
            actual.Should().Be(string.Empty);
        }

        [Fact]
        public void Format_WhenHasOneFormatter_ExpectFormattedValue()
        {
            // Arrange
            var formatter = new SeparatedFormatter(
                new CharacterFormatter(','),
                new IFormatter[] { new LiteralFormatter("value") }
            );

            // Act
            var actual = formatter.Format();

            // Assert
            actual.Should().Be("value");
        }

        [Fact]
        public void Format_WhenHasMultipleFormatters_ExpectFormattedValuesSeparatedBySeparator()
        {
            // Arrange
            var formatter = new SeparatedFormatter(
                new CombinedFormatter(new CharacterFormatter(','), new SpaceFormatter()),
                new IFormatter[]
                {
                    new LiteralFormatter("value1"),
                    new LiteralFormatter("value2"),
                    new LiteralFormatter("value3"),
                    new LiteralFormatter("value4"),
                }
            );

            // Act
            var actual = formatter.Format();

            // Assert
            actual.Should().Be("value1, value2, value3, value4");
        }
    }
}
