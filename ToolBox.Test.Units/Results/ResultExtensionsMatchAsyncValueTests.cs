using FluentAssertions;
using ZeidLab.ToolBox.Results;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsMatchAsyncValueTests
{
    [Fact]
    public async Task MatchAsync_WhenTaskResultIsSuccess_ShouldCallSuccessFunction()
    {
        // Arrange
        var value = 42;
        var taskResult = TestHelper.CreateSuccessResult(value).AsTaskAsync();
        var successCalled = false;
        var failureCalled = false;

        // Act
        var result = await taskResult.MatchAsync(
            success: v =>
            {
                successCalled = true;
                v.Should().Be(value);
                return "success";
            },
            failure: e =>
            {
                failureCalled = true;
                return "failure";
            });

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
        result.Should().Be("success");
    }

    [Fact]
    public async Task MatchAsync_WhenTaskResultIsFailure_ShouldCallFailureFunction()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError).AsTaskAsync();
        var successCalled = false;
        var failureCalled = false;

        // Act
        var result = await taskResult.MatchAsync(
            success: v =>
            {
                successCalled = true;
                return "success";
            },
            failure: e =>
            {
                failureCalled = true;
                e.Should().Be(TestHelper.DefaultResultError);
                return "failure";
            });

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
        result.Should().Be("failure");
    }

    [Fact]
    public async Task MatchAsync_WhenResultTaskIsFaulted_ShouldThrowException()
    {
        // Arrange
        var expectedException = new InvalidOperationException();
        Task<Result<int>> taskResult = Task.FromException<Result<int>>(expectedException);

        // Act
        var act = () => taskResult.MatchAsync(
            success: v => "success",
            failure: e => "failure");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex == expectedException);
    }

    [Fact]
    public async Task MatchAsync_WhenTaskResultIsSuccessAndSuccessFuncReturnsSuccess_ShouldReturnSuccessResult()
    {
        // Arrange
        var value = 42;
        var taskResult = TestHelper.CreateSuccessResult(value).AsTaskAsync();
        var successValue = "success";

        // Act
        var result = await taskResult.MatchAsync(
            success: v =>
            {
                v.Should().Be(value);
                return Result.Success(successValue);
            },
            failure: e => Result.Success("failure"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue);
    }

    [Fact]
    public async Task MatchAsync_WhenTaskResultIsSuccessAndSuccessFuncReturnsFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateSuccessResult(42).AsTaskAsync();
        var error = TestHelper.DefaultResultError;

        // Act
        var result = await taskResult.MatchAsync(
            success: _ => Result.Failure<string>(error),
            failure: e => Result.Success("failure"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task MatchAsync_WhenTaskResultIsFailureAndFailureFuncReturnsSuccess_ShouldReturnSuccessResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError).AsTaskAsync();
        var successValue = "success from failure";

        // Act
        var result = await taskResult.MatchAsync(
            success: v => Result.Success("success"),
            failure: e =>
            {
                e.Should().Be(TestHelper.DefaultResultError);
                return Result.Success(successValue);
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue);
    }

    [Fact]
    public async Task MatchAsync_WhenTaskResultWithAsyncFuncs_ShouldCallSuccessFunction()
    {
        // Arrange
        var value = 42;
        var taskResult = TestHelper.CreateSuccessResult(value).AsTaskAsync();
        var successCalled = false;
        var failureCalled = false;

        // Act
        var result = await taskResult.MatchAsync(
            success: async v =>
            {
                await Task.Yield();
                successCalled = true;
                v.Should().Be(value);
                return Result.Success("success");
            },
            failure: async e =>
            {
                await Task.Yield();
                failureCalled = true;
                return Result.Success("failure");
            });

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("success");
    }

    [Fact]
    public async Task MatchAsync_WhenTaskResultWithAsyncFuncs_ShouldCallFailureFunction()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError).AsTaskAsync();
        var successCalled = false;
        var failureCalled = false;

        // Act
        var result = await taskResult.MatchAsync(
            success: async v =>
            {
                await Task.Yield();
                successCalled = true;
                return Result.Success("success");
            },
            failure: async e =>
            {
                await Task.Yield();
                failureCalled = true;
                e.Should().Be(TestHelper.DefaultResultError);
                return Result.Success("failure");
            });

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("failure");
    }

    [Fact]
    public async Task MatchAsync_WhenAsyncSuccessFunctionThrows_ShouldPropagateException()
    {
        // Arrange
        var value = 42;
        var taskResult = TestHelper.CreateSuccessResult(value).AsTaskAsync();
        var expectedException = new InvalidOperationException();

        // Act
        var act = () => taskResult.MatchAsync(
            success: async _ => throw expectedException,
            failure: async e => Result.Success("failure"));

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex == expectedException);
    }

    [Fact]
    public async Task MatchAsync_WhenAsyncFailureFunctionThrows_ShouldPropagateException()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError).AsTaskAsync();
        var expectedException = new InvalidOperationException();

        // Act
        var act = () => taskResult.MatchAsync(
            success: async v => Result.Success("success"),
            failure: async _ => throw expectedException);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex == expectedException);
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsync_ShouldCallSuccessFunctionAndReturnValue()
    {
        // Arrange
        var value = 42;
        var tryAsync = TestHelper.CreateTryAsyncFuncWithSuccess(value);
        var successCalled = false;
        var failureCalled = false;

        // Act
        var result = await tryAsync.MatchAsync(
            success: v =>
            {
                successCalled = true;
                v.Should().Be(value);
                return "success";
            },
            failure: e =>
            {
                failureCalled = true;
                return "failure";
            });

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
        result.Should().Be("success");
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsync_ShouldCallFailureFunctionAndReturnValue()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
        var successCalled = false;
        var failureCalled = false;

        // Act
        var result = await tryAsync.MatchAsync(
            success: v =>
            {
                successCalled = true;
                return "success";
            },
            failure: e =>
            {
                failureCalled = true;
                e.Should().Be(TestHelper.DefaultResultError);
                return "failure";
            });

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
        result.Should().Be("failure");
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsyncWithResultReturningFunctions_ShouldExecuteSuccessPath()
    {
        // Arrange
        var value = 42;
        var tryAsync = TestHelper.CreateTryAsyncFuncWithSuccess(value);
        var successValue = "success";

        // Act
        var result = await tryAsync.MatchAsync(
            success: v =>
            {
                v.Should().Be(value);
                return Result.Success(successValue);
            },
            failure: e => Result.Success("failure"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue);
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsyncWithResultReturningFunctions_ShouldExecuteFailurePath()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
        var failureValue = "failure";

        // Act
        var result = await tryAsync.MatchAsync(
            success: v => Result.Success("success"),
            failure: e =>
            {
                e.Should().Be(TestHelper.DefaultResultError);
                return Result.Success(failureValue);
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(failureValue);
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsyncWithAsyncResultReturningFunctions_ShouldExecuteSuccessPath()
    {
        // Arrange
        var value = 42;
        var tryAsync = TestHelper.CreateTryAsyncFuncWithSuccess(value);
        var successValue = "success";

        // Act
        var result = await tryAsync.MatchAsync(
            success: async v =>
            {
                await Task.Yield();
                v.Should().Be(value);
                return Result.Success(successValue);
            },
            failure: async e =>
            {
                await Task.Yield();
                return Result.Success("failure");
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue);
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsyncWithAsyncResultReturningFunctions_ShouldExecuteFailurePath()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
        var failureValue = "failure";

        // Act
        var result = await tryAsync.MatchAsync(
            success: async v =>
            {
                await Task.Yield();
                return Result.Success("success");
            },
            failure: async e =>
            {
                await Task.Yield();
                e.Should().Be(TestHelper.DefaultResultError);
                return Result.Success(failureValue);
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(failureValue);
    }
}