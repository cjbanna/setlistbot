using CSharpFunctionalExtensions;
using Setlistbot.Domain.Formatters;
using Xunit;

namespace Setlist.Domain.UnitTests.Formatters
{
    public class MaybeFormatterTests
    {
        [Fact]
        public void Format_WhenHasValue_ExpectFormattedValue()
        {
            // Arrange
            var maybe = Maybe.From("value");
            var formatter = new MaybeFormatter<string>(maybe, new LiteralFormatter(maybe.Value));

            // Act
            var actual = formatter.Format();

            // Assert
            Assert.Equal("value", actual);
        }

        [Fact]
        public void Format_WhenHasNoValueAndDefaultFormatterProvided_ExpectDefaultFormatter()
        {
            // Arrange
            var maybe = Maybe<string>.None;
            var foo = new LiteralFormatter("foo");
            var defaultFormatter = new LiteralFormatter("default");
            var formatter = new MaybeFormatter<string>(maybe, foo, defaultFormatter);

            // Act
            var actual = formatter.Format();

            // Assert
            Assert.Equal("default", actual);
        }

        [Fact]
        public void Format_WhenHasNoValue_ExpectEmptyString()
        {
            // Arrange
            var maybe = Maybe<string>.None;
            var formatter = new MaybeFormatter<string>(
                maybe,
                () => new LiteralFormatter(maybe.Value)
            );

            // Act
            var actual = formatter.Format();

            // Assert
            Assert.Equal(string.Empty, actual);
        }
    }
}
