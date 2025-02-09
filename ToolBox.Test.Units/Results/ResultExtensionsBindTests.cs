using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

/// <summary>
/// Contains unit tests for the synchronous Result extension methods related to binding operations.
/// These tests verify the behavior of synchronous binding operations on Result and Try types.
/// </summary>
public class ResultExtensionsBindTests
{
    #region Synchronous Result Binding Tests

    [Fact]
    public void When_BindingSuccessResult_Should_ApplyFunction()
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
    public void When_BindingFailureResult_Should_PropagateError()
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

    #endregion

    #region Try Binding Tests

    [Fact]
    public void When_BindingSuccessfulTry_Should_ApplyFunction()
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
    public void When_BindingFailedTry_Should_PropagateError()
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
    public void When_BindingSuccessfulTryToTry_Should_ApplyFunction()
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
    public void When_BindingFailedTryToTry_Should_PropagateError()
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

    #endregion
}