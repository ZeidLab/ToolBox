using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NSubstitute;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
public class ResultExtensionsMatchTests
{
    private static readonly int SuccessValue = 42;
    private static readonly string TransformedValue = "42";

    [Fact]
    public void Match_ResultSuccess_InvokesSuccessFunction()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(SuccessValue);
        var successCalled = false;
        
        // Act
        var matched = result.Match(
            success: value =>
            {
                successCalled = true;
                return Result.Success(value.ToString());
            },
            failure: _ => Result.Success("error"));

        // Assert
        successCalled.Should().BeTrue();
        matched.IsSuccess.Should().BeTrue();
        matched.Value.Should().Be(SuccessValue.ToString());
    }

    [Fact]
    public void Match_ResultFailure_InvokesFailureFunction()
    {
        // Arrange
        var result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        var failureCalled = false;

        // Act
        var matched = result.Match(
            success: _ => Result.Success("success"),
            failure: error =>
            {
                failureCalled = true;
                return Result.Success(error.Message);
            });

        // Assert
        failureCalled.Should().BeTrue();
        matched.IsSuccess.Should().BeTrue();
        matched.Value.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Fact]
    public void Match_ResultSuccessToValue_ReturnsTransformedValue()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(SuccessValue);

        // Act
        var matched = result.Match(
            success: value => value.ToString(),
            failure: _ => "error");

        // Assert
        matched.Should().Be(SuccessValue.ToString());
    }

    [Fact]
    public void Match_ResultFailureToValue_ReturnsFailureTransformation()
    {
        // Arrange
        var result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);

        // Act
        var matched = result.Match(
            success: _ => "success",
            failure: error => error.Message);

        // Assert
        matched.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Fact]
    public void Match_TrySuccess_InvokesSuccessFunction()
    {
        // Arrange
        var tryFunc = TestHelper.CreateTryFuncWithSuccess(SuccessValue);
        var successCalled = false;

        // Act
        var matched = tryFunc.Match(
            success: value =>
            {
                successCalled = true;
                return Result.Success(value.ToString());
            },
            failure: _ => Result.Success("error"));

        // Assert
        successCalled.Should().BeTrue();
        matched.IsSuccess.Should().BeTrue();
        matched.Value.Should().Be(SuccessValue.ToString());
    }

    [Fact]
    public void Match_TryFailure_InvokesFailureFunction()
    {
        // Arrange
        var tryFunc = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
        var failureCalled = false;

        // Act
        var matched = tryFunc.Match(
            success: _ => Result.Success("success"),
            failure: error =>
            {
                failureCalled = true;
                return Result.Success(error.Message);
            });

        // Assert
        failureCalled.Should().BeTrue();
        matched.IsSuccess.Should().BeTrue();
        matched.Value.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Fact]
    public void Match_TrySuccessToValue_ReturnsTransformedValue()
    {
        // Arrange
        var tryFunc = TestHelper.CreateTryFuncWithSuccess(SuccessValue);

        // Act
        var matched = tryFunc.Match(
            success: value => value.ToString(),
            failure: _ => "error");

        // Assert
        matched.Should().Be(SuccessValue.ToString());
    }

    [Fact]
    public void Match_TryFailureToValue_ReturnsFailureTransformation()
    {
        // Arrange
        var tryFunc = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);

        // Act
        var matched = tryFunc.Match(
            success: _ => "success",
            failure: error => error.Message);

        // Assert
        matched.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Fact]
    public void Match_ResultSuccessWithActions_InvokesSuccessAction()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(SuccessValue);
        var successCalled = false;

        // Act
        result.Match(
            success: _ => successCalled = true,
            failure: _ => { });

        // Assert
        successCalled.Should().BeTrue();
    }

    [Fact]
    public void Match_ResultFailureWithActions_InvokesFailureAction()
    {
        // Arrange
        var result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        var failureCalled = false;

        // Act
        result.Match(
            success: _ => { },
            failure: _ => failureCalled = true);

        // Assert
        failureCalled.Should().BeTrue();
    }

    [Fact]
    public void Match_TrySuccessWithActions_InvokesSuccessAction()
    {
        // Arrange
        var tryFunc = TestHelper.CreateTryFuncWithSuccess(SuccessValue);
        var successCalled = false;

        // Act
        tryFunc.Match(
            success: _ => successCalled = true,
            failure: _ => { });

        // Assert
        successCalled.Should().BeTrue();
    }

    [Fact]
    public void Match_TryFailureWithActions_InvokesFailureAction()
    {
        // Arrange
        var tryFunc = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
        var failureCalled = false;

        // Act
        tryFunc.Match(
            success: _ => { },
            failure: _ => failureCalled = true);

        // Assert
        failureCalled.Should().BeTrue();
    }

    [Fact]
    public void Match_ResultSuccessWithResultFailureReturn_ReturnsFailureResult()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(SuccessValue);
        var expectedError = ResultError.New("Transformed error");

        // Act
        var matched = result.Match(
            success: _ => Result.Failure<string>(expectedError),
            failure: _ => Result.Success("error"));

        // Assert
        matched.IsSuccess.Should().BeFalse();
        matched.Error.Should().Be(expectedError);
    }

    [Fact]
    public void Match_ResultWithDifferentTypes_TransformsCorrectly()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult("42");
        
        // Act
        var matched = result.Match(
            success: int.Parse,
            failure: _ => -1);

        // Assert
        matched.Should().Be(42);
    }

    [Fact]
    public void Match_TryWithException_HandlesExceptionAsFailure()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        Try<int> tryFunc = () => throw exception;
        var failureCalled = false;
        ResultError? capturedError = null;

        // Act
        var matched = tryFunc.Match(
            success: _ => Result.Success("success"),
            failure: error =>
            {
                failureCalled = true;
                capturedError = error;
                return Result.Success(error.ToString());
            });

        // Assert
        failureCalled.Should().BeTrue();
        capturedError.Should().NotBeNull();
        capturedError!.ToString().Should().Contain(exception.Message);
    }

    [Fact]
    public void Match_TryWithNestedResults_HandlesNestedTransformations()
    {
        // Arrange
        var tryFunc = TestHelper.CreateTryFuncWithSuccess(SuccessValue);

        // Act
        var matched = tryFunc.Match(
            success: v => Result.Success(Result.Success(v.ToString())),
            failure: e => Result.Success(Result.Failure<string>(e)));

        // Assert
        matched.IsSuccess.Should().BeTrue();
        matched.Value.IsSuccess.Should().BeTrue();
        matched.Value.Value.Should().Be(SuccessValue.ToString());
    }

    [Fact]
    public void Match_ResultWithActionSideEffects_MaintainsOrder()
    {
        // Arrange
        var result = TestHelper.CreateSuccessResult(SuccessValue);
        var operationOrder = new List<object>();

        // Act
        result.Match(
            success: v =>
            {
                operationOrder.Add(1);
                operationOrder.Add(v);
            },
            failure: _ => operationOrder.Add(-1));

        // Assert
        operationOrder.Should().BeEquivalentTo([1, SuccessValue], o => o.WithStrictOrdering());
    }

    [Fact]
    public void Match_TryWithActionSideEffects_HandlesRecursion()
    {
        // Arrange
        var counter = 0;
        Try<int> tryFunc = () =>
        {
            counter++;
            return counter > 1 
                ? Result.Success(counter) 
                : Result.Failure<int>(TestHelper.DefaultResultError);
        };

        var attempts = new List<string>();

        // Act
        tryFunc.Match(
            success: v => attempts.Add($"Success: {v}"),
            failure: _ =>
            {
                attempts.Add($"Attempt: {counter}");
                // Create a new match call for the recursive attempt
                tryFunc.Match(
                    success: v => attempts.Add($"Success: {v}"),
                    failure: _ => attempts.Add($"Unexpected additional failure")
                );
            });

        // Assert
        attempts.Should().BeEquivalentTo(["Attempt: 1", "Success: 2"], o => o.WithStrictOrdering());
    }
}