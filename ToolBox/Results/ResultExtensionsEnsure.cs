using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

public static class ResultExtensionsEnsure
{
    /// <summary>
    /// Ensures that the value of a successful result satisfies a predicate.
    /// </summary>
    /// <remarks>
    /// If the result is failed, this method returns the original result.
    /// If the result is successful but the predicate is false, this method returns a failed result with the provided error.
    /// Otherwise, this method returns the original result.
    /// </remarks>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="predicate">The predicate to evaluate.</param>
    /// <param name="error">The error to include in the result when the predicate is false.</param>
    /// <returns>The result of the predicate evaluation.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> Ensure<TIn>(this Result<TIn> result, Func<TIn, bool> predicate, Error error)
        => result.IsFailure || predicate(result.Value) ? result : error;


    /// <summary>
    /// Ensures that the value of a successful result satisfies a predicate.
    /// </summary>
    /// <remarks>
    /// If the result is failed, this method returns the original result.
    /// If the result is successful but the predicate is false, this method returns a failed result with the provided error.
    /// Otherwise, this method returns the original result.
    /// </remarks>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="self">The result to check.</param>
    /// <param name="predicate">The predicate to evaluate.</param>
    /// <param name="error">The error to include in the result when the predicate is false.</param>
    /// <returns>The result of the predicate evaluation.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> EnsureAsync<TIn>(this Task<Result<TIn>> self,
        Func<TIn, bool> predicate, Error error)
        => (await self.ConfigureAwait(false)).Ensure(predicate, error);

    /// <summary>
    /// Ensures that the value of a successful result satisfies a predicate.
    /// </summary>
    /// <remarks>
    /// If the result is failed, this method returns the original result.
    /// If the result is successful but the predicate is false, this method returns a failed result with the provided error.
    /// Otherwise, this method returns the original result.
    /// </remarks>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="self">The result to check.</param>
    /// <param name="predicate">The predicate to evaluate.</param>
    /// <param name="error">The error to include in the result when the predicate is false.</param>
    /// <returns>The result of the predicate evaluation.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> EnsureAsync<TIn>(this Task<Result<TIn>> self,
        Func<TIn, Task<bool>> predicate, Error error)
    {
        var result = await self.ConfigureAwait(false);
        return result.IsFailure || await predicate(result.Value).ConfigureAwait(false) ? result : error;
    }
}