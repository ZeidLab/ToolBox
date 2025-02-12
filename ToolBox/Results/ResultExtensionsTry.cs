using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Represents a delegate that encapsulates a synchronous operation which may either succeed
/// or fail within a try-catch block. This delegate follows the railway-oriented programming (ROP)
/// pattern, providing a structured way to handle both success and failure cases.
/// </summary>
/// <typeparam name="TIn">The type of the value returned by a successful operation.</typeparam>
/// <returns>A <see cref="Result{TIn}"/> representing either the successful outcome with a value,
/// or a failure with an error description.</returns>
/// <example>
/// Basic usage with a simple calculation:
/// <code><![CDATA[
/// Try<int> divide = () => {
///     int dividend = 10;
///     int divisor = 0;
///     if (divisor == 0)
///         return Result.Failure<int>(new DivideByZeroException("Cannot divide by zero"));
///     return Result.Success(dividend / divisor);
/// };
///
/// var result = divide.Try(); // Returns Failure with DivideByZeroException
/// result.Match(
///     success: value => Console.WriteLine($"Result: {value}"),
///     failure: error => Console.WriteLine($"Error: {error.Message}")
/// );
/// ]]></code>
///
/// Handling exceptions properly:
/// <code><![CDATA[
/// Try<int> parseNumber = () => {
///         return Result.Success(int.Parse("invalid"));
/// };
///
/// var result = parseNumber.Try(); // Returns Failure with FormatException
/// ]]></code>
/// </example>
#pragma warning disable CA1716
public delegate Result<TIn> Try<TIn>();
#pragma warning restore CA1716

/// <summary>
/// Represents a delegate that encapsulates an asynchronous operation which may either succeed
/// or fail within a try-catch block. This delegate follows the railway-oriented programming (ROP)
/// pattern, providing a structured way to handle both success and failure cases asynchronously.
/// </summary>
/// <typeparam name="TIn">The type of the value returned by a successful operation.</typeparam>
/// <returns>A <see cref="Task{T}"/> of <see cref="Result{TIn}"/> representing either the successful
/// outcome with a value, or a failure with an error description.</returns>
/// <example>
/// Basic usage with an async operation:
/// <code><![CDATA[
/// TryAsync<string> fetchData = async () => {
///         using var client = new HttpClient();
///         var response = await client.GetStringAsync("https://api.example.com/data");
///         return Result.Success(response);
/// };
///
/// var result = await fetchData.TryAsync(); // Handles both success and exceptions
/// result.Match(
///     success: data => Console.WriteLine($"Data: {data}"),
///     failure: error => Console.WriteLine($"Error: {error.Message}")
/// );
/// ]]></code>
///
/// Converting and chaining sync/async operations:
/// <code><![CDATA[
/// // Synchronous operation
/// Try<int> getValue = () => Result.Success(42);
///
/// // Convert to async and chain
/// var asyncResult = await getValue
///     .ToAsync()
///     .TryAsync();
///
/// asyncResult.Match(
///     success: value => Console.WriteLine($"Value: {value}"),
///     failure: error => Console.WriteLine($"Error: {error.Message}")
/// );
/// ]]></code>
/// </example>
public delegate Task<Result<TIn>> TryAsync<TIn>();

