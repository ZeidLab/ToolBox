using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsEnsureTests
{
    [Fact]
    public void Ensure_WhenResultIsFailure_ShouldReturnOriginalResult()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var result = Result<int>.Failure(error);
        Func<int, bool> predicate = x => x > 0;

        // Act
        var ensuredResult = result.Ensure(predicate, ResultError.New("Should not be used"));

        // Assert
        ensuredResult.IsFailure.Should().BeTrue();
        ensuredResult.ResultError.Should().Be(error);
    }

    [Fact]
    public void Ensure_WhenResultIsSuccessAndPredicateIsTrue_ShouldReturnOriginalResult()
    {
        // Arrange
        var result = Result<int>.Success(42);
        Func<int, bool> predicate = x => x > 0;

        // Act
        var ensuredResult = result.Ensure(predicate, ResultError.New("Should not be used"));

        // Assert
        ensuredResult.IsSuccess.Should().BeTrue();
        ensuredResult.Value.Should().Be(42);
    }

    [Fact]
    public void Ensure_WhenResultIsSuccessAndPredicateIsFalse_ShouldReturnFailedResultWithProvidedError()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var error = ResultError.New("Predicate failed");
        Func<int, bool> predicate = x => x < 0;

        // Act
        var ensuredResult = result.Ensure(predicate, error);

        // Assert
        ensuredResult.IsFailure.Should().BeTrue();
        ensuredResult.ResultError.Should().Be(error);
    }

    [Fact]
    public async Task EnsureAsync_WhenResultIsFailure_ShouldReturnOriginalResult()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var result = Task.FromResult(Result<int>.Failure(error));
        Func<int, bool> predicate = x => x > 0;

        // Act
        var ensuredResult = await result.EnsureAsync(predicate, ResultError.New("Should not be used"));

        // Assert
        ensuredResult.IsFailure.Should().BeTrue();
        ensuredResult.ResultError.Should().Be(error);
    }

    [Fact]
    public async Task EnsureAsync_WhenResultIsSuccessAndPredicateIsTrue_ShouldReturnOriginalResult()
    {
        // Arrange
        var result = Task.FromResult(Result<int>.Success(42));
        Func<int, bool> predicate = x => x > 0;

        // Act
        var ensuredResult = await result.EnsureAsync(predicate, ResultError.New("Should not be used"));

        // Assert
        ensuredResult.IsSuccess.Should().BeTrue();
        ensuredResult.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_WhenResultIsSuccessAndPredicateIsFalse_ShouldReturnFailedResultWithProvidedError()
    {
        // Arrange
        var result = Task.FromResult(Result<int>.Success(42));
        var error = ResultError.New("Predicate failed");
        Func<int, bool> predicate = x => x < 0;

        // Act
        var ensuredResult = await result.EnsureAsync(predicate, error);

        // Assert
        ensuredResult.IsFailure.Should().BeTrue();
        ensuredResult.ResultError.Should().Be(error);
    }

    [Fact]
    public async Task EnsureAsync_WithAsyncPredicate_WhenResultIsFailure_ShouldReturnOriginalResult()
    {
        // Arrange
        var error = ResultError.New("Test error");
        var result = Task.FromResult(Result<int>.Failure(error));
        Func<int, Task<bool>> predicate = x => Task.FromResult(x > 0);

        // Act
        var ensuredResult = await result.EnsureAsync(predicate, ResultError.New("Should not be used"));

        // Assert
        ensuredResult.IsFailure.Should().BeTrue();
        ensuredResult.ResultError.Should().Be(error);
    }

    [Fact]
    public async Task EnsureAsync_WithAsyncPredicate_WhenResultIsSuccessAndPredicateIsTrue_ShouldReturnOriginalResult()
    {
        // Arrange
        var result = Task.FromResult(Result<int>.Success(42));
        Func<int, Task<bool>> predicate = x => Task.FromResult(x > 0);

        // Act
        var ensuredResult = await result.EnsureAsync(predicate, ResultError.New("Should not be used"));

        // Assert
        ensuredResult.IsSuccess.Should().BeTrue();
        ensuredResult.Value.Should().Be(42);
    }

    [Fact]
    public async Task
        EnsureAsync_WithAsyncPredicate_WhenResultIsSuccessAndPredicateIsFalse_ShouldReturnFailedResultWithProvidedError()
    {
        // Arrange
        var result = Task.FromResult(Result<int>.Success(42));
        var error = ResultError.New("Predicate failed");
        Func<int, Task<bool>> predicate = x => Task.FromResult(x < 0);

        // Act
        var ensuredResult = await result.EnsureAsync(predicate, error);

        // Assert
        ensuredResult.IsFailure.Should().BeTrue();
        ensuredResult.ResultError.Should().Be(error);
    }
}