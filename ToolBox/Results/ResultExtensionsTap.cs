using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for tapping operations on <see cref="Result"/> objects.
/// Executes a side effect if the result is successful without interrupting the flow of the process.
/// </summary>
public static class ResultExtensionsTap
{
    /// <summary>
    /// Executes a side effect if the result is successful without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The result to tap.</param>
    /// <param name="action">The side effect to execute.</param>
    /// <returns>The original result.</returns>
    /// <example>
    /// Basic usage:
    /// <code>
    /// var result = new Result&lt;int&gt;(5);
    /// result.Tap(value => Console.WriteLine($"Value: {value}")); // Output: Value: 5
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> Tap<TIn>(this Result<TIn> self, Action<TIn> action)
    {
#pragma warning disable CA1062
        if (self.IsSuccess) action(self.Value);
#pragma warning restore CA1062
        return self;
    }

    /// <summary>
    /// Executes a side effect on the value of a successful <see cref="Try{TIn}"/> without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The Try instance to tap.</param>
    /// <param name="action">The side effect to execute on the value if successful.</param>
    /// <returns>The original result after executing the side effect.</returns>
    /// <example>
    /// <code>
    /// var tryResult = new Try&lt;int&gt;(() => 10);
    /// tryResult.Tap(value => Console.WriteLine($"Try Value: {value}")); // Output: Try Value: 10
    /// </code>
    /// </example>
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
    /// <example>
    /// <code>
    /// var taskResult = Task.FromResult(new Result&lt;int&gt;(20));
    /// await taskResult.TapAsync(value => Console.WriteLine($"Async Value: {value}")); // Output: Async Value: 20
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> TapAsync<TIn>(this Task<Result<TIn>> self, Action<TIn> action)
    {
#pragma warning disable CA1062
        var result = await self.ConfigureAwait(false);
        if (result.IsSuccess) action(result.Value);
#pragma warning restore CA1062
        return result;
    }

    /// <summary>
    /// Asynchronously executes a side effect on the value of a successful <see cref="TryAsync{TIn}"/> without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The asynchronous <see cref="TryAsync{TIn}"/> to tap.</param>
    /// <param name="action">The side effect to execute on the value if successful.</param>
    /// <returns>A task representing the asynchronous operation, containing the original result.</returns>
    /// <example>
    /// <code>
    /// var tryAsyncResult = new TryAsync&lt;int&gt;(async () => 30);
    /// await tryAsyncResult.TapAsync(value => Console.WriteLine($"Async Try Value: {value}")); // Output: Async Try Value: 30
    /// </code>
    /// </example>
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
    /// <example>
    /// <code>
    /// var result = new Result&lt;int&gt;(40);
    /// await result.TapAsync(async value => await Task.Delay(1000)); // Simulates async operation
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> TapAsync<TIn>(this Result<TIn> self, Func<TIn, Task> func)
    {
#pragma warning disable CA1062
        if (self.IsSuccess) await func(self.Value).ConfigureAwait(false);
#pragma warning restore CA1062
        return self;
    }

    /// <summary>
    /// Asynchronously executes a side effect on the value of a successful <see cref="Try{TIn}"/> without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The Try instance to tap.</param>
    /// <param name="func">The asynchronous side effect to execute on the value if successful.</param>
    /// <returns>The original result after executing the side effect.</returns>
    /// <example>
    /// <code>
    /// var tryResult = new Try&lt;int&gt;(() => 50);
    /// await tryResult.TapAsync(async value => await Task.Delay(500)); // Simulates async operation
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> TapAsync<TIn>(this Try<TIn> self, Func<TIn, Task> func)
    {
        var result = self.Try();
#pragma warning disable CA1062
        if (result.IsSuccess) await func(result.Value).ConfigureAwait(false);
#pragma warning restore CA1062
        return result;
    }

    /// <summary>
    /// Asynchronously executes a side effect on the value of a successful <see cref="TryAsync{TIn}"/> without interrupting the flow of the process.
    /// </summary>
    /// <typeparam name="TIn">The type of the result value.</typeparam>
    /// <param name="self">The asynchronous <see cref="TryAsync{TIn}"/> to tap.</param>
    /// <param name="func">The asynchronous side effect to execute on the value if successful.</param>
    /// <returns>A task representing the asynchronous operation, containing the original result.</returns>
    /// <example>
    /// <code>
    /// var tryAsyncResult = new TryAsync&lt;int&gt;(async () => 60);
    /// await tryAsyncResult.TapAsync(async value => await Task.Delay(2000)); // Simulates async operation
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> TapAsync<TIn>(this TryAsync<TIn> self, Func<TIn, Task> func)
    {
        var result = await self.TryAsync().ConfigureAwait(false);
#pragma warning disable CA1062
        if (result.IsSuccess) await func(result.Value).ConfigureAwait(false);
#pragma warning restore CA1062
        return result;
    }
}
