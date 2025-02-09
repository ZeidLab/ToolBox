using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
///
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsMatchAsync
{
	/// <summary>
	/// Asynchronously matches the content of a <see><cref>Task{Result{TIn}}</cref></see>
	/// instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns a new value.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns a new value.</param>
	/// <returns>The result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, TOut> success,
		Func<ResultError, TOut> failure)
	{
		var result = await self.ConfigureAwait(false);
		return result.Match(success, failure);
	}

	/// <summary>
	/// Asynchronously matches the content of a <see><cref>Task{Result{TIn}}</cref></see>
	/// instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns a new result.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns a new result.</param>
	/// <returns>The result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self,
		Func<TIn, Result<TOut>> success,
		Func<ResultError, Result<TOut>> failure)
	{
		var result = await self.ConfigureAwait(false);
		return result.Match(success, failure);
	}

	/// <summary>
	/// Asynchronously matches the content of a <see><cref>Task{Result{TIn}}</cref></see> instance to either a
	/// successful or failed result by applying asynchronous functions.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">An asynchronous function that takes the value of the successful result and returns a new result.</param>
	/// <param name="failure">An asynchronous function that takes the error of the failed result and returns a new result.</param>
	/// <returns>A task representing the asynchronous operation, containing the result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self,
		Func<TIn, Task<Result<TOut>>> success,
		Func<ResultError, Task<Result<TOut>>> failure)
	{
		var result = await self.ConfigureAwait(false);
		return await result.Match(success, failure).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously matches the content of a <see cref="TryAsync{TIn}"/> instance to either a successful or failed result by applying functions.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns a new value.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns a new value.</param>
	/// <returns>The result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TOut> MatchAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, TOut> success,
		Func<ResultError, TOut> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		return result.Match(success, failure);
	}

	/// <summary>
	/// Asynchronously matches the content of a <see cref="TryAsync{TIn}"/> instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns a new result.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns a new result.</param>
	/// <returns>A task representing the asynchronous operation, containing the result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> MatchAsync<TIn, TOut>(this TryAsync<TIn> self,
		Func<TIn, Result<TOut>> success,
		Func<ResultError, Result<TOut>> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		return result.Match(success, failure);
	}

	/// <summary>
	/// Asynchronously matches the content of a <see cref="TryAsync{TIn}"/> instance to either a successful or failed result by applying asynchronous functions.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">An asynchronous function that takes the value of the successful result and returns a new value.</param>
	/// <param name="failure">An asynchronous function that takes the error of the failed result and returns a new value.</param>
	/// <returns>The result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TOut> MatchAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Task<TOut>> success,
		Func<ResultError, Task<TOut>> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		return result.IsSuccess
			? await success(result.Value).ConfigureAwait(false)
			: await failure(result.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously matches the content of a <see cref="TryAsync{TIn}"/> instance to either a successful or failed result by applying asynchronous functions.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">An asynchronous function that takes the value of the successful result and returns a new result.</param>
	/// <param name="failure">An asynchronous function that takes the error of the failed result and returns a new result.</param>
	/// <returns>A task representing the asynchronous operation, containing the result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> MatchAsync<TIn, TOut>(this TryAsync<TIn> self,
		Func<TIn, Task<Result<TOut>>> success,
		Func<ResultError, Task<Result<TOut>>> failure)
		=> self.TryAsync().MatchAsync(success, failure);

	/// <summary>
	/// Asynchronously matches the content of a <see><cref>Task{Result{TIn}}</cref></see>
	/// instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns no value.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns no value.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
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
	/// Asynchronously matches the content of a <see cref="TryAsync{TIn}"/> instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns no value.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns no value.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this TryAsync<TIn> self, Action<TIn> success, Action<ResultError> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		if (result.IsSuccess)
			success(result.Value);
		else
			failure(result.Error);
	}

	/// <summary>
	/// Asynchronously matches the content of a <see cref="Result{TIn}"/> instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">An asynchronous function that takes the value of the successful result and returns no value.</param>
	/// <param name="failure">An asynchronous function that takes the error of the failed result and returns no value.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this Result<TIn> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
	{
		if (self.IsSuccess)
			await success(self.Value).ConfigureAwait(false);
		else
			await failure(self.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously matches the content of a <see cref="Try{TIn}"/> instance to either a successful or failed result by applying asynchronous functions.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">An asynchronous function to execute if the result is successful.</param>
	/// <param name="failure">An asynchronous function to execute if the result is failed.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this Try<TIn> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
	{
		var result = self.Try();
		if (result.IsSuccess)
			await success(result.Value).ConfigureAwait(false);
		else
			await failure(result.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously matches the content of a <see><cref>Task{Result{TIn}}</cref></see>
	/// instance to either a successful or failed result by applying asynchronous functions.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">The task representing the result to match.</param>
	/// <param name="success">An asynchronous function that takes the value of the successful result and returns a task.</param>
	/// <param name="failure">An asynchronous function that takes the error of the failed result and returns a task.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
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

	/// <summary>
	/// Asynchronously matches the content of a <see cref="TryAsync{TIn}"/> instance to either a successful or failed result by applying asynchronous functions.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">The asynchronous <see cref="TryAsync{TIn}"/> to match.</param>
	/// <param name="success">An asynchronous function to execute if the result is successful.</param>
	/// <param name="failure">An asynchronous function to execute if the result is failed.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this TryAsync<TIn> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		if (result.IsSuccess)
			await success(result.Value).ConfigureAwait(false);
		else
			await failure(result.Error).ConfigureAwait(false);
	}
}