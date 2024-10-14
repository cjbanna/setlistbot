using Newtonsoft.Json;
using Xunit;

namespace Setlistbot.Domain.UnitTests
{
    public class NonEmptyStringTests
    {
        [Fact]
        public void NonEmptyString_WhenValueIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var value = string.Empty;

            // Act
            void Action() => new NonEmptyString(value);

            // Assert
            Assert.Throws<ArgumentException>(Action);
        }

        [Fact]
        public void NonEmptyString_WhenValueIsWhitespace_ThrowsArgumentException()
        {
            // Arrange
            var value = " ";

            // Act
            void Action() => new NonEmptyString(value);

            // Assert
            Assert.Throws<ArgumentException>(Action);
        }

        [Fact]
        public void NonEmptyString_WhenValueIsNotNullOrWhiteSpace_ReturnsNonEmptyString()
        {
            // Arrange
            var value = "value";

            // Act
            var nonEmptyString = new NonEmptyString(value);

            // Assert
            Assert.Equal(value, nonEmptyString);
        }

        [Fact]
        public void CompareTo_WhenValueIsEqual_ReturnsZero()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = new NonEmptyString(value);

            // Act
            var result = nonEmptyString.CompareTo(value);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CompareTo_WhenValueIsNotEqual_ReturnsNonZero()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = new NonEmptyString(value);

            // Act
            var result = nonEmptyString.CompareTo("other");

            // Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Equals_WhenValueIsEqual_ReturnsTrue()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = new NonEmptyString(value);

            // Act
            var result = nonEmptyString.Equals(value);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WhenValueIsNotEqual_ReturnsFalse()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = new NonEmptyString(value);

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
            var nonEmptyString1 = new NonEmptyString(value);
            var nonEmptyString2 = new NonEmptyString(value);

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
            var nonEmptyString1 = new NonEmptyString(value);
            var nonEmptyString2 = new NonEmptyString("other");

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
            var nonEmptyString = new NonEmptyString(value);

            // Act
            var result1 = nonEmptyString.GetHashCode();
            var result2 = new NonEmptyString(value).GetHashCode();

            // Assert
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void GetHashCode_WhenValueIsNotEqual_ReturnsDifferentHashCodes()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = new NonEmptyString(value);

            // Act
            var result1 = nonEmptyString.GetHashCode();
            var result2 = new NonEmptyString("other").GetHashCode();

            // Assert
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void ToString_WhenValueIsNotNullOrWhiteSpace_ReturnsValue()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = new NonEmptyString(value);

            // Act
            var result = nonEmptyString.ToString();

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void ImplicitOperator_WhenValueIsNotNullOrWhiteSpace_ReturnsValue()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = new NonEmptyString(value);

            // Act
            string result = nonEmptyString;

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void ImplicitOperator_WhenValueIsNotNullOrWhiteSpace_ReturnsNonEmptyString()
        {
            // Arrange
            var value = "value";

            // Act
            NonEmptyString result = value;

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void NewtonsoftJsonSerialize_WhenValueIsNotNullOrWhiteSpace_ReturnsValue()
        {
            // Arrange
            var value = "value";
            var nonEmptyString = new NonEmptyString(value);

            // Act
            var result = JsonConvert.SerializeObject(nonEmptyString);

            // Assert
            Assert.Equal($"\"{value}\"", result);
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
            Assert.Equal(value, result);
        }
    }
}
