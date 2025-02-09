using FluentAssertions;
using NSubstitute;
using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsTapTests
{
    #region Synchronous Tap Tests

    [Fact]
    public void Given_SuccessResult_When_Tap_Then_ExecutesActionAndReturnsOriginalResult()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(42);
        var action = Substitute.For<Action<int>>();

        // Act
        var returnedResult = result.Tap(action);

        // Assert
        returnedResult.Should().Be(result);
        action.Received(1).Invoke(42);
    }

    [Fact]
    public void Given_FailureResult_When_Tap_Then_DoesNotExecuteActionAndReturnsOriginalResult()
    {
        // Arrange
        var result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        var action = Substitute.For<Action<int>>();

        // Act
        var returnedResult = result.Tap(action);

        // Assert
        returnedResult.Should().Be(result);
        action.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    [Fact]
    public void Given_NullAction_When_Tap_Then_ThrowsNullReferenceException()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(42);
        Action<int> action = null;

        // Act
        Action act = () => result.Tap(action);

        // Assert
        act.Should().Throw<NullReferenceException>();
    }

    #endregion

    #region Try Tap Tests

    [Fact]
    public void Given_SuccessfulTry_When_Tap_Then_ExecutesActionAndReturnsSuccessResult()
    {
        // Arrange
        var tryResult = TestHelper.CreateTryFuncWithSuccess(42);
        var action = Substitute.For<Action<int>>();

        // Act
        var result = tryResult.Tap(action);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        action.Received(1).Invoke(42);
    }

    [Fact]
    public void Given_FailedTry_When_Tap_Then_DoesNotExecuteActionAndReturnsFailureResult()
    {
        // Arrange
        var tryResult = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
        var action = Substitute.For<Action<int>>();

        // Act
        var result = tryResult.Tap(action);

        // Assert
        result.IsFailure.Should().BeTrue();
        action.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    [Fact]
    public void Given_NullAction_When_TryTap_Then_ThrowsNullReferenceException()
    {
        // Arrange
        var tryResult = TestHelper.CreateTryFuncWithSuccess(42);
        Action<int> action = null;

        // Act
        Action act = () => tryResult.Tap(action);

        // Assert
        act.Should().Throw<NullReferenceException>();
    }

    #endregion

    #region Task Result Tap Tests

    [Fact]
    public async Task Given_SuccessTaskResult_When_TapAsync_Then_ExecutesActionAndReturnsOriginalResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateSuccessResult(42).AsTaskAsync();
        var action = Substitute.For<Action<int>>();

        // Act
        var returnedResult = await taskResult.TapAsync(action);

        // Assert
        returnedResult.IsSuccess.Should().BeTrue();
        returnedResult.Value.Should().Be(42);
        action.Received(1).Invoke(42);
    }

    [Fact]
    public async Task Given_FailureTaskResult_When_TapAsync_Then_DoesNotExecuteActionAndReturnsOriginalResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError).AsTaskAsync();
        var action = Substitute.For<Action<int>>();

        // Act
        var returnedResult = await taskResult.TapAsync(action);

        // Assert
        returnedResult.IsFailure.Should().BeTrue();
        action.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    [Fact]
    public void Given_NullAction_When_TaskResultTapAsync_Then_ThrowsArgumentNullException()
    {
        // Arrange
        var taskResult = TestHelper.CreateSuccessResult(42).AsTaskAsync();

        // Act
        Func<Task> act = () => taskResult.TapAsync(null);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>();
    }

    #endregion

    #region Async Tap Tests

    [Fact]
    public async Task Given_SuccessResult_When_TapAsync_Then_ExecutesFuncAndReturnsOriginalResult()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(42);
        var func = Substitute.For<Func<int, Task>>();

        // Act
        var returnedResult = await result.TapAsync(func);

        // Assert
        returnedResult.Should().Be(result);
        await func.Received(1).Invoke(42);
    }

    [Fact]
    public async Task Given_FailureResult_When_TapAsync_Then_DoesNotExecuteFuncAndReturnsOriginalResult()
    {
        // Arrange
        var result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        var func = Substitute.For<Func<int, Task>>();

        // Act
        var returnedResult = await result.TapAsync(func);

        // Assert
        returnedResult.Should().Be(result);
        await func.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    [Fact]
    public void Given_NullFunc_When_TapAsync_Then_ThrowsArgumentNullException()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(42);
        Func<int, Task> func = null;

        // Act
        Func<Task> act = () => result.TapAsync(func);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>();
    }

    #endregion

    #region Try Async Tap Tests

    [Fact]
    public async Task Given_SuccessfulTry_When_TapAsync_Then_ExecutesFuncAndReturnsSuccessResult()
    {
        // Arrange
        var tryResult = TestHelper.CreateTryFuncWithSuccess(42);
        var func = Substitute.For<Func<int, Task>>();

        // Act
        var result = await tryResult.TapAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        await func.Received(1).Invoke(42);
    }

    [Fact]
    public async Task Given_FailedTry_When_TapAsync_Then_DoesNotExecuteFuncAndReturnsFailureResult()
    {
        // Arrange
        var tryResult = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
        var func = Substitute.For<Func<int, Task>>();

        // Act
        var result = await tryResult.TapAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        await func.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    #endregion

    #region TryAsync Tap Tests

    [Fact]
    public async Task Given_SuccessfulTryAsync_When_TapAsync_Then_ExecutesFuncAndReturnsSuccessResult()
    {
        // Arrange
        var tryAsyncResult = TestHelper.CreateTryAsyncFuncWithSuccess(42);
        var func = Substitute.For<Func<int, Task>>();

        // Act
        var result = await tryAsyncResult.TapAsync(func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        await func.Received(1).Invoke(42);
    }

    [Fact]
    public async Task Given_FailedTryAsync_When_TapAsync_Then_DoesNotExecuteFuncAndReturnsFailureResult()
    {
        // Arrange
        var tryAsyncResult = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
        var func = Substitute.For<Func<int, Task>>();

        // Act
        var result = await tryAsyncResult.TapAsync(func);

        // Assert
        result.IsFailure.Should().BeTrue();
        await func.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    [Fact]
    public void Given_NullFunc_When_TryAsyncTapAsync_Then_ThrowsArgumentNullException()
    {
        // Arrange
        var tryAsyncResult = TestHelper.CreateTryAsyncFuncWithSuccess(42);

        // Act
        Func<Task> act = () => tryAsyncResult.TapAsync(null);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>();
    }

    #endregion
}