using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// provides extension methods for matching operations on <see cref="Result{TValue}"/> objects.
/// you need to consider both success and failure cases while using this extension methods
/// which is the main idea of doing railway oriented programming
/// </summary>
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
        Func<Error, TOut> failure)
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
        Func<Error, Result<TOut>> failure)
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
    public static TOut Match<TIn, TOut>(this Try<TIn> self, Func<TIn, TOut> success, Func<Error, TOut> failure)
        => self.Try().Match(success, failure);

    #endregion

    #region MatchAsync

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
        Func<Error, TOut> failure)
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
        Func<Error, Result<TOut>> failure)
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
        Func<Error, Task<Result<TOut>>> failure)
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
        Func<Error, TOut> failure)
    {
        var result = await self.Try().ConfigureAwait(false);
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
        Func<Error, Result<TOut>> failure)
    {
        var result = await self.Try().ConfigureAwait(false);
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
        Func<Error, Task<TOut>> failure)
    {
        var result = await self.Try().ConfigureAwait(false);
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
        Func<Error, Task<Result<TOut>>> failure)
        => self.Try().MatchAsync(success, failure);

    #endregion
}