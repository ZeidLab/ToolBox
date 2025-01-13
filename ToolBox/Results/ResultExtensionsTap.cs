using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// provides extension methods for tapping operations on <see cref="Result{TValue}"/> objects.
/// it executes a side effect if the result is successful without interrupting the flow of the process.
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsTap
{
    /// <summary>
    /// Executes a side effect if the result is successful without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The result to tap.</param>
    /// <param name="action">The side effect to execute.</param>
    /// <returns>The original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> Tap<TIn>(this Result<TIn> self, Action<TIn> action)
    {
        if (self.IsSuccess) action(self.Value);
        return self;
    }

    /// <summary>
    /// Executes a side effect on the value of a successful <see cref="Try{TIn}"/> without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The Try instance to tap.</param>
    /// <param name="action">The side effect to execute on the value if successful.</param>
    /// <returns>The original result after executing the side effect.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> Tap<TIn>(this Try<TIn> self, Action<TIn> action)
        => self.Try().Tap(action);

    /// <summary>
    /// Asynchronously executes a side effect if the result is successful without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The task representing the result to tap.</param>
    /// <param name="action">The side effect to execute.</param>
    /// <returns>A task representing the asynchronous operation, containing the original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> TapAsync<TIn>(this Task<Result<TIn>> self, Action<TIn> action)
    {
        var result = await self.ConfigureAwait(false);
        if (result.IsSuccess) action(result.Value);

        return result;
    }

    /// <summary>
    /// Asynchronously executes a side effect on the value of a successful <see cref="TryAsync{TIn}"/> without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The asynchronous <see cref="TryAsync{TIn}"/> to tap.</param>
    /// <param name="action">The side effect to execute on the value if successful.</param>
    /// <returns>A task representing the asynchronous operation, containing the original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TIn>> TapAsync<TIn>(this TryAsync<TIn> self, Action<TIn> action)
        => self.TryAsync().TapAsync(action);


    /// <summary>
    /// Asynchronously executes a side effect if the result is successful without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The result to tap.</param>
    /// <param name="func">The asynchronous side effect to execute.</param>
    /// <returns>The original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> TapAsync<TIn>(this Result<TIn> self, Func<TIn, Task> func)
    {
        if (self.IsSuccess) await func(self.Value).ConfigureAwait(false);
        return self;
    }

    /// <summary>
    /// Asynchronously executes a side effect on the value of a successful <see cref="Try{TIn}"/> without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The Try instance to tap.</param>
    /// <param name="func">The asynchronous side effect to execute on the value if successful.</param>
    /// <returns>The original result after executing the side effect.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> TapAsync<TIn>(this Try<TIn> self, Func<TIn, Task> func)
    {
        var result = self.Try();
        if (result.IsSuccess) await func(result.Value).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Asynchronously executes a side effect on the value of a successful <see cref="TryAsync{TIn}"/> without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The asynchronous <see cref="TryAsync{TIn}"/> to tap.</param>
    /// <param name="func">The asynchronous side effect to execute on the value if successful.</param>
    /// <returns>A task representing the asynchronous operation, containing the original result.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> TapAsync<TIn>(this TryAsync<TIn> self, Func<TIn, Task> func)
    {
        var result = await self.TryAsync().ConfigureAwait(false);
        if (result.IsSuccess) await func(result.Value).ConfigureAwait(false);
        return result;
    }
}