using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// provides extension methods for matching operations on <see cref="Result{TValue}"/> objects.
/// you need to consider both success and failure cases while using this extension methods
/// which is the main idea of doing railway oriented programming
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsMatch
{
	#region Match

	/// <summary>
	/// Matches the content of a <see cref="Result{TIn}"/> instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns a new result.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns a new result.</param>
	/// <returns>The result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Match<TIn, TOut>(this Result<TIn> self, Func<TIn, Result<TOut>> success,
		Func<ResultError, Result<TOut>> failure)
		=> self.IsSuccess ? success(self.Value) : failure(self.Error);

	/// <summary>
	/// Matches the content of a <see cref="Result{TIn}"/> instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns a new result.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns a new result.</param>
	/// <returns>The result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TOut Match<TIn, TOut>(this Result<TIn> self, Func<TIn, TOut> success,
		Func<ResultError, TOut> failure)
		=> self.IsSuccess ? success(self.Value) : failure(self.Error);

	/// <summary>
	/// Matches the content of a <see cref="Try{TIn}"/> instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns a new result.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns a new result.</param>
	/// <returns>The result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<TOut> Match<TIn, TOut>(this Try<TIn> self, Func<TIn, Result<TOut>> success,
		Func<ResultError, Result<TOut>> failure)
		=> self.Try().Match(success, failure);

	/// <summary>
	/// Matches the content of a <see cref="Try{TIn}"/> instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <typeparam name="TOut">The type of the value of the result returned by the success or failure function.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and returns a new result.</param>
	/// <param name="failure">A function that takes the error of the failed result and returns a new result.</param>
	/// <returns>The result of applying the success or failure function to the content of the original result.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TOut Match<TIn, TOut>(this Try<TIn> self, Func<TIn, TOut> success, Func<ResultError, TOut> failure)
		=> self.Try().Match(success, failure);


	/// <summary>
	/// Matches the content of a <see cref="Result{TIn}"/> instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">An action that takes the value of the successful result and performs side effects.</param>
	/// <param name="failure">An action that takes the error of the failed result and performs side effects.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Match<TIn>(this Result<TIn> self, Action<TIn> success, Action<ResultError> failure)
	{
		if (self.IsSuccess)
			success(self.Value);
		else
			failure(self.Error);
	}


	/// <summary>
	/// Matches the content of a <see cref="Try{TIn}"/> instance to either a successful or failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value contained in the result.</typeparam>
	/// <param name="self">The result to match.</param>
	/// <param name="success">A function that takes the value of the successful result and executes side effects.</param>
	/// <param name="failure">A function that takes the error of the failed result and executes side effects.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Match<TIn>(this Try<TIn> self, Action<TIn> success, Action<ResultError> failure)
	{
		var result = self.Try();
		if (result.IsSuccess)
			success(result.Value);
		else
			failure(result.Error);
	}

	#endregion

	#region MatchAsync

	#endregion
}