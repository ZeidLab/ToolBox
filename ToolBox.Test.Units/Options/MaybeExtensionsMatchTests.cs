using FluentAssertions;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Test.Units.Options;

public sealed class MaybeExtensionsMatchTests
{
	[Fact]
	public void Match_WithSomeValue_ShouldExecuteSomeAction()
	{
		// Arrange
		var initial = Maybe.Some(42);
		var result = 0;

		// Act
		initial.Match(
			some: x => { result = x; },
			none: () => { result = -1; }
		);

		// Assert
		result.Should().Be(42);
	}

	[Fact]
	public void Match_WithNone_ShouldExecuteNoneAction()
	{
		// Arrange
		var initial = Maybe.None<int>();
		var result = 0;

		// Act
		initial.Match(
			some: x => { result = x; },
			none: () => { result = -1; }
		);

		// Assert
		result.Should().Be(-1);
	}

	[Fact]
	public void Match_WithSomeValue_ShouldReturnSomeResult()
	{
		// Arrange
		var initial = Maybe.Some(42);

		// Act
		var result = initial.Match(
			some: x => x.ToString(),
			none: () => "none"
		);

		// Assert
		result.Should().Be("42");
	}

	[Fact]
	public void Match_WithNone_ShouldReturnNoneResult()
	{
		// Arrange
		var initial = Maybe.None<int>();

		// Act
		var result = initial.Match(
			some: x => x.ToString(),
			none: () => "none"
		);

		// Assert
		result.Should().Be("none");
	}

	[Fact]
	public async Task MatchAsync_WithSomeValue_ShouldExecuteSomeActionAsynchronously()
	{
		// Arrange
		var initial = Maybe.Some(42);
		var result = 0;

		// Act
		await initial.MatchAsync(
			some: async x =>
			{
				await Task.Delay(1); // Simulate async work
				result = x;
			},
			none: async () =>
			{
				await Task.Delay(1); // Simulate async work
				result = -1;
			}
		);

		// Assert
		result.Should().Be(42);
	}

	[Fact]
	public async Task MatchAsync_WithNone_ShouldExecuteNoneActionAsynchronously()
	{
		// Arrange
		var initial = Maybe.None<int>();
		var result = 0;

		// Act
		await initial.MatchAsync(
			some: async x =>
			{
				await Task.Delay(1); // Simulate async work
				result = x;
			},
			none: async () =>
			{
				await Task.Delay(1); // Simulate async work
				result = -1;
			}
		);

		// Assert
		result.Should().Be(-1);
	}

	[Fact]
	public async Task MatchAsync_WithSomeValue_ShouldReturnSomeResultAsynchronously()
	{
		// Arrange
		var initial = Maybe.Some(42);

		// Act
		var result = await initial.MatchAsync(
			some: async x =>
			{
				await Task.Delay(1); // Simulate async work
				return x.ToString();
			},
			none: async () =>
			{
				await Task.Delay(1); // Simulate async work
				return "none";
			}
		);

		// Assert
		result.Should().Be("42");
	}

	[Fact]
	public async Task MatchAsync_WithNone_ShouldReturnNoneResultAsynchronously()
	{
		// Arrange
		var initial = Maybe.None<int>();

		// Act
		var result = await initial.MatchAsync(
			some: async x =>
			{
				await Task.Delay(1); // Simulate async work
				return x.ToString();
			},
			none: async () =>
			{
				await Task.Delay(1); // Simulate async work
				return "none";
			}
		);

		// Assert
		result.Should().Be("none");
	}

	[Fact]
	public void Match_WithSomeValue_ShouldExecuteAction()
	{
		// Arrange
		var initial = Maybe.Some(42);
		var result = 0;

		// Act
		initial.Match(
			some: x => result = x,
			none: () => { }
		);

		// Assert
		result.Should().Be(42);
	}

	[Fact]
	public void Match_WithNone_ShouldExecuteNoneActionWithFlag()
	{
		// Arrange
		var initial = Maybe.None<int>();
		var actionExecuted = false;

		// Act
		initial.Match(
			some: _ => { },
			none: () => { actionExecuted = true;}
		);

		// Assert
		actionExecuted.Should().BeTrue();
	}

	[Fact]
	public async Task MatchAsync_WithTask_SomeValue_ShouldExecuteAsyncAction()
	{
		// Arrange
		var initial = Task.FromResult(Maybe.Some(42));
		var result = 0;

		// Act
		await initial.MatchAsync(
			some: async (int x) =>
			{
				await Task.Delay(10);
				result = x;
			},
			none: async () => { }
		);

		// Assert
		result.Should().Be(42);
	}

	[Fact]
	public async Task MatchAsync_WithTask_None_ShouldExecuteAsyncNoneAction()
	{
		// Arrange
		var initial = Task.FromResult(Maybe.None<int>());
		var actionExecuted = false;

		// Act
		await initial.MatchAsync(
			some: async _ => { },
			none: async () =>
			{
				await Task.Delay(10);
				actionExecuted = true;
			}
		);

		// Assert
		actionExecuted.Should().BeTrue();
	}

	[Fact]
	public async Task MatchAsync_WithTaskValue_SomeValue_ShouldReturnAsyncResult()
	{
		// Arrange
		var initial = Task.FromResult(Maybe.Some(42));

		// Act
		var result = await initial.MatchAsync(
			some: x => Task.FromResult(x.ToString()),
			none: () => Task.FromResult("none")
		);

		// Assert
		result.Should().Be("42");
	}

	[Fact]
	public async Task MatchAsync_WithTaskValue_None_ShouldReturnAsyncNoneResult()
	{
		// Arrange
		var initial = Task.FromResult(Maybe.None<int>());

		// Act
		var result = await initial.MatchAsync(
			some: async x => x.ToString(),
			none: async () => "none"
		);

		// Assert
		result.Should().Be("none");
	}
}