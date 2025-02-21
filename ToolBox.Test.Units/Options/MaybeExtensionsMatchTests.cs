using FluentAssertions;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Test.Units.Options;

public sealed class MaybeExtensionsMatchTests
{
	#region Match_Sync_ValueReturn

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

	#region MatchAsync_AsyncValueReturn

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

	#region Match_Sync_Action

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

	#region MatchAsync_AsyncAction

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

	#region MatchAsync_TaskMaybe_SyncValueReturn

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

	#region MatchAsync_TaskMaybe_AsyncValueReturn

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

	#region MatchAsync_TaskMaybe_AsyncAction

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