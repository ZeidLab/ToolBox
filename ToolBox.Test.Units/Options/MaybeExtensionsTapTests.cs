namespace ZeidLab.ToolBox.Test.Units.Options;
using ZeidLab.ToolBox.Options;


using FluentAssertions;
using Xunit;

public sealed class MaybeExtensionsTapTests
{
    #region TapIfSome<TIn>(this Maybe<TIn> self, Action<TIn> action)

    [Fact]
    public void TapIfSome_Some_ActionIsExecuted()
    {
        // Arrange
        var maybe = Maybe.Some(10);
        var actionExecuted = false;

        // Act
        var result = maybe.TapIfSome(value => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeTrue();
        result.Should().Be(maybe);
    }

    [Fact]
    public void TapIfSome_None_ActionIsNotExecuted()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        var actionExecuted = false;

        // Act
        var result = maybe.TapIfSome(value => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeFalse();
        result.Should().Be(maybe);
    }

    #endregion

    #region TapIfSomeAsync<TIn>(this Maybe<TIn> self, Func<TIn,Task> action)

    [Fact]
    public async Task TapIfSomeAsync_InstanceSome_ActionIsExecuted()
    {
        // Arrange
        var maybe = Maybe.Some(10);
        var actionExecuted = false;

        // Act
        var result = await maybe.TapIfSomeAsync(async value =>
        {
            await Task.Delay(1);
            actionExecuted = true;
        });

        // Assert
        actionExecuted.Should().BeTrue();
        result.Should().Be(maybe);
    }

    [Fact]
    public async Task TapIfSomeAsync_InstanceNone_ActionIsNotExecuted()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        var actionExecuted = false;

        // Act
        var result = await maybe.TapIfSomeAsync(async value =>
        {
            await Task.Delay(1);
            actionExecuted = true;
        });

        // Assert
        actionExecuted.Should().BeFalse();
        result.Should().Be(maybe);
    }

    #endregion

    #region TapIfNone<TIn>(this Maybe<TIn> self, Action action)

    [Fact]
    public void TapIfNone_None_ActionIsExecuted()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        var actionExecuted = false;

        // Act
        var result = maybe.TapIfNone(() => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeTrue();
        result.Should().Be(maybe);
    }

    [Fact]
    public void TapIfNone_Some_ActionIsNotExecuted()
    {
        // Arrange
        var maybe = Maybe.Some(10);
        var actionExecuted = false;

        // Act
        var result = maybe.TapIfNone(() => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeFalse();
        result.Should().Be(maybe);
    }

    #endregion

    #region TapIfNoneAsync<TIn>(this Maybe<TIn> self, Func<Task> action)

    [Fact]
    public async Task TapIfNoneAsync_InstanceNone_ActionIsExecuted()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        var actionExecuted = false;

        // Act
        var result = await maybe.TapIfNoneAsync(async () =>
        {
            await Task.Delay(1);
            actionExecuted = true;
        });

        // Assert
        actionExecuted.Should().BeTrue();
        result.Should().Be(maybe);
    }

    [Fact]
    public async Task TapIfNoneAsync_InstanceSome_ActionIsNotExecuted()
    {
        // Arrange
        var maybe = Maybe.Some(10);
        var actionExecuted = false;

        // Act
        var result = await maybe.TapIfNoneAsync(async () =>
        {
            await Task.Delay(1);
            actionExecuted = true;
        });

        // Assert
        actionExecuted.Should().BeFalse();
        result.Should().Be(maybe);
    }

    #endregion

    #region TapIfSomeAsync<TIn>(this Task<Maybe<TIn>> self, Func<TIn, Task> action)

    [Fact]
    public async Task TapIfSomeAsync_Some_ActionIsExecuted()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe.Some(10));
        var actionExecuted = false;

        // Act
        var result = await maybeTask.TapIfSomeAsync(async value => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeTrue();
        result.Should().Be(await maybeTask);
    }

    [Fact]
    public async Task TapIfSomeAsync_None_ActionIsNotExecuted()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe.None<int>());
        var actionExecuted = false;

        // Act
        var result = await maybeTask.TapIfSomeAsync(async value => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeFalse();
        result.Should().Be(await maybeTask);
    }

    #endregion

    #region TapIfNoneAsync<TIn>(this Task<Maybe<TIn>> self, Func<Task> action)

    [Fact]
    public async Task TapIfNoneAsync_None_ActionIsExecuted()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe.None<int>());
        var actionExecuted = false;

        // Act
        var result = await maybeTask.TapIfNoneAsync(async () => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeTrue();
        result.Should().Be(await maybeTask);
    }

    [Fact]
    public async Task TapIfNoneAsync_Some_ActionIsNotExecuted()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe.Some(10));
        var actionExecuted = false;

        // Act
        var result = await maybeTask.TapIfNoneAsync(async () => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeFalse();
        result.Should().Be(await maybeTask);
    }

    #endregion
}