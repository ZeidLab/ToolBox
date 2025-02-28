﻿using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultErrorTests
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
        var error = new ResultError(code, name, message, exception);

        // Assert
        error.Code.Should().Be(code);
        error.Name.Should().Be(name);
        error.Message.Should().Be(message);
        error.Exception.Should().Be(exception);
    }


    [Fact]
    public void New_WithMessage_ShouldCreateErrorWithDefaultCodeAndName()
    {
        // Arrange
        var message = "Test error message";

        // Act
        var error = ResultError.New(message);

        // Assert
        error.Code.Should().Be(ResultError.DefaultCode);
        error.Name.Should().Be(ResultError.DefaultName);
        error.Message.Should().Be(message);
        error.Exception.Should().BeNull();
    }

    [Fact]
    public void New_WithException_ShouldCreateErrorWithExceptionDetails()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var error = ResultError.New(exception);

        // Assert
        error.Code.Should().Be(exception.HResult);
        error.Name.Should().Be(ResultError.DefaultName);
        error.Message.Should().Be(exception.Message);
        error.Exception.Should().Be(exception);
    }

    [Fact]
    public void New_WithCodeAndMessage_ShouldCreateErrorWithSpecifiedCodeAndDefaultName()
    {
        // Arrange
        var code = 500;
        var message = "Internal server error";

        // Act
        var error = ResultError.New(code, message);

        // Assert
        error.Code.Should().Be(code);
        error.Name.Should().Be(ResultError.DefaultName);
        error.Message.Should().Be(message);
        error.Exception.Should().BeNull();
    }

    [Fact]
    public void New_WithNameAndMessage_ShouldCreateErrorWithSpecifiedNameAndDefaultCode()
    {
        // Arrange
        var name = "CustomError";
        var message = "Custom error message";

        // Act
        var error = ResultError.New(name, message);

        // Assert
        error.Code.Should().Be(ResultError.DefaultCode);
        error.Name.Should().Be(name);
        error.Message.Should().Be(message);
        error.Exception.Should().BeNull();
    }

    [Fact]
    public void New_WithNameMessageAndException_ShouldCreateErrorWithSpecifiedDetails()
    {
        // Arrange
        var name = "CustomError";
        var message = "Custom error message";
        var exception = new Exception("Test exception");

        // Act
        var error = ResultError.New(name, message, exception);

        // Assert
        error.Code.Should().Be(ResultError.DefaultCode);
        error.Name.Should().Be(name);
        error.Message.Should().Be(message);
        error.Exception.Should().Be(exception);
    }

    [Fact]
    public void New_WithCodeMessageAndException_ShouldCreateErrorWithSpecifiedDetails()
    {
        // Arrange
        var code = 400;
        var message = "Bad request";
        var exception = new Exception("Test exception");

        // Act
        var error = ResultError.New(code, message, exception);

        // Assert
        error.Code.Should().Be(code);
        error.Name.Should().Be(ResultError.DefaultName);
        error.Message.Should().Be(message);
        error.Exception.Should().Be(exception);
    }

    [Fact]
    public void New_WithMessageAndException_ShouldCreateErrorWithExceptionDetails()
    {
        // Arrange
        var message = "Test error message";
        var exception = new Exception("Test exception");

        // Act
        var error = ResultError.New(message, exception);

        // Assert
        error.Code.Should().Be(ResultError.DefaultCode);
        error.Name.Should().Be(ResultError.DefaultName);
        error.Message.Should().Be(message);
        error.Exception.Should().Be(exception);
    }

    [Fact]
    public void ImplicitOperator_FromException_ShouldCreateErrorWithExceptionDetails()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        ResultError resultError = exception;

        // Assert
        resultError.Code.Should().Be(exception.HResult);
        resultError.Name.Should().Be(ResultError.DefaultName);
        resultError.Message.Should().Be(exception.Message);
        resultError.Exception.Should().Be(exception);
    }

    [Fact]
    public void ImplicitOperator_FromString_ShouldCreateErrorWithDefaultCodeAndName()
    {
        // Arrange
        var message = "Test error message";

        // Act
        ResultError resultError = message;

        // Assert
        resultError.Code.Should().Be(ResultError.DefaultCode);
        resultError.Name.Should().Be(ResultError.DefaultName);
        resultError.Message.Should().Be(message);
        resultError.Exception.Should().BeNull();
    }

    [Fact]
    public void Deconstruct_ShouldReturnCorrectValues()
    {
        // Arrange
        var code = 404;
        var name = "NotFound";
        var message = "Resource not found";
        var exception = new Exception("Test exception");
        var error = new ResultError(code, name, message, exception);

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
        Action act = () => ResultError.New(message);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void New_WithNullException_ShouldThrowArgumentNullException()
    {
        // Arrange
        Exception exception = null!;

        // Act
        Action act = () => ResultError.New(exception);

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
        Action act = () => ResultError.New(code, message);

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
        Action act = () => ResultError.New(code, message);

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
        Action act = () => ResultError.New(name, message);

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
        Action act = () => ResultError.New(message, exception);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}