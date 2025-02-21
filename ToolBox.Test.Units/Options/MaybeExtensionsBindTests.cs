using FluentAssertions;
using ZeidLab.ToolBox.Options;
using Xunit;

namespace ZeidLab.ToolBox.Test.Units.Options;

public sealed class MaybeExtensionsBindTests
{
    #region Bind

    [Fact]
    public void Bind_SomeValue_TransformsValue()
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
    public void Bind_None_ReturnsNone()
    {
        // Arrange
        var initial = Maybe.None<int>();

        // Act
        var result = initial.Bind(x => Maybe.Some(x.ToString()));

        // Assert
        result.IsNone.Should().BeTrue();
    }

    #endregion

    #region BindAsync_ValueMaybe_Func

    [Fact]
    public async Task BindAsync_ValueMaybeFunc_SomeValue_TransformsValueAsynchronously()
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
    public async Task BindAsync_ValueMaybeFunc_None_ReturnsNoneAsynchronously()
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

    #endregion

    #region BindAsync_TaskMaybe_Func

    [Fact]
    public async Task BindAsync_TaskMaybeFunc_SomeValue_TransformsValue()
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
    public async Task BindAsync_TaskMaybeFunc_None_ReturnsNone()
    {
        // Arrange
        var initial = Task.FromResult(Maybe.None<int>());

        // Act
        var result = await initial.BindAsync(x => Maybe.Some(x.ToString()));

        // Assert
        result.IsNone.Should().BeTrue();
    }

    #endregion

    #region BindAsync_TaskMaybe_FuncAsync

    [Fact]
    public async Task BindAsync_TaskMaybeFuncAsync_SomeValue_TransformsValueWithAsyncTransformation()
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
    public async Task BindAsync_TaskMaybeFuncAsync_None_ReturnsNoneWithAsyncTransformation()
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

    #endregion
}