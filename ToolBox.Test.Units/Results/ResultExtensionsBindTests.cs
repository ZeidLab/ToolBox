using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsBindTests
{
    private static Result<TValue> CreateSuccessResult<TValue>(TValue value) => Result<TValue>.Success(value);
    private static Result<TValue> CreateFailureResult<TValue>(Error error) => Result<TValue>.Failure(error);
    private static Error CreateError(string message = "Error message") => Error.New(message);

    [Fact]
    public void Bind_WithSuccessResult_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        var successResult = CreateSuccessResult(42);
        Func<int, Result<string>> func = value => CreateSuccessResult(value.ToString());

        // Act
        var result = successResult.Bind(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void Bind_WithFailureResult_ShouldReturnFailureResultWithoutApplyingFunction()
    {
        // Arrange
        var error = CreateError();
        var failureResult = CreateFailureResult<int>(error);
        Func<int, Result<string>> func = value => CreateSuccessResult(value.ToString());

        // Act
        var result = failureResult.Bind(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Bind_WithTryDelegate_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        var successResult = CreateSuccessResult(42);
        Try<int> tryDelegate = () => successResult;
        Func<int, Result<string>> func = value => CreateSuccessResult(value.ToString());

        // Act
        var result = tryDelegate.Bind(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void BindAsync_WithSuccessResult_ShouldApplyAsyncFunctionAndReturnNewResult()
    {
        // Arrange
        var successResult = CreateSuccessResult(42);
        Func<int, Task<Result<string>>> func = value => Task.FromResult(CreateSuccessResult(value.ToString()));

        // Act
        var resultTask = successResult.BindAsync(func);
        resultTask.Wait();
        var result = resultTask.Result;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void BindAsync_WithFailureResult_ShouldReturnFailureResultWithoutApplyingAsyncFunction()
    {
        // Arrange
        var error = CreateError();
        var failureResult = CreateFailureResult<int>(error);
        Func<int, Task<Result<string>>> func = value => Task.FromResult(CreateSuccessResult(value.ToString()));

        // Act
        var resultTask = failureResult.BindAsync(func);
        resultTask.Wait();
        var result = resultTask.Result;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task BindAsync_WithTaskSuccessResult_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        var successResult = Task.FromResult(CreateSuccessResult(42));
        Func<int, Result<string>> func = value => CreateSuccessResult(value.ToString());

        // Act
        var result = await successResult.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WithTaskFailureResult_ShouldReturnFailureResultWithoutApplyingFunction()
    {
        // Arrange
        var error = CreateError();
        var failureResult = Task.FromResult(CreateFailureResult<int>(error));
        Func<int, Result<string>> func = value => CreateSuccessResult(value.ToString());

        // Act
        var result = await failureResult.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task BindAsync_WithTaskSuccessResultAndAsyncFunction_ShouldApplyAsyncFunctionAndReturnNewResult()
    {
        // Arrange
        var successResult = Task.FromResult(CreateSuccessResult(42));
        Func<int, Task<Result<string>>> func = value => Task.FromResult(CreateSuccessResult(value.ToString()));

        // Act
        var result = await successResult.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WithTaskFailureResultAndAsyncFunction_ShouldReturnFailureResultWithoutApplyingAsyncFunction()
    {
        // Arrange
        var error = CreateError();
        var failureResult = Task.FromResult(CreateFailureResult<int>(error));
        Func<int, Task<Result<string>>> func = value => Task.FromResult(CreateSuccessResult(value.ToString()));

        // Act
        var result = await failureResult.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task BindAsync_WithTryAsyncDelegate_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        var successResult = CreateSuccessResult(42);
        TryAsync<int> tryAsyncDelegate = () => Task.FromResult(successResult);
        Func<int, Result<string>> func = value => CreateSuccessResult(value.ToString());

        // Act
        var result = await tryAsyncDelegate.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WithTryAsyncDelegateAndAsyncFunction_ShouldApplyAsyncFunctionAndReturnNewResult()
    {
        // Arrange
        var successResult = CreateSuccessResult(42);
        TryAsync<int> tryAsyncDelegate = () => Task.FromResult(successResult);
        Func<int, Task<Result<string>>> func = value => Task.FromResult(CreateSuccessResult(value.ToString()));

        // Act
        var result = await tryAsyncDelegate.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }
}