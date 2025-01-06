using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsTryTests
{
    [Fact]
    public async Task ToAsync_ShouldConvertTryToTryAsync_WhenTryIsNotNull()
    {
        // Arrange
        var expectedValue = 42;
        var tryDelegate = Substitute.For<Try<int>>();
        tryDelegate.Invoke().Returns(Result<int>.Success(expectedValue));

        // Act
        var tryAsyncDelegate = tryDelegate.ToAsync();
        var result = await tryAsyncDelegate();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Try_ShouldReturnSuccessResult_WhenTryDelegateIsSuccessful()
    {
        // Arrange
        var expectedValue = 42;
        var tryDelegate = Substitute.For<Try<int>>();
        tryDelegate.Invoke().Returns(Result<int>.Success(expectedValue));

        // Act
        var result = tryDelegate.Try();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void Try_ShouldReturnFailureResult_WhenTryDelegateThrowsException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var tryDelegate = Substitute.For<Try<int>>();
        tryDelegate.Invoke().Throws(exception);

        // Act
        var result = tryDelegate.Try();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.GetValueOrDefault().Exception.Should().Be(exception);
    }

    

    [Fact]
    public async Task TryAsync_ShouldReturnSuccessResult_WhenTryAsyncDelegateIsSuccessful()
    {
        // Arrange
        var expectedValue = 42;
        var tryAsyncDelegate = Substitute.For<TryAsync<int>>();
        tryAsyncDelegate.Invoke().Returns(Result<int>.Success(expectedValue));

        // Act
        var result = await tryAsyncDelegate.Try();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedValue);
    }

    [Fact]
    public async Task TryAsync_ShouldReturnFailureResult_WhenTryAsyncDelegateThrowsException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var tryAsyncDelegate = Substitute.For<TryAsync<int>>();
        tryAsyncDelegate.Invoke().Throws(exception);

        // Act
        var result = await tryAsyncDelegate.Try();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.GetValueOrDefault().Exception.Should().Be(exception);
    }
   
}