using FluentAssertions;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Test.Units.Options;

public sealed class MaybeExtensionsMatchTests
{
    #region Match<TIn, TOut>(this Maybe<TIn> self, Func<TIn, TOut> some, Func<TOut> none)

    [Fact]
    public void Match_SomeValue_ReturnsSomeResult()
    {
        // Arrange
        var maybe = Maybe.Some(10);

        // Act
        var result = maybe.Match(
            some: value => value * 2,
            none: () => 0);

        // Assert
        result.Should().Be(20);
    }

    [Fact]
    public void Match_NoneValue_ReturnsNoneResult()
    {
        // Arrange
        var maybe = Maybe.None<int>();

        // Act
        var result = maybe.Match(
            some: value => value * 2,
            none: () => 0);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region MatchAsync<TIn, TOut>(this Maybe<TIn> self, Func<TIn, Task<TOut>> some, Func<Task<TOut>> none)

    [Fact]
    public async Task MatchAsync_SomeValue_ReturnsSomeResult()
    {
        // Arrange
        var maybe = Maybe.Some(10);

        // Act
        var result = await maybe.MatchAsync(
            some: value => Task.FromResult(value * 2),
            none: () => Task.FromResult(0));

        // Assert
        result.Should().Be(20);
    }

    [Fact]
    public async Task MatchAsync_NoneValue_ReturnsNoneResult()
    {
        // Arrange
        var maybe = Maybe.None<int>();

        // Act
        var result = await maybe.MatchAsync(
            some: value => Task.FromResult(value * 2),
            none: () => Task.FromResult(0));

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region Match<TIn>(this Maybe<TIn> self, Action<TIn> some, Action none)

    [Fact]
    public void Match_SomeValue_ExecutesSomeAction()
    {
        // Arrange
        var maybe = Maybe.Some(10);
        int result = 0;

        // Act
        maybe.Match(
            some: value => { result = value * 2; },
            none: () => { result = 0; });

        // Assert
        result.Should().Be(20);
    }

    [Fact]
    public void Match_NoneValue_ExecutesNoneAction()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        int result = 0;

        // Act
        maybe.Match(
            some: value => { result = value * 2; },
            none: () => { result = 0; });

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region MatchAsync<TIn>(this Maybe<TIn> self, Func<TIn, Task> some, Func<Task> none)

    [Fact]
    public async Task MatchAsync_SomeValue_ExecutesSomeAction()
    {
        // Arrange
        var maybe = Maybe.Some(10);
        int result = 0;

        // Act
        await maybe.MatchAsync(
            some: value =>
            {
                result = value * 2;
                return Task.CompletedTask;
            },
            none: () =>
            {
                result = 0;
                return Task.CompletedTask;
            });

        // Assert
        result.Should().Be(20);
    }

    [Fact]
    public async Task MatchAsync_NoneValue_ExecutesNoneAction()
    {
        // Arrange
        var maybe = Maybe.None<int>();
        int result = 0;

        // Act
        await maybe.MatchAsync(
            some: value =>
            {
                result = value * 2;
                return Task.CompletedTask;
            },
            none: () =>
            {
                result = 0;
                return Task.CompletedTask;
            });

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region MatchAsync<TIn, TOut>(this Task<Maybe<TIn>> self, Func<TIn, TOut> some, Func<TOut> none)

    [Fact]
    public async Task MatchAsync_TaskSomeValue_ReturnsSomeResult()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.Some(10));

        // Act
        var result = await maybeTask.MatchAsync(
            some: value => value * 2,
            none: () => 0);

        // Assert
        result.Should().Be(20);
    }

    [Fact]
    public async Task MatchAsync_TaskNoneValue_ReturnsNoneResult()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.None<int>());

        // Act
        var result = await maybeTask.MatchAsync(
            some: value => value * 2,
            none: () => 0);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region MatchAsync<TIn, TOut>(this Task<Maybe<TIn>> self, Func<TIn, Task<TOut>> some, Func<Task<TOut>> none)

    [Fact]
    public async Task MatchAsync_TaskSomeValue_ReturnsAsyncSomeResult()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.Some(10));

        // Act
        var result = await maybeTask.MatchAsync(
            some: value => Task.FromResult(value * 2),
            none: () => Task.FromResult(0));

        // Assert
        result.Should().Be(20);
    }

    [Fact]
    public async Task MatchAsync_TaskNoneValue_ReturnsAsyncNoneResult()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.None<int>());

        // Act
        var result = await maybeTask.MatchAsync(
            some: value => Task.FromResult(value * 2),
            none: () => Task.FromResult(0));

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region MatchAsync<TIn>(this Task<Maybe<TIn>> self, Func<TIn, Task> some, Func<Task> none)

    [Fact]
    public async Task MatchAsync_TaskSomeValue_ExecutesSomeAction()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.Some(10));
        int result = 0;

        // Act
        await maybeTask.MatchAsync(
            some: value =>
            {
                result = value * 2;
                return Task.CompletedTask;
            },
            none: () =>
            {
                result = 0;
                return Task.CompletedTask;
            });

        // Assert
        result.Should().Be(20);
    }

    [Fact]
    public async Task MatchAsync_TaskNoneValue_ExecutesNoneAction()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.None<int>());
        int result = 0;

        // Act
        await maybeTask.MatchAsync(
            some: value =>
            {
                result = value * 2;
                return Task.CompletedTask;
            },
            none: () =>
            {
                result = 0;
                return Task.CompletedTask;
            });

        // Assert
        result.Should().Be(0);
    }

    #endregion
}