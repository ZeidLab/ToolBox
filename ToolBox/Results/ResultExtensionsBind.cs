using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for binding operations on <see cref="Result{TValue}"/> objects.
/// These methods enable monadic composition of Result types, allowing for clean error handling
/// and functional programming patterns.
/// </summary>
/// <remarks>
/// <para>
/// Binding operations allow you to chain multiple operations that return Results while
/// maintaining proper error propagation.
/// </para>
/// <example>
/// Configuration parsing example:
/// <code><![CDATA[
/// using ZeidLab.ToolBox.Results;
/// namespace ZeidLab.ToolBox.ExampleProject.ResultExamples
/// {
/// 	public static class BindExampleErrors
///	{
/// 		public static readonly ResultError NotPositive = ResultError.New("Not positive");
/// 		public static readonly ResultError NotEven = ResultError.New("Not even");
/// 	}
/// 	public static class BindExamples
/// 	{
/// 		// Validate that a number is positive.
/// 		private static Result<int> ValidatePositive(int x)
/// 			// implicit conversion from int to Result<int>
/// 			=> x > 0 ? x : BindExampleErrors.NotPositive;
///
/// 		// Validate that a number is even.
/// 		private static Result<int> ValidateEven(int x) =>
/// 			// implicit conversion from int to Result<int>
/// 			x % 2 == 0 ? x : BindExampleErrors.NotEven;
///
/// 		// Multiply by two asynchronously.
/// 		private static async Task<Result<int>> MultiplyByTwoAsync(int x)
/// 		{
/// 			await Task.Delay(10); // Simulate async work.
/// 			// implicit conversion from int to Result<int>
/// 			return x * 2;
/// 		}
///
/// 		// Try operation: Parse a string to an int.
/// 		// implicit conversion and exception handling
/// 		private static Try<int> ParseNumber(string input)
/// 			=> () => int.Parse(input);
///
/// 		// TryAsync operation: Fetch data asynchronously.
/// 		private static TryAsync<string> FetchDataAsync(string data)
/// 			=> async () =>
/// 			{
/// 				await Task.Delay(10); // Simulate async fetch.
/// 				return Result.Success(data);
/// 			};
///
/// 		public static async Task RunAsync()
/// 		{
/// 			await FetchDataAsync("258")
/// 				.BindAsync(ParseNumber)
/// 				.BindAsync(ValidatePositive)
/// 				.BindAsync(ValidateEven)
/// 				.BindAsync(MultiplyByTwoAsync)
///					// Result<int>.Value or Result<int>.Error is not accessible publicly
///					// to get the result value, you need to use the Match or MatchAsync method
/// 				.MatchAsync(
/// 					success: (int x) => Console.WriteLine($"Async Success: {x}"),
/// 					failure: (ResultError error) => Console.WriteLine($"Async Failure: {error.Message}")
/// 				);
///
/// 			ParseNumber("InvalidString")
/// 				.Bind(ValidatePositive)
/// 				.Bind(x => x % 2 == 0 ? Result.Success(x + 3) : BindExampleErrors.NotEven)
///					// Result<int>.Value or Result<int>.Error is not accessible publicly
///					// to get the result value, you need to use the Match or MatchAsync method
/// 				.Match(
/// 					success: (int x) => Console.WriteLine($"Async Success: {x}"),
/// 					failure: (ResultError error) => Console.WriteLine($"Async Failure: {error.Message}")
/// 				);
/// 		}
/// 	}
/// }
/// ]]></code>
/// </example>
/// </remarks>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsBind
{
	#region Result<TIn>

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> self, Func<TIn, Result<TOut>> func)
		=> self.IsSuccess
			? func(self.Value)
			: Result.Failure<TOut>(self.Error);

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> self, Func<TIn, Try<TOut>> func)
		=> self.IsSuccess
				? func(self.Value).Try()
				: Result.Failure<TOut>(self.Error);

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, Task<Result<TOut>>> func)
		=> self.IsSuccess
			? func(self.Value)
			: Result.Failure<TOut>(self.Error).AsTaskAsync();

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, TryAsync<TOut>> func)
		=> self.IsSuccess
			? func(self.Value).TryAsync()
			: Result.Failure<TOut>(self.Error).AsTaskAsync();



	#endregion

	#region Try<TIn>

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Result<TOut>> func)
		=> self.Try().Bind(func);

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Try<TOut>> func)
		=> self.Try().Bind(func);

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, Task<Result<TOut>>> func)
		=> self.Try().BindAsync(func);

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, TryAsync<TOut>> func)
		=> self.Try().BindAsync(func);



	#endregion

	#region Task<Result<TIn>>

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self,
		Func<TIn, Result<TOut>> func)
		=> (await self.ConfigureAwait(false)).Bind(func);

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self,
		Func<TIn, Try<TOut>> func)
		=> (await self.ConfigureAwait(false)).Bind(func);

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self,
		Func<TIn, Task<Result<TOut>>> func)
	{
		var result = await self.ConfigureAwait(false);
		return result.IsSuccess
			? await func(result.Value).ConfigureAwait(false)
			: Result.Failure<TOut>(result.Error);
	}

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self,
		Func<TIn, TryAsync<TOut>> func)
	{
		var result = await self.ConfigureAwait(false);
		return result.IsSuccess
			? await func(result.Value).TryAsync()
			: Result.Failure<TOut>(result.Error);
	}



	#endregion

	#region TryAsync<TIn>

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Result<TOut>> func)
		=> (await self.TryAsync().ConfigureAwait(false)).Bind(func);

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Try<TOut>> func)
		=> self.TryAsync().BindAsync(input => func(input).Try());


	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Task<Result<TOut>>> func)
		=> self.TryAsync().BindAsync(func);

	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, TryAsync<TOut>> func)
		=> self.TryAsync().BindAsync(input => func(input).TryAsync());

	#endregion
}