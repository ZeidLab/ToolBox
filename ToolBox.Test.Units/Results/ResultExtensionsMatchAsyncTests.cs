using FluentAssertions;
using ZeidLab.ToolBox.Results;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsMatchAsyncTests
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
    public async Task MatchAsync_WhenSuccessFunctionThrows_ShouldPropagateException()
    {
        // Arrange
        var value = 42;
        var taskResult = TestHelper.CreateSuccessResult(value).AsTaskAsync();
        var expectedException = new InvalidOperationException();

        // Act
        var act = () => taskResult.MatchAsync(
            success: _ => throw expectedException,
            failure: e => "failure");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex == expectedException);
    }

    [Fact]
    public async Task MatchAsync_WhenFailureFunctionThrows_ShouldPropagateException()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError).AsTaskAsync();
        var expectedException = new InvalidOperationException();

        // Act
        var act = () => taskResult.MatchAsync(
            success: v => "success",
            failure: _ => throw expectedException);

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
    public async Task MatchAsync_WhenTaskResultIsFailureAndFailureFuncReturnsFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError).AsTaskAsync();
        var newError = ResultError.New("New error");

        // Act
        var result = await taskResult.MatchAsync(
            success: v => Result.Success("success"),
            failure: e => Result.Failure<string>(newError));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(newError);
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsyncIsSuccessful_ShouldCallSuccessFunction()
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
    public async Task MatchAsync_WhenTryAsyncFails_ShouldCallFailureFunction()
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
    public async Task MatchAsync_WhenTryAsyncIsSuccessfulAndReturnsSuccessResult_ShouldReturnSuccess()
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
    public async Task MatchAsync_WhenTryAsyncFailsAndReturnsSuccessResult_ShouldReturnSuccess()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
        var successValue = "success from failure";

        // Act
        var result = await tryAsync.MatchAsync(
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
    public async Task MatchAsync_WhenSuccessFunctionReturnsFailureResult_ShouldReturnFailure()
    {
        // Arrange
        var value = 42;
        var tryAsync = TestHelper.CreateTryAsyncFuncWithSuccess(value);
        var error = TestHelper.DefaultResultError;

        // Act
        var result = await tryAsync.MatchAsync(
            success: _ => Result.Failure<string>(error),
            failure: e => Result.Success("failure"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task MatchAsync_WhenFailureFunctionReturnsFailureResult_ShouldReturnFailure()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
        var newError = ResultError.New("New error");

        // Act
        var result = await tryAsync.MatchAsync(
            success: v => Result.Success("success"),
            failure: e => Result.Failure<string>(newError));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(newError);
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsyncIsSuccessfulAndAsyncFunctionsReturnSuccess_ShouldReturnSuccess()
    {
        // Arrange
        var value = 42;
        var tryAsync = TestHelper.CreateTryAsyncFuncWithSuccess(value);
        var successValue = "success";

        // Act
        var result = await tryAsync.MatchAsync(
            success: async v =>
            {
                v.Should().Be(value);
                return Result.Success(successValue);
            },
            failure: async e => Result.Success("failure"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue);
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsyncFailsAndAsyncFailureFunctionReturnsSuccess_ShouldReturnSuccess()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
        var successValue = "success from failure";

        // Act
        var result = await tryAsync.MatchAsync(
            success: async v => Result.Success("success"),
            failure: async e =>
            {
                e.Should().Be(TestHelper.DefaultResultError);
                return Result.Success(successValue);
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(successValue);
    }

    [Fact]
    public async Task MatchAsync_WhenAsyncSuccessFunctionReturnsFailure_ShouldReturnFailure()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithSuccess(42);
        var error = TestHelper.DefaultResultError;

        // Act
        var result = await tryAsync.MatchAsync(
            success: async _ => Result.Failure<string>(error),
            failure: async e => Result.Success("failure"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task MatchAsync_WhenAsyncFailureFunctionThrowsInTryAsync_ShouldPropagateException()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
        var expectedException = new InvalidOperationException();

        // Act
        var act = () => tryAsync.MatchAsync(
            success: async v => Result.Success("success"),
            failure: async _ => throw expectedException);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex == expectedException);
    }

    [Fact]
    public async Task MatchAsync_WhenAsyncSuccessFunctionThrowsInTryAsync_ShouldPropagateException()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithSuccess(42);
        var expectedException = new InvalidOperationException();

        // Act
        var act = () => tryAsync.MatchAsync(
            success: async _ => throw expectedException,
            failure: async e => Result.Success("failure"));

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex == expectedException);
    }
}