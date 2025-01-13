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
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> self, Func<TIn, Result<TOut>> func)
        => self.IsSuccess
            ? func(self.Value)
            : Result<TOut>.Failure(self.Error);

    /// <summary>
    /// Binds the value of a successful <see cref="Try{TIn}"/> to a new result by applying the specified function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Result<TOut>> func)
        => self.Try().Bind(func);

    /// <summary>
    /// Binds the value of a successful <see cref="Try{TIn}"/> to a new result by applying the specified function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Try<TOut>> func)
        => self.Try().Bind(input => func(input).Try());

    #endregion

    #region BindAsync

    /// <summary>
    /// Binds the value of a successful result to a new result by applying the specified asynchronous function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.IsSuccess
            ? func(self.Value)
            : Result<TOut>.Failure(self.Error).AsTask();

    /// <summary>
    /// Binds the value of a successful result to a new result by applying the specified function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Result<TOut>> func)
        => (await self.ConfigureAwait(false)).Bind(func);

    /// <summary>
    /// Asynchronously binds the value of a successful result to a new result by applying the specified asynchronous function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Task<Result<TOut>>> func)
    {
        var result = await self;
        return result.IsSuccess
            ? await func(result.Value).ConfigureAwait(false)
            : Result<TOut>.Failure(result.Error);
    }

    /// <summary>
    /// Binds the value of a successful <see cref="Try{TIn}"/> to a new result by applying the specified asynchronous function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, TryAsync<TOut>> func)
        => self.Try().BindAsync(input => func(input).Try());

    /// <summary>
    /// Binds the value of a successful <see cref="Try{TIn}"/> to a new result by applying the specified asynchronous function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.Try().BindAsync(input => func(input));

    /// <summary>
    /// Binds the value of a successful result to a new result by applying the specified function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, TryAsync<TOut>> func)
        => self.Try().BindAsync(input => func(input).Try());

    /// <summary>
    /// Asynchronously binds the value of a successful <see cref="TryAsync{TIn}"/> to a new result by applying the specified function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Try<TOut>> func)
        => self.Try().BindAsync(input => func(input).Try());

    /// <summary>
    /// Asynchronously binds the value of a successful result to a new result by applying the specified function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Result<TOut>> func)
        => (await self.Try()).Bind(func);

    /// <summary>
    /// Asynchronously binds the value of a successful result to a new result by applying the specified asynchronous function.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.Try().BindAsync(func);

    #endregion
}