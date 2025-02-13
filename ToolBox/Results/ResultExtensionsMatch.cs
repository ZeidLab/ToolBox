using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for pattern matching operations on <see cref="Result{TValue}"/> and <see cref="Try{TValue}"/> types.
/// These methods enable railway-oriented programming by handling both success and failure cases explicitly.
/// </summary>
/// <remarks>
/// Pattern matching is a core concept in railway-oriented programming that ensures all possible outcomes
/// are handled appropriately. These methods force developers to consider both success and failure paths.
/// </remarks>
/// <example>
/// Basic result matching:
/// <code><![CDATA[
/// var result = Result.Success<int>(42);
/// var matched = result.Match(
///     success: value => $"Got {value}",
///     failure: error => $"Error: {error.Message}"
/// ); // matched = "Got 42"
/// ]]></code>
///
/// Chaining with Try:
/// <code><![CDATA[
/// var tryResult = new Try<int>(() => int.Parse("xyz"));
/// var handled = tryResult.Match(
///     success: x => Result.Success($"Parsed {x}"),
///     failure: err => Result.Success("Failed to parse")
/// ); // handled = Result.Success("Failed to parse")
/// ]]></code>
/// </example>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsMatch
{


    /// <summary>
    /// Matches a <see cref="Result{TIn}"/> to a new <see cref="Result{TOut}"/> by applying
    /// appropriate transformation functions based on the result's state.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the source result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the target result.</typeparam>
    /// <param name="self">The source result to match against.</param>
    /// <param name="success">Function to transform the value if the result is successful.</param>
    /// <param name="failure">Function to transform the error if the result is a failure.</param>
    /// <returns>A new result containing the transformed value or error.</returns>
    /// <example>
    /// Using Match to transform results:
    /// <code><![CDATA[
    /// var result = Result.Success<int>(42);
    /// var transformed = result.Match(
    ///     success: x => Result.Success(x * 2),
    ///     failure: err => Result.Failure<int>(err)
    /// ); // transformed = Result.Success(84)
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Match<TIn, TOut>(this Result<TIn> self, Func<TIn, Result<TOut>> success,
        Func<ResultError, Result<TOut>> failure)
        => self.IsSuccess ? success(self.Value) : failure(self.Error);

    /// <summary>
    /// Matches a <see cref="Result{TIn}"/> to a value of type <typeparamref name="TOut"/> by applying
    /// transformation functions based on the result's state.
    /// </summary>
    /// <inheritdoc cref="Match{TIn, TOut}(Result{TIn}, Func{TIn, Result{TOut}}, Func{ResultError, Result{TOut}})"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOut Match<TIn, TOut>(this Result<TIn> self, Func<TIn, TOut> success,
        Func<ResultError, TOut> failure)
        => self.IsSuccess ? success(self.Value) : failure(self.Error);

    /// <summary>
    /// Matches a <see cref="Try{TIn}"/> operation to a new <see cref="Result{TOut}"/> by applying
    /// transformation functions based on the operation's outcome.
    /// </summary>
    /// <inheritdoc cref="Match{TIn, TOut}(Result{TIn}, Func{TIn, Result{TOut}}, Func{ResultError, Result{TOut}})"/>
    /// <remarks>
    /// This overload automatically executes the <see cref="Try{TIn}"/> operation and matches its result.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Match<TIn, TOut>(this Try<TIn> self, Func<TIn, Result<TOut>> success,
        Func<ResultError, Result<TOut>> failure)
        => self.Try().Match(success, failure);

    /// <summary>
    /// Matches a <see cref="Try{TIn}"/> operation to a value of type <typeparamref name="TOut"/> by applying
    /// transformation functions based on the operation's outcome.
    /// </summary>
    /// <inheritdoc cref="Match{TIn, TOut}(Result{TIn}, Func{TIn, TOut}, Func{ResultError, TOut})"/>
    /// <remarks>
    /// This overload automatically executes the <see cref="Try{TIn}"/> operation and matches its result.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOut Match<TIn, TOut>(this Try<TIn> self, Func<TIn, TOut> success, Func<ResultError, TOut> failure)
        => self.Try().Match(success, failure);

    /// <summary>
    /// Performs side effects based on the state of a <see cref="Result{TIn}"/> by executing
    /// appropriate actions for success or failure cases.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the result.</typeparam>
    /// <param name="self">The result to match against.</param>
    /// <param name="success">Action to execute if the result is successful.</param>
    /// <param name="failure">Action to execute if the result is a failure.</param>
    /// <example>
    /// Handling side effects:
    /// <code><![CDATA[
    /// var result = Result.Success<int>(42);
    /// result.Match(
    ///     success: value => Console.WriteLine($"Got value: {value}"),
    ///     failure: error => Console.WriteLine($"Error: {error.Message}")
    /// ); // Prints: "Got value: 42"
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Match<TIn>(this Result<TIn> self, Action<TIn> success, Action<ResultError> failure)
    {
        if (self.IsSuccess)
            success(self.Value);
        else
            failure(self.Error);
    }

    /// <summary>
    /// Performs side effects based on the outcome of a <see cref="Try{TIn}"/> operation by executing
    /// appropriate actions for success or failure cases.
    /// </summary>
    /// <inheritdoc cref="Match{TIn}(Result{TIn}, Action{TIn}, Action{ResultError})"/>
    /// <remarks>
    /// This overload automatically executes the <see cref="Try{TIn}"/> operation and matches its result.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Match<TIn>(this Try<TIn> self, Action<TIn> success, Action<ResultError> failure)
    {
        var result = self.Try();
        if (result.IsSuccess)
            success(result.Value);
        else
            failure(result.Error);
    }

}