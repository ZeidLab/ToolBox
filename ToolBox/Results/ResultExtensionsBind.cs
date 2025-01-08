using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for binding operations on <see cref="Result{TValue}"/> objects.
/// </summary>
public static class ResultExtensionsBind
{
    #region Bind

    /// <summary>
    /// Binds the value of a successful result to a new result by applying the specified function.
    /// </summary>
    /// <typeparam name="TIn">The type of the value of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the value of the new result.</typeparam>
    /// <param name="self">The original result.</param>
    /// <param name="func">The function to apply to the value of the original result.</param>
    /// <returns>The result of applying the specified function to the value of the original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> self,
        Func<TIn, Result<TOut>> func) 
        => self.IsSuccess 
            ? func(self.Value) 
            : Result<TOut>
                .Failure(self.Error.GetValueOrDefault());

    /// <summary>
    /// Binds the value of a successful <see cref="Try{TIn}"/> to a new result by applying the specified function.
    /// </summary>
    /// <typeparam name="TIn">The type of the value of the original <see cref="Try{TIn}"/>.</typeparam>
    /// <typeparam name="TOut">The type of the value of the new result.</typeparam>
    /// <param name="self">The original <see cref="Try{TIn}"/>.</param>
    /// <param name="func">The function to apply to the value of the original <see cref="Try{TIn}"/>.</param>
    /// <returns>The result of applying the specified function to the value of the original <see cref="Try{TIn}"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self,
        Func<TIn, Result<TOut>> func)
        => self.Try().Bind(func);

    #endregion

    #region BindAsync

    /// <summary>
    /// Binds the value of a successful result to a new result by applying the specified asynchronous function.
    /// </summary>
    /// <typeparam name="TIn">The type of the value of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the value of the new result.</typeparam>
    /// <param name="self">The original result.</param>
    /// <param name="func">The asynchronous function to apply to the value of the original result.</param>
    /// <returns>The result of applying the specified asynchronous function to the value of the original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> self,
        Func<TIn, Task<Result<TOut>>> func)
    => self.IsSuccess
            ? func(self.Value)
            : Result<TOut>
                .Failure(self.Error.GetValueOrDefault())
                .AsTask();
    


    /// <summary>
    /// Binds the value of a successful result to a new result by applying the specified function.
    /// </summary>
    /// <typeparam name="TIn">The type of the value of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the value of the new result.</typeparam>
    /// <param name="self">The original result.</param>
    /// <param name="func">The function to apply to the value of the original result.</param>
    /// <returns>The result of applying the specified function to the value of the original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self,
        Func<TIn, Result<TOut>> func)
        => (await self.ConfigureAwait(false)).Bind(func);

    /// <summary>
    /// Asynchronously binds the value of a successful result to a new result by applying the specified asynchronous function.
    /// </summary>
    /// <typeparam name="TIn">The type of the value of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the value of the new result.</typeparam>
    /// <param name="self">A task that represents the asynchronous operation of obtaining the original result.</param>
    /// <param name="func">The asynchronous function to apply to the value of the original result.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of applying the specified asynchronous function to the value of the original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self,
        Func<TIn, Task<Result<TOut>>> func)
    {
        var result = await self;
        return result.IsSuccess
            ? await func(result.Value).ConfigureAwait(false)
            : Result<TOut>.Failure(result.Error.GetValueOrDefault());
    }

    /// <summary>
    /// Asynchronously binds the value of a successful result to a new result by applying the specified function.
    /// </summary>
    /// <typeparam name="TIn">The type of the value of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the value of the new result.</typeparam>
    /// <param name="self">A try-async that represents the asynchronous operation of obtaining the original result.</param>
    /// <param name="func">The function to apply to the value of the original result.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of applying the specified function to the value of the original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self,
        Func<TIn, Result<TOut>> func)
        => (await self.Try()).Bind(func);

    /// <summary>
    /// Asynchronously binds the value of a successful result to a new result by applying the specified asynchronous function.
    /// </summary>
    /// <typeparam name="TIn">The type of the value of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the value of the new result.</typeparam>
    /// <param name="self">A try-async that represents the asynchronous operation of obtaining the original result.</param>
    /// <param name="func">The asynchronous function to apply to the value of the original result.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of applying the specified asynchronous function to the value of the original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.Try().BindAsync(func);

    #endregion
}