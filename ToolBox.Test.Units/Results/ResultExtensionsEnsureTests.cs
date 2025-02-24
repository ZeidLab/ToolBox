﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using ZeidLab.ToolBox.Results;
using ZeidLab.ToolBox.Test.Units.Common;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsEnsureTests
{
    #region Result<TIn>.Ensure

    [Fact]
    public void Ensure_PredicateIsTrue_ReturnsOriginalResult()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(42);
        Func<int, bool> predicate = x => x > 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = result.Ensure(predicate, resultError);

        // Assert
        actual.Should().Be(result);
        actual.IsSuccess.Should().BeTrue();
        actual.IsFailure.Should().BeFalse();
        actual.Value.Should().Be(42);
    }

    [Fact]
    public void Ensure_PredicateIsFalse_ReturnsFailureResult()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(42);
        Func<int, bool> predicate = x => x < 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = result.Ensure(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(resultError);
    }

    [Fact]
    public void Ensure_ResultIsFailure_ReturnsOriginalResult()
    {
        // Arrange
        var result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, bool> predicate = x => x > 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = result.Ensure(predicate, resultError);

        // Assert
        actual.Should().Be(result);
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(TestHelper.DefaultResultError);
    }

    #endregion

    #region Result<TIn>.EnsureAsync

    [Fact]
    public async Task EnsureAsync_PredicateIsTrue_ReturnsOriginalResult()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(42);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x > 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await result.EnsureAsync(predicate, resultError);

        // Assert
        actual.Should().Be(result);
        actual.IsSuccess.Should().BeTrue();
        actual.IsFailure.Should().BeFalse();
        actual.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_PredicateIsFalse_ReturnsFailureResult()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(42);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x < 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await result.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(resultError);
    }

    [Fact]
    public async Task EnsureAsync_ResultIsFailure_ReturnsOriginalResult()
    {
        // Arrange
        var result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x > 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await result.EnsureAsync(predicate, resultError);

        // Assert
        actual.Should().Be(result);
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(TestHelper.DefaultResultError);
    }

    #endregion

    #region Try<TIn>.Ensure

    [Fact]
    public void Ensure_TryPredicateIsTrue_ReturnsOriginalResult()
    {
        // Arrange
        Try<int> tryResult = () => TestHelper.CreateSuccessResult(42);
        Func<int, bool> predicate = x => x > 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = tryResult.Ensure(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.IsFailure.Should().BeFalse();
        actual.Value.Should().Be(42);
    }

    [Fact]
    public void Ensure_TryPredicateIsFalse_ReturnsFailureResult()
    {
        // Arrange
        Try<int> tryResult = () => TestHelper.CreateSuccessResult(42);
        Func<int, bool> predicate = x => x < 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = tryResult.Ensure(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(resultError);
    }

    [Fact]
    public void Ensure_TryResultIsFailure_ReturnsOriginalResult()
    {
        // Arrange
        Try<int> tryResult = () => TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, bool> predicate = x => x > 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = tryResult.Ensure(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(TestHelper.DefaultResultError);
    }

    #endregion

    #region Try<TIn>.EnsureAsync

    [Fact]
    public async Task EnsureAsync_TryPredicateIsTrue_ReturnsOriginalResult()
    {
        // Arrange
        Try<int> tryResult = () => TestHelper.CreateSuccessResult(42);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x > 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await tryResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.IsFailure.Should().BeFalse();
        actual.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_TryPredicateIsFalse_ReturnsFailureResult()
    {
        // Arrange
        Try<int> tryResult = () => TestHelper.CreateSuccessResult(42);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x < 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await tryResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(resultError);
    }

    [Fact]
    public async Task EnsureAsync_TryResultIsFailure_ReturnsOriginalResult()
    {
        // Arrange
        Try<int> tryResult = () => TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x > 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await tryResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(TestHelper.DefaultResultError);
    }

    #endregion

    #region Task<Result<TIn>>.EnsureAsync (Sync Predicate)

    [Fact]
    public async Task EnsureAsync_TaskResultSyncPredicateIsTrue_ReturnsOriginalResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateSuccessResultTaskAsync(42);
        Func<int, bool> predicate = x => x > 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await taskResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.IsFailure.Should().BeFalse();
        actual.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_TaskResultSyncPredicateIsFalse_ReturnsFailureResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateSuccessResultTaskAsync(42);
        Func<int, bool> predicate = x => x < 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await taskResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(resultError);
    }

    [Fact]
    public async Task EnsureAsync_TaskResultSyncResultIsFailure_ReturnsOriginalResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResultTaskAsync<int>(TestHelper.DefaultResultError);
        Func<int, bool> predicate = x => x > 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await taskResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(TestHelper.DefaultResultError);
    }

    #endregion

    #region Task<Result<TIn>>.EnsureAsync (Async Predicate)

    [Fact]
    public async Task EnsureAsync_TaskResultAsyncPredicateIsTrue_ReturnsOriginalResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateSuccessResultTaskAsync(42);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x > 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await taskResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.IsFailure.Should().BeFalse();
        actual.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_TaskResultAsyncPredicateIsFalse_ReturnsFailureResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateSuccessResultTaskAsync(42);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x < 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await taskResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(resultError);
    }

    [Fact]
    public async Task EnsureAsync_TaskResultAsyncResultIsFailure_ReturnsOriginalResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResultTaskAsync<int>(TestHelper.DefaultResultError);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x > 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await taskResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(TestHelper.DefaultResultError);
    }

    #endregion

    #region TryAsync<TIn>.EnsureAsync (Sync Predicate)

    [Fact]
    public async Task EnsureAsync_TryAsyncSyncPredicateIsTrue_ReturnsOriginalResult()
    {
        // Arrange
        TryAsync<int> tryAsyncResult = () => TestHelper.CreateSuccessResultTaskAsync(42);
        Func<int, bool> predicate = x => x > 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await tryAsyncResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.IsFailure.Should().BeFalse();
        actual.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_TryAsyncSyncPredicateIsFalse_ReturnsFailureResult()
    {
        // Arrange
        TryAsync<int> tryAsyncResult = () => TestHelper.CreateSuccessResultTaskAsync(42);
        Func<int, bool> predicate = x => x < 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await tryAsyncResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(resultError);
    }

    [Fact]
    public async Task EnsureAsync_TryAsyncSyncResultIsFailure_ReturnsOriginalResult()
    {
        // Arrange
        TryAsync<int> tryAsyncResult = () => TestHelper.CreateFailureResultTaskAsync<int>(TestHelper.DefaultResultError);
        Func<int, bool> predicate = x => x > 0;
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await tryAsyncResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(TestHelper.DefaultResultError);
    }

    #endregion

    #region TryAsync<TIn>.EnsureAsync (Async Predicate)

    [Fact]
    public async Task EnsureAsync_TryAsyncAsyncPredicateIsTrue_ReturnsOriginalResult()
    {
        // Arrange
        TryAsync<int> tryAsyncResult = () => TestHelper.CreateSuccessResultTaskAsync(42);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x > 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await tryAsyncResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeTrue();
        actual.IsFailure.Should().BeFalse();
        actual.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_TryAsyncAsyncPredicateIsFalse_ReturnsFailureResult()
    {
        // Arrange
        TryAsync<int> tryAsyncResult = () => TestHelper.CreateSuccessResultTaskAsync(42);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x < 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await tryAsyncResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(resultError);
    }

    [Fact]
    public async Task EnsureAsync_TryAsyncAsyncResultIsFailure_ReturnsOriginalResult()
    {
        // Arrange
        TryAsync<int> tryAsyncResult = () => TestHelper.CreateFailureResultTaskAsync<int>(TestHelper.DefaultResultError);
        Func<int, Task<bool>> predicate = async x =>
        {
            await Task.Delay(1);
            return x > 0;
        };
        var resultError = TestHelper.DefaultResultError;

        // Act
        var actual = await tryAsyncResult.EnsureAsync(predicate, resultError);

        // Assert
        actual.IsSuccess.Should().BeFalse();
        actual.IsFailure.Should().BeTrue();
        actual.Error.Should().Be(TestHelper.DefaultResultError);
    }

    #endregion
}