/// <summary>
/// Provides extension methods for working with <see cref="Try{TIn}"/> and <see cref="TryAsync{TIn}"/>
/// delegates in a railway-oriented programming (ROP) style. These methods facilitate error handling
/// and composition of operations that may fail.
/// </summary>
/// <example>
/// Basic usage combining sync and async operations:
/// <code><![CDATA[
/// // Synchronous operation
/// Try<int> getData = () => Result.Success(42);
///
/// // Convert to async and chain operations
/// var result = await getData
///     .ToAsync()
///     .TryAsync();
///
/// Console.WriteLine(result.Match(
///     success: x => $"Result: {x}",     // Outputs: "Result: 84"
///     failure: err => err.ToString()
/// ));
/// ]]></code>
/// </example>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
public static class ResultExtensionsTry
{
    /// <summary>
    /// Converts a synchronous <see cref="Try{TIn}"/> delegate to an asynchronous <see cref="TryAsync{TIn}"/>
    /// delegate. This allows synchronous operations to be used in async workflows.
    /// </summary>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="self">The synchronous operation to convert.</param>
    /// <returns>An asynchronous operation that wraps the original synchronous operation.</returns>
/// <example>
/// Converting and using a synchronous operation in an async context:
/// <code><![CDATA[
/// // Synchronous operation that might fail
/// Try<int> parseNumber = () => {
///         return Result.Success(int.Parse("abc"));
/// };
///
/// // Convert to async and execute safely
/// var asyncOp = parseNumber.ToAsync();
/// var result = await asyncOp.TryAsync();
///
/// result.Match(
///     success: value => Console.WriteLine($"Parsed: {value}"),
///     failure: error => Console.WriteLine($"Error: {error.Message}") // Output: "Error: Input string was not in a correct format."
/// );
/// ]]></code>
///
/// Converting a successful operation:
/// <code><![CDATA[
/// Try<int> getValue = () => Result.Success(42);
///
/// var asyncResult = await getValue
///     .ToAsync()
///     .TryAsync();
///
/// asyncResult.Match(
///     success: value => Console.WriteLine($"Value: {value}"), // Output: "Value: 42"
///     failure: error => Console.WriteLine($"Error: {error.Message}")
/// );
/// ]]></code>
/// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable AMNF0002
    public static TryAsync<TIn> ToAsync<TIn>(this Try<TIn> self)
#pragma warning restore AMNF0002
        => () => self.Try().AsTaskAsync();


    /// <summary>
    /// Invokes the synchronous operation and wraps any thrown exceptions in a failure result.
    /// This method ensures that no exceptions escape the operation, making error handling more
    /// predictable and functional.
    /// </summary>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="self">The operation to execute safely.</param>
    /// <returns>A success result containing the operation's value, or a failure result containing
    /// any thrown exception.</returns>
    /// <example>
    /// Safely executing an operation that might throw:
    /// <code><![CDATA[
    /// Try<int> divideByZero = () => {
    ///     return Result.Success(100 / 0); // Would throw DivideByZeroException
    /// };
    ///
    /// var result = divideByZero.Try();
    /// Console.WriteLine(result.IsFailure); // Outputs: True
    /// Console.WriteLine(result.Error is DivideByZeroException); // Outputs: True
    /// ]]></code>
    /// </example>
    [Pure]
    public static Result<TIn> Try<TIn>(this Try<TIn> self)
    {
        try
        {
            return self.Invoke();
        }
        catch (Exception ex)
        {
            return Result.Failure<TIn>(ex);
        }
    }

    /// <summary>
    /// Invokes the asynchronous operation and wraps any thrown exceptions in a failure result.
    /// This method ensures that no exceptions escape the operation, making error handling more
    /// predictable and functional in async workflows.
    /// </summary>
    /// <typeparam name="TIn">The type of the value within the result.</typeparam>
    /// <param name="self">The async operation to execute safely.</param>
    /// <returns>A task that resolves to either a success result containing the operation's value,
    /// or a failure result containing any thrown exception.</returns>
    /// <example>
    /// Safely executing an async operation that might throw:
    /// <code><![CDATA[
    /// TryAsync<string> fetchData = async () => {
    ///     using var client = new HttpClient();
    ///     var data = await client.GetStringAsync("https://invalid-url");
    ///     return Result.Success(data);
    /// };
    ///
    /// var result = await fetchData.TryAsync();
    /// if (result.IsFailure)
    ///     Console.WriteLine("Failed to fetch data");
    /// ]]></code>
    /// </example>
    [Pure]
    public static async Task<Result<TIn>> TryAsync<TIn>(this TryAsync<TIn> self)
    {
        try
        {
            return await self().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result.Failure<TIn>(ex);
        }
    }
}