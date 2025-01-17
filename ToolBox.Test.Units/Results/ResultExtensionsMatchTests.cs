using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;
using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsMatchTests
{
    // Helper function to create a successful Result<T>
    private static Result<T> CreateSuccessResult<T>(T value) => Result<T>.Success(value);

    // Helper function to create a failed Result<T> with an Error
    private static Result<T> CreateFailureResult<T>(ResultError resultError) => Result<T>.Failure(resultError);

    // Helper function to create a Try<int> with a failure
    private static Try<int> CreateTryFuncWithFailure(ResultError resultError) =>
        new Try<int>(() => CreateFailureResult<int>(resultError));

    // Helper function to create a Try<int> with a failure
    private static TryAsync<int> CreateTryAsyncFuncWithFailure(ResultError resultError) =>
        new TryAsync<int>(() => CreateFailureResult<int>(resultError).AsTaskAsync());

    [Fact]
    public void Match_ShouldReturnSuccessResult_WhenInputResultIsSuccess()
    {
        // Arrange
        var successValue = 42;
        var successResult = CreateSuccessResult(successValue);

        // Act
        var result = successResult
            .Match(value => CreateSuccessResult(value.ToString()),
                error => CreateFailureResult<string>(error));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue.ToString());
    }

    [Fact]
    public void Match_ShouldReturnFailureResult_WhenInputResultIsFailure()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var failureResult = CreateFailureResult<int>(error);

        // Act
        var result = failureResult
            .Match(value => CreateSuccessResult(value.ToString()),
                err => CreateFailureResult<string>(err));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Should().Be(error);
    }

    [Fact]
    public void Match_ShouldReturnSuccessValue_WhenInputResultIsSuccess()
    {
        // Arrange
        var successValue = 42;
        var successResult = CreateSuccessResult(successValue);

        // Act
        var result = successResult
            .Match(value => value.ToString(),
                error => error.Message);

        // Assert
        result.Should().Be(successValue.ToString());
    }

    [Fact]
    public void Match_ShouldReturnFailureValue_WhenInputResultIsFailure()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var failureResult = CreateFailureResult<int>(error);

        // Act
        var result = failureResult
            .Match(value => value.ToString(),
                err => err.Message);

        // Assert
        result.Should().Be(error.Message);
    }

    [Fact]
    public void Match_ShouldReturnSuccessResult_WhenInputTryIsSuccess()
    {
        // Arrange
        var successValue = 42;
        var tryFunc = new Try<int>(() => CreateSuccessResult(successValue));

        // Act
        var result = tryFunc
            .Match(value => CreateSuccessResult(value.ToString()),
                error => CreateFailureResult<string>(error));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue.ToString());
    }

    [Fact]
    public void Match_ShouldReturnFailureResult_WhenInputTryIsFailure()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var tryFunc = CreateTryFuncWithFailure(error);

        // Act
        var result = tryFunc
            .Match(value => CreateSuccessResult(value.ToString()),
                err => CreateFailureResult<string>(err));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Should().Be(error);
    }

    [Fact]
    public void Match_ShouldHandleException_WhenInputTryThrowsException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var tryFunc = new Try<int>(() => throw exception);

        // Act
        var result = tryFunc
            .Match(value => CreateSuccessResult(value.ToString()),
                err => CreateFailureResult<string>(err));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Exception.Should().Be(exception);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnSuccessValue_WhenInputTaskResultIsSuccess()
    {
        // Arrange
        var successValue = 42;
        var successResult = Task.FromResult(CreateSuccessResult(successValue));

        // Act
        var result = await successResult
            .MatchAsync(value => value.ToString(),
                error => error.Message);

        // Assert
        result.Should().Be(successValue.ToString());
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnFailureValue_WhenInputTaskResultIsFailure()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var failureResult = Task.FromResult(CreateFailureResult<int>(error));

        // Act
        var result = await failureResult
            .MatchAsync(value => value.ToString(),
                err => err.Message);

        // Assert
        result.Should().Be(error.Message);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnSuccessResult_WhenInputTaskResultIsSuccess()
    {
        // Arrange
        var successValue = 42;
        var successResult = Task.FromResult(CreateSuccessResult(successValue));

        // Act
        var result = await successResult
            .MatchAsync(value => CreateSuccessResult(value.ToString()),
                error => CreateFailureResult<string>(error));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue.ToString());
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnFailureResult_WhenInputTaskResultIsFailure()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var failureResult = Task.FromResult(CreateFailureResult<int>(error));

        // Act
        var result = await failureResult
            .MatchAsync(value => CreateSuccessResult(value.ToString()),
                err => CreateFailureResult<string>(err));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Should().Be(error);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnSuccessResult_WhenInputTryAsyncIsSuccess()
    {
        // Arrange
        var successValue = 42;
        var tryAsyncFunc = new TryAsync<int>(() => CreateSuccessResult(successValue).AsTaskAsync());

        // Act
        var result = await tryAsyncFunc
            .MatchAsync(value => CreateSuccessResult(value.ToString()),
                error => CreateFailureResult<string>(error));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue.ToString());
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnFailureResult_WhenInputTryAsyncIsFailure()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var tryAsyncFunc = CreateTryAsyncFuncWithFailure(error);

        // Act
        var result = await tryAsyncFunc
            .MatchAsync(value => CreateSuccessResult(value.ToString()),
                err => CreateFailureResult<string>(err));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Should().Be(error);
    }

    [Fact]
    public async Task MatchAsync_ShouldHandleException_WhenInputTryAsyncThrowsException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var tryAsyncFunc = new TryAsync<int>(() => throw exception);

        // Act
        var result = await tryAsyncFunc
            .MatchAsync(value => CreateSuccessResult(value.ToString()),
                err => CreateFailureResult<string>(err));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Exception.Should().Be(exception);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnSuccessValue_WhenInputTryAsyncIsSuccessWithAsyncFuncs()
    {
        // Arrange
        var successValue = 42;
        var tryAsyncFunc = new TryAsync<int>(() => CreateSuccessResult(successValue).AsTaskAsync());

        // Act
        var result = await tryAsyncFunc
            .MatchAsync(value => Task.FromResult(value.ToString()),
                error => Task.FromResult(error.Message));

        // Assert
        result.Should().Be(successValue.ToString());
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnFailureValue_WhenInputTryAsyncIsFailureWithAsyncFuncs()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var tryAsyncFunc = CreateTryAsyncFuncWithFailure(error);

        // Act
        var result = await tryAsyncFunc
            .MatchAsync(value => Task.FromResult(value.ToString()),
                err => Task.FromResult(err.Message));

        // Assert
        result.Should().Be(error.Message);
    }

    [Fact]
    public void Match_ShouldReturnSuccessResult_WhenInputTryIsSuccessWithValue()
    {
        // Arrange
        var successValue = 42;
        var tryFunc = new Try<int>(() => CreateSuccessResult(successValue));

        // Act
        var result = tryFunc
            .Match(value => value.ToString(),
                error => error.Message);

        // Assert
        result.Should().Be(successValue.ToString());
    }

    [Fact]
    public void Match_ShouldReturnFailureResult_WhenInputTryIsFailureWithValue()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var tryFunc = CreateTryFuncWithFailure(error);

        // Act
        var result = tryFunc
            .Match(value => value.ToString(),
                err => err.Message);

        // Assert
        result.Should().Be(error.Message);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnSuccessResult_WhenInputTaskResultIsSuccessWithAsyncFuncs()
    {
        // Arrange
        var successValue = 42;
        var successResult = Task.FromResult(CreateSuccessResult(successValue));

        // Act
        var result = await successResult
            .MatchAsync(value => Task.FromResult(CreateSuccessResult(value.ToString())),
                error => Task.FromResult(CreateFailureResult<string>(error)));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue.ToString());
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnFailureResult_WhenInputTaskResultIsFailureWithAsyncFuncs()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var failureResult = Task.FromResult(CreateFailureResult<int>(error));

        // Act
        var result = await failureResult
            .MatchAsync(value => Task.FromResult(CreateSuccessResult(value.ToString())),
                err => Task.FromResult(CreateFailureResult<string>(err)));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Should().Be(error);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnSuccessValue_WhenInputTryAsyncIsSuccessWithValue()
    {
        // Arrange
        var successValue = 42;
        var tryAsyncFunc = new TryAsync<int>(() => CreateSuccessResult(successValue).AsTaskAsync());

        // Act
        var result = await tryAsyncFunc
            .MatchAsync(value => value.ToString(),
                error => error.Message);

        // Assert
        result.Should().Be(successValue.ToString());
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnFailureValue_WhenInputTryAsyncIsFailureWithValue()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var tryAsyncFunc = CreateTryAsyncFuncWithFailure(error);

        // Act
        var result = await tryAsyncFunc
            .MatchAsync(value => value.ToString(),
                err => err.Message);

        // Assert
        result.Should().Be(error.Message);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnSuccessResult_WhenInputTryAsyncIsSuccessWithAsyncFuncs()
    {
        // Arrange
        var successValue = 42;
        var tryAsyncFunc = new TryAsync<int>(() => CreateSuccessResult(successValue).AsTaskAsync());

        // Act
        var result = await tryAsyncFunc
            .MatchAsync(value => Task.FromResult(CreateSuccessResult(value.ToString())),
                error => Task.FromResult(CreateFailureResult<string>(error)));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue.ToString());
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnFailureResult_WhenInputTryAsyncIsFailureWithAsyncFuncs()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var tryAsyncFunc = CreateTryAsyncFuncWithFailure(error);

        // Act
        var result = await tryAsyncFunc
            .MatchAsync(value => Task.FromResult(CreateSuccessResult(value.ToString())),
                err => Task.FromResult(CreateFailureResult<string>(err)));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Should().Be(error);
    }

     [Fact]
        public void Match_ShouldInvokeSuccessAction_WhenResultIsSuccess()
        {
            // Arrange
            var successValue = 42;
            var successResult = TestHelper.CreateSuccessResult(successValue);
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            successResult.Match(successAction, failureAction);

            // Assert
            successAction.Received(1).Invoke(successValue);
            failureAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public void Match_ShouldInvokeFailureAction_WhenResultIsFailure()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var failureResult = TestHelper.CreateFailureResult<int>(error);
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            failureResult.Match(successAction, failureAction);

            // Assert
            failureAction.Received(1).Invoke(error);
            successAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public void Match_ShouldInvokeSuccessFunc_WhenResultIsSuccess()
        {
            // Arrange
            var successValue = 42;
            var successResult = TestHelper.CreateSuccessResult(successValue);
            var successFunc = Substitute.For<Func<int, string>>();
            var failureFunc = Substitute.For<Func<ResultError, string>>();
            successFunc.Invoke(successValue).Returns("Success");

            // Act
            var result = successResult.Match(successFunc, failureFunc);

            // Assert
            result.Should().Be("Success");
            successFunc.Received(1).Invoke(successValue);
            failureFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public void Match_ShouldInvokeFailureFunc_WhenResultIsFailure()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var failureResult = TestHelper.CreateFailureResult<int>(error);
            var successFunc = Substitute.For<Func<int, string>>();
            var failureFunc = Substitute.For<Func<ResultError, string>>();
            failureFunc.Invoke(error).Returns("Failure");

            // Act
            var result = failureResult.Match(successFunc, failureFunc);

            // Assert
            result.Should().Be("Failure");
            failureFunc.Received(1).Invoke(error);
            successFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeSuccessFunc_WhenTaskResultIsSuccess()
        {
            // Arrange
            var successValue = 42;
            var successResult = Task.FromResult(TestHelper.CreateSuccessResult(successValue));
            var successFunc = Substitute.For<Func<int, Task<string>>>();
            var failureFunc = Substitute.For<Func<ResultError, Task<string>>>();
            successFunc.Invoke(successValue).Returns(Task.FromResult("Success"));

            // Act
            var result =await await successResult.MatchAsync(successFunc, failureFunc);

            // Assert
            result.Should().Be("Success");
            await successFunc.Received(1).Invoke(successValue);
            await failureFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeFailureFunc_WhenTaskResultIsFailure()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var failureResult = Task.FromResult(TestHelper.CreateFailureResult<int>(error));
            var successFunc = Substitute.For<Func<int, string>>();
            var failureFunc = Substitute.For<Func<ResultError, string>>();
            failureFunc.Invoke(error).Returns("Failure");

            // Act
            var result = await failureResult.MatchAsync(successFunc, failureFunc);

            // Assert
            result.Should().Be("Failure");
            failureFunc.Received(1).Invoke(error);
            successFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeSuccessAction_WhenTaskResultIsSuccess()
        {
            // Arrange
            var successValue = 42;
            var successResult = Task.FromResult(TestHelper.CreateSuccessResult(successValue));
            var successAction = Substitute.For<Func<int, Task>>();
            var failureAction = Substitute.For<Func<ResultError, Task>>();

            // Act
            await successResult.MatchAsync(successAction, failureAction);

            // Assert
            await successAction.Received(1).Invoke(successValue);
            await failureAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeFailureAction_WhenTaskResultIsFailure()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var failureResult = Task.FromResult(TestHelper.CreateFailureResult<int>(error));
            var successAction = Substitute.For<Func<int, Task>>();
            var failureAction = Substitute.For<Func<ResultError, Task>>();

            // Act
            await failureResult.MatchAsync(successAction, failureAction);

            // Assert
            await failureAction.Received(1).Invoke(error);
            await successAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeSuccessFunc_WhenTryAsyncIsSuccess()
        {
            // Arrange
            var successValue = 42;
            var tryAsyncFunc = new TryAsync<int>(() => TestHelper.CreateSuccessResult(successValue).AsTaskAsync());
            var successFunc = Substitute.For<Func<int, Task<string>>>();
            var failureFunc = Substitute.For<Func<ResultError, Task<string>>>();
            successFunc.Invoke(successValue).Returns(Task.FromResult("Success"));

            // Act
            var result = await tryAsyncFunc.MatchAsync(successFunc, failureFunc);

            // Assert
            result.Should().Be("Success");
            await successFunc.Received(1).Invoke(successValue);
            await failureFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeFailureFunc_WhenTryAsyncIsFailure()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var tryAsyncFunc = TestHelper.CreateTryAsyncFuncWithFailure<int>(error);
            var successFunc = Substitute.For<Func<int, Task<string>>>();
            var failureFunc = Substitute.For<Func<ResultError, Task<string>>>();
            failureFunc.Invoke(error).Returns(Task.FromResult("Failure"));

            // Act
            var result = await tryAsyncFunc.MatchAsync(successFunc, failureFunc);

            // Assert
            result.Should().Be("Failure");
            await failureFunc.Received(1).Invoke(error);
            await successFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public void Match_ShouldInvokeSuccessAction_WhenTryIsSuccess()
        {
            // Arrange
            var successValue = 42;
            var tryFunc = new Try<int>(() => TestHelper.CreateSuccessResult(successValue));
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            tryFunc.Match(successAction, failureAction);

            // Assert
            successAction.Received(1).Invoke(successValue);
            failureAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public void Match_ShouldInvokeFailureAction_WhenTryIsFailure()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var tryFunc = TestHelper.CreateTryFuncWithFailure<int>(error);
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            tryFunc.Match(successAction, failureAction);

            // Assert
            failureAction.Received(1).Invoke(error);
            successAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeSuccessAction_WhenTryAsyncIsSuccess()
        {
            // Arrange
            var successValue = 42;
            var tryAsyncFunc = new TryAsync<int>(() => TestHelper.CreateSuccessResult(successValue).AsTaskAsync());
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            await tryAsyncFunc.MatchAsync(successAction, failureAction);

            // Assert
            successAction.Received(1).Invoke(successValue);
            failureAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeFailureAction_WhenTryAsyncIsFailure()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var tryAsyncFunc = TestHelper.CreateTryAsyncFuncWithFailure<int>(error);
            var successAction = Substitute.For<Action<int>>();
            var failureAction = Substitute.For<Action<ResultError>>();

            // Act
            await tryAsyncFunc.MatchAsync(successAction, failureAction);

            // Assert
            failureAction.Received(1).Invoke(error);
            successAction.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeSuccessFunc_WhenResultIsSuccessWithAsyncFuncs()
        {
            // Arrange
            var successValue = 42;
            var successResult = TestHelper.CreateSuccessResult(successValue);
            var successFunc = Substitute.For<Func<int, Task>>();
            var failureFunc = Substitute.For<Func<ResultError, Task>>();

            // Act
            await successResult.MatchAsync(successFunc, failureFunc);

            // Assert
            await successFunc.Received(1).Invoke(successValue);
            await failureFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeFailureFunc_WhenResultIsFailureWithAsyncFuncs()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var failureResult = TestHelper.CreateFailureResult<int>(error);
            var successFunc = Substitute.For<Func<int, Task>>();
            var failureFunc = Substitute.For<Func<ResultError, Task>>();

            // Act
            await failureResult.MatchAsync(successFunc, failureFunc);

            // Assert
            await failureFunc.Received(1).Invoke(error);
            await successFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeSuccessFunc_WhenTryIsSuccessWithAsyncFuncs()
        {
            // Arrange
            var successValue = 42;
            var tryFunc = new Try<int>(() => TestHelper.CreateSuccessResult(successValue));
            var successFunc = Substitute.For<Func<int, Task>>();
            var failureFunc = Substitute.For<Func<ResultError, Task>>();

            // Act
            await tryFunc.MatchAsync(successFunc, failureFunc);

            // Assert
            await successFunc.Received(1).Invoke(successValue);
            await failureFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Fact]
        public async Task MatchAsync_ShouldInvokeFailureFunc_WhenTryIsFailureWithAsyncFuncs()
        {
            // Arrange
            var error = ResultError.New("Test error");
            var tryFunc = TestHelper.CreateTryFuncWithFailure<int>(error);
            var successFunc = Substitute.For<Func<int, Task>>();
            var failureFunc = Substitute.For<Func<ResultError, Task>>();

            // Act
            await tryFunc.MatchAsync(successFunc, failureFunc);

            // Assert
            await failureFunc.Received(1).Invoke(error);
            await successFunc.DidNotReceiveWithAnyArgs().Invoke(default);
        }
}