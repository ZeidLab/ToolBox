using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsTryTests
{
    [Fact]
    public async Task ToAsync_WhenGivenSuccessfulSync_ShouldReturnSuccessfulAsync()
    {
        // Arrange
        const int expectedValue = 42;
        var tryDelegate = TestHelper.CreateTryFuncWithSuccess(expectedValue);

        // Act
        var tryAsyncDelegate = tryDelegate.ToAsync();
        var result = await tryAsyncDelegate();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public async Task ToAsync_WhenGivenFailedSync_ShouldReturnFailedAsync() 
    {
        // Arrange
        var error = TestHelper.DefaultResultError;
        var tryDelegate = TestHelper.CreateTryFuncWithFailure<int>(error);

        // Act
        var tryAsyncDelegate = tryDelegate.ToAsync();
        var result = await tryAsyncDelegate();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Try_WhenDelegateSucceeds_ShouldReturnSuccessResult()
    {
        // Arrange
        const int expectedValue = 42;
        var tryDelegate = TestHelper.CreateTryFuncWithSuccess(expectedValue);

        // Act
        var result = tryDelegate.Try();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Try_WhenDelegateThrowsException_ShouldCatchAndReturnFailure()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var tryDelegate = new Try<int>(() => throw exception);

        // Act
        var result = tryDelegate.Try();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Exception.Should().Be(exception);
    }

    [Fact]
    public async Task TryAsync_WhenDelegateSucceeds_ShouldReturnSuccessResult()
    {
        // Arrange
        const int expectedValue = 42;
        var tryAsyncDelegate = TestHelper.CreateTryAsyncFuncWithSuccess(expectedValue);

        // Act
        var result = await tryAsyncDelegate.TryAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public async Task TryAsync_WhenDelegateThrowsException_ShouldCatchAndReturnFailure() 
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var tryAsyncDelegate = new TryAsync<int>(() => throw exception);

        // Act
        var result = await tryAsyncDelegate.TryAsync();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Exception.Should().Be(exception);
    }

    [Fact]
    public async Task TryAsync_WhenDelegateReturnsFailure_ShouldReturnSameFailure()
    {
        // Arrange 
        var error = TestHelper.DefaultResultError;
        var tryAsyncDelegate = TestHelper.CreateTryAsyncFuncWithFailure<int>(error);

        // Act
        var result = await tryAsyncDelegate.TryAsync();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }
}