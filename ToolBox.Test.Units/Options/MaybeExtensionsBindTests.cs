using FluentAssertions;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Test.Units.Options;

public sealed class MaybeExtensionsBindTests
{
    [Fact]
    public void Bind_WithSomeValue_ShouldTransformValue()
    {
        // Arrange
        var initial = Maybe.Some(42);

        // Act
        var result = initial.Bind(x => Maybe.Some(x.ToString()));

        // Assert
        result.IsSome.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void Bind_WithNone_ShouldReturnNone()
    {
        // Arrange
        var initial = Maybe.None<int>();

        // Act
        var result = initial.Bind(x => Maybe.Some(x.ToString()));

        // Assert
        result.IsNone.Should().BeTrue();
    }

    [Fact]
    public async Task BindAsync_WithSomeValue_ShouldTransformValueAsynchronously()
    {
        // Arrange
        var initial = Maybe.Some(42);

        // Act
        var result = await initial.BindAsync(async x =>
        {
            await Task.Delay(1); // Simulate async work
            return Maybe.Some(x.ToString());
        });

        // Assert
        result.IsSome.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WithNone_ShouldReturnNoneAsynchronously()
    {
        // Arrange
        var initial = Maybe.None<int>();

        // Act
        var result = await initial.BindAsync(async x =>
        {
            await Task.Delay(1); // Simulate async work
            return Maybe.Some(x.ToString());
        });

        // Assert
        result.IsNone.Should().BeTrue();
    }

    [Fact]
    public async Task BindAsync_WithSomeValue_ShouldTransformValue()
    {
        // Arrange
        var initial = Task.FromResult(Maybe.Some(42));

        // Act
        var result = await initial.BindAsync(x => Maybe.Some(x.ToString()));

        // Assert
        result.IsSome.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WithNone_ShouldReturnNone()
    {
        // Arrange
        var initial = Task.FromResult(Maybe.None<int>());

        // Act
        var result = await initial.BindAsync(x => Maybe.Some(x.ToString()));

        // Assert
        result.IsNone.Should().BeTrue();
    }

    [Fact]
    public async Task BindAsync_WithSomeValue_ShouldTransformValueWithAsyncTransformation()
    {
        // Arrange
        var initial = Task.FromResult(Maybe.Some(42));

        // Act
        var result = await initial.BindAsync(async x =>
        {
            await Task.Delay(1); // Simulate async work
            return Maybe.Some(x.ToString());
        });

        // Assert
        result.IsSome.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_WithNone_ShouldReturnNoneWithAsyncTransformation()
    {
        // Arrange
        var initial = Task.FromResult(Maybe.None<int>());

        // Act
        var result = await initial.BindAsync(async x =>
        {
            await Task.Delay(1); // Simulate async work
            return Maybe.Some(x.ToString());
        });

        // Assert
        result.IsNone.Should().BeTrue();
    }
}