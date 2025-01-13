using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultTests
{
    [Fact]
    public void Success_ShouldReturnResultWithValue_WhenValueIsNotNull()
    {
        // Arrange
        var value = "success";

        // Act
        var result = Result<string>.Success(value);

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
        Action act = () => Result<string>.Success(value);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_ShouldReturnResultWithError_WhenErrorIsNotNull()
    {
        // Arrange
        var error = ResultError.New("failure");

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.ResultError.Should().Be(error);
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
        result.ResultError.Should().Be(error);
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
        result.ResultError.Should().NotBeNull();
        result.ResultError.Message.Should().Be(exception.Message);
    }

    [Fact]
    public void IsDefault_ShouldReturnTrue_WhenValueIsDefault()
    {
        // Arrange
        var result = Result<int>.Success(0);

        // Act & Assert
        result.IsDefault.Should().BeTrue();
    }

    [Fact]
    public void IsDefault_ShouldReturnFalse_WhenValueIsNotDefault()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act & Assert
        result.IsDefault.Should().BeFalse();
    }

    [Fact]
    public void TryDelegate_ShouldReturnSuccessResult_WhenDelegateDoesNotThrow()
    {
        // Arrange
        var tryDelegate = new Try<string>(() => Result<string>.Success("success"));

        // Act
        var result = tryDelegate();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("success");
        
    }
    
    [Fact]
    public async Task TryAsyncDelegate_ShouldReturnSuccessResult_WhenDelegateDoesNotThrow()
    {
        // Arrange
        var tryAsyncDelegate = new TryAsync<string>(() => Task.FromResult(Result<string>.Success("success")));

        // Act
        var result = await tryAsyncDelegate();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("success");
        
    }

   
}