using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for pattern matching operations on <see cref="Result{TValue}"/> and <see cref="Try{TValue}"/> types.
/// These methods enable railway-oriented programming by handling both success and failure cases explicitly.
/// </summary>
/// <remarks>
/// Pattern matching is a core concept in railway-oriented programming that ensures all possible outcomes
/// are handled appropriately. These methods force developers to consider both success and failure paths.
/// </remarks>
/// <example>
/// Basic result matching:
/// <code><![CDATA[
/// var result = Result.Success(42);
/// var matched = result.Match(
///     success: value => $"Got {value}",
///     failure: error => $"Error: {error.Message}"
/// ); // matched = "Got 42"
/// ]]></code>
///
/// Chaining with Try:
/// <code><![CDATA[
/// var tryResult = new Try<int>(() => int.Parse("xyz"));
/// tryResult.Match(
///     success: x => Console.WriteLine($"Parsed {x}"),
///     failure: err => Console.WriteLine($"Failed to parse, {err.Message}")
/// ); // Prints: "Failed to parse, Input string was not in a correct format."
/// ]]></code>
/// </example>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsMatch
{
	#region Result<TIn>

	/// <summary>
	/// Transforms the value contained within a <see cref="Result{TIn}"/> based on its outcome.
	/// If the result is successful, applies the <paramref name="success"/> function to the value.
	/// If the result is a failure, applies the <paramref name="failure"/> function to the error.
	/// </summary>
	/// <typeparam name="TIn">The type of the value in the source result.</typeparam>
	/// <typeparam name="TOut">The type of the value to return after transformation.</typeparam>
	/// <param name="self">The source <see cref="Result{TIn}"/> to match against.</param>
	/// <param name="success">
	/// A function to transform the value if the result is successful.
	/// This function takes a parameter of type <typeparamref name="TIn"/> and returns a value of type <typeparamref name="TOut"/>.
	/// </param>
	/// <param name="failure">
	/// A function to transform the error if the result is a failure.
	/// This function takes a parameter of type <see cref="ResultError"/> and returns a value of type <typeparamref name="TOut"/>.
	/// </param>
	/// <returns>
	/// A value of type <typeparamref name="TOut"/> resulting from applying the appropriate transformation function
	/// based on the outcome of the <paramref name="self"/> result.
	/// </returns>
	/// <example>
	/// <code><![CDATA[
	/// var result = Result.Success(42);
	/// var matched = result.Match(
	///     success: x => x * 2,
	///     failure: _ => 0
	/// ); // matched = 84
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TOut Match<TIn, TOut>(this Result<TIn> self, Func<TIn, TOut> success,
		Func<ResultError, TOut> failure)
		=> self.IsSuccess ? success(self.Value) : failure(self.Error);

	/// <summary>
	/// Asynchronously transforms the value contained within a <see cref="Result{TIn}"/> based on its outcome.
	/// If the result is successful, applies the <paramref name="success"/> function to the value.
	/// If the result is a failure, applies the <paramref name="failure"/> function to the error.
	/// </summary>
	/// <typeparam name="TIn">The type of the value in the source result.</typeparam>
	/// <typeparam name="TOut">The type of the value to return after transformation.</typeparam>
	/// <param name="self">The source <see cref="Result{TIn}"/> to match against.</param>
	/// <param name="success">
	/// A function to transform the value if the result is successful.
	/// This function takes a parameter of type <typeparamref name="TIn"/> and returns a <see cref="Task{TOut}"/>.
	/// </param>
	/// <param name="failure">
	/// A function to transform the error if the result is a failure.
	/// This function takes a parameter of type <see cref="ResultError"/> and returns a <see cref="Task{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{TOut}"/> representing the asynchronous operation.
	/// The task result is the transformed value obtained by applying the appropriate function
	/// based on the outcome of the <paramref name="self"/> result.
	/// </returns>
	/// <example>
	/// <code>
	/// var result = Result.Success(42);
	/// var matched = await result.MatchAsync(
	///     success: async x => await Task.FromResult(x * 2),
	///     failure: async _ => await Task.FromResult(0)
	/// ); // matched = 84
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<TOut> MatchAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, Task<TOut>> success,
		Func<ResultError, Task<TOut>> failure)
		=> self.IsSuccess ? success(self.Value) : failure(self.Error);

	/// <summary>
	/// Evaluates the outcome of a <see cref="Result{TIn}"/> and executes the corresponding action.
	/// If the result is successful, the <paramref name="success"/> action is invoked with the result's value.
	/// If the result is a failure, the <paramref name="failure"/> action is invoked with the result's error.
	/// </summary>
	/// <typeparam name="TIn">The type of the value in the source result.</typeparam>
	/// <param name="self">The source <see cref="Result{TIn}"/> to evaluate.</param>
	/// <param name="success">
	/// An action to execute if the result is successful.
	/// This action takes a parameter of type <typeparamref name="TIn"/> (the value in the source result).
	/// </param>
	/// <param name="failure">
	/// An action to execute if the result is a failure.
	/// This action takes a parameter of type <see cref="ResultError"/> (the error in the result).
	/// </param>
	/// <example>
	/// <code>
	/// var result = Result.Success(42);
	/// result.Match(
	///     success: x => Console.WriteLine($"Success: {x}"),
	///     failure: error => Console.WriteLine($"Failure: {error}")
	/// );
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Match<TIn>(this Result<TIn> self, Action<TIn> success,
		Action<ResultError> failure)
	{
		if (self.IsSuccess)
			success(self.Value);
		else
			failure(self.Error);
	}

	/// <summary>
	/// Asynchronously evaluates the outcome of a <see cref="Result{TIn}"/> and executes the corresponding asynchronous action.
	/// If the result is successful, the <paramref name="success"/> function is invoked with the result's value.
	/// If the result is a failure, the <paramref name="failure"/> function is invoked with the result's error.
	/// </summary>
	/// <typeparam name="TIn">The type of the value in the source result.</typeparam>
	/// <param name="self">The source <see cref="Result{TIn}"/> to evaluate.</param>
	/// <param name="success">
	/// An asynchronous function to execute if the result is successful.
	/// This function takes a parameter of type <typeparamref name="TIn"/> (the value in the source result)
	/// and returns a <see cref="Task"/> representing the asynchronous operation.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function to execute if the result is a failure.
	/// This function takes a parameter of type <see cref="ResultError"/> (the error in the result)
	/// and returns a <see cref="Task"/> representing the asynchronous operation.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> representing the asynchronous operation.
	/// The task completes when either the `success` or `failure` function has completed.
	/// </returns>
	/// <example>
	/// <code>
	/// var result = Result.Success(42);
	/// await result.MatchAsync(
	///     success: async x => { await Task.Delay(100); Console.WriteLine($"Success: {x}"); },
	///     failure: async error => { await Task.Delay(100); Console.WriteLine($"Failure: {error}"); }
	/// );
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task MatchAsync<TIn>(this Result<TIn> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
		=> self.IsSuccess ? success(self.Value) : failure(self.Error);

	#endregion

	#region Try<TIn>

	/// <summary>
	/// Transforms the outcome of a <see cref="Try{TIn}"/> delegate into a value of type <typeparamref name="TOut"/>.
	/// When the delegate is invoked, it returns a <see cref="Result{TIn}"/>. Depending on whether the result is successful,
	/// the <paramref name="success"/> function or the <paramref name="failure"/> function is applied.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the <see cref="Try{TIn}"/> delegate.</typeparam>
	/// <typeparam name="TOut">The type of the value returned after transformation.</typeparam>
	/// <param name="self">The <see cref="Try{TIn}"/> delegate to evaluate.</param>
	/// <param name="success">
	/// A function to transform the value if invoking <paramref name="self"/> returns a successful <see cref="Result{TIn}"/>.
	/// Accepts a parameter of type <typeparamref name="TIn"/> and returns a value of type <typeparamref name="TOut"/>.
	/// </param>
	/// <param name="failure">
	/// A function to transform the error if invoking <paramref name="self"/> returns a failed <see cref="Result{TIn}"/>.
	/// Accepts a parameter of type <see cref="ResultError"/> and returns a value of type <typeparamref name="TOut"/>.
	/// </param>
	/// <returns>
	/// A value of type <typeparamref name="TOut"/> produced by applying the appropriate transformation function.
	/// </returns>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Example: Parsing a string into an integer
	/// // If there is a invalid format exception it will be captured
	/// Try<int> ParseNumber(string input)
	///     => () => int.Parse(input);
	/// // Use the Try delegate and match its outcome:
	/// var output = ParseNumber("42").Match(
	///     success: x => x * 2,
	///     failure: error => 0
	/// ); output equals 84
	///
	/// var output = ParseNumber("InvalidString").Match(
	///     success: x => x * 2,
	///		// there is a invalid format exception
	///     failure: error => 0
	/// ); output equals 0
	/// ]]>
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TOut Match<TIn, TOut>(this Try<TIn> self, Func<TIn, TOut> success,
		Func<ResultError, TOut> failure)
		=> self.Try().Match(success: success, failure: failure);

	/// <summary>
	/// Asynchronously transforms the outcome of a <see cref="Try{TIn}"/> delegate into a value of type <typeparamref name="TOut"/>.
	/// When the delegate is invoked, it returns a <see cref="Result{TIn}"/>. Depending on the result,
	/// the corresponding asynchronous transformation function (<paramref name="success"/> or <paramref name="failure"/>)
	/// is applied.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the <see cref="Try{TIn}"/> delegate.</typeparam>
	/// <typeparam name="TOut">The type of the value returned after transformation.</typeparam>
	/// <param name="self">The <see cref="Try{TIn}"/> delegate to evaluate.</param>
	/// <param name="success">
	/// An asynchronous function to transform the value if invoking <paramref name="self"/> returns a successful result.
	/// Accepts a parameter of type <typeparamref name="TIn"/> and returns a <see cref="Task{TOut}"/>.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function to transform the error if invoking <paramref name="self"/> returns a failed result.
	/// Accepts a parameter of type <see cref="ResultError"/> and returns a <see cref="Task{TOut}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{TOut}"/> representing the asynchronous operation that produces a value of type <typeparamref name="TOut"/>.
	/// </returns>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Example: Parsing a string into an integer asynchronously
	/// // If there is an invalid format exception it will be captured
	/// Try<int> ParseNumber(string input)
	///     => () => int.Parse(input);
	/// // Use the Try delegate and match its outcome asynchronously:
	/// var output = await ParseNumber("42").MatchAsync(
	///     success: async x => await Task.FromResult(x * 2),
	///     failure: async error => await Task.FromResult(0)
	/// ); // output equals 84
	///
	/// var output = await ParseNumber("InvalidString").MatchAsync(
	///     success: async x => await Task.FromResult(x * 2),
	///     // there is an invalid format exception
	///     failure: async error => await Task.FromResult(0)
	/// ); // output equals 0
	/// ]]>
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<TOut> MatchAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, Task<TOut>> success,
		Func<ResultError, Task<TOut>> failure)
		=> self.Try().MatchAsync(success: success, failure: failure);


	/// <summary>
	/// Evaluates a <see cref="Try{TIn}"/> delegate and executes the corresponding action based on its outcome.
	/// When invoked, the delegate returns a <see cref="Result{TIn}"/>. If the result is successful,
	/// the <paramref name="success"/> action is executed with the resulting value; if it is a failure,
	/// the <paramref name="failure"/> action is executed with the error.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the <see cref="Try{TIn}"/> delegate.</typeparam>
	/// <param name="self">The <see cref="Try{TIn}"/> delegate to evaluate.</param>
	/// <param name="success">
	/// An action to execute if invoking <paramref name="self"/> returns a successful result.
	/// Accepts a parameter of type <typeparamref name="TIn"/> representing the value.
	/// </param>
	/// <param name="failure">
	/// An action to execute if invoking <paramref name="self"/> returns a failed result.
	/// Accepts a parameter of type <see cref="ResultError"/> representing the error.
	/// </param>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Example: Parsing a string into an integer and executing actions based on the result
	/// // If there is an invalid format exception it will be captured
	/// Try<int> ParseNumber(string input)
	///     => () => int.Parse(input);
	/// // Use the Try delegate and match its outcome:
	/// ParseNumber("42").Match(
	///     success: x => Console.WriteLine($"Parsed successfully: {x}"),
	///     failure: error => Console.WriteLine("Failed to parse number")
	/// );
	/// // When input is "42", prints: "Parsed successfully: 42"
	///
	/// ParseNumber("InvalidString").Match(
	///     success: x => Console.WriteLine($"Parsed successfully: {x}"),
	///     // there is an invalid format exception
	///     failure: error => Console.WriteLine("Failed to parse number")
	/// );
	/// // When input is "InvalidString", prints: "Failed to parse number"
	/// ]]>
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Match<TIn>(this Try<TIn> self, Action<TIn> success,
		Action<ResultError> failure)
		=> self.Try().Match(success: success, failure: failure);


	/// <summary>
	/// Asynchronously evaluates a <see cref="Try{TIn}"/> delegate and executes the corresponding asynchronous action
	/// based on its outcome. When invoked, the delegate returns a <see cref="Result{TIn}"/>. If the result is successful,
	/// the <paramref name="success"/> asynchronous function is executed with the value; if it is a failure,
	/// the <paramref name="failure"/> asynchronous function is executed with the error.
	/// </summary>
	/// <typeparam name="TIn">The type of the value produced by the <see cref="Try{TIn}"/> delegate.</typeparam>
	/// <param name="self">The <see cref="Try{TIn}"/> delegate to evaluate.</param>
	/// <param name="success">
	/// An asynchronous function to execute if invoking <paramref name="self"/> returns a successful result.
	/// Accepts a parameter of type <typeparamref name="TIn"/> and returns a <see cref="Task"/>.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function to execute if invoking <paramref name="self"/> returns a failed result.
	/// Accepts a parameter of type <see cref="ResultError"/> and returns a <see cref="Task"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> representing the asynchronous operation that completes when the appropriate action has executed.
	/// </returns>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Example: Asynchronously parsing a string into an integer and executing actions based on the result
	/// // If there is an invalid format exception it will be captured
	/// Try<int> ParseNumber(string input)
	///     => () => int.Parse(input);
	/// // Use the Try delegate and match its outcome asynchronously:
	/// await ParseNumber("42").ToTryAsync().MatchAsync(
	///     success: async x => { await Task.Delay(50); Console.WriteLine($"Parsed successfully: {x}"); },
	///     failure: async error => { await Task.Delay(50); Console.WriteLine("Failed to parse number"); }
	/// );
	/// // When input is "42", prints: "Parsed successfully: 42"
	///
	/// await ParseNumber("InvalidString").ToTryAsync().MatchAsync(
	///     success: async x => { await Task.Delay(50); Console.WriteLine($"Parsed successfully: {x}"); },
	///     // there is an invalid format exception
	///     failure: async error => { await Task.Delay(50); Console.WriteLine("Failed to parse number"); }
	/// );
	/// // When input is "InvalidString", prints: "Failed to parse number"
	/// ]]>
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task MatchAsync<TIn>(this Try<TIn> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
		=> self.Try().MatchAsync(success: success, failure: failure);

	#endregion

	#region Task<Result<TIn>>

	/// <summary>
	/// Asynchronously transforms a <c>Task</c> that produces a <see cref="Result{TIn}"/> into a value of type <typeparamref name="TOut"/>.
	/// Once the task completes, if the result is successful, applies the synchronous <paramref name="success"/> function;
	/// otherwise, applies the synchronous <paramref name="failure"/> function.
	/// </summary>
	/// <typeparam name="TIn">The type contained in the Result.</typeparam>
	/// <typeparam name="TOut">The type of the output value.</typeparam>
	/// <param name="self">A Task that produces a <see cref="Result{TIn}"/>.</param>
	/// <param name="success">A function to transform the value if the result is successful.</param>
	/// <param name="failure">A function to transform the error if the result is a failure.</param>
	/// <returns>A Task producing the transformed value of type <typeparamref name="TOut"/>.</returns>
	/// <example>
	/// <code>
	/// var output = await Task.FromResult(Result.Success(42))
	///     .MatchAsync(x => x * 2, error => 0); // output equals 84
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, TOut> success,
		Func<ResultError, TOut> failure)
	{
		var result = await self.ConfigureAwait(false);
		return result.IsSuccess ? success(result.Value) : failure(result.Error);
	}

	/// <summary>
	/// Asynchronously transforms a <c>Task</c> that produces a <see cref="Result{TIn}"/> into a value of type <typeparamref name="TOut"/>.
	/// Once the task completes, if the result is successful, awaits the asynchronous <paramref name="success"/> function;
	/// otherwise, awaits the asynchronous <paramref name="failure"/> function.
	/// </summary>
	/// <typeparam name="TIn">The type contained in the Result.</typeparam>
	/// <typeparam name="TOut">The type of the output value.</typeparam>
	/// <param name="self">A Task that produces a <see cref="Result{TIn}"/>.</param>
	/// <param name="success">An asynchronous function to transform the value if the result is successful.</param>
	/// <param name="failure">An asynchronous function to transform the error if the result is a failure.</param>
	/// <returns>A Task producing the transformed value of type <typeparamref name="TOut"/>.</returns>
	/// <example>
	/// <code>
	/// var output = await Task.FromResult(Result.Success(42))
	///     .MatchAsync(async x => await Task.FromResult(x * 2),
	///                 async error => await Task.FromResult(0)); // output equals 84
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Task<TOut>> success,
		Func<ResultError, Task<TOut>> failure)
	{
		var result = await self.ConfigureAwait(false);
		return result.IsSuccess
			? await success(result.Value).ConfigureAwait(false)
			: await failure(result.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously processes a <c>Task</c> that produces a <see cref="Result{TIn}"/> by executing an action based on its outcome.
	/// Once the task completes, if the result is successful, executes the <paramref name="success"/> action;
	/// otherwise, executes the <paramref name="failure"/> action.
	/// </summary>
	/// <typeparam name="TIn">The type contained in the Result.</typeparam>
	/// <param name="self">A Task that produces a <see cref="Result{TIn}"/>.</param>
	/// <param name="success">An action to execute if the result is successful.</param>
	/// <param name="failure">An action to execute if the result is a failure.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <example>
	/// <code>
	/// await Task.FromResult(Result.Success(42))
	///     .MatchAsync(x => Console.WriteLine($"Success: {x}"),
	///                 error => Console.WriteLine("Error")); // Prints "Success: 42"
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this Task<Result<TIn>> self, Action<TIn> success,
		Action<ResultError> failure)
	{
		var result = await self.ConfigureAwait(false);
		if (result.IsSuccess)
			success(result.Value);
		else
			failure(result.Error);
	}

	/// <summary>
	/// Asynchronously processes a <c>Task</c> that produces a <see cref="Result{TIn}"/> by executing an asynchronous action based on its outcome.
	/// Once the task completes, if the result is successful, awaits the <paramref name="success"/> function;
	/// otherwise, awaits the <paramref name="failure"/> function.
	/// </summary>
	/// <typeparam name="TIn">The type contained in the Result.</typeparam>
	/// <param name="self">A Task that produces a <see cref="Result{TIn}"/>.</param>
	/// <param name="success">An asynchronous function to execute if the result is successful.</param>
	/// <param name="failure">An asynchronous function to execute if the result is a failure.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <example>
	/// <code>
	/// await Task.FromResult(Result.Success(42))
	///     .MatchAsync(async x => { await Task.Delay(50); Console.WriteLine($"Success: {x}"); },
	///                 async error => { await Task.Delay(50); Console.WriteLine("Error"); });
	/// // Prints "Success: 42" after a short delay
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this Task<Result<TIn>> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
	{
		var result = await self.ConfigureAwait(false);
		if (result.IsSuccess)
			await success(result.Value).ConfigureAwait(false);
		else
			await failure(result.Error).ConfigureAwait(false);
	}

	#endregion

	#region TryAsync<TIn>

	/// <summary>
	/// Asynchronously evaluates a <see cref="TryAsync{TIn}"/> delegate (which returns a <see cref="Task"/> of <see cref="Result{TIn}"/>)
	/// and synchronously transforms its outcome into a value of type <typeparamref name="TOut"/>.
	/// If the result is successful, the <paramref name="success"/> function is applied; otherwise,
	/// the <paramref name="failure"/> function is applied.
	/// </summary>
	/// <typeparam name="TIn">The type produced by the TryAsync delegate.</typeparam>
	/// <typeparam name="TOut">The type of the output value.</typeparam>
	/// <param name="self">A TryAsync delegate that returns a <see cref="Task"/> of <see cref="Result{TIn}"/>.</param>
	/// <param name="success">A synchronous function to transform the value if successful.</param>
	/// <param name="failure">A synchronous function to transform the error if a failure occurred.</param>
	/// <returns>A Task producing the transformed value of type <typeparamref name="TOut"/>.</returns>
	/// <example>
	/// <code><![CDATA[
	/// TryAsync<int> ParseNumberAsync(string input)
	///     => async () =>
	///     {
	///         await Task.Delay(10);
	///         return int.Parse(input);
	///     };
	///
	/// var output = await ParseNumberAsync("42")
	///     .MatchAsync(
	///         x => x * 2,
	///         error => 0);
	/// // output equals 84
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<TOut> MatchAsync<TIn, TOut>(
		this TryAsync<TIn> self,
		Func<TIn, TOut> success,
		Func<ResultError, TOut> failure)
		=> self.TryAsync().MatchAsync(success: success, failure: failure);



	/// <summary>
	/// Asynchronously evaluates a <see cref="TryAsync{TIn}"/> delegate (which returns a <see cref="Task"/> of <see cref="Result{TIn}"/>)
	/// and transforms its outcome into a value of type <typeparamref name="TOut"/> using asynchronous functions.
	/// If the result is successful, the <paramref name="success"/> function is awaited; otherwise,
	/// the <paramref name="failure"/> function is awaited.
	/// </summary>
	/// <typeparam name="TIn">The type produced by the TryAsync delegate.</typeparam>
	/// <typeparam name="TOut">The type of the output value.</typeparam>
	/// <param name="self">A TryAsync delegate that returns a <see cref="Task"/> of <see cref="Result{TIn}"/>.</param>
	/// <param name="success">An asynchronous function to transform the value if successful.</param>
	/// <param name="failure">An asynchronous function to transform the error if a failure occurred.</param>
	/// <returns>A Task producing the transformed value of type <typeparamref name="TOut"/>.</returns>
	/// <example>
	/// <code><![CDATA[
	/// TryAsync<int> ParseNumberAsync(string input)
	///     => async () =>
	///     {
	///         await Task.Delay(10);
	///         return int.Parse(input);
	///     };
	///
	/// var output = await ParseNumberAsync("42")
	///     .MatchAsync(
	///         async x => await Task.FromResult(x * 2),
	///         async error => await Task.FromResult(0));
	/// // output equals 84
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<TOut> MatchAsync<TIn, TOut>(
		this TryAsync<TIn> self,
		Func<TIn, Task<TOut>> success,
		Func<ResultError, Task<TOut>> failure)
		=> self.TryAsync().MatchAsync(success: success, failure: failure);



	/// <summary>
	/// Asynchronously evaluates a <see cref="TryAsync{TIn}"/> delegate (which returns a <see cref="Task"/> of <see cref="Result{TIn}"/>)
	/// and executes a synchronous action based on its outcome.
	/// If the result is successful, the <paramref name="success"/> action is invoked; otherwise,
	/// the <paramref name="failure"/> action is invoked.
	/// </summary>
	/// <typeparam name="TIn">The type produced by the TryAsync delegate.</typeparam>
	/// <param name="self">A TryAsync delegate that returns a <see cref="Task"/> of <see cref="Result{TIn}"/>.</param>
	/// <param name="success">A synchronous action to execute if successful.</param>
	/// <param name="failure">A synchronous action to execute if a failure occurred.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <example>
	/// <code><![CDATA[
	/// TryAsync<int> ParseNumberAsync(string input)
	///     => async () =>
	///     {
	///         await Task.Delay(10);
	///         return int.Parse(input);
	///     };
	///
	/// await ParseNumberAsync("42")
	///     .MatchAsync(
	///         x => Console.WriteLine($"Parsed: {x}"),
	///         error => Console.WriteLine("Failed"));
	/// // When input is "42", prints: "Parsed: 42"
	/// ]]></code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task MatchAsync<TIn>(
		this TryAsync<TIn> self, Action<TIn> success, Action<ResultError> failure)
		=> self.TryAsync().MatchAsync(success: success, failure: failure);


	/// <summary>
	/// Asynchronously evaluates a <see cref="TryAsync{TIn}"/> delegate (which returns a <see cref="Task"/> of <see cref="Result{TIn}"/>)
	/// and executes an asynchronous action based on its outcome.
	/// If the result is successful, the <paramref name="success"/> function is awaited; otherwise,
	/// the <paramref name="failure"/> function is awaited.
	/// </summary>
	/// <typeparam name="TIn">The type produced by the TryAsync delegate.</typeparam>
	/// <param name="self">A TryAsync delegate that returns a <see cref="Task"/> of <see cref="Result{TIn}"/>.</param>
	/// <param name="success">An asynchronous function to execute if successful.</param>
	/// <param name="failure">An asynchronous function to execute if a failure occurred.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <example>
	/// <code><![CDATA[
	/// TryAsync<int> ParseNumberAsync(string input)
	///     => async () =>
	///     {
	///         await Task.Delay(10);
	///         return int.Parse(input);
	///     };
	///
	/// await ParseNumberAsync("42")
	///     .MatchAsync(
	///         async x =>
	///         {
	///             await Task.Delay(50);
	///             Console.WriteLine($"Parsed: {x}");
	///         },
	///         async error =>
	///         {
	///             await Task.Delay(50);
	///             Console.WriteLine("Failed");
	///         });
	/// // When input is "42", prints: "Parsed: 42" after a short delay
	/// ]]></code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task MatchAsync<TIn>(this TryAsync<TIn> self,
		Func<TIn, Task> success, Func<ResultError, Task> failure)
		=> self.TryAsync().MatchAsync(success: success, failure: failure);

	#endregion
}