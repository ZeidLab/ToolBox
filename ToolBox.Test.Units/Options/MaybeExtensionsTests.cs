﻿using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NSubstitute;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Test.Units.Options;

[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
public class MaybeExtensionsTests
{
	private void AlakiMethod()
	{
		// Initial Maybe value
		Maybe<int> maybeNumber = Maybe.Some(10);

		// Function that multiplies the number by 2 and
		// returns an implicitly converted instance of Maybe<int> with the state of Some
		Func<int, Maybe<int>> multiplyByTwo = x => x * 2;

		// Function that converts the number to a string if it's even
		// returns an implicitly converted instance of Maybe<string> with the state of Some
		Func<int, Maybe<string>> convertToString = x => x.ToString();

		// Chaining operations using Bind
		Maybe<string> result = maybeNumber
			.Bind(multiplyByTwo)
			.Bind(convertToString);

		// Output result
		result.Do(
			some: val => Console.WriteLine($"Result: {val}"),
			none: () => Console.WriteLine("No result")
		);
	}
    [Fact]
    public void ToSome_WhenCalledWithNonNullValue_ShouldReturnSomeWithValue()
    {
        // Arrange
        var value = 42;

        // Act
        var maybe = value.ToSome();

        // Assert
        maybe.IsNull.Should().BeFalse();
        maybe.Value.Should().Be(value);
    }

    [Fact]
    public void ToSome_WhenCalledWithNullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        string? nullValue = null;

        // Act & Assert
        var act = () => nullValue!.ToSome();
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToNone_WhenCalledWithValue_ShouldReturnNone()
    {
        // Arrange
        var value = 42;

        // Act
        var maybe = value.ToNone();

        // Assert
        maybe.IsNull.Should().BeTrue();
    }

    [Fact]
    public void Bind_WhenMaybeIsSome_ShouldReturnMappedValue()
    {
        // Arrange
        var maybe = Maybe.Some(42);
        Func<int, Maybe<string>> mapper = x => x.ToString().ToSome();

        // Act
        var result = maybe.Bind(mapper);

        // Assert
        result.IsNull.Should().BeFalse();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void Bind_WhenMaybeIsNone_ShouldReturnNone()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        Func<int, Maybe<string>> mapper = x => x.ToString().ToSome();

        // Act
        var result = maybe.Bind(mapper);

        // Assert
        result.IsNull.Should().BeTrue();
    }

    [Fact]
    public void Map_WhenMaybeIsSome_ShouldApplySomeFunction()
    {
        // Arrange
        var maybe = Maybe.Some(42);
        Func<int, string> some = x => x.ToString();
        var none = () => "none";

        // Act
        var result = maybe.Map(some, none);

        // Assert
        result.Should().Be("42");
    }

    [Fact]
    public void Map_WhenMaybeIsNone_ShouldApplyNoneFunction()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        Func<int, string> some = x => x.ToString();
        var none = () => "none";

        // Act
        var result = maybe.Map(some, none);

        // Assert
        result.Should().Be("none");
    }

    [Fact]
    public void Do_WhenMaybeIsSome_ShouldApplySomeFunction()
    {
        // Arrange
        var maybe = Maybe.Some(42);
		var output = string.Empty;
        Action<int> some = x => output = x.ToString();
        Action none = () => output = "none";

        // Act
        var result = maybe.Do(some, none);

        // Assert
        output.Should().Be("42");
    }

    [Fact]
    public void Do_WhenMaybeIsNone_ShouldApplyNoneFunction()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        var output = string.Empty;
        Action<int> some = x => output = x.ToString();
        Action none = () => output = "none";

        // Act
        var result = maybe.Do(some, none);

        // Assert
        output.Should().Be("none");
    }

    [Fact]
    public void Reduce_WhenMaybeIsSome_ShouldReturnValue()
    {
        // Arrange
        var maybe = Maybe.Some(42);
        var substitute = 0;

        // Act
        var result = maybe.Reduce(substitute);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void Reduce_WhenMaybeIsNone_ShouldReturnSubstituteValue()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        var substitute = 42;

        // Act
        var result = maybe.Reduce(substitute);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void Reduce_WhenMaybeIsNoneAndSubstituteFunctionProvided_ShouldReturnFunctionResult()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        var substitute = () => 42;

        // Act
        var result = maybe.Reduce(substitute);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void DoIfSome_WhenMaybeIsSome_ShouldExecuteAction()
    {
        // Arrange
        var maybe = Maybe.Some(42);
        var action = Substitute.For<Action<int>>();

        // Act
        var result = maybe.DoIfSome(action);

        // Assert
        result.Should().Be(maybe);
        action.Received(1)(42);
    }

    [Fact]
    public void DoIfSome_WhenMaybeIsNone_ShouldNotExecuteAction()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        var action = Substitute.For<Action<int>>();

        // Act
        var result = maybe.DoIfSome(action);

        // Assert
        result.Should().Be(maybe);
        action.DidNotReceiveWithAnyArgs();
    }

    [Fact]
    public void DoIfNone_WhenMaybeIsNone_ShouldExecuteAction()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        var action = Substitute.For<Action>();

        // Act
        var result = maybe.DoIfNone(action);

        // Assert
        result.Should().Be(maybe);
        action.Received(1)();
    }

    [Fact]
    public void DoIfNone_WhenMaybeIsSome_ShouldNotExecuteAction()
    {
        // Arrange
        var maybe = Maybe.Some(42);
        var action = Substitute.For<Action>();

        // Act
        var result = maybe.DoIfNone(action);

        // Assert
        result.Should().Be(maybe);
        action.DidNotReceiveWithAnyArgs();
    }

    [Fact]
    public void If_WhenMaybeIsSomeAndPredicateIsTrue_ShouldReturnTrue()
    {
        // Arrange
        var maybe = Maybe.Some(42);
        Func<int, bool> predicate = x => x > 0;

        // Act
        var result = maybe.If(predicate);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void If_WhenMaybeIsSomeAndPredicateIsFalse_ShouldReturnFalse()
    {
        // Arrange
        var maybe = Maybe.Some(42);
        Func<int, bool> predicate = x => x < 0;

        // Act
        var result = maybe.If(predicate);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void If_WhenMaybeIsNone_ShouldReturnFalse()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        Func<int, bool> predicate = x => x > 0;

        // Act
        var result = maybe.If(predicate);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Filter_WhenMaybeIsSomeAndPredicateIsTrue_ShouldReturnSome()
    {
        // Arrange
        var maybe = Maybe.Some(42);
        Func<int, bool> predicate = x => x > 0;

        // Act
        var result = maybe.Filter(predicate);

        // Assert
        result.IsNull.Should().BeFalse();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Filter_WhenMaybeIsSomeAndPredicateIsFalse_ShouldReturnNone()
    {
        // Arrange
        var maybe = Maybe.Some(42);
        Func<int, bool> predicate = x => x < 0;

        // Act
        var result = maybe.Filter(predicate);

        // Assert
        result.IsNull.Should().BeTrue();
    }

    [Fact]
    public void Filter_WhenMaybeIsNone_ShouldReturnNone()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        Func<int, bool> predicate = x => x > 0;

        // Act
        var result = maybe.Filter(predicate);

        // Assert
        result.IsNull.Should().BeTrue();
    }

    [Fact]
    public void Where_WhenMaybeIsSomeAndPredicateIsTrue_ShouldReturnFilteredSequence()
    {
        // Arrange
        var maybeList = new List<Maybe<int>>
        {
            Maybe.Some(2),
            Maybe.Some(10),
            Maybe.None<int>(),
            Maybe.Some(42)
        };
        Func<int, bool> predicate = x => x > 5;

        // Act
        var result = maybeList.Where(predicate).ToList();

        // Assert
        result.Should().HaveCount(2);
        result[0].Value.Should().Be(10);
        result[1].Value.Should().Be(42);
    }

    [Fact]
    public void Where_WhenMaybeIsSomeAndPredicateIsFalse_ShouldReturnEmptySequence()
    {
        // Arrange
        var maybeList = new List<Maybe<int>>
        {
            Maybe.Some(2),
            Maybe.Some(4),
            Maybe.None<int>(),
            Maybe.Some(6)
        };
        Func<int, bool> predicate = x => x > 10;

        // Act
        var result = maybeList.Where(predicate).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Flatten_WhenSequenceContainsSomeValues_ShouldReturnAllSomeValues()
    {
        // Arrange
        var maybeList = new List<Maybe<int>>
        {
            Maybe.Some(1),
            Maybe.None<int>(),
            Maybe.Some(3)
        };

        // Act
        var result = maybeList.Flatten().ToList();

        // Assert
        result.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void Flatten_WhenSequenceContainsOnlyNone_ShouldReturnEmptySequence()
    {
        // Arrange
        var maybeList = new List<Maybe<int>>
        {
            Maybe.None<int>(),
            Maybe.None<int>(),
            Maybe.None<int>()
        };

        // Act
        var result = maybeList.Flatten().ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Flatten_WhenSubstituteValueProvided_ShouldReplaceNoneWithSubstitute()
    {
        // Arrange
        var maybeList = new List<Maybe<int>>
        {
            Maybe.Some(1),
            Maybe.None<int>(),
            Maybe.Some(3)
        };
        var substitute = 42;

        // Act
        var result = maybeList.Flatten(substitute).ToList();

        // Assert
        result.Should().BeEquivalentTo([1, 42, 3]);
    }

    [Fact]
    public void Flatten_WhenSubstituteFunctionProvided_ShouldReplaceNoneWithFunctionResult()
    {
        // Arrange
        var maybeList = new List<Maybe<int>>
        {
            Maybe.Some(1),
            Maybe.None<int>(),
            Maybe.Some(3)
        };
        var substitute = () => 42;

        // Act
        var result = maybeList.Flatten(substitute).ToList();

        // Assert
        result.Should().BeEquivalentTo([1, 42, 3]);
    }

    [Fact]
    public void ValueOrThrow_WhenMaybeIsSome_ShouldReturnValue()
    {
        // Arrange
        var maybe = Maybe.Some(42);

        // Act
        var result = maybe.ValueOrThrow();

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void ValueOrThrow_WhenMaybeIsNone_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var maybe = Maybe.None<int>();

        // Act
        var act = () => maybe.ValueOrThrow();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}