using FluentAssertions;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Test.Units.Common;

public class UnitTests
{
    [Fact]
    public void Default_Should_Return_UnitInstance()
    {
        // Act
        Unit unit = Unit.Default;

        // Assert
        unit.Should().NotBeNull();
    }

    [Fact]
    public void ToString_Should_Return_Parentheses()
    {
        // Arrange
        Unit unit = Unit.Default;

        // Act
        string result = unit.ToString();

        // Assert
        result.Should().Be("()");
    }

    [Fact]
    public void CompareTo_Should_Return_Zero()
    {
        // Arrange
        Unit unit1 = Unit.Default;
        Unit unit2 = Unit.Default;

        // Act
        int result = unit1.CompareTo(unit2);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void GreaterThanOperator_Should_Return_False()
    {
        // Arrange
        Unit unit1 = Unit.Default;
        Unit unit2 = Unit.Default;

        // Act
        bool result = unit1 > unit2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GreaterThanOrEqualToOperator_Should_Return_True()
    {
        // Arrange
        Unit unit1 = Unit.Default;
        Unit unit2 = Unit.Default;

        // Act
        bool result = unit1 >= unit2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void LessThanOperator_Should_Return_False()
    {
        // Arrange
        Unit unit1 = Unit.Default;
        Unit unit2 = Unit.Default;

        // Act
        bool result = unit1 < unit2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void LessThanOrEqualToOperator_Should_Return_True()
    {
        // Arrange
        Unit unit1 = Unit.Default;
        Unit unit2 = Unit.Default;

        // Act
        bool result = unit1 <= unit2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AdditionOperator_Should_Return_DefaultUnit()
    {
        // Arrange
        Unit unit1 = Unit.Default;
        Unit unit2 = Unit.Default;

        // Act
        Unit result = unit1 + unit2;

        // Assert
        result.Should().Be(Unit.Default);
    }

    [Fact]
    public void Return_Value_Should_Return_ProvidedValue()
    {
        // Arrange
        Unit unit = Unit.Default;
        int expectedValue = 42;

        // Act
        int result = unit.Return(expectedValue);

        // Assert
        result.Should().Be(expectedValue);
    }

    [Fact]
    public void Return_Func_Should_Execute_FunctionAndReturn_Result()
    {
        // Arrange
        Unit unit = Unit.Default;
        int expectedValue = 42;
        Func<int> func = () => expectedValue;

        // Act
        int result = unit.Return(func);

        // Assert
        result.Should().Be(expectedValue);
    }

    [Fact]
    public void ImplicitOperator_UnitToValueTuple_Should_ConvertCorrectly()
    {
        // Arrange
        Unit unit = Unit.Default;

        // Act
        ValueTuple valueTuple = unit;

        // Assert
        valueTuple.Should().Be(ValueTuple.Create());
    }

    [Fact]
    public void ImplicitOperator_ValueTupleToUnit_Should_ConvertCorrectly()
    {
        // Arrange
        ValueTuple valueTuple = ValueTuple.Create();

        // Act
        Unit unit = valueTuple;

        // Assert
        unit.Should().Be(Unit.Default);
    }
}