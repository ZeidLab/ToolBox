using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

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
        Func<Error, Result<TOut>> failure)
        => self.IsSuccess ? success(self.Value) : failure(self.Error.GetValueOrDefault());

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOut Match<TIn, TOut>(this Result<TIn> self, Func<TIn, TOut> success,
        Func<Error, TOut> failure)
        => self.IsSuccess ? success(self.Value) : failure(self.Error.GetValueOrDefault());

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Match<TIn, TOut>(this Try<TIn> self, Func<TIn, Result<TOut>> success,
        Func<Error, Result<TOut>> failure)
        => self.Try().Match(success, failure);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOut Match<TIn, TOut>(this Try<TIn> self, Func<TIn, TOut> success, Func<Error, TOut> failure)
        => self.Try().Match(success, failure);

    #endregion

    #region MatchAsync

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, TOut> success,
        Func<Error, TOut> failure)
    {
        var result = await self.ConfigureAwait(false);
        return result.Match(success, failure);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self,
        Func<TIn, Result<TOut>> success,
        Func<Error, Result<TOut>> failure)
    {
        var result = await self.ConfigureAwait(false);
        return result.Match(success, failure);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self,
        Func<TIn, Task<Result<TOut>>> success,
        Func<Error, Task<Result<TOut>>> failure)
    {
        var result = await self.ConfigureAwait(false);
        return await result.Match(success, failure).ConfigureAwait(false);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TOut> MatchAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, TOut> success,
        Func<Error, TOut> failure)
    {
        var result = await self.Try().ConfigureAwait(false);
        return result.Match(success, failure);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> MatchAsync<TIn, TOut>(this TryAsync<TIn> self,
        Func<TIn, Result<TOut>> success,
        Func<Error, Result<TOut>> failure)
    {
        var result = await self.Try().ConfigureAwait(false);
        return result.Match(success, failure);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TOut> MatchAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Task<TOut>> success,
        Func<Error, Task<TOut>> failure)
    {
        var result = await self.Try().ConfigureAwait(false);
        return result.IsSuccess
            ? await success(result.Value).ConfigureAwait(false)
            : await failure(result.Error.GetValueOrDefault()).ConfigureAwait(false);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> MatchAsync<TIn, TOut>(this TryAsync<TIn> self,
        Func<TIn, Task<Result<TOut>>> success,
        Func<Error, Task<Result<TOut>>> failure)
        => self.Try().MatchAsync(success, failure);

    #endregion
}