using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultTests
{
    private record struct CustomValueType(int Value);

    [Fact]
    public void Success_ShouldReturnResultWithValue_WhenValueIsNotNull()
    {
        // Arrange
        var value = "success";

        // Act
        var result = Result.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);

    }

    [Fact]
    public void Success_ShouldThrowArgumentNullException_WhenValueIsNull()
    {
        // Arrange
        string value = null!;

        // Act
        Action act = () => Result.Success(value);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_ShouldReturnResultWithError_WhenErrorIsNotNull()
    {
        // Arrange
        var error = ResultError.New("failure");

        // Act
        var result = Result.Failure<string>(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void ImplicitOperator_ShouldReturnSuccessResult_WhenValueIsProvided()
    {
        // Arrange
        string value = "success";

        // Act
        Result<string> result = value;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);

    }

    [Fact]
    public void ImplicitOperator_ShouldReturnFailureResult_WhenErrorIsProvided()
    {
        // Arrange
        var error = ResultError.New("failure");

        // Act
        Result<string> result = error;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void ImplicitOperator_ShouldReturnFailureResult_WhenExceptionIsProvided()
    {
        // Arrange
        var exception = new Exception("failure");

        // Act
        Result<string> result = exception;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error.Message.Should().Be(exception.Message);
    }

    [Fact]
    public void IsDefault_ShouldReturnTrue_WhenValueIsDefault()
    {
        // Arrange
        var result = Result.Success(0);

        // Act & Assert
        result.IsDefault.Should().BeTrue();
    }

    [Fact]
    public void IsDefault_ShouldReturnFalse_WhenValueIsNotDefault()
    {
        // Arrange
        var result = Result.Success(42);

        // Act & Assert
        result.IsDefault.Should().BeFalse();
    }

    [Fact]
    public void Failure_ShouldAcceptNullError_WhenCreatingFailureResult()
    {
        // Arrange & Act
        var result = Result.Failure<string>(default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Name.Should().Be(ResultError.Default.Name);
        result.Error.Code.Should().Be(ResultError.Default.Code);
    }

    [Fact]
    public void FromException_ShouldCreateFailureResult_WithExceptionMessage()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = Result.FromException<string>(exception);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Message.Should().Be(exception.Message);
    }

    [Fact]
    public void IsFailure_ShouldBeOppositeOfIsSuccess()
    {
        // Arrange
        var successResult = Result.Success("test");
        var failureResult = Result.Failure<string>(ResultError.New("error"));

        // Assert
        successResult.IsFailure.Should().BeFalse();
        failureResult.IsFailure.Should().BeTrue();
    }


    [Fact]
    public void Success_ShouldWorkWithValueTypes()
    {
        // Arrange & Act
        var result = Result.Success(42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        result.Error.Should().Be(ResultError.Default);
    }

    [Fact]
    public void Result_WithDefaultStruct_ShouldDetectDefaultValue()
    {
        // Arrange & Act
        var result = Result.Success(default(DateTime));

        // Assert
        result.IsDefault.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(default);
    }


    [Fact]
    public void Result_ShouldHaveValueSemantics()
    {
        // Arrange
        var result1 = Result.Success("test");
        var result2 = Result.Success("test");
        var result3 = Result.Success("different");

        // Assert
        result1.Should().Be(result2);
        result1.Should().NotBe(result3);
    }

    [Fact]
    public void Result_WithCustomStruct_ShouldHandleDefaultCorrectly()
    {
        // Arrange & Act
        var defaultResult = Result.Success(new CustomValueType());
        var nonDefaultResult = Result.Success(new CustomValueType(42));

        // Assert
        defaultResult.IsDefault.Should().BeTrue();
        nonDefaultResult.IsDefault.Should().BeFalse();
    }
}