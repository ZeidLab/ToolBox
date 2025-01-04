using FluentAssertions;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Test.Units.Options;

public class MaybeTests
{
    #region Refrence Type Tests

    [Fact]
    public void Given_Maybe_Default_Constructor_When_Created_Then_IsNone_True()
    {
        // Arrange
        var maybe = new Maybe<string>();

        // Assert
        maybe.IsNone.Should().BeTrue();
        maybe.IsSome.Should().BeFalse();
        maybe.Content.Should().BeNull();
    }


    [Fact]
    public void Given_Maybe_Some_Method_With_Null_Value_When_Called_Then_Throws_ArgumentNullException()
    {
        // Arrange
        string value = null;

        // Act & Assert
        var action = () => Maybe<string>.Some(value);
        action.Should().Throw<ArgumentNullException>().WithParameterName("value");
    }

    [Fact]
    public void Given_Maybe_Some_Method_With_Value_When_Called_Then_Creates_Maybe_With_Content()
    {
        // Arrange
        var value = "TestValue";

        // Act
        var maybe = Maybe<string>.Some(value);

        // Assert
        maybe.IsSome.Should().BeTrue();
        maybe.IsNone.Should().BeFalse();
        maybe.Content.Should().Be(value);
    }

    [Fact]
    public void Given_Maybe_None_Method_When_Called_Then_Returns_Maybe_With_IsNone_True()
    {
        // Arrange
        var maybe = Maybe<string>.None();

        // Assert
        maybe.IsNone.Should().BeTrue();
        maybe.IsSome.Should().BeFalse();
        maybe.Content.Should().BeNull();
    }

    [Fact]
    public void Given_Implicit_Operator_With_Value_When_Converted_Then_Creates_Maybe_With_Content()
    {
        // Arrange
        var value = "TestValue";

        // Act
        Maybe<string> maybe = value;

        // Assert
        maybe.IsSome.Should().BeTrue();
        maybe.IsNone.Should().BeFalse();
        maybe.Content.Should().Be(value);
    }

