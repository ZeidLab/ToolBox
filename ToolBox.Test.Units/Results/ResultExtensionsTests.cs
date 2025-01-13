using FluentAssertions;
using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results
{
    public class ResultExtensionsTests
    {
        // Helper method to create a sample error
        private static ResultError CreateSampleError(string message = "Sample error message")
        {
            return ResultError.New(message);
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
            result.ResultError.Should().Be(error);
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
            result.ResultError.Should().NotBeNull();
            result.ResultError.Exception.Should().Be(exception);
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
        
         [Fact]
        public void ToUnitResult_ShouldReturnSuccessResultWithUnit_WhenInputResultIsSuccess()
        {
            // Arrange
            var successResult = TestHelper.CreateSuccessResult(42);

            // Act
            var unitResult = successResult.ToUnitResult();

            // Assert
            unitResult.IsSuccess.Should().BeTrue();
            unitResult.Value.Should().Be(Unit.Default);
        }

        [Fact]
        public void ToUnitResult_ShouldReturnFailureResultWithError_WhenInputResultIsFailure()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var failureResult = TestHelper.CreateFailureResult<int>(error);

            // Act
            var unitResult = failureResult.ToUnitResult();

            // Assert
            unitResult.IsFailure.Should().BeTrue();
            unitResult.ResultError.Should().Be(error);
        }

        [Fact]
        public async Task ToUnitResultAsync_ShouldReturnSuccessResultWithUnit_WhenInputResultIsSuccess()
        {
            // Arrange
            var successResult = Task.FromResult(TestHelper.CreateSuccessResult(42));

            // Act
            var unitResult = await successResult.ToUnitResultAsync();

            // Assert
            unitResult.IsSuccess.Should().BeTrue();
            unitResult.Value.Should().Be(Unit.Default);
        }

        [Fact]
        public async Task ToUnitResultAsync_ShouldReturnFailureResultWithError_WhenInputResultIsFailure()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var failureResult = Task.FromResult(TestHelper.CreateFailureResult<int>(error));

            // Act
            var unitResult = await failureResult.ToUnitResultAsync();

            // Assert
            unitResult.IsFailure.Should().BeTrue();
            unitResult.ResultError.Should().Be(error);
        }
    }
}