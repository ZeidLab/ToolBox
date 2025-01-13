using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultErrorExtensionsTests
{
    [Fact]
    public void TryGetException_WhenUnhandledExceptionIsNotNull_ShouldReturnTrueAndException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var error = new ResultError(1, "TestError", "Test message", exception);

        // Act
        var result = error.TryGetException(out var actualException);

        // Assert
        result.Should().BeTrue();
        actualException.Should().Be(exception);
    }

    [Fact]
    public void TryGetException_WhenUnhandledExceptionIsNull_ShouldReturnFalseAndNull()
    {
        // Arrange
        var error = new ResultError(1, "TestError", "Test message");

        // Act
        var result = error.TryGetException(out var actualException);

        // Assert
        result.Should().BeFalse();
        actualException.Should().BeNull();
    }

    [Fact]
    public void TryGetException_WhenUnhandledExceptionIsNotNull_ShouldReturnSomeException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var error = new ResultError(1, "TestError", "Test message", exception);

        // Act
        var result = error.TryGetException();

        // Assert
        result.IsSome.Should().BeTrue();
        result.Content.Should().Be(exception);
    }

    [Fact]
    public void TryGetException_WhenUnhandledExceptionIsNull_ShouldReturnNone()
    {
        // Arrange
        var error = new ResultError(1, "TestError", "Test message");

        // Act
        var result = error.TryGetException();

        // Assert
        result.IsSome.Should().BeFalse();
    }

    [Fact]
    public void WithCode_WhenCodeIsPositive_ShouldReturnNewErrorWithUpdatedCode()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        var newCode = 2;

        // Act
        var result = originalError.WithCode(newCode);

        // Assert
        result.Code.Should().Be(newCode);
        result.Name.Should().Be(originalError.Name);
        result.Message.Should().Be(originalError.Message);
        result.Exception.Should().Be(originalError.Exception);
    }

    [Fact]
    public void WithCode_WhenCodeIsZero_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        var newCode = 0;

        // Act
        Action act = () => originalError.WithCode(newCode);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void WithCode_WhenCodeIsNegative_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        var newCode = -1;

        // Act
        Action act = () => originalError.WithCode(newCode);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void WithMessage_WhenMessageIsValid_ShouldReturnNewErrorWithUpdatedMessage()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        var newMessage = "New message";

        // Act
        var result = originalError.WithMessage(newMessage);

        // Assert
        result.Message.Should().Be(newMessage);
        result.Code.Should().Be(originalError.Code);
        result.Name.Should().Be(originalError.Name);
        result.Exception.Should().Be(originalError.Exception);
    }

    [Fact]
    public void WithMessage_WhenMessageIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        string newMessage = null!;

        // Act
        Action act = () => originalError.WithMessage(newMessage);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WithMessage_WhenMessageIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        var newMessage = string.Empty;

        // Act
        Action act = () => originalError.WithMessage(newMessage);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WithName_WhenNameIsValid_ShouldReturnNewErrorWithUpdatedName()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        var newName = "NewErrorName";

        // Act
        var result = originalError.WithName(newName);

        // Assert
        result.Name.Should().Be(newName);
        result.Code.Should().Be(originalError.Code);
        result.Message.Should().Be(originalError.Message);
        result.Exception.Should().Be(originalError.Exception);
    }

    [Fact]
    public void WithName_WhenNameIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        string newName = null!;

        // Act
        Action act = () => originalError.WithName(newName);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WithName_WhenNameIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        var newName = string.Empty;

        // Act
        Action act = () => originalError.WithName(newName);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WithException_WhenExceptionIsValid_ShouldReturnNewErrorWithUpdatedException()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        var newException = new Exception("New exception");

        // Act
        var result = originalError.WithException(newException);

        // Assert
        result.Exception.Should().Be(newException);
        result.Code.Should().Be(originalError.Code);
        result.Name.Should().Be(originalError.Name);
        result.Message.Should().Be(originalError.Message);
    }

    [Fact]
    public void WithException_WhenExceptionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var originalError = new ResultError(1, "TestError", "Test message");
        Exception newException = null!;

        // Act
        Action act = () => originalError.WithException(newException);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}