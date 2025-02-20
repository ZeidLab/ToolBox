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

	/// <summary>
	/// Chains a transformation function onto a successful <see cref="Result{TIn}"/> instance,
	/// yielding a new <see cref="Result{TOut}"/>. If the input result is a failure, the error is propagated
	/// without executing the transformation function.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the value contained in the input <see cref="Result{TIn}"/>.
	/// </typeparam>
	/// <typeparam name="TOut">
	/// The type of the value to be produced and wrapped in the resulting <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Result{TIn}"/> representing the outcome of a previous operation. In case of failure,
	/// the error is carried forward and <paramref name="func"/> is not executed.
	/// </param>
	/// <param name="func">
	/// A function that takes a value of type <typeparamref name="TIn"/> and returns a <see cref="Result{TOut}"/>
	/// representing the result of the transformation.
	/// </param>
	/// <returns>
	/// Returns a <see cref="Result{TOut}"/>, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> self, Func<TIn, Result<TOut>> func)
		=> self.IsSuccess
			? func(self.Value)
			: Result.Failure<TOut>(self.Error);

	/// <summary>
	/// Chains a transformation function onto a successful <see cref="Result{TIn}"/>,
	/// where the transformation function returns a <see cref="Try{TOut}"/>. This method converts
	/// the <see cref="Try{TOut}"/> to a <see cref="Result{TOut}"/> via the <c>Try()</c> method. If the input result
	/// is a failure, the error is propagated without invoking the transformation function.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the value contained in the input <see cref="Result{TIn}"/>.
	/// </typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation and wrapped in the resulting <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Result{TIn}"/> representing the outcome of a previous operation. If this result is a failure,
	/// its error is directly returned.
	/// </param>
	/// <param name="func">
	/// A function that accepts a value of type <typeparamref name="TIn"/> and returns a <see cref="Try{TOut}"/>
	/// representing an attempt to compute a new value of type <typeparamref name="TOut"/>.
	/// </param>
	/// <returns>
	/// Returns a <see cref="Result{TOut}"/>, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> self, Func<TIn, Try<TOut>> func)
		=> self.IsSuccess
			? func(self.Value).Try()
			: Result.Failure<TOut>(self.Error);

	/// <summary>
	/// Asynchronously chains a transformation function onto a successful <see cref="Result{TIn}"/> instance.
	/// If the input result is successful, the provided function is invoked to compute a new value; otherwise,
	/// the existing error is propagated. The asynchronous operation returns a task that, when completed,
	/// produces a <see cref="Result{TOut}"/>.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the value contained in the input <see cref="Result{TIn}"/>.
	/// </typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation, wrapped in a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Result{TIn}"/> representing the outcome of a previous operation. If this result is a failure,
	/// its error is carried forward.
	/// </param>
	/// <param name="func">
	/// An asynchronous function that takes a value of type <typeparamref name="TIn"/> and returns a
	/// <see cref="Task"/> which produces a <see cref="Result{TOut}"/> upon completion.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, Task<Result<TOut>>> func)
		=> self.IsSuccess
			? func(self.Value)
			: Result.Failure<TOut>(self.Error).AsTaskAsync();

	/// <summary>
	/// Asynchronously chains a transformation function onto a successful <see cref="Result{TIn}"/> instance,
	/// where the function returns a <see cref="TryAsync{TOut}"/>. If the input result is successful,
	/// the transformation is executed and its asynchronous outcome is converted into a <see cref="Result{TOut}"/>;
	/// otherwise, the error from the input result is propagated. The operation returns a task that produces the result.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the value contained in the input <see cref="Result{TIn}"/>.
	/// </typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation, wrapped in a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Result{TIn}"/> representing the outcome of a previous operation. If this result is a failure,
	/// its error is propagated.
	/// </param>
	/// <param name="func">
	/// An asynchronous function that takes a value of type <typeparamref name="TIn"/> and returns a
	/// <see cref="TryAsync{TOut}"/> representing an attempt to compute a new value asynchronously.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, TryAsync<TOut>> func)
		=> self.IsSuccess
			? func(self.Value).TryAsync()
			: Result.Failure<TOut>(self.Error).AsTaskAsync();

	#endregion

	#region Try<TIn>

	/// <summary>
	/// Converts a <see cref="Try{TIn}"/> into a <see cref="Result{TIn}"/> and then synchronously chains a transformation
	/// function that returns a <see cref="Result{TOut}"/>. If the try operation succeeds, the function is invoked;
	/// otherwise, the error is propagated.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the try operation.</typeparam>
	/// <typeparam name="TOut">The type of the value produced by the transformation function, wrapped in a <see cref="Result{TOut}"/>.</typeparam>
	/// <param name="self">A <see cref="Try{TIn}"/> representing an attempted operation.</param>
	/// <param name="func">A function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="Result{TOut}"/>.</param>
	/// <returns>
	/// Returns a <see cref="Result{TOut}"/>, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Result<TOut>> func)
		=> self.Try().Bind(func);


	/// <summary>
	/// Converts a <see cref="Try{TIn}"/> into a <see cref="Result{TIn}"/> and then synchronously chains a transformation
	/// function that returns a <see cref="Try{TOut}"/>. If the try operation succeeds, the function is applied;
	/// otherwise, the error is propagated.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the try operation.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation function, which is converted into a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">A <see cref="Try{TIn}"/> representing an attempted operation.</param>
	/// <param name="func">A function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="Try{TOut}"/>.</param>
	/// <returns>
	/// Returns a <see cref="Result{TOut}"/>, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Try<TOut>> func)
		=> self.Try().Bind(func);


	/// <summary>
	/// Converts a <see cref="Try{TIn}"/> into a <see cref="Result{TIn}"/> and then asynchronously chains a transformation
	/// function that returns a <see cref="Task"/> yielding a <see cref="Result{TOut}"/>. If the try operation succeeds,
	/// the function is invoked; otherwise, the error is propagated.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the try operation.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the asynchronous transformation function, wrapped in a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">A <see cref="Try{TIn}"/> representing an attempted operation.</param>
	/// <param name="func">
	/// An asynchronous function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="Result{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, Task<Result<TOut>>> func)
		=> self.Try().BindAsync(func);

	/// <summary>
	/// Converts a <see cref="Try{TIn}"/> into a <see cref="Result{TIn}"/> and then asynchronously chains a transformation
	/// function that returns a <see cref="TryAsync{TOut}"/>. If the try operation succeeds, the function is applied;
	/// otherwise, the error is propagated.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the try operation.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation function, which is later converted into a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">A <see cref="Try{TIn}"/> representing an attempted operation.</param>
	/// <param name="func">
	/// An asynchronous function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="TryAsync{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, TryAsync<TOut>> func)
		=> self.Try().BindAsync(func);

	#endregion

	#region Task<Result<TIn>>

	/// <summary>
	/// Awaits a <see cref="Task"/> that produces a <see cref="Result{TIn}"/> and then synchronously applies a transformation
	/// function that returns a <see cref="Result{TOut}"/> if the result is successful; otherwise, propagates the error.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the awaited result.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation function, wrapped in a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Task"/> that yields a <see cref="Result{TIn}"/> upon completion.
	/// </param>
	/// <param name="func">
	/// A function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="Result{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
		this Task<Result<TIn>> self,
		Func<TIn, Result<TOut>> func)
		=> (await self.ConfigureAwait(false)).Bind(func);

	/// <summary>
	/// Awaits a <see cref="Task"/> that produces a <see cref="Result{TIn}"/> and then synchronously applies a transformation
	/// function that returns a <see cref="Try{TOut}"/>. The resulting try is converted to a <see cref="Result{TOut}"/>;
	/// if the awaited result is a failure, its error is propagated.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the awaited result.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation function, converted into a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Task"/> that yields a <see cref="Result{TIn}"/> upon completion.
	/// </param>
	/// <param name="func">
	/// A function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="Try{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
		this Task<Result<TIn>> self,
		Func<TIn, Try<TOut>> func)
		=> (await self.ConfigureAwait(false)).Bind(func);


	/// <summary>
	/// Awaits a <see cref="Task"/> that yields a <see cref="Result{TIn}"/> and, if successful, invokes an asynchronous transformation
	/// function that returns a <see cref="Task"/> producing a <see cref="Result{TOut}"/>. If the initial result is a failure, its error is propagated.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the awaited result.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the asynchronous transformation function, wrapped in a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Task"/> that yields a <see cref="Result{TIn}"/> when awaited.
	/// </param>
	/// <param name="func">
	/// An asynchronous function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="Result{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
		this Task<Result<TIn>> self,
		Func<TIn, Task<Result<TOut>>> func)
	{
		var result = await self.ConfigureAwait(false);
		return result.IsSuccess
			? await func(result.Value).ConfigureAwait(false)
			: Result.Failure<TOut>(result.Error);
	}

	/// <summary>
	/// Awaits a <see cref="Task"/> that yields a <see cref="Result{TIn}"/> and, if successful, invokes an asynchronous transformation
	/// function that returns a <see cref="TryAsync{TOut}"/>. The resulting asynchronous try is converted to a <see cref="Result{TOut}"/>.
	/// If the awaited result is a failure, its error is propagated.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the awaited result.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation function, wrapped in a <see cref="Result{TOut}"/> after conversion.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Task"/> that, when awaited, returns a <see cref="Result{TIn}"/>.
	/// </param>
	/// <param name="func">
	/// An asynchronous function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="TryAsync{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
		this Task<Result<TIn>> self,
		Func<TIn, TryAsync<TOut>> func)
	{
		var result = await self.ConfigureAwait(false);
		return result.IsSuccess
			? await func(result.Value).TryAsync().ConfigureAwait(false)
			: Result.Failure<TOut>(result.Error);
	}

	#endregion

	#region TryAsync<TIn>

	/// <summary>
	/// Awaits a <see cref="TryAsync{TIn}"/> to convert it into a <see cref="Result{TIn}"/>, and then synchronously applies a transformation
	/// function that returns a <see cref="Result{TOut}"/> if successful; otherwise, propagates the error.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the asynchronous try operation.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation function, wrapped in a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">A <see cref="TryAsync{TIn}"/> representing an asynchronous attempted operation.</param>
	/// <param name="func">
	/// A function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="Result{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
		this TryAsync<TIn> self,
		Func<TIn, Result<TOut>> func)
		=> (await self.TryAsync().ConfigureAwait(false)).Bind(func);

	/// <summary>
	/// Awaits a <see cref="TryAsync{TIn}"/> to convert it into a <see cref="Result{TIn}"/>, and then asynchronously chains a transformation
	/// function that returns a <see cref="Try{TOut}"/>. The resulting try is converted to a <see cref="Result{TOut}"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the asynchronous try operation.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation function, which is converted into a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">A <see cref="TryAsync{TIn}"/> representing an asynchronous attempted operation.</param>
	/// <param name="func">
	/// A function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="Try{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(
		this TryAsync<TIn> self,
		Func<TIn, Try<TOut>> func)
		=> self.TryAsync().BindAsync(input => func(input).Try());

	/// <summary>
	/// Awaits a <see cref="TryAsync{TIn}"/> to convert it into a <see cref="Result{TIn}"/>, and then asynchronously chains a transformation
	/// function that returns a <see cref="Task"/> yielding a <see cref="Result{TOut}"/>. If the try operation fails, its error is propagated.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the asynchronous try operation.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the asynchronous transformation function, wrapped in a <see cref="Result{TOut}"/>.
	/// </typeparam>
	/// <param name="self">A <see cref="TryAsync{TIn}"/> representing an asynchronous attempted operation.</param>
	/// <param name="func">
	/// An asynchronous function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="Result{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(
		this TryAsync<TIn> self,
		Func<TIn, Task<Result<TOut>>> func)
		=> self.TryAsync().BindAsync(func);

	/// <summary>
	/// Awaits a <see cref="TryAsync{TIn}"/> to convert it into a <see cref="Result{TIn}"/>, and then asynchronously chains a transformation
	/// function that returns a <see cref="TryAsync{TOut}"/>. The resulting asynchronous try is converted into a <see cref="Result{TOut}"/>.
	/// Any errors encountered are propagated.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the asynchronous try operation.</typeparam>
	/// <typeparam name="TOut">
	/// The type of the value produced by the transformation function, wrapped in a <see cref="Result{TOut}"/> after conversion.
	/// </typeparam>
	/// <param name="self">A <see cref="TryAsync{TIn}"/> representing an asynchronous attempted operation.</param>
	/// <param name="func">
	/// A function that transforms a value of type <typeparamref name="TIn"/> into a <see cref="TryAsync{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that returns a <see cref="Result{TOut}"/> when awaited, representing the final outcome.
	/// </returns>
	/// <include file='../../DocFragments.xml' path='/doc/BindExtensionMethods/*'/>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> BindAsync<TIn, TOut>(
		this TryAsync<TIn> self,
		Func<TIn, TryAsync<TOut>> func)
		=> self.TryAsync().BindAsync(input => func(input).TryAsync());

	#endregion
}