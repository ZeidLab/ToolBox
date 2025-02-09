using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

/// <summary>
/// Contains unit tests for the asynchronous Result extension methods related to binding operations.
/// These tests verify the behavior of asynchronous binding operations on Result, Try, and TryAsync types.
/// </summary>
public class ResultExtensionsBindAsyncTests
{
    #region Result to Task<Result> Tests

    [Fact]
    public async Task When_BindingSuccessResultToTaskResult_Should_ApplyAsyncFunction()
    {
        // Arrange
        var successResult = TestHelper.CreateSuccessResult(42);
        Func<int, Task<Result<string>>> func = x => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await successResult.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task When_BindingFailureResultToTaskResult_Should_PropagateError()
    {
        // Arrange
        var failureResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, Task<Result<string>>> func = x => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await failureResult.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task When_BindingSuccessResultToTaskResult_And_AsyncOperationFails_Should_PropagateError()
    {
        // Arrange
        var successResult = TestHelper.CreateSuccessResult(42);
        var expectedError = new ResultError(1, "Error", "Async operation failed", null);
        Func<int, Task<Result<string>>> func = _ => Task.FromResult(Result.Failure<string>(expectedError));

        // Act
        var result = await successResult.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }

    #endregion

    #region Task<Result> to Task<Result> Tests

    [Fact]
    public async Task When_BindingTaskResultToTaskResult_Should_ApplyAsyncFunction()
    {
        // Arrange
        var successResult = Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, Task<Result<string>>> func = x => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await successResult.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task When_BindingTaskResultToTaskResult_And_TaskFails_Should_PropagateError()
    {
        // Arrange
        var failureResult = Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
        Func<int, Task<Result<string>>> func = x => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await failureResult.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task When_BindingTaskResultToTaskResult_And_AsyncFunctionFails_Should_PropagateError()
    {
        // Arrange
        var successResult = Task.FromResult(TestHelper.CreateSuccessResult(42));
        var expectedError = new ResultError(2, "Error", "Async function failed", null);
        Func<int, Task<Result<string>>> func = _ => Task.FromResult(Result.Failure<string>(expectedError));

        // Act
        var result = await successResult.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }

    #endregion

    #region Try to Async Tests

    [Fact]
    public async Task When_BindingSuccessfulTryToTryAsync_Should_ApplyAsyncFunction()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateSuccessResult(42);
        Func<int, TryAsync<string>> func = x => () =>
            Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task When_BindingFailedTryToTryAsync_Should_PropagateError()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, TryAsync<string>> func = x => () =>
            Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryFunc.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task When_BindingSuccessfulTryToTaskResult_Should_ApplyAsyncFunction()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateSuccessResult(42);
        Func<int, Task<Result<string>>> func = x =>
            Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task When_BindingFailedTryToTaskResult_Should_PropagateError()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, Task<Result<string>>> func = x =>
            Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryFunc.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    #endregion

    #region TryAsync Tests

    [Fact]
    public async Task When_BindingSuccessfulTryAsyncToTryAsync_Should_ApplyAsyncFunction()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, TryAsync<string>> func = x => () =>
            Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task When_BindingFailedTryAsyncToTryAsync_Should_PropagateError()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () =>
            Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
        Func<int, TryAsync<string>> func = x => () =>
            Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task When_BindingSuccessfulTryAsyncToTry_Should_ApplyFunction()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, Try<string>> func = x => () => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task When_BindingSuccessfulTryAsyncToResult_Should_ApplyFunction()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, Result<string>> func = x => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task When_BindingSuccessfulTryAsyncToTaskResult_Should_ApplyAsyncFunction()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, Task<Result<string>>> func = x =>
            Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    #endregion

    #region Null Argument Tests

    [Fact]
    public async Task When_BindingResultWithNullAsyncFunc_Should_ThrowNullReferenceException()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(42);
        Func<int, Task<Result<string>>> func = null!;

        // Act
        var act = () => result.BindAsync(func);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task When_BindingTaskResultWithNullFunc_Should_ThrowArgumentNullException()
    {
        // Arrange
        var result = Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, Result<string>> func = null!;

        // Act
        var act = () => result.BindAsync(func);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task When_BindingTaskResultWithNullAsyncFunc_Should_ThrowNullReferenceException()
    {
        // Arrange
        var result = Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, Task<Result<string>>> func = null!;

        // Act
        var act = () => result.BindAsync(func);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    #endregion
}