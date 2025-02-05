using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsBindTests
{
    [Fact]
    public void Bind_WhenResultIsSuccess_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        var successResult = TestHelper.CreateSuccessResult(42);
        Func<int, Result<string>> func = x => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = successResult.Bind(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void Bind_WhenResultIsFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var failureResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, Result<string>> func = x => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = failureResult.Bind(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task BindAsync_WhenResultIsSuccess_ShouldApplyAsyncFunctionAndReturnNewResult()
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
    public async Task BindAsync_WhenResultIsFailure_ShouldReturnFailureResult()
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
    public async Task BindAsync_WhenTaskResultIsSuccess_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        var successResult = Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, Result<string>> func = x => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = await successResult.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WhenTaskResultIsFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var failureResult = Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
        Func<int, Result<string>> func = x => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = await failureResult.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task BindAsync_WhenTaskResultIsSuccess_ShouldApplyAsyncFunctionAndReturnNewResult()
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
    public async Task BindAsync_WhenTaskResultIsFailure_ShouldReturnFailureResultAsync()
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
    public void Bind_WhenTryIsSuccess_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateSuccessResult(42);
        Func<int, Result<string>> func = x => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = tryFunc.Bind(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void Bind_WhenTryIsFailure_ShouldReturnFailureResult()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, Result<string>> func = x => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = tryFunc.Bind(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task BindAsync_WhenTryIsSuccess_ShouldApplyAsyncFunctionAndReturnNewResult()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateSuccessResult(42);
        Func<int, Task<Result<string>>> func = x => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WhenTryIsFailure_ShouldReturnFailureResultAsync()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, Task<Result<string>>> func = x => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryFunc.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task BindAsync_WhenTryAsyncIsSuccess_ShouldApplyFunctionAndReturnNewResult()
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
    public async Task BindAsync_WhenTryAsyncIsFailure_ShouldReturnFailureResult()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
        Func<int, Result<string>> func = x => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task BindAsync_WhenTryAsyncIsSuccess_ShouldApplyAsyncFunctionAndReturnNewResultAsync()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, Task<Result<string>>> func = x => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WhenTryAsyncIsFailure_ShouldReturnFailureResultAsync()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
        Func<int, Task<Result<string>>> func = x => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }
        
    [Fact]
    public void Bind_WhenTryIsSuccessAndFuncReturnsTry_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateSuccessResult(42);
        Func<int, Try<string>> func = x => () => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = tryFunc.Bind(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void Bind_WhenTryIsFailureAndFuncReturnsTry_ShouldReturnFailureResult()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, Try<string>> func = x => () => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = tryFunc.Bind(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task BindAsync_WhenTryIsSuccessAndFuncReturnsTryAsync_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateSuccessResult(42);
        Func<int, TryAsync<string>> func = x => () => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WhenTryIsFailureAndFuncReturnsTryAsync_ShouldReturnFailureResult()
    {
        // Arrange
        Try<int> tryFunc = () => TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, TryAsync<string>> func = x => () => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryFunc.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task BindAsync_WhenTryAsyncIsSuccessAndFuncReturnsTryAsync_ShouldApplyFunctionAndReturnNewResult()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateSuccessResult(42));
        Func<int, TryAsync<string>> func = x => () => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WhenTryAsyncIsFailureAndFuncReturnsTryAsync_ShouldReturnFailureResult()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
        Func<int, TryAsync<string>> func = x => () => Task.FromResult(TestHelper.CreateSuccessResult(x.ToString()));

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }

    [Fact]
    public async Task BindAsync_WhenTryAsyncIsSuccessAndFuncReturnsTry_ShouldApplyFunctionAndReturnNewResult()
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
    public async Task BindAsync_WhenTryAsyncIsFailureAndFuncReturnsTry_ShouldReturnFailureResult()
    {
        // Arrange
        TryAsync<int> tryAsyncFunc = () => Task.FromResult(TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError));
        Func<int, Try<string>> func = x => () => TestHelper.CreateSuccessResult(x.ToString());

        // Act
        var result = await tryAsyncFunc.BindAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestHelper.DefaultResultError);
    }
}