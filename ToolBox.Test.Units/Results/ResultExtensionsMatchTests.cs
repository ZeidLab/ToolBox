using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NSubstitute;
using ZeidLab.ToolBox.Results;
using ZeidLab.ToolBox.Test.Units.Results;

namespace ZeidLab.ToolBox.Tests.Results
{
    [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
    public class ResultExtensionsMatchTests
    {
        [Fact]
        public void Match_WithSuccessResult_ShouldInvokeSuccessFunction()
        {
            // Arrange
            var successResult = TestHelper.CreateSuccessResult(42);
            Func<int, Result<string>> successFunc = value => Result.Success(value.ToString());
            Func<ResultError, Result<string>> failureFunc = _ => throw new InvalidOperationException("Should not be called");

            // Act
            var result = successResult.Match(successFunc, failureFunc);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("42");
        }

        [Fact]
        public void Match_WithFailureResult_ShouldInvokeFailureFunction()
        {
            // Arrange
            var failureResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
            Func<int, Result<string>> successFunc = _ => throw new InvalidOperationException("Should not be called");
            Func<ResultError, Result<string>> failureFunc = error => Result.Failure<string>(error);

            // Act
            var result = failureResult.Match(successFunc, failureFunc);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TestHelper.DefaultResultError);
        }

        [Fact]
        public void Match_WithSuccessResultAndValueReturn_ShouldInvokeSuccessFunction()
        {
            // Arrange
            var successResult = TestHelper.CreateSuccessResult(42);
            Func<int, string> successFunc = value => value.ToString();
            Func<ResultError, string> failureFunc = _ => throw new InvalidOperationException("Should not be called");

            // Act
            var result = successResult.Match(successFunc, failureFunc);

            // Assert
            result.Should().Be("42");
        }

        [Fact]
        public void Match_WithFailureResultAndValueReturn_ShouldInvokeFailureFunction()
        {
            // Arrange
            var failureResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
            Func<int, string> successFunc = _ => throw new InvalidOperationException("Should not be called");
            Func<ResultError, string> failureFunc = error => error.Message;

            // Act
            var result = failureResult.Match(successFunc, failureFunc);

            // Assert
            result.Should().Be(TestHelper.DefaultResultError.Message);
        }

        [Fact]
        public void Match_WithTrySuccess_ShouldInvokeSuccessFunction()
        {
            // Arrange
            var trySuccess = TestHelper.CreateTryFuncWithSuccess(42);
            Func<int, Result<string>> successFunc = value => Result.Success(value.ToString());
            Func<ResultError, Result<string>> failureFunc = _ => throw new InvalidOperationException("Should not be called");

            // Act
            var result = trySuccess.Match(successFunc, failureFunc);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("42");
        }

        [Fact]
        public void Match_WithTryFailure_ShouldInvokeFailureFunction()
        {
            // Arrange
            var tryFailure = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, Result<string>> successFunc = _ => throw new InvalidOperationException("Should not be called");
            Func<ResultError, Result<string>> failureFunc = error => Result.Failure<string>(error);

            // Act
            var result = tryFailure.Match(successFunc, failureFunc);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TestHelper.DefaultResultError);
        }

        [Fact]
        public void Match_WithTrySuccessAndValueReturn_ShouldInvokeSuccessFunction()
        {
            // Arrange
            var trySuccess = TestHelper.CreateTryFuncWithSuccess(42);
            Func<int, string> successFunc = value => value.ToString();
            Func<ResultError, string> failureFunc = _ => throw new InvalidOperationException("Should not be called");

            // Act
            var result = trySuccess.Match(successFunc, failureFunc);

            // Assert
            result.Should().Be("42");
        }

        [Fact]
        public void Match_WithTryFailureAndValueReturn_ShouldInvokeFailureFunction()
        {
            // Arrange
            var tryFailure = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, string> successFunc = _ => throw new InvalidOperationException("Should not be called");
            Func<ResultError, string> failureFunc = error => error.Message;

            // Act
            var result = tryFailure.Match(successFunc, failureFunc);

            // Assert
            result.Should().Be(TestHelper.DefaultResultError.Message);
        }

