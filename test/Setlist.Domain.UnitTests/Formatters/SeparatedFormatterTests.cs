using CSharpFunctionalExtensions;
using Setlistbot.Domain.Formatters;
using Xunit;

namespace Setlist.Domain.UnitTests.Formatters
{
    public class SeparatedFormatterTests
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
            Assert.Equal(string.Empty, actual);
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
            Assert.Equal("value", actual);
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
            Assert.Equal("value1, value2, value3, value4", actual);
        }

        // [Fact]
        // public void Format_WhenHasTwoMaybeFormatters_ExpectFormattedValuesSeparatedBySeparator()
        // {
        //     // Arrange
        //     var formatter = new SeparatedFormatter(
        //         new CombinedFormatter(new CharacterFormatter(','), new SpaceFormatter()),
        //         new IFormatter[]
        //         {
        //             new MaybeFormatter<string>(
        //                 Maybe.From("value1"),
        //                 new LiteralFormatter("value1")
        //             ),
        //             new MaybeFormatter<string>(
        //                 Maybe.From("value2"),
        //                 new LiteralFormatter("value2")
        //             )
        //         }
        //     );
        //
        //     // Act
        //     var actual = formatter.Format();
        //
        //     // Assert
        //     Assert.Equal("value1, value2", actual);
        // }
    }
}
