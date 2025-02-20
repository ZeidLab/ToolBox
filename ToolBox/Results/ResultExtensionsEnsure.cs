using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for validating values within <see cref="Result{TValue}"/> instances using predicates.
/// These methods help enforce business rules and data validation while maintaining the Result pattern.
/// </summary>
/// <remarks>
/// This class contains methods that allow you to:
/// <list type="bullet">
///   <item><description>Validate successful results using synchronous predicates</description></item>
///   <item><description>Validate successful results using asynchronous predicates</description></item>
///   <item><description>Convert valid results to failures when predicates are not satisfied</description></item>
/// </list>
///
/// Key features:
/// <list type="bullet">
///   <item><description>Preserves the original result if it's already a failure</description></item>
///   <item><description>Returns a new failure result with the specified error if validation fails</description></item>
///   <item><description>Supports both synchronous and asynchronous operations</description></item>
/// </list>
/// </remarks>
/// <example>
/// Basic usage:
/// <code><![CDATA[
/// // Validate that a number is positive.
/// var result = Result.Success(42)
///     .Ensure(x => x > 0, ResultError.New("Number must be positive"));
///
/// // Chain multiple validations
/// var result = Result.Success(userInput)
///     .Ensure(x => !string.IsNullOrEmpty(x), ResultError.New("Input cannot be empty"))
///     .Ensure(x => x.Length >= 3, ResultError.New("Input must be at least 3 characters"));
/// ]]></code>
/// </example>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsEnsure
{
	#region Result<TIn>

	/// <summary>
	/// Validates the <paramref name="result"/> by applying a synchronous <paramref name="predicate"/> to its value.
	/// Returns the original result if the predicate returns <c>true</c> or if the result is already a failure;
	/// otherwise, returns a failure result containing the specified <paramref name="resultError"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="result">
	/// The result to validate. If <paramref name="result"/> is already a failure, it is returned without further validation.
	/// </param>
	/// <param name="predicate">
	/// A synchronous predicate that defines the validation rule for the result's value.
	/// </param>
	/// <param name="resultError">
	/// The error to return if the predicate evaluates to <c>false</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Result{TIn}"/> that is either the original successful result if the predicate returns <c>true</c>,
	/// or a failure result containing the specified <paramref name="resultError"/>.
	/// </returns>
	/// <example>
	/// <![CDATA[
	/// var successResult = Result.Success(42);
	/// // Validates that the value is positive.
	/// var validatedResult = successResult.Ensure(x => x > 0, ResultError.New("Value must be positive"));
	/// ]]>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TIn> Ensure<TIn>(this Result<TIn> result, Func<TIn, bool> predicate, ResultError resultError)
		=> result.IsFailure || predicate(result.Value) ? result : resultError;

	/// <summary>
	/// Asynchronously validates the <paramref name="result"/> by applying an asynchronous <paramref name="predicate"/>
	/// to its value. Returns the original result if the predicate returns <c>true</c> or if the result is already a failure;
	/// otherwise, returns a failure result containing the specified <paramref name="resultError"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="result">
	/// The result to validate. If <paramref name="result"/> is already a failure, it is returned without further processing.
	/// </param>
	/// <param name="predicate">
	/// An asynchronous predicate that defines the validation rule for the result's value.
	/// </param>
	/// <param name="resultError">
	/// The error to return if the predicate evaluates to <c>false</c>.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation, containing a <see cref="Result{TIn}"/> that is either the original result
	/// or a failure result with <paramref name="resultError"/>.
	/// </returns>
	/// <example>
	/// <![CDATA[
	/// var successResult = Result.Success(42);
	/// // Asynchronously validates that the value is even.
	/// var validatedResult = await successResult.EnsureAsync(async x =>
	/// {
	///     await Task.Delay(10);
	///     return x % 2 == 0;
	/// }, ResultError.New("Value must be even"));
	/// ]]>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TIn>> EnsureAsync<TIn>(this Result<TIn> result, Func<TIn, Task<bool>> predicate, ResultError resultError)
		=> result.IsFailure || await predicate(result.Value).ConfigureAwait(false) ? result : resultError;

	#endregion

	#region Result<TIn> from Try<TIn>

	/// <summary>
	/// Converts a <see cref="Try{TIn}"/> to a <see cref="Result{TIn}"/>, then synchronously validates it using the specified <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the try result.</typeparam>
	/// <param name="result">
	/// The try result to be converted and validated.
	/// </param>
	/// <param name="predicate">
	/// A synchronous predicate that determines if the value is valid.
	/// </param>
	/// <param name="resultError">
	/// The error to return if the predicate evaluates to <c>false</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Result{TIn}"/> representing either a successful validation of the try result or a failure with <paramref name="resultError"/>.
	/// </returns>
	/// <example>
	/// <![CDATA[
	/// var tryResult = Try(() => 42);
	/// // Validates that the value is greater than 10.
	/// var validatedResult = tryResult.Ensure(x => x > 10, ResultError.New("Value must be greater than 10"));
	/// ]]>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TIn> Ensure<TIn>(this Try<TIn> result, Func<TIn, bool> predicate, ResultError resultError)
		=> result.Try().Ensure(predicate, resultError);

	/// <summary>
	/// Converts a <see cref="Try{TIn}"/> to a <see cref="Result{TIn}"/>, then asynchronously validates it using the specified asynchronous <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the try result.</typeparam>
	/// <param name="result">
	/// The try result to be converted and validated.
	/// </param>
	/// <param name="predicate">
	/// An asynchronous predicate that determines if the value is valid.
	/// </param>
	/// <param name="resultError">
	/// The error to return if the predicate evaluates to <c>false</c>.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation, containing a <see cref="Result{TIn}"/> that is either the successful try result or a failure with <paramref name="resultError"/>.
	/// </returns>
	/// <example>
	/// <![CDATA[
	/// var tryResult = Try(() => 42);
	/// // Asynchronously validates that the value is not zero.
	/// var validatedResult = await tryResult.EnsureAsync(async x =>
	/// {
	///     await Task.Delay(10);
	///     return x != 0;
	/// }, ResultError.New("Value must not be zero"));
	/// ]]>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TIn>> EnsureAsync<TIn>(this Try<TIn> result, Func<TIn, Task<bool>> predicate, ResultError resultError)
		=> result.Try().EnsureAsync(predicate, resultError);

	#endregion

	#region Task<Result<TIn>>

	/// <summary>
	/// Asynchronously validates a <see cref="Task"/> containing <see cref="Result{TIn}"/> by applying a synchronous <paramref name="predicate"/>
	/// to its value once the task completes.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">
	/// A task representing the asynchronous operation that returns a <see cref="Result{TIn}"/>.
	/// </param>
	/// <param name="predicate">
	/// A synchronous predicate to validate the result's value.
	/// </param>
	/// <param name="resultError">
	/// The error to return if the predicate evaluates to <c>false</c>.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation, containing a <see cref="Result{TIn}"/> that is either the original result
	/// or a failure result with <paramref name="resultError"/>.
	/// </returns>
	/// <example>
	/// <![CDATA[
	/// var taskResult = Task.FromResult(Result.Success(42));
	/// // Validates synchronously after the task completes.
	/// var validatedResult = await taskResult.EnsureAsync(x => x > 50, ResultError.New("Value must be greater than 50"));
	/// ]]>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TIn>> EnsureAsync<TIn>(this Task<Result<TIn>> self,
		Func<TIn, bool> predicate, ResultError resultError)
		=> (await self.ConfigureAwait(false)).Ensure(predicate, resultError);

	/// <summary>
	/// Asynchronously validates a <see cref="Task"/> containing <see cref="Result{TIn}"/> by applying an asynchronous <paramref name="predicate"/>
	/// to its value once the task completes.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">
	/// A task that returns a <see cref="Result{TIn}"/> upon completion.
	/// </param>
	/// <param name="predicate">
	/// An asynchronous predicate to validate the result's value.
	/// </param>
	/// <param name="resultError">
	/// The error to return if the predicate evaluation fails.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation, containing a <see cref="Result{TIn}"/> that is either the original result
	/// or a failure result with <paramref name="resultError"/>.
	/// </returns>
	/// <example>
	/// <![CDATA[
	/// var taskResult = Task.FromResult(Result.Success(42));
	/// // Asynchronously validates that the value is a multiple of 3.
	/// var validatedResult = await taskResult.EnsureAsync(async x =>
	/// {
	///     await Task.Delay(10);
	///     return x % 3 == 0;
	/// }, ResultError.New("Value must be a multiple of 3"));
	/// ]]>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TIn>> EnsureAsync<TIn>(this Task<Result<TIn>> self,
		Func<TIn, Task<bool>> predicate, ResultError resultError)
		=> await (await self.ConfigureAwait(false)).EnsureAsync(predicate, resultError).ConfigureAwait(false);

	#endregion

	#region Result<TIn> from TryAsync<TIn>

	/// <summary>
	/// Converts a <see cref="TryAsync{TIn}"/> to a <see cref="Result{TIn}"/>, then asynchronously validates it using the specified synchronous <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the asynchronous try result.</typeparam>
	/// <param name="result">
	/// The asynchronous try result to be converted and validated.
	/// </param>
	/// <param name="predicate">
	/// A synchronous predicate that determines if the value is valid.
	/// </param>
	/// <param name="resultError">
	/// The error to return if the predicate evaluates to <c>false</c>.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation, containing a <see cref="Result{TIn}"/> that is either the successful conversion
	/// or a failure with <paramref name="resultError"/>.
	/// </returns>
	/// <example>
	/// <![CDATA[
	/// var tryAsyncResult = TryAsync(() => Task.FromResult(42));
	/// // Validates that the value is less than 100.
	/// var validatedResult = await tryAsyncResult.EnsureAsync(x => x < 100, ResultError.New("Value must be less than 100"));
	/// ]]>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TIn>> EnsureAsync<TIn>(this TryAsync<TIn> result, Func<TIn, bool> predicate, ResultError resultError)
		=> result.TryAsync().EnsureAsync(predicate, resultError);

	/// <summary>
	/// Converts a <see cref="TryAsync{TIn}"/> to a <see cref="Result{TIn}"/>, then asynchronously validates it using the specified asynchronous <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the asynchronous try result.</typeparam>
	/// <param name="result">
	/// The asynchronous try result to be converted and validated.
	/// </param>
	/// <param name="predicate">
	/// An asynchronous predicate that determines if the value is valid.
	/// </param>
	/// <param name="resultError">
	/// The error to return if the predicate evaluation fails.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation, containing a <see cref="Result{TIn}"/> that is either the successful conversion
	/// or a failure with <paramref name="resultError"/>.
	/// </returns>
	/// <example>
	/// <![CDATA[
	/// var tryAsyncResult = TryAsync(() => Task.FromResult(42));
	/// // Asynchronously validates that the value is non-negative.
	/// var validatedResult = await tryAsyncResult.EnsureAsync(async x =>
	/// {
	///     await Task.Delay(10);
	///     return x >= 0;
	/// }, ResultError.New("Value must be non-negative"));
	/// ]]>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TIn>> EnsureAsync<TIn>(this TryAsync<TIn> result, Func<TIn, Task<bool>> predicate, ResultError resultError)
		=> result.TryAsync().EnsureAsync(predicate, resultError);

	#endregion
}
