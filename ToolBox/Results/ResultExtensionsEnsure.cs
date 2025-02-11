using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for validating values within <see cref="Result{TValue}"/> instances using predicates.
/// These methods help enforce business rules and data validation while maintaining the Result pattern.
/// </summary>
/// <remarks>
/// This class contains methods that allow you to:
/// <list type="bullet">
/// <item><description>Validate successful results using synchronous predicates</description></item>
/// <item><description>Validate successful results using asynchronous predicates</description></item>
/// <item><description>Convert valid results to failures when predicates are not satisfied</description></item>
/// </list>
///
/// Key features:
/// <list type="bullet">
/// <item><description>Preserves the original result if it's already a failure</description></item>
/// <item><description>Returns a new failure result with the specified error if validation fails</description></item>
/// <item><description>Supports both synchronous and asynchronous operations</description></item>
/// </list>
/// </remarks>
/// <example>
/// Basic usage:
/// <code><![CDATA[
/// // Validate a number is positive
/// var result = Result.Success(42)
///     .Ensure(x => x > 0, ResultError.New("Number must be positive"));
///
/// // Chain multiple validations
/// var result = Result.Success(userInput)
///     .Ensure(x => !string.IsNullOrEmpty(x), ResultError.New("Input cannot be empty"))
///     .Ensure(x => x.Length >= 3, ResultError.New("Input must be at least 3 characters"));
/// ]]></code>
/// </example>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsEnsure
{
    /// <summary>
    /// Ensures that the value of a successful result satisfies a predicate.
    /// </summary>
    /// <remarks>
    /// If the result is failed, this method returns the original result.
    /// If the result is successful but the predicate is false, this method returns a failed result with the provided error.
    /// Otherwise, this method returns the original result.
    /// </remarks>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="predicate">The predicate to evaluate.</param>
    /// <param name="resultError">The error to include in the result when the predicate is false.</param>
    /// <returns>The result of the predicate evaluation.</returns>
    /// <example>
    /// Basic usage:
    /// <code><![CDATA[
    /// var result = Result.Success(5).Ensure(x => x > 0, ResultError.New("Value must be positive"));
    /// Console.WriteLine(result.IsSuccess); // Output: True
    /// ]]></code>
    ///
    /// Edge case handling:
    /// <code><![CDATA[
    /// var result = Result.Success(-1).Ensure(x => x > 0, ResultError.New("Value must be positive"));
    /// Console.WriteLine(result.IsFailure); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> Ensure<TIn>(this Result<TIn> result, Func<TIn, bool> predicate, ResultError resultError)
        => result.IsFailure || predicate(result.Value) ? result : resultError;


    /// <summary>
    /// Ensures that the value of a successful result satisfies a predicate.
    /// </summary>
    /// <remarks>
    /// If the result is failed, this method returns the original result.
    /// If the result is successful but the predicate is false, this method returns a failed result with the provided error.
    /// Otherwise, this method returns the original result.
    /// </remarks>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="self">The result to check.</param>
    /// <param name="predicate">The predicate to evaluate.</param>
    /// <param name="resultError">The error to include in the result when the predicate is false.</param>
    /// <returns>The result of the predicate evaluation.</returns>
    /// <example>
    /// Basic usage:
    /// <code><![CDATA[
    /// var resultTask = Task.FromResult(Result.Success(5)).EnsureAsync(x => x > 0, ResultError.New("Value must be positive"));
    /// var result = await resultTask;
    /// Console.WriteLine(result.IsSuccess); // Output: True
    /// ]]></code>
    ///
    /// Edge case handling:
    /// <code><![CDATA[
    /// var resultTask = Task.FromResult(Result.Success(-1)).EnsureAsync(x => x > 0, ResultError.New("Value must be positive"));
    /// var result = await resultTask;
    /// Console.WriteLine(result.IsFailure); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> EnsureAsync<TIn>(this Task<Result<TIn>> self,
        Func<TIn, bool> predicate, ResultError resultError)
        => (await self.ConfigureAwait(false)).Ensure(predicate, resultError);

    /// <summary>
    /// Ensures that the value of a successful result satisfies a predicate.
    /// </summary>
    /// <remarks>
    /// If the result is failed, this method returns the original result.
    /// If the result is successful but the predicate is false, this method returns a failed result with the provided error.
    /// Otherwise, this method returns the original result.
    /// </remarks>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="self">The result to check.</param>
    /// <param name="predicate">The predicate to evaluate.</param>
    /// <param name="resultError">The error to include in the result when the predicate is false.</param>
    /// <returns>The result of the predicate evaluation.</returns>
    /// <example>
    /// Basic usage:
    /// <code><![CDATA[
    /// var resultTask = Task.FromResult(Result.Success(5)).EnsureAsync(async x => await Task.FromResult(x > 0), ResultError.New("Value must be positive"));
    /// var result = await resultTask;
    /// Console.WriteLine(result.IsSuccess); // Output: True
    /// ]]></code>
    ///
    /// Edge case handling:
    /// <code><![CDATA[
    /// var resultTask = Task.FromResult(Result.Success(-1)).EnsureAsync(async x => await Task.FromResult(x > 0), ResultError.New("Value must be positive"));
    /// var result = await resultTask;
    /// Console.WriteLine(result.IsFailure); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TIn>> EnsureAsync<TIn>(this Task<Result<TIn>> self,
        Func<TIn, Task<bool>> predicate, ResultError resultError)
    {
        var result = await self.ConfigureAwait(false);
        return result.IsFailure || await predicate(result.Value).ConfigureAwait(false) ? result : resultError;
    }
}