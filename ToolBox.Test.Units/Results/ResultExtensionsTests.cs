using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results
{
    public class ResultExtensionsTests
    {
        // Helper method to create a sample error
        private static Error CreateSampleError(string message = "Sample error message")
        {
            return Error.New(message);
        }

        // Helper method to create a sample exception
        private static Exception CreateSampleException(string message = "Sample exception message")
        {
            return new Exception(message);
        }

        [Fact]
        public void ToSuccess_WithValidValue_ShouldReturnSuccessfulResult()
        {
            // Arrange
            var value = 42;

            // Act
            var result = value.ToSuccess();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(value);
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeNull();
        }

        [Fact]
        public void ToSuccess_WithNullValue_ShouldThrowArgumentNullException()
        {
            // Arrange
            string value = null;

            // Act
            Action act = () => value.ToSuccess();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToFailure_WithError_ShouldReturnFailedResult()
        {
            // Arrange
            var error = CreateSampleError();

            // Act
            var result = error.ToFailure<int>();

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(error);
            result.IsSuccess.Should().BeFalse();
            result.Value.Should().Be(default);
        }

        [Fact]
        public void ToFailure_WithException_ShouldReturnFailedResult()
        {
            // Arrange
            var exception = CreateSampleException();

            // Act
            var result = exception.ToFailure<int>();

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.GetValueOrDefault().Exception.Should().Be(exception);
            result.IsSuccess.Should().BeFalse();
            result.Value.Should().Be(default);
        }

        [Fact]
        public void ToMaybe_WithSuccessfulResult_ShouldReturnSome()
        {
            // Arrange
            var value = 42;
            var result = value.ToSuccess();

            // Act
            var maybe = result.ToMaybe();

            // Assert
            maybe.IsSome.Should().BeTrue();
            maybe.IsNone.Should().BeFalse();
            maybe.Content.Should().Be(value);
        }

        [Fact]
        public void ToMaybe_WithFailedResult_ShouldReturnNone()
        {
            // Arrange
            var error = CreateSampleError();
            var result = error.ToFailure<int>();

            // Act
            var maybe = result.ToMaybe();

            // Assert
            maybe.IsNone.Should().BeTrue();
            maybe.IsSome.Should().BeFalse();
            maybe.Content.Should().Be(default);
        }

        [Fact]
        public void ToMaybe_WithDefaultValue_ShouldReturnSomeWithDefaultValue()
        {
            // Arrange
            var value = default(int);
            var result = value.ToSuccess();

            // Act
            var maybe = result.ToMaybe();

            // Assert
            maybe.IsSome.Should().BeTrue();
            maybe.IsNone.Should().BeFalse();
            maybe.Content.Should().Be(value);
        }
        

        [Fact]
        public void ToFailure_WithNullException_ShouldThrowArgumentNullException()
        {
            // Arrange
            Exception exception = null!;

            // Act
            Action act = () => exception.ToFailure<int>();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}