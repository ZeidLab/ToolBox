using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for binding operations on <see cref="Result{TValue}"/> objects.
/// These methods enable monadic composition of Result types, allowing for clean error handling
/// and functional programming patterns.
/// </summary>
/// <remarks>
/// <para>
/// Binding operations allow you to chain multiple operations that return Results while
/// maintaining proper error propagation.
/// </para>
/// <example>
/// Basic usage with successful results:
/// <code>
/// var result = Result.Success(5)
///     .Bind(x => Result.Success(x * 2))
///     .Bind(x => Result.Success(x + 1));
/// // result is Success(11)
/// </code>
/// </example>
/// <example>
/// Error propagation:
/// <code>
/// var result = Result.Success(5)
///     .Bind(x => Result.Failure&lt;int&gt;(new ResultError("Operation failed")))
///     .Bind(x => Result.Success(x * 2));
/// // result is Failure("Operation failed")
/// </code>
/// </example>
/// </remarks>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsBind
{
    /// <summary>
    /// Binds the value of a successful result to a new result by applying the specified function.
    /// If the input result is a failure, the error is propagated to the output result.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the input result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the output result.</typeparam>
    /// <param name="self">The result to bind.</param>
    /// <param name="func">The function to apply to the value if the result is successful.</param>
    /// <returns>
    /// A new result that will be:
    /// <list type="bullet">
    /// <item><description>The result of applying <paramref name="func"/> to the value if the input result is successful</description></item>
    /// <item><description>A failure containing the original error if the input result is a failure</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// Basic usage:
    /// <code>
    /// Result&lt;int&gt; Divide(int x, int y) =>
    ///     y == 0 ? Result.Failure&lt;int&gt;(new ResultError("Division by zero"))
    ///            : Result.Success(x / y);
    ///
    /// var result = Result.Success(10)
    ///     .Bind(x => Divide(x, 2))
    ///     .Bind(x => Divide(x, 0));
    /// // result is Failure("Division by zero")
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> self, Func<TIn, Result<TOut>> func)
        => self.IsSuccess
            ? func(self.Value)
            : Result.Failure<TOut>(self.Error);

    /// <summary>
    /// Binds the value of a successful <see cref="Try{TIn}"/> to a new result by applying the specified function.
    /// If the Try operation fails, the exception is converted to a ResultError.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the input Try.</typeparam>
    /// <typeparam name="TOut">The type of the value in the output Result.</typeparam>
    /// <param name="self">The Try operation to bind.</param>
    /// <param name="func">The function to apply to the value if the Try operation succeeds.</param>
    /// <returns>
    /// A new Result that will be:
    /// <list type="bullet">
    /// <item><description>The result of applying <paramref name="func"/> to the value if the Try operation succeeds</description></item>
    /// <item><description>A failure containing the exception as a ResultError if the Try operation fails</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// Basic usage with exception handling:
    /// <code>
    /// var tryParse = Try.Create((string s) => int.Parse(s));
    /// var result = tryParse
    ///     .Bind(x => Result.Success(x * 2));
    /// // For "123": result is Success(246)
    /// // For "abc": result is Failure(FormatException)
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Result<TOut>> func)
        => self.Try().Bind(func);

    /// <summary>
    /// Binds the value of a successful <see cref="Try{TIn}"/> to a new result by applying another Try operation.
    /// Both Try operations must succeed for the result to be successful.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the input Try.</typeparam>
    /// <typeparam name="TOut">The type of the value in the output Result.</typeparam>
    /// <param name="self">The first Try operation to bind.</param>
    /// <param name="func">The function returning a second Try operation to apply if the first succeeds.</param>
    /// <returns>
    /// A new Result that will be:
    /// <list type="bullet">
    /// <item><description>Successful if both Try operations succeed</description></item>
    /// <item><description>A failure containing the first encountered exception as a ResultError</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// Chaining multiple Try operations:
    /// <code>
    /// var tryParse = Try.Create((string s) => int.Parse(s));
    /// var tryDivide = Try.Create((int x) => 100 / x);
    ///
    /// var result = tryParse
    ///     .Bind(x => tryDivide);
    /// // For "10": result is Success(10)
    /// // For "0": result is Failure(DivideByZeroException)
    /// // For "abc": result is Failure(FormatException)
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Try<TOut>> func)
        => self.Try().Bind(input => func(input).Try());
}