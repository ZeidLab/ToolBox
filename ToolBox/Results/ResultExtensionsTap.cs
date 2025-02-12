using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for tapping operations on <see cref="Result"/> objects.
/// Executes a side effect if the result is successful without interrupting the flow of the process.
/// These methods are particularly useful for logging, debugging, or performing side effects without
/// breaking the railway-oriented programming chain.
/// </summary>
/// <remarks>
/// <para>The Tap methods follow the functional programming principle of maintaining immutability while
/// allowing controlled side effects.
/// </para>
/// They are commonly used for:
/// <list type="bullet">
/// <item>
/// <description>Logging intermediate values in a chain of operations</description>
/// </item>
/// <item>
/// <description>Debugging by inspecting values at specific points</description>
/// </item>
/// <item>
/// <description>Performing notifications or updates without affecting the result flow</description>
/// </item>
/// </list>
/// </remarks>
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
    /// <code><![CDATA[
    /// // Success case - logs the value
    /// var successResult = Result.Success(42);
    /// successResult.Tap(value => Console.WriteLine($"Processing value: {value}"))
    ///             .Tap(value => SaveToAuditLog(value));
    /// // Output: Processing value: 42
    ///
    /// // Failure case - tap is not executed
    /// var failureResult = Result.Failure<int>(ResultError.New("Invalid input"));
    /// failureResult.Tap(value => Console.WriteLine($"This won't be printed"));
    /// // No output as the tap is skipped for failures
    /// ]]></code>
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
    /// <code><![CDATA[
    /// // Success case - operation succeeds and tap is executed
    /// var successTry = new Try<int>(() => 10);
    /// successTry.Tap(value => Console.WriteLine($"Processing value: {value}"));
    /// // Output: Processing value: 10
    ///
    /// // Failure case - operation fails and tap is skipped
    /// var failureTry = new Try<int>(() => throw new InvalidOperationException("Operation failed"));
    /// failureTry.Tap(value => Console.WriteLine($"This won't be printed"));
    /// // No output as the operation failed and tap is skipped
    /// ]]></code>
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
    /// <code><![CDATA[
    /// // Success case - logs the value asynchronously
    /// var successTask = Task.FromResult(Result.Success(20));
    /// await successTask.TapAsync(value => Console.WriteLine($"Processing value: {value}"));
    /// // Output: Processing value: 20
    ///
    /// // Failure case - tap is not executed
    /// var failureTask = Task.FromResult(Result.Failure<int>(ResultError.New("Invalid input")));
    /// await failureTask.TapAsync(value => Console.WriteLine($"This won't be printed"));
    /// // No output as the tap is skipped for failures
    /// ]]></code>
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
    /// <code><![CDATA[
    /// // Success case - operation succeeds and tap is executed
    /// var successTry = new TryAsync<int>(async () => {
    ///     await Task.Delay(100); // Simulate async work
    ///     return 30;
    /// });
    /// await successTry.TapAsync(value => Console.WriteLine($"Processing value: {value}"));
    /// // Output: Processing value: 30
    ///
    /// // Failure case - operation fails and tap is skipped
    /// var failureTry = new TryAsync<int>(async () => {
    ///     await Task.Delay(100); // Simulate async work
    ///     throw new InvalidOperationException("Operation failed");
    /// });
    /// await failureTry.TapAsync(value => Console.WriteLine($"This won't be printed"));
    /// // No output as the operation failed and tap is skipped
    /// ]]></code>
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
    /// <code><![CDATA[
    /// // Success case - executes async side effect
    /// var successResult = Result.Success(40);
    /// await successResult.TapAsync(async value => {
    ///     await Task.Delay(100); // Simulate async work
    ///     Console.WriteLine($"Processed value: {value}");
    /// });
    /// // Output: Processed value: 40
    ///
    /// // Failure case - async tap is not executed
    /// var failureResult = Result.Failure<int>(ResultError.New("Invalid input"));
    /// await failureResult.TapAsync(async value => {
    ///     await Task.Delay(100);
    ///     Console.WriteLine($"This won't be printed");
    /// });
    /// // No output as the tap is skipped for failures
    /// ]]></code>
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
    /// <code><![CDATA[
    /// // Success case - operation succeeds and async tap is executed
    /// var successTry = new Try<int>(() => 50);
    /// await successTry.TapAsync(async value => {
    ///     await Task.Delay(100); // Simulate async work
    ///     Console.WriteLine($"Processing value: {value}");
    /// });
    /// // Output: Processing value: 50
    ///
    /// // Failure case - operation fails and async tap is skipped
    /// var failureTry = new Try<int>(() => throw new InvalidOperationException("Operation failed"));
    /// await failureTry.TapAsync(async value => {
    ///     await Task.Delay(100);
    ///     Console.WriteLine($"This won't be printed");
    /// });
    /// // No output as the operation failed and tap is skipped
    /// ]]></code>
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
    /// <code><![CDATA[
    /// // Success case - async operation succeeds and tap is executed
    /// var successTry = new TryAsync<int>(async () => {
    ///     await Task.Delay(100); // Simulate async work
    ///     return 60;
    /// });
    /// await successTry.TapAsync(async value => {
    ///     await Task.Delay(100); // Simulate async side effect
    ///     Console.WriteLine($"Processing value: {value}");
    /// });
    /// // Output: Processing value: 60
    ///
    /// // Failure case - async operation fails and tap is skipped
    /// var failureTry = new TryAsync<int>(async () => {
    ///     await Task.Delay(100);
    ///     throw new InvalidOperationException("Operation failed");
    /// });
    /// await failureTry.TapAsync(async value => {
    ///     await Task.Delay(100);
    ///     Console.WriteLine($"This won't be printed");
    /// });
    /// // No output as the operation failed and tap is skipped
    /// ]]></code>
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
