using FluentAssertions;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Test.Units.Options
{
    public class MaybeTests
    {
        // Helper function to create a Maybe with a value
        private Maybe<T> CreateMaybe<T>(T value) => Maybe.Some(value);

        // Helper function to create a Maybe in the 'None' state
        private Maybe<T> CreateNoneMaybe<T>() => Maybe.None<T>();

        [Fact]
        public void Some_WithNonNullValue_ShouldReturnMaybeWithValue()
        {
            // Arrange
            var value = 42;

            // Act
            var maybe = CreateMaybe(value);

            // Assert
            maybe.IsSome.Should().BeTrue();
            maybe.IsNone.Should().BeFalse();
            maybe.Value.Should().Be(value);
            maybe.IsDefault.Should().BeFalse();
        }

        [Fact]
        public void Some_WithNullValue_ShouldThrowArgumentNullException()
        {
            // Arrange
            string? value = null;

            // Act
            Action act = () => CreateMaybe(value);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void None_ShouldReturnMaybeInNoneState()
        {
            // Act
            var maybe = CreateNoneMaybe<int>();

            // Assert
            maybe.IsSome.Should().BeFalse();
            maybe.IsNone.Should().BeTrue();
            maybe.Value.Should().Be(default);
            maybe.IsDefault.Should().BeTrue();
        }

        [Fact]
        public void ImplicitConversion_WithNonNullValue_ShouldReturnMaybeWithValue()
        {
            // Arrange
            var value = "test";

            // Act
            Maybe<string> maybe = value;

            // Assert
            maybe.IsSome.Should().BeTrue();
            maybe.IsNone.Should().BeFalse();
            maybe.Value.Should().Be(value);
            maybe.IsDefault.Should().BeFalse();
        }

        [Fact]
        public void CompareTo_WithBothNone_ShouldReturnZero()
        {
            // Arrange
            var maybe1 = CreateNoneMaybe<int>();
            var maybe2 = CreateNoneMaybe<int>();

            // Act
            var result = maybe1.CompareTo(maybe2);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CompareTo_WithThisNoneAndOtherSome_ShouldReturnNegativeOne()
        {
            // Arrange
            var maybe1 = CreateNoneMaybe<int>();
            var maybe2 = CreateMaybe(42);

            // Act
            var result = maybe1.CompareTo(maybe2);

            // Assert
            result.Should().Be(-1);
        }

        [Fact]
        public void CompareTo_WithThisSomeAndOtherNone_ShouldReturnOne()
        {
            // Arrange
            var maybe1 = CreateMaybe(42);
            var maybe2 = CreateNoneMaybe<int>();

            // Act
            var result = maybe1.CompareTo(maybe2);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void CompareTo_WithBothSomeAndEqualValues_ShouldReturnZero()
        {
            // Arrange
            var maybe1 = CreateMaybe(42);
            var maybe2 = CreateMaybe(42);

            // Act
            var result = maybe1.CompareTo(maybe2);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CompareTo_WithBothSomeAndThisValueLessThanOther_ShouldReturnNegativeOne()
        {
            // Arrange
            var maybe1 = CreateMaybe(42);
            var maybe2 = CreateMaybe(100);

            // Act
            var result = maybe1.CompareTo(maybe2);

            // Assert
            result.Should().Be(-1);
        }

        [Fact]
        public void CompareTo_WithBothSomeAndThisValueGreaterThanOther_ShouldReturnOne()
        {
            // Arrange
            var maybe1 = CreateMaybe(100);
            var maybe2 = CreateMaybe(42);

            // Act
            var result = maybe1.CompareTo(maybe2);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void CompareTo_WithNonMaybeObject_ShouldThrowArgumentException()
        {
            // Arrange
            var maybe = CreateMaybe(42);
            var obj = new object();

            // Act
            Action act = () => maybe.CompareTo(obj);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void LessThanOperator_WithLeftLessThanRight_ShouldReturnTrue()
        {
            // Arrange
            var left = CreateMaybe(42);
            var right = CreateMaybe(100);

            // Act
            var result = left < right;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void LessThanOperator_WithLeftGreaterThanRight_ShouldReturnFalse()
        {
            // Arrange
            var left = CreateMaybe(100);
            var right = CreateMaybe(42);

            // Act
            var result = left < right;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void LessThanOrEqualOperator_WithLeftLessThanRight_ShouldReturnTrue()
        {
            // Arrange
            var left = CreateMaybe(42);
            var right = CreateMaybe(100);

            // Act
            var result = left <= right;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void LessThanOrEqualOperator_WithLeftEqualToRight_ShouldReturnTrue()
        {
            // Arrange
            var left = CreateMaybe(42);
            var right = CreateMaybe(42);

            // Act
            var result = left <= right;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GreaterThanOperator_WithLeftGreaterThanRight_ShouldReturnTrue()
        {
            // Arrange
            var left = CreateMaybe(100);
            var right = CreateMaybe(42);

            // Act
            var result = left > right;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GreaterThanOperator_WithLeftLessThanRight_ShouldReturnFalse()
        {
            // Arrange
            var left = CreateMaybe(42);
            var right = CreateMaybe(100);

            // Act
            var result = left > right;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GreaterThanOrEqualOperator_WithLeftGreaterThanRight_ShouldReturnTrue()
        {
            // Arrange
            var left = CreateMaybe(100);
            var right = CreateMaybe(42);

            // Act
            var result = left >= right;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GreaterThanOrEqualOperator_WithLeftEqualToRight_ShouldReturnTrue()
        {
            // Arrange
            var left = CreateMaybe(42);
            var right = CreateMaybe(42);

            // Act
            var result = left >= right;

            // Assert
            result.Should().BeTrue();
        }
    }
}