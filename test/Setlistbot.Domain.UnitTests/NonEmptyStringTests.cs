using System.Text.Json;
using Vogen;

namespace Setlistbot.Domain.UnitTests
{
    public sealed class NonEmptyStringTests
    {
        [Fact]
        public void NonEmptyString_WhenValueIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var value = string.Empty;

            // Act & Assert
            Assert.Throws<ValueObjectValidationException>(() => NonEmptyString.From(value));
        }

        [Fact]
        public void NonEmptyString_WhenValueIsWhitespace_ThrowsArgumentException()
        {
            // Arrange
            var value = " ";

            // Act & Assert
            Assert.Throws<ValueObjectValidationException>(() => NonEmptyString.From(value));
        }

        [Fact]
        public void NonEmptyString_WhenValueIsNotNullOrWhiteSpace_ReturnsNonEmptyString()
        {
            // Arrange
            const string value = "value";

            // Act
            var nonEmptyString = NonEmptyString.From(value);

            // Assert
            Assert.Equal(NonEmptyString.From(value), nonEmptyString);
        }

        [Fact]
        public void CompareTo_WhenValueIsEqual_ReturnsZero()
        {
            // Arrange
            var value = NonEmptyString.From("value");
            var other = NonEmptyString.From("value");

            // Act
            var result = value.CompareTo(other);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CompareTo_WhenValueIsNotEqual_ReturnsNonZero()
        {
            // Arrange
            var value = NonEmptyString.From("value");
            var other = NonEmptyString.From("other");

            // Act
            var result = value.CompareTo(other);

            // Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Equals_WhenValueIsEqual_ReturnsTrue()
        {
            // Arrange
            var nonEmptyString = NonEmptyString.From("value");
            var other = NonEmptyString.From("value");

            // Act
            var result = nonEmptyString.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WhenValueIsNotEqual_ReturnsFalse()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = NonEmptyString.From(value);

            // Act
            var result = nonEmptyString.Equals("other");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsOperator_WhenValuesAreEqual_ReturnsTrue()
        {
            // Arrange
            var value = "value";
            var nonEmptyString1 = NonEmptyString.From(value);
            var nonEmptyString2 = NonEmptyString.From(value);

            // Act
            var result = nonEmptyString1 == nonEmptyString2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsOperator_WhenValuesAreNotEqual_ReturnsFalse()
        {
            // Arrange
            var value = "value";
            var nonEmptyString1 = NonEmptyString.From(value);
            var nonEmptyString2 = NonEmptyString.From("other");

            // Act
            var result = nonEmptyString1 == nonEmptyString2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_WhenValueIsEqual_ReturnsEqualHashCodes()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = NonEmptyString.From(value);

            // Act
            var result1 = nonEmptyString.GetHashCode();
            var result2 = NonEmptyString.From(value).GetHashCode();

            // Assert
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void GetHashCode_WhenValueIsNotEqual_ReturnsDifferentHashCodes()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = NonEmptyString.From(value);

            // Act
            var result1 = nonEmptyString.GetHashCode();
            var result2 = NonEmptyString.From("other").GetHashCode();

            // Assert
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void ToString_WhenValueIsNotNullOrWhiteSpace_ReturnsValue()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = NonEmptyString.From(value);

            // Act
            var result = nonEmptyString.ToString();

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void JsonSerialize_WhenValueIsNotNullOrWhiteSpace_ReturnsValue()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = NonEmptyString.From(value);

            // Act
            var result = JsonSerializer.Serialize(nonEmptyString);

            // Assert
            Assert.Equal($"\"{value}\"", result);
        }

        [Fact]
        public void JsonDeserialize_WhenValueIsNotNullOrWhiteSpace_ReturnsValue()
        {
            // Arrange
            var value = "value";
            var json = $"\"{value}\"";

            // Act
            var result = JsonSerializer.Deserialize<NonEmptyString>(json);

            // Assert
            Assert.Equal(value, result.Value);
        }
    }
}
