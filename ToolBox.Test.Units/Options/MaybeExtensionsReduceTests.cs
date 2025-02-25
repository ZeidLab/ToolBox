namespace ZeidLab.ToolBox.Test.Units.Options;

using FluentAssertions;
using ZeidLab.ToolBox.Options;
using Xunit;

public sealed class MaybeExtensionsReduceTests
{
    #region Reduce<T>(this Maybe<T> self, T substitute)

    [Fact]
    public void Reduce_SubstituteValue_Some_ReturnsValue()
    {
        // Arrange
        var maybe = Maybe.Some(5); 
        var substitute = 0;

        // Act
        var result = maybe.Reduce(substitute);

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void Reduce_SubstituteValue_None_ReturnsSubstitute()
    {
        // Arrange
        var maybe = Maybe.None<int>(); 
        var substitute = 0;

        // Act
        var result = maybe.Reduce(substitute);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region Reduce<T>(this Maybe<T> self, Func<T> substitute)

    [Fact]
    public void Reduce_SubstituteFunc_Some_ReturnsValue()
    {
        // Arrange
        var maybe = Maybe.Some("Hello"); 
        Func<string> substituteFunc = () => "Default";

        // Act
        var result = maybe.Reduce(substituteFunc);

        // Assert
        result.Should().Be("Hello");
    }

    [Fact]
    public void Reduce_SubstituteFunc_None_ReturnsSubstitute()
    {
        // Arrange
        var maybe = Maybe.None<string>(); 
        Func<string> substituteFunc = () => "Default";

        // Act
        var result = maybe.Reduce(substituteFunc);

        // Assert
        result.Should().Be("Default");
    }

    #endregion

    #region ReduceAsync<T>(this Maybe<T> self, Func<Task<T>> substitute)

    [Fact]
    public async Task ReduceAsync_SubstituteTask_Some_ReturnsValue()
    {
        // Arrange
        var maybe = Maybe.Some(10); 
        Func<Task<int>> substituteTask = () => Task.FromResult(0);

        // Act
        var result = await maybe.ReduceAsync(substituteTask);

        // Assert
        result.Should().Be(10);
    }

    [Fact]
    public async Task ReduceAsync_SubstituteTask_None_ReturnsSubstitute()
    {
        // Arrange
        var maybe = Maybe.None<int>(); 
        Func<Task<int>> substituteTask = () => Task.FromResult(0);

        // Act
        var result = await maybe.ReduceAsync(substituteTask);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region ReduceAsync<T>(this Task<Maybe<T>> self, T substitute)

    [Fact]
    public async Task ReduceAsync_SubstituteValue_TaskMaybe_Some_ReturnsValue()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.Some(20)); 
        var substitute = 0;

        // Act
        var result = await maybeTask.ReduceAsync(substitute);

        // Assert
        result.Should().Be(20);
    }

    [Fact]
    public async Task ReduceAsync_SubstituteValue_TaskMaybe_None_ReturnsSubstitute()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.None<int>()); 
        var substitute = 0;

        // Act
        var result = await maybeTask.ReduceAsync(substitute);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region ReduceAsync<T>(this Task<Maybe<T>> self, Func<T> substitute)

    [Fact]
    public async Task ReduceAsync_SubstituteFunc_TaskMaybe_Some_ReturnsValue()
    {
        // Arrange
        Task<Maybe<string>> maybeTask = Task.FromResult(Maybe.Some("Async")); 
        Func<string> substituteFunc = () => "Default";

        // Act
        var result = await maybeTask.ReduceAsync(substituteFunc);

        // Assert
        result.Should().Be("Async");
    }

    [Fact]
    public async Task ReduceAsync_SubstituteFunc_TaskMaybe_None_ReturnsSubstitute()
    {
        // Arrange
        Task<Maybe<string>> maybeTask = Task.FromResult(Maybe.None<string>()); 
        Func<string> substituteFunc = () => "Default";

        // Act
        var result = await maybeTask.ReduceAsync(substituteFunc);

        // Assert
        result.Should().Be("Default");
    }

    #endregion

    #region ReduceAsync<T>(this Task<Maybe<T>> self, Func<Task<T>> substitute)

    [Fact]
    public async Task ReduceAsync_SubstituteTask_TaskMaybe_Some_ReturnsValue()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.Some(30)); 
        Func<Task<int>> substituteTask = () => Task.FromResult(0);

        // Act
        var result = await maybeTask.ReduceAsync(substituteTask);

        // Assert
        result.Should().Be(30);
    }

    [Fact]
    public async Task ReduceAsync_SubstituteTask_TaskMaybe_None_ReturnsSubstitute()
    {
        // Arrange
        Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.None<int>()); 
        Func<Task<int>> substituteTask = () => Task.FromResult(0);

        // Act
        var result = await maybeTask.ReduceAsync(substituteTask);

        // Assert
        result.Should().Be(0);
    }

    #endregion
}