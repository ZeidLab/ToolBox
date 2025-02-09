using FluentAssertions;
using NSubstitute;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results
{
    public class ResultExtensionsTapTests
    {
        // Helper methods
        private static Result<T> CreateSuccessResult<T>(T value) => Result.Success(value);
        private static Result<T> CreateFailureResult<T>(ResultError error) => Result.Failure<T>(error);

        // Tests for Tap (synchronous)
        [Fact]
        public void Tap_ShouldExecuteAction_WhenResultIsSuccess()
        {
            // Arrange
            var result = CreateSuccessResult(42);
            var action = Substitute.For<Action<int>>();

            // Act
            var returnedResult = result.Tap(action);

            // Assert
            returnedResult.Should().Be(result);
            action.Received(1).Invoke(42);
        }

        [Fact]
        public void Tap_ShouldNotExecuteAction_WhenResultIsFailure()
        {
            // Arrange
            var result = CreateFailureResult<int>(ResultError.New("Error"));
            var action = Substitute.For<Action<int>>();

            // Act
            var returnedResult = result.Tap(action);

            // Assert
            returnedResult.Should().Be(result);
            action.DidNotReceiveWithAnyArgs().Invoke(default);
        }


        [Fact]
        public void TapAsync_WithAction_ShouldThrowArgumentNullException_WhenActionIsNull()
        {
            // Arrange
            var result = CreateSuccessResult(42);

            // Act
            Func<Task> act = () => result.TapAsync(null);

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>();
        }

        // Tests for TapAsync (with Func)
        [Fact]
        public async Task TapAsync_WithFunc_ShouldExecuteFunc_WhenResultIsSuccess()
        {
            // Arrange
            var result = CreateSuccessResult(42);
            var func = Substitute.For<Func<int, Task>>();

            // Act
            var returnedResult = await result.TapAsync(func);

            // Assert
            returnedResult.Should().Be(result);
            await func.Received(1).Invoke(42);
        }

        [Fact]
        public async Task TapAsync_WithFunc_ShouldNotExecuteFunc_WhenResultIsFailure()
        {
            // Arrange
            var result = CreateFailureResult<int>(ResultError.New("Error"));
            var func = Substitute.For<Func<int, Task>>();

            // Act
            var returnedResult = await result.TapAsync(func);

            // Assert
            returnedResult.Should().Be(result);
            await func.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public void TapAsync_WithFunc_ShouldThrowArgumentNullException_WhenFuncIsNull()
        {
            // Arrange
            var result = CreateSuccessResult(42);

            // Act
            Func<Task> act = () => result.TapAsync(null);

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>();
        }

        // Tests for TapAsync with Try
        [Fact]
        public async Task TapAsync_WithTry_ShouldExecuteFunc_WhenTryIsSuccess()
        {
            // Arrange
            var result = CreateSuccessResult(42);
            var tryResult = new Try<int>(() => result);
            var func = Substitute.For<Func<int, Task>>();

            // Act
            var returnedResult = await tryResult.TapAsync(func);

            // Assert
            returnedResult.Should().Be(result);
            await func.Received(1).Invoke(42);
        }

        [Fact]
        public async Task TapAsync_WithTry_ShouldNotExecuteFunc_WhenTryIsFailure()
        {
            // Arrange
            var result = CreateFailureResult<int>(ResultError.New("Error"));
            var tryResult = new Try<int>(() => result);
            var func = Substitute.For<Func<int, Task>>();

            // Act
            var returnedResult = await tryResult.TapAsync(func);

            // Assert
            returnedResult.Should().Be(result);
            await func.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public void TapAsync_WithTry_ShouldThrowArgumentNullException_WhenFuncIsNull()
        {
            // Arrange
            var result = CreateSuccessResult(42);
            var tryResult = new Try<int>(() => result);

            // Act
            Func<Task> act = () => tryResult.TapAsync(null);

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>();
        }

        // Tests for TapAsync with TryAsync
        [Fact]
        public async Task TapAsync_WithTryAsync_ShouldExecuteFunc_WhenTryAsyncIsSuccess()
        {
            // Arrange
            var result = CreateSuccessResult(42);
            var tryAsyncResult = new TryAsync<int>(() => Task.FromResult(result));
            var func = Substitute.For<Func<int, Task>>();

            // Act
            var returnedResult = await tryAsyncResult.TapAsync(func);

            // Assert
            returnedResult.Should().Be(result);
            await func.Received(1).Invoke(42);
        }

        [Fact]
        public async Task TapAsync_WithTryAsync_ShouldNotExecuteFunc_WhenTryAsyncIsFailure()
        {
            // Arrange
            var result = CreateFailureResult<int>(ResultError.New("Error"));
            var tryAsyncResult = new TryAsync<int>(() => Task.FromResult(result));
            var func = Substitute.For<Func<int, Task>>();

            // Act
            var returnedResult = await tryAsyncResult.TapAsync(func);

            // Assert
            returnedResult.Should().Be(result);
            await func.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public void TapAsync_WithTryAsync_ShouldThrowArgumentNullException_WhenFuncIsNull()
        {
            // Arrange
            var result = CreateSuccessResult(42);
            var tryAsyncResult = new TryAsync<int>(() => Task.FromResult(result));

            // Act
            Func<Task> act = () => tryAsyncResult.TapAsync(null);

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}