using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ErrorTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var code = 404;
        var name = "NotFound";
        var message = "Resource not found";
        var exception = new Exception("Test exception");

        // Act
        var error = new Error(code, name, message, exception);

        // Assert
        error.Code.Should().Be(code);
        error.Name.Should().Be(name);
        error.Message.Should().Be(message);
        error.UnhandledException.Should().Be(exception);
    }

    [Fact]
    public void PublicConstructor_ShouldThrowInvalidOperationException()
    {
        // arrange && act
#pragma warning disable CS0618 // Type or member is obsolete
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new Error();
#pragma warning restore CS0618 // Type or member is obsolete

        // assert
        act.Should().Throw<InvalidOperationException>();
    }
    [Fact]
    public void New_WithMessage_ShouldCreateErrorWithDefaultCodeAndName()
    {
        // Arrange
        var message = "Test error message";

        // Act
        var error = Error.New(message);

        // Assert
        error.Code.Should().Be(Error.DefaultCode);
        error.Name.Should().Be(Error.DefaultName);
        error.Message.Should().Be(message);
        error.UnhandledException.Should().BeNull();
    }

    [Fact]
    public void New_WithException_ShouldCreateErrorWithExceptionDetails()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var error = Error.New(exception);

        // Assert
        error.Code.Should().Be(exception.HResult);
        error.Name.Should().Be(Error.DefaultName);
        error.Message.Should().Be(exception.Message);
        error.UnhandledException.Should().Be(exception);
    }

    [Fact]
    public void New_WithCodeAndMessage_ShouldCreateErrorWithSpecifiedCodeAndDefaultName()
    {
        // Arrange
        var code = 500;
        var message = "Internal server error";

        // Act
        var error = Error.New(code, message);

        // Assert
        error.Code.Should().Be(code);
        error.Name.Should().Be(Error.DefaultName);
        error.Message.Should().Be(message);
        error.UnhandledException.Should().BeNull();
    }

    [Fact]
    public void New_WithNameAndMessage_ShouldCreateErrorWithSpecifiedNameAndDefaultCode()
    {
        // Arrange
        var name = "CustomError";
        var message = "Custom error message";

        // Act
        var error = Error.New(name, message);

        // Assert
        error.Code.Should().Be(Error.DefaultCode);
        error.Name.Should().Be(name);
        error.Message.Should().Be(message);
        error.UnhandledException.Should().BeNull();
    }

    [Fact]
    public void New_WithNameMessageAndException_ShouldCreateErrorWithSpecifiedDetails()
    {
        // Arrange
        var name = "CustomError";
        var message = "Custom error message";
        var exception = new Exception("Test exception");

        // Act
        var error = Error.New(name, message, exception);

        // Assert
        error.Code.Should().Be(exception.HResult);
        error.Name.Should().Be(name);
        error.Message.Should().Be(message);
        error.UnhandledException.Should().Be(exception);
    }

    [Fact]
    public void New_WithCodeMessageAndException_ShouldCreateErrorWithSpecifiedDetails()
    {
        // Arrange
        var code = 400;
        var message = "Bad request";
        var exception = new Exception("Test exception");

        // Act
        var error = Error.New(code, message, exception);

        // Assert
        error.Code.Should().Be(code);
        error.Name.Should().Be(Error.DefaultName);
        error.Message.Should().Be(message);
        error.UnhandledException.Should().Be(exception);
    }

    [Fact]
    public void New_WithMessageAndException_ShouldCreateErrorWithExceptionDetails()
    {
        // Arrange
        var message = "Test error message";
        var exception = new Exception("Test exception");

        // Act
        var error = Error.New(message, exception);

        // Assert
        error.Code.Should().Be(exception.HResult);
        error.Name.Should().Be(Error.DefaultName);
        error.Message.Should().Be(message);
        error.UnhandledException.Should().Be(exception);
    }

    [Fact]
    public void ImplicitOperator_FromException_ShouldCreateErrorWithExceptionDetails()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        Error error = exception;

        // Assert
        error.Code.Should().Be(exception.HResult);
        error.Name.Should().Be(Error.DefaultName);
        error.Message.Should().Be(exception.Message);
        error.UnhandledException.Should().Be(exception);
    }

    [Fact]
    public void ImplicitOperator_FromString_ShouldCreateErrorWithDefaultCodeAndName()
    {
        // Arrange
        var message = "Test error message";

        // Act
        Error error = message;

        // Assert
        error.Code.Should().Be(Error.DefaultCode);
        error.Name.Should().Be(Error.DefaultName);
        error.Message.Should().Be(message);
        error.UnhandledException.Should().BeNull();
    }

    [Fact]
    public void Deconstruct_ShouldReturnCorrectValues()
    {
        // Arrange
        var code = 404;
        var name = "NotFound";
        var message = "Resource not found";
        var exception = new Exception("Test exception");
        var error = new Error(code, name, message, exception);

        // Act
        var (deconstructedCode, deconstructedName, deconstructedMessage, deconstructedException) = error;

        // Assert
        deconstructedCode.Should().Be(code);
        deconstructedName.Should().Be(name);
        deconstructedMessage.Should().Be(message);
        deconstructedException.Should().Be(exception);
    }

    [Fact]
    public void New_WithNullMessage_ShouldThrowArgumentException()
    {
        // Arrange
        string message = null!;

        // Act
        Action act = () => Error.New(message);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void New_WithNullException_ShouldThrowArgumentNullException()
    {
        // Arrange
        Exception exception = null!;

        // Act
        Action act = () => Error.New(exception);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'exception')");
    }

    [Fact]
    public void New_WithZeroCode_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var code = 0;
        var message = "Test message";

        // Act
        Action act = () => Error.New(code, message);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void New_WithNegativeCode_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var code = -1;
        var message = "Test message";
           
        // Act
        Action act = () => Error.New(code, message);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void New_WithNullName_ShouldThrowArgumentException()
    {
        // Arrange
        string name = null!;
        var message = "Test message";

        // Act
        Action act = () => Error.New(name, message);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void New_WithNullMessageAndException_ShouldThrowArgumentException()
    {
        // Arrange
        string message = null!;
        var exception = new Exception("Test exception");

        // Act
        Action act = () => Error.New(message, exception);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}