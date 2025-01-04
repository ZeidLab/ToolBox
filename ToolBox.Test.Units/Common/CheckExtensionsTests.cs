using FluentAssertions;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Test.Units.Common;

public class CheckExtensionsTests
{
    // Tests for IsDefault<TIn>

    [Fact]
    public void Given_DefaultInteger_When_CheckingIsDefault_Then_ReturnsTrue()
    {
        // Arrange
        int value = default;

        // Act
        var result = value.IsDefault();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Given_NonDefaultInteger_When_CheckingIsDefault_Then_ReturnsFalse()
    {
        // Arrange
        var value = 10;

        // Act
        var result = value.IsDefault();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Given_DefaultBoolean_When_CheckingIsDefault_Then_ReturnsTrue()
    {
        // Arrange
        bool value = default;

        // Act
        var result = value.IsDefault();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Given_DefaultString_When_CheckingIsDefault_Then_ReturnsTrue()
    {
        // Arrange
        string value = default;

        // Act
        var result = value.IsDefault();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Given_NonDefaultString_When_CheckingIsDefault_Then_ReturnsFalse()
    {
        // Arrange
        var value = "Test";

        // Act
        var result = value.IsDefault();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Given_NullableIntWithNull_When_CheckingIsDefault_Then_ReturnsTrue()
    {
        // Arrange
        int? value = null;

        // Act
        var result = value.IsDefault();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Given_NullableIntWithNonNull_When_CheckingIsDefault_Then_ReturnsFalse()
    {
        // Arrange
        int? value = 5;

        // Act
        var result = value.IsDefault();

        // Assert
        result.Should().BeFalse();
    }

    // Tests for IsNull<TIn>

    [Fact]
    public void Given_NullString_When_CheckingIsNull_Then_ReturnsTrue()
    {
        // Arrange
        string value = null;

        // Act
        var result = value.IsNull();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Given_NonNullString_When_CheckingIsNull_Then_ReturnsFalse()
    {
        // Arrange
        var value = "Test";

        // Act
        var result = value.IsNull();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Given_DefaultInt_When_CheckingIsNull_Then_ReturnsFalse()
    {
        // Arrange
        int value = default;

        // Act
        var result = value.IsNull();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Given_NullableIntWithNull_When_CheckingIsNull_Then_ReturnsTrue()
    {
        // Arrange
        int? value = null;

        // Act
        var result = value.IsNull();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Given_NullableIntWithNonNull_When_CheckingIsNull_Then_ReturnsFalse()
    {
        // Arrange
        int? value = 5;

        // Act
        var result = value.IsNull();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Given_DefaultDateTime_When_CheckingIsNull_Then_ReturnsFalse()
    {
        // Arrange
        DateTime value = default;

        // Act
        var result = value.IsNull();

        // Assert
        result.Should().BeFalse();
    }
}