using FluentAssertions;
using NSubstitute;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Test.Units.Options;

public class MaybeExtensionsTests
{
    [Fact]
    public void ToSome_GivenNonNullValue_ReturnsSomeWithValue()
    {
        // Arrange
        int value = 42;

        // Act
        Maybe<int> maybe = value.ToSome();

        // Assert
        maybe.IsNull.Should().BeFalse();
        maybe.Value.Should().Be(value);
    }

    [Fact]
    public void ToNone_GivenNonNullValue_ReturnsNone()
    {
        // Arrange
        int value = 42;

        // Act
        Maybe<int> maybe = value.ToNone();

        // Assert
        maybe.IsNull.Should().BeTrue();
    }

    [Fact]
    public void Bind_GivenSomeValue_ReturnsNewValue()
    {
        // Arrange
        Maybe<int> maybe = Maybe<int>.Some(42);
        Func<int, Maybe<string>> mapper = x => x.ToString().ToSome();

        // Act
        Maybe<string> result = maybe.Bind(mapper);

        // Assert
        result.IsNull.Should().BeFalse();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void Bind_GivenNone_ReturnsNone()
    {
        // Arrange
        Maybe<int> maybe = Maybe<int>.None();
        Func<int, Maybe<string>> mapper = x => x.ToString().ToSome();


        // Act
        Maybe<string> result = maybe.Bind(mapper);

        // Assert
        result.IsNull.Should().BeTrue();
    }

    [Fact]
    public void Reduce_GivenSomeValue_ReturnsValue()
    {
        // Arrange
        Maybe<int> maybe = Maybe<int>.Some(42);
        int substitute = 0;

        // Act
        int result = maybe.Reduce(substitute);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void Reduce_GivenNone_ReturnsSubstituteValue()
    {
        // Arrange
        Maybe<int> maybe = Maybe<int>.None();
        int substitute = 42;

        // Act
        int result = maybe.Reduce(substitute);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void Reduce_GivenNoneAndSubstituteFunction_ReturnsFunctionResult()
    {
        // Arrange
        Maybe<int> maybe = Maybe<int>.None();
        Func<int> substitute = () => 42;

        // Act
        int result = maybe.Reduce(substitute);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void DoIfSome_GivenSomeValue_ExecutesAction()
    {
        // Arrange
        Maybe<int> maybe = Maybe<int>.Some(42);
        Action<int> action = Substitute.For<Action<int>>();

        // Act
        Maybe<int> result = maybe.DoIfSome(action);

        // Assert
        result.Should().Be(maybe);
        action.Received(1)(42);
    }

    [Fact]
    public void DoIfSome_GivenNone_DoesNotExecuteAction()
    {
        // Arrange
        Maybe<int> maybe = Maybe<int>.None();
        Action<int> action = Substitute.For<Action<int>>();

        // Act
        Maybe<int> result = maybe.DoIfSome(action);

        // Assert
        result.Should().Be(maybe);
        action.DidNotReceiveWithAnyArgs();
    }

    [Fact]
    public void DoIfNone_GivenNone_ExecutesAction()
    {
        // Arrange
        Maybe<int> maybe = Maybe<int>.None();
        Action action = Substitute.For<Action>();

        // Act
        Maybe<int> result = maybe.DoIfNone(action);

        // Assert
        result.Should().Be(maybe);
        action.Received(1)();
    }

    [Fact]
    public void DoIfNone_GivenSome_DoesNotExecuteAction()
    {
        // Arrange
        Maybe<int> maybe = Maybe<int>.Some(42);
        Action action = Substitute.For<Action>();

        // Act
        Maybe<int> result = maybe.DoIfNone(action);

        // Assert
        result.Should().Be(maybe);
        action.DidNotReceiveWithAnyArgs();
    }

    [Fact]
        public void If_WhenMaybeIsSomeAndPredicateIsTrue_ShouldReturnTrue()
        {
            // Arrange
            var maybe = Maybe<int>.Some(42);
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
            var maybe = Maybe<int>.Some(42);
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
            var maybe = Maybe<int>.None();
            Func<int, bool> predicate = x => x > 0;

            // Act
            var result = maybe.If(predicate);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Where_WhenMaybeIsSomeAndPredicateIsTrue_ShouldReturnFilteredSequence()
        {
            // Arrange
            var maybeList = new List<Maybe<int>>
            {
                Maybe<int>.Some(2),
                Maybe<int>.Some(10),
                Maybe<int>.None(),
                Maybe<int>.Some(42)
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
                Maybe<int>.Some(2),
                Maybe<int>.Some(4),
                Maybe<int>.None(),
                Maybe<int>.Some(6)
            };
            Func<int, bool> predicate = x => x > 10;

            // Act
            var result = maybeList.Where(predicate).ToList();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Where_WhenMaybeIsNone_ShouldReturnEmptySequence()
        {
            // Arrange
            var maybeList = new List<Maybe<int>>
            {
                Maybe<int>.None(),
                Maybe<int>.None(),
                Maybe<int>.None()
            };
            Func<int, bool> predicate = x => x > 0;

            // Act
            var result = maybeList.Where(predicate).ToList();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void ToEnumerable_WhenMaybeIsSome_ShouldReturnSequenceOfContent()
        {
            // Arrange
            var maybeList = new List<Maybe<int>>
            {
                Maybe<int>.Some(1),
                Maybe<int>.None(),
                Maybe<int>.Some(3)
            };

            // Act
            var result = maybeList.Flatten().ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Should().Be(1);
            result[1].Should().Be(3);
        }

        [Fact]
        public void ToEnumerable_WhenMaybeIsNone_ShouldReturnEmptySequence()
        {
            // Arrange
            var maybeList = new List<Maybe<int>>
            {
                Maybe<int>.None(),
                Maybe<int>.None(),
                Maybe<int>.None()
            };

            // Act
            var result = maybeList.Flatten().ToList();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void ToEnumerable_WhenMaybeIsSomeAndSubstituteIsProvided_ShouldReturnSequenceWithSubstituteForNone()
        {
            // Arrange
            var maybeList = new List<Maybe<int>>
            {
                Maybe<int>.Some(1),
                Maybe<int>.None(),
                Maybe<int>.Some(3)
            };
            var substitute = 42;

            // Act
            var result = maybeList.Flatten(substitute).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Should().Be(1);
            result[1].Should().Be(substitute);
            result[2].Should().Be(3);
        }

        [Fact]
        public void ToEnumerable_WhenMaybeIsSomeAndSubstituteFunctionIsProvided_ShouldReturnSequenceWithSubstituteForNone()
        {
            // Arrange
            var maybeList = new List<Maybe<int>>
            {
                Maybe<int>.Some(1),
                Maybe<int>.None(),
                Maybe<int>.Some(3)
            };
            Func<int> substitute = () => 42;

            // Act
            var result = maybeList.Flatten(substitute).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Should().Be(1);
            result[1].Should().Be(42);
            result[2].Should().Be(3);
        }
}