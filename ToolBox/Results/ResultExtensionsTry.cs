using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Represents a delegate that encapsulates a synchronous operation which may either succeed
/// or fail with a try-cache block that handles an unexpected exception. The operation returns a <see cref="Result{TIn}"/> to represent its outcome.
/// This delegate is commonly used in railway-oriented programming (ROP) to model operations
/// that can be chained together, with each step either continuing on the "happy path" (success)
/// or diverting to an "error track" (failure).
/// </summary>
/// <typeparam name="TIn">The type of the value returned by a successful operation.</typeparam>
/// <returns>A <see cref="Result{TIn}"/> representing the outcome of the operation.</returns>
#pragma warning disable CA1716
public delegate Result<TIn> Try<TIn>();
#pragma warning restore CA1716

/// <summary>
/// Represents a delegate that encapsulates an asynchronous operation which may either succeed
/// or fail with a try-cache block that handles an unexpected exception. The operation returns a <see><cref>Task{Result{TIn}}</cref></see> to represent its outcome.
/// This delegate is commonly used in railway-oriented programming (ROP) to model asynchronous
/// operations that can be chained together, with each step either continuing on the "happy path"
/// (success) or diverting to an "error track" (failure).
/// </summary>
/// <typeparam name="TIn">The type of the value returned by a successful operation.</typeparam>
/// <returns>A <see><cref>Task{Result{TIn}}</cref></see> representing the asynchronous outcome of the operation.</returns>
public delegate Task<Result<TIn>> TryAsync<TIn>();

/// <summary>
/// Provides extension methods for working with <see><cref>Try{TIn}</cref></see> and <see cref="Results.TryAsync{TIn}"/>
/// delegates. These methods simplify the invocation and transformation of synchronous and asynchronous
/// operations that return <see cref="Result{TIn}"/> instances.
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
public static class ResultExtensionsTry
{
	/// <summary>
	/// Converts a synchronous <see><cref>Try{TIn}</cref></see>
	/// delegate into an asynchronous <see cref="Results.TryAsync{TIn}"/> delegate.
	/// </summary>
	/// <typeparam name="TIn">The type of the value within the result.</typeparam>
	/// <param name="self">The synchronous <see><cref>Try{TIn}</cref></see> delegate to convert.</param>
	/// <returns>An asynchronous <see cref="Results.TryAsync{TIn}"/> delegate that wraps the original synchronous delegate.</returns>
	[Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable AMNF0002
    public static TryAsync<TIn> ToAsync<TIn>(this Try<TIn> self)
#pragma warning restore AMNF0002
	    => () => self.Try().AsTaskAsync();


    /// <summary>
    /// Invokes the specified synchronous <see><cref>Try{TIn}</cref></see> delegate and
    /// returns its result. If the delegate throws an exception, it is caught and
    /// returned as a failed result.
    /// </summary>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="self">The synchronous <see><cref>Try{TIn}</cref></see> delegate to invoke.</param>
    /// <returns>The result of invoking the specified delegate.</returns>
    [Pure]
    public static Result<TIn> Try<TIn>(this Try<TIn> self)
    {
        try
        {
            return self.Invoke();
        }
        catch (Exception ex)
        {
            return Result<TIn>.Failure(ex);
        }
    }

	/// <summary>
	/// Invokes the specified asynchronous <see cref="Results.TryAsync{TIn}"/> delegate and
	/// returns its result. If the delegate throws an exception, it is caught and
	/// returned as a failed result.
	/// </summary>
	/// <typeparam name="TIn">The type of the value within the result.</typeparam>
	/// <param name="self">The asynchronous <see cref="Results.TryAsync{TIn}"/> delegate to invoke.</param>
	/// <returns>The result of invoking the specified delegate.</returns>
	[Pure]
#pragma warning disable AMNF0001
	public static async Task<Result<TIn>> Try<TIn>(this TryAsync<TIn> self)
#pragma warning restore AMNF0001
	{
        try
        {
            return await self().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result<TIn>.Failure(ex);
        }
    }
}