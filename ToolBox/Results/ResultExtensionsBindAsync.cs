using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for asynchronous binding operations on <see cref="Result{TValue}"/> objects.
/// These methods enable monadic composition of Result types with async operations.
/// </summary>
/// <remarks>
/// <para>
/// These methods provide asynchronous variants of the binding operations, allowing seamless
/// integration with async/await patterns while maintaining proper error propagation.
/// </para>
/// <example>
/// Basic async usage:
/// <code>
/// async Task&lt;Result&lt;int&gt;&gt; FetchDataAsync(int id) =>
///     await Task.FromResult(Result.Success(id * 2));
///
/// var result = await Result.Success(5)
///     .BindAsync(async x => await FetchDataAsync(x))
///     .BindAsync(async x => await FetchDataAsync(x));
/// // result is Success(20)
/// </code>
/// </example>
/// <example>
/// Error propagation in async operations:
/// <code>
/// async Task&lt;Result&lt;int&gt;&gt; ValidateAsync(int x) =>
///     x > 0 ? Result.Success(x)
///           : Result.Failure&lt;int&gt;(new ResultError("Value must be positive"));
///
/// var result = await Result.Success(-5)
///     .BindAsync(async x => await ValidateAsync(x))
///     .BindAsync(async x => await FetchDataAsync(x));
/// // result is Failure("Value must be positive")
/// </code>
/// </example>
/// </remarks>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsBindAsync
{
    /// <summary>
    /// Asynchronously binds the value of a successful result to a new result by applying the specified function.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the input result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the output result.</typeparam>
    /// <param name="self">The result to bind.</param>
    /// <param name="func">The asynchronous function to apply to the value if the result is successful.</param>
    /// <returns>A task that represents the asynchronous bind operation.</returns>
    /// <example>
    /// Basic usage with async operations:
    /// <code>
    /// async Task&lt;Result&lt;string&gt;&gt; ValidateAsync(int x) =>
    ///     x >= 0 ? Result.Success($"Valid: {x}")
    ///            : Result.Failure&lt;string&gt;(new ResultError("Invalid number"));
    ///
    /// var result = await Result.Success(42)
    ///     .BindAsync(async x => await ValidateAsync(x));
    /// // result is Success("Valid: 42")
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.IsSuccess
            ? func(self.Value)
            : Result.Failure<TOut>(self.Error).AsTaskAsync();

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload handles the case where the input Result is wrapped in a Task, but the binding
    /// function is synchronous. This is useful when you have an asynchronous Result that you want
    /// to transform using a synchronous operation.
    /// </remarks>
    /// <example>
    /// Binding an async result with a synchronous operation:
    /// <code>
    /// async Task&lt;Result&lt;int&gt;&gt; FetchNumberAsync() =>
    ///     await Task.FromResult(Result.Success(42));
    ///
    /// var result = await FetchNumberAsync()
    ///     .BindAsync(x => Result.Success(x * 2));
    /// // result is Success(84)
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Result<TOut>> func)
        => (await self.ConfigureAwait(false)).Bind(func);

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload handles both an asynchronous input Result and an asynchronous binding function.
    /// This is particularly useful when chaining multiple asynchronous operations that each return Results.
    /// </remarks>
    /// <example>
    /// Chaining multiple async operations:
    /// <code>
    /// async Task&lt;Result&lt;int&gt;&gt; FetchAsync(int id) =>
    ///     await Task.FromResult(Result.Success(id + 1));
    ///
    /// var result = await Task.FromResult(Result.Success(5))
    ///     .BindAsync(async x => await FetchAsync(x))
    ///     .BindAsync(async x => await FetchAsync(x));
    /// // result is Success(7)
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Task<Result<TOut>>> func)
    {
        var result = await self.ConfigureAwait(false);
        return result.IsSuccess
            ? await func(result.Value).ConfigureAwait(false)
            : Result.Failure<TOut>(result.Error);
    }

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload converts a synchronous Try operation to an asynchronous Result by applying
    /// an asynchronous Try operation. It combines exception handling with async processing.
    /// </remarks>
    /// <example>
    /// Combining Try with async operations:
    /// <code>
    /// var tryParse = Try.Create((string s) => int.Parse(s));
    /// async Task&lt;Result&lt;string&gt;&gt; FormatAsync(int x) =>
    ///     await Task.FromResult(Result.Success($"Number: {x}"));
    ///
    /// var result = await tryParse
    ///     .BindAsync(async x => await FormatAsync(x));
    /// // For "123": result is Success("Number: 123")
    /// // For "abc": result is Failure(FormatException)
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, TryAsync<TOut>> func)
        => self.Try().BindAsync(input => func(input).TryAsync());

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload converts a synchronous Try operation to an asynchronous Result by applying
    /// an asynchronous Result operation. Use this when you want to combine exception handling
    /// with async Result operations.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.Try().BindAsync(input => func(input));

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload chains two asynchronous Try operations together. Both operations must complete
    /// successfully for the result to be successful. Any exception in either operation will result
    /// in a failure.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, TryAsync<TOut>> func)
        => self.TryAsync().BindAsync(input => func(input).TryAsync());

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload combines an asynchronous Try operation with a synchronous Try operation.
    /// The async operation must complete first, then its result is passed to the synchronous
    /// Try operation.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Try<TOut>> func)
        => self.TryAsync().BindAsync(input => func(input).Try());

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload combines an asynchronous Try operation with a synchronous Result operation.
    /// The async Try must complete successfully before the Result operation is attempted.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Result<TOut>> func)
        => (await self.TryAsync().ConfigureAwait(false)).Bind(func);

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload combines an asynchronous Try operation with an asynchronous Result operation.
    /// The Try operation must complete successfully before the async Result operation is attempted.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.TryAsync().BindAsync(func);
}