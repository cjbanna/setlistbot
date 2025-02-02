using Newtonsoft.Json;
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

            // Act
            Action act = () => NonEmptyString.From(value);

            // Assert
            act.Should().ThrowExactly<ValueObjectValidationException>();
        }

        [Fact]
        public void NonEmptyString_WhenValueIsWhitespace_ThrowsArgumentException()
        {
            // Arrange
            var value = " ";

            // Act
            Action act = () => NonEmptyString.From(value);

            // Assert
            act.Should().ThrowExactly<ValueObjectValidationException>();
        }

        [Fact]
        public void NonEmptyString_WhenValueIsNotNullOrWhiteSpace_ReturnsNonEmptyString()
        {
            // Arrange
            const string value = "value";

            // Act
            var nonEmptyString = NonEmptyString.From(value);

            // Assert
            nonEmptyString.Should().Be(NonEmptyString.From(value));
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
            result.Should().Be(0);
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
            result.Should().NotBe(0);
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
            result.Should().BeTrue();
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
            result.Should().BeFalse();
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
            result.Should().BeTrue();
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
            result.Should().BeFalse();
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
            result2.Should().Be(result1);
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
            result2.Should().NotBe(result1);
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
            result.Should().Be(value);
        }

        [Fact]
        public void NewtonsoftJsonSerialize_WhenValueIsNotNullOrWhiteSpace_ReturnsValue()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = NonEmptyString.From(value);

            // Act
            var result = JsonConvert.SerializeObject(nonEmptyString);

            // Assert
            result.Should().Be($"\"{value}\"");
        }

        [Fact]
        public void NewtonsoftJsonDeserialize_WhenValueIsNotNullOrWhiteSpace_ReturnsValue()
        {
            // Arrange
            var value = "value";
            var json = $"\"{value}\"";

            // Act
            var result = JsonConvert.DeserializeObject<NonEmptyString>(json);

            // Assert
            result.Value.Should().Be(value);
        }
    }
}