    [Fact]
    public void Given_CompareTo_Both_None_When_Compared_Then_Returns_Zero()
    {
        // Arrange
        var maybe1 = Maybe<string>.None();
        var maybe2 = Maybe<string>.None();

        // Act
        var result = maybe1.CompareTo(maybe2);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Given_CompareTo_Left_None_Right_Some_When_Compared_Then_Returns_Negative_One()
    {
        // Arrange
        var maybe1 = Maybe<string>.None();
        var maybe2 = Maybe<string>.Some("TestValue");

        // Act
        var result = maybe1.CompareTo(maybe2);

        // Assert
        result.Should().Be(-1);
    }

    [Fact]
    public void Given_CompareTo_Left_Some_Right_None_When_Compared_Then_Returns_Positive_One()
    {
        // Arrange
        var maybe1 = Maybe<string>.Some("TestValue");
        var maybe2 = Maybe<string>.None();

        // Act
        var result = maybe1.CompareTo(maybe2);

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void Given_CompareTo_Both_Some_With_Equal_Content_When_Compared_Then_Returns_Zero()
    {
        // Arrange
        var maybe1 = Maybe<string>.Some("TestValue");
        var maybe2 = Maybe<string>.Some("TestValue");

        // Act
        var result = maybe1.CompareTo(maybe2);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Given_CompareTo_Both_Some_With_Different_Content_When_Compared_Then_Returns_Correct_Order()
    {
        // Arrange
        var maybe1 = Maybe<string>.Some("Apple");
        var maybe2 = Maybe<string>.Some("Banana");

        // Act
        var result = maybe1.CompareTo(maybe2);

        // Assert
        result.Should().BeLessThan(0);
    }

    [Fact]
    public void Given_CompareTo_With_Object_When_Compared_Then_Throws_ArgumentException()
    {
        // Arrange
        var maybe = Maybe<string>.Some("TestValue");
        object obj = new object();

        // Act & Assert
        var action = () => maybe.CompareTo(obj);
        action.Should().Throw<ArgumentException>()
            .WithMessage($"Object must be of type {typeof(Maybe<string>)}");
    }

    #endregion

    #region ValuTypes tests

    // Tests for int
    [Fact]
    public void Given_Maybe_Int_Some_NonDefault_When_Created_Then_IsSome_True_IsDefault_False()
    {
        var maybe = Maybe<int>.Some(5);
        maybe.IsSome.Should().BeTrue();
        maybe.IsNone.Should().BeFalse();
        maybe.IsDefault.Should().BeFalse();
        maybe.Content.Should().Be(5);
    }

    [Fact]
    public void Given_Maybe_Int_Some_Default_When_Created_Then_IsSome_True_IsDefault_True()
    {
        var maybe = Maybe<int>.Some(0);
        maybe.IsSome.Should().BeTrue();
        maybe.IsNone.Should().BeFalse();
        maybe.IsDefault.Should().BeTrue();
        maybe.Content.Should().Be(0);
    }

    [Fact]
    public void Given_Maybe_Int_None_When_Created_Then_IsNone_True_Content_Default()
    {
        var maybe = Maybe<int>.None();
        maybe.IsNone.Should().BeTrue();
        maybe.IsSome.Should().BeFalse();
        maybe.Content.Should().Be(default(int));
    }

    // Tests for decimal
    [Fact]
    public void Given_Maybe_Decimal_Some_NonDefault_When_Created_Then_IsSome_True_IsDefault_False()
    {
        var maybe = Maybe<decimal>.Some(10.5m);
        maybe.IsSome.Should().BeTrue();
        maybe.IsNone.Should().BeFalse();
        maybe.IsDefault.Should().BeFalse();
        maybe.Content.Should().Be(10.5m);
    }

    [Fact]
    public void Given_Maybe_Decimal_Some_Default_When_Created_Then_IsSome_True_IsDefault_True()
    {
        var maybe = Maybe<decimal>.Some(0.0m);
        maybe.IsSome.Should().BeTrue();
        maybe.IsNone.Should().BeFalse();
        maybe.IsDefault.Should().BeTrue();
        maybe.Content.Should().Be(0.0m);
    }

    [Fact]
    public void Given_Maybe_Decimal_None_When_Created_Then_IsNone_True_Content_Default()
    {
        var maybe = Maybe<decimal>.None();
        maybe.IsNone.Should().BeTrue();
        maybe.IsSome.Should().BeFalse();
        maybe.Content.Should().Be(default(decimal));
    }

    // Tests for Guid
    [Fact]
    public void Given_Maybe_Guid_Some_NonDefault_When_Created_Then_IsSome_True_IsDefault_False()
    {
        var guid = Guid.NewGuid();
        var maybe = Maybe<Guid>.Some(guid);
        maybe.IsSome.Should().BeTrue();
        maybe.IsNone.Should().BeFalse();
        maybe.IsDefault.Should().BeFalse();
        maybe.Content.Should().Be(guid);
    }

    [Fact]
    public void Given_Maybe_Guid_Some_Default_When_Created_Then_IsSome_True_IsDefault_True()
    {
        var maybe = Maybe<Guid>.Some(Guid.Empty);
        maybe.IsSome.Should().BeTrue();
        maybe.IsNone.Should().BeFalse();
        maybe.IsDefault.Should().BeTrue();
        maybe.Content.Should().Be(Guid.Empty);
    }

    [Fact]
    public void Given_Maybe_Guid_None_When_Created_Then_IsNone_True_Content_Default()
    {
        var maybe = Maybe<Guid>.None();
        maybe.IsNone.Should().BeTrue();
        maybe.IsSome.Should().BeFalse();
        maybe.Content.Should().Be(default(Guid));
    }

    // Comparison tests for int
    [Fact]
    public void Given_Maybe_Int_CompareTo_Same_When_Compared_Then_Returns_Zero()
    {
        var maybe1 = Maybe<int>.Some(5);
        var maybe2 = Maybe<int>.Some(5);
        maybe1.CompareTo(maybe2).Should().Be(0);
    }

    [Fact]
    public void Given_Maybe_Int_CompareTo_Less_When_Compared_Then_Returns_Negative()
    {
        var maybe1 = Maybe<int>.Some(3);
        var maybe2 = Maybe<int>.Some(5);
        maybe1.CompareTo(maybe2).Should().BeLessThan(0);
    }

    [Fact]
    public void Given_Maybe_Int_CompareTo_Greater_When_Compared_Then_Returns_Positive()
    {
        var maybe1 = Maybe<int>.Some(7);
        var maybe2 = Maybe<int>.Some(5);
        maybe1.CompareTo(maybe2).Should().BeGreaterThan(0);
    }

    // Comparison tests for decimal
    [Fact]
    public void Given_Maybe_Decimal_CompareTo_Same_When_Compared_Then_Returns_Zero()
    {
        var maybe1 = Maybe<decimal>.Some(10.5m);
        var maybe2 = Maybe<decimal>.Some(10.5m);
        maybe1.CompareTo(maybe2).Should().Be(0);
    }

    [Fact]
    public void Given_Maybe_Decimal_CompareTo_Less_When_Compared_Then_Returns_Negative()
    {
        var maybe1 = Maybe<decimal>.Some(5.5m);
        var maybe2 = Maybe<decimal>.Some(10.5m);
        maybe1.CompareTo(maybe2).Should().BeLessThan(0);
    }

    [Fact]
    public void Given_Maybe_Decimal_CompareTo_Greater_When_Compared_Then_Returns_Positive()
    {
        var maybe1 = Maybe<decimal>.Some(15.5m);
        var maybe2 = Maybe<decimal>.Some(10.5m);
        maybe1.CompareTo(maybe2).Should().BeGreaterThan(0);
    }

    // Comparison tests for Guid
    [Fact]
    public void Given_Maybe_Guid_CompareTo_Same_When_Compared_Then_Returns_Zero()
    {
        var guid = Guid.NewGuid();
        var maybe1 = Maybe<Guid>.Some(guid);
        var maybe2 = Maybe<Guid>.Some(guid);
        maybe1.CompareTo(maybe2).Should().Be(0);
    }

    [Fact]
    public void Given_Maybe_Guid_CompareTo_Different_When_Compared_Then_Returns_Correct_Order()
    {
        var maybe1 = Maybe<Guid>.Some(Guid.NewGuid());
        var maybe2 = Maybe<Guid>.Some(Guid.NewGuid());
        maybe1.CompareTo(maybe2).Should().NotBe(0);
    }

    // General comparison tests
    [Fact]
    public void Given_Maybe_None_CompareTo_None_When_Compared_Then_Returns_Zero()
    {
        var maybe1 = Maybe<int>.None();
        var maybe2 = Maybe<int>.None();
        maybe1.CompareTo(maybe2).Should().Be(0);
    }

    [Fact]
    public void Given_Maybe_None_CompareTo_Some_When_Compared_Then_Returns_Negative_One()
    {
        var maybe1 = Maybe<int>.None();
        var maybe2 = Maybe<int>.Some(5);
        maybe1.CompareTo(maybe2).Should().Be(-1);
    }

    [Fact]
    public void Given_Maybe_Some_CompareTo_None_When_Compared_Then_Returns_Positive_One()
    {
        var maybe1 = Maybe<int>.Some(5);
        var maybe2 = Maybe<int>.None();
        maybe1.CompareTo(maybe2).Should().Be(1);
    }

    // Implicit operator tests
    [Fact]
    public void Given_Implicit_Operator_Int_When_Converted_Then_Creates_Maybe_With_Content()
    {
        Maybe<int> maybe = 10;
        maybe.IsSome.Should().BeTrue();
        maybe.Content.Should().Be(10);
    }

    [Fact]
    public void Given_Implicit_Operator_Decimal_When_Converted_Then_Creates_Maybe_With_Content()
    {
        Maybe<decimal> maybe = 20.5m;
        maybe.IsSome.Should().BeTrue();
        maybe.Content.Should().Be(20.5m);
    }

    [Fact]
    public void Given_Implicit_Operator_Guid_When_Converted_Then_Creates_Maybe_With_Content()
    {
        var guid = Guid.NewGuid();
        Maybe<Guid> maybe = guid;
        maybe.IsSome.Should().BeTrue();
        maybe.Content.Should().Be(guid);
    }

    // Exception tests for CompareTo
    [Fact]
    public void Given_Maybe_CompareTo_With_Non_Maybe_Object_When_Compared_Then_Throws_ArgumentException()
    {
        var maybe = Maybe<int>.Some(5);
        var obj = new object();
       
        Func<int> act = () => maybe.CompareTo(obj);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Object must be of type {typeof(Maybe<int>)}");
    }

    #endregion
}