        [Fact]
        public void Match_WithSuccessResultAndAction_ShouldInvokeSuccessAction()
        {
            // Arrange
            var successResult = TestHelper.CreateSuccessResult(42);
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            successResult.Match(successAction, failureAction);

            // Assert
            successAction.Received(1).Invoke(42);
            failureAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public void Match_WithFailureResultAndAction_ShouldInvokeFailureAction()
        {
            // Arrange
            var failureResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            failureResult.Match(successAction, failureAction);

            // Assert
            successAction.DidNotReceiveWithAnyArgs().Invoke(default);
            failureAction.Received(1).Invoke(TestHelper.DefaultResultError);
        }

        [Fact]
        public async Task MatchAsync_WithSuccessTaskResult_ShouldInvokeSuccessFunction()
        {
            // Arrange
            var successTaskResult = Task.FromResult(TestHelper.CreateSuccessResult(42));
            Func<int, string> successFunc = value => value.ToString();
            Func<ResultError, string> failureFunc = _ => throw new InvalidOperationException("Should not be called");

            // Act
            var result = await successTaskResult.MatchAsync(successFunc, failureFunc);

            // Assert
            result.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_WithFailureTaskResult_ShouldInvokeFailureFunction()
        {
            // Arrange
            var failureTaskResult = Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
            Func<int, string> successFunc = _ => throw new InvalidOperationException("Should not be called");
            Func<ResultError, string> failureFunc = error => error.Message;

            // Act
            var result = await failureTaskResult.MatchAsync(successFunc, failureFunc);

            // Assert
            result.Should().Be(TestHelper.DefaultResultError.Message);
        }

        [Fact]
        public async Task MatchAsync_WithTryAsyncSuccess_ShouldInvokeSuccessFunction()
        {
            // Arrange
            var tryAsyncSuccess = TestHelper.CreateTryAsyncFuncWithSuccess(42);
            Func<int, string> successFunc = value => value.ToString();
            Func<ResultError, string> failureFunc = _ => throw new InvalidOperationException("Should not be called");

            // Act
            var result = await tryAsyncSuccess.MatchAsync(successFunc, failureFunc);

            // Assert
            result.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_WithTryAsyncFailure_ShouldInvokeFailureFunction()
        {
            // Arrange
            var tryAsyncFailure = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, string> successFunc = _ => throw new InvalidOperationException("Should not be called");
            Func<ResultError, string> failureFunc = error => error.Message;

            // Act
            var result = await tryAsyncFailure.MatchAsync(successFunc, failureFunc);

            // Assert
            result.Should().Be(TestHelper.DefaultResultError.Message);
        }

        [Fact]
        public async Task MatchAsync_WithSuccessTaskResultAndAction_ShouldInvokeSuccessAction()
        {
            // Arrange
            var successTaskResult = Task.FromResult(TestHelper.CreateSuccessResult(42));
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            await successTaskResult.MatchAsync(successAction, failureAction);

            // Assert
            successAction.Received(1).Invoke(42);
            failureAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_WithFailureTaskResultAndAction_ShouldInvokeFailureAction()
        {
            // Arrange
            var failureTaskResult = Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            await failureTaskResult.MatchAsync(successAction, failureAction);

            // Assert
            successAction.DidNotReceiveWithAnyArgs().Invoke(default);
            failureAction.Received(1).Invoke(TestHelper.DefaultResultError);
        }

        [Fact]
        public async Task MatchAsync_WithSuccessResultAndAsyncAction_ShouldInvokeSuccessAction()
        {
            // Arrange
            var successResult = TestHelper.CreateSuccessResult(42);
            var successAction = Substitute.For<Func<int, Task>>();
            var failureAction = Substitute.For<Func<ResultError, Task>>();

            // Act
            await successResult.MatchAsync(successAction, failureAction);

            // Assert
            await successAction.Received(1).Invoke(42);
            await failureAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_WithFailureResultAndAsyncAction_ShouldInvokeFailureAction()
        {
            // Arrange
            var failureResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
            var successAction = Substitute.For<Func<int, Task>>();
            var failureAction = Substitute.For<Func<ResultError, Task>>();

            // Act
            await failureResult.MatchAsync(successAction, failureAction);

            // Assert
            await successAction.DidNotReceiveWithAnyArgs().Invoke(default);
            await failureAction.Received(1).Invoke(TestHelper.DefaultResultError);
        }

        [Fact]
        public async Task MatchAsync_WithTrySuccessAndAsyncAction_ShouldInvokeSuccessAction()
        {
            // Arrange
            var trySuccess = TestHelper.CreateTryFuncWithSuccess(42);
            var successAction = Substitute.For<Func<int, Task>>();
            var failureAction = Substitute.For<Func<ResultError, Task>>();

            // Act
            await trySuccess.MatchAsync(successAction, failureAction);

            // Assert
            await successAction.Received(1).Invoke(42);
            await failureAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_WithTryFailureAndAsyncAction_ShouldInvokeFailureAction()
        {
            // Arrange
            var tryFailure = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
            var successAction = Substitute.For<Func<int, Task>>();
            var failureAction = Substitute.For<Func<ResultError, Task>>();

            // Act
            await tryFailure.MatchAsync(successAction, failureAction);

            // Assert
            await successAction.DidNotReceiveWithAnyArgs().Invoke(default);
            await failureAction.Received(1).Invoke(TestHelper.DefaultResultError);
        }

        [Fact]
        public async Task MatchAsync_WithSuccessTaskResultAndAsyncAction_ShouldInvokeSuccessAction()
        {
            // Arrange
            var successTaskResult = Task.FromResult(TestHelper.CreateSuccessResult(42));
            var successAction = Substitute.For<Func<int, Task>>();
            var failureAction = Substitute.For<Func<ResultError, Task>>();

            // Act
            await successTaskResult.MatchAsync(successAction, failureAction);

            // Assert
            await successAction.Received(1).Invoke(42);
            await failureAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_WithFailureTaskResultAndAsyncAction_ShouldInvokeFailureAction()
        {
            // Arrange
            var failureTaskResult = Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
            var successAction = Substitute.For<Func<int, Task>>();
            var failureAction = Substitute.For<Func<ResultError, Task>>();

            // Act
            await failureTaskResult.MatchAsync(successAction, failureAction);

            // Assert
            await successAction.DidNotReceiveWithAnyArgs().Invoke(default);
            await failureAction.Received(1).Invoke(TestHelper.DefaultResultError);
        }
    }
}