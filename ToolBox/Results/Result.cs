using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Represents the result of an operation that may either succeed or fail, providing a fundamental
/// building block in railway-oriented programming (ROP).
/// </summary>
/// <typeparam name="TValue">The type of the value returned by a successful operation.</typeparam>
/// <remarks>
/// This type encapsulates either a successful value or an error, providing a robust way to handle
/// failures without relying on exceptions for control flow.
/// Key features include:
/// <list type="bullet">
/// <item><description>Immutable and thread-safe design</description></item>
/// <item><description>Value semantics (lightweight record struct)</description></item>
/// <item><description>Clear success/failure state indicators</description></item>
/// <item><description>Implicit conversions from value, error, and exception types</description></item>
/// </list>
/// </remarks>
/// <example>
/// Basic success/failure handling:
/// <code><![CDATA[
/// // Create a successful result
/// Result<int> success = Result.Success(42);
/// Console.WriteLine(success.IsSuccess); // true
/// 
/// // Create a failure result
/// Result<int> failure = ResultError.New("Operation failed");
/// Console.WriteLine(failure.IsFailure); // true
/// 
/// // Handle exceptions implicitly
/// Result<int> exceptionResult = new InvalidOperationException("Invalid operation");
/// Console.WriteLine(exceptionResult.IsFailure); // true
/// 
/// // Pattern matching example
/// var message = success.Match(
///     success: value => $"Success: {value}",
///     failure: error => $"Error: {error.Message}"
/// );
/// Console.WriteLine(message); // "Success: 42"
/// ]]></code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
public readonly record struct Result<TValue>
{
    internal readonly TValue Value;
    internal readonly ResultError Error;

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <value>
    /// <c>true</c> if the operation succeeded; otherwise, <c>false</c>.
    /// </value>
    public readonly bool IsSuccess;

    /// <summary>
    /// Gets a value indicating whether the <see cref="Value"/> is the default value for <typeparamref name="TValue"/>.
    /// </summary>
    /// <value>
    /// <c>true</c> if the <see cref="Value"/> is the default value for <typeparamref name="TValue"/>; otherwise, <c>false</c>.
    /// </value>
    public readonly bool IsDefault;

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    /// <value>
    /// <c>true</c> if the operation failed; otherwise, <c>false</c>.
    /// </value>
    public readonly bool IsFailure;

    internal Result(bool isSuccess, TValue value, ResultError error = default)
    {
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
        IsFailure = !isSuccess;
        IsDefault = Value.IsDefault();
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    /// <remarks>
    /// Use factory methods like <see cref="Result.Success{TValue}(TValue)"/>
    /// or <see cref="Result.Failure{TValue}(ResultError)"/> instead.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown when the constructor is called directly.</exception>
#pragma warning disable S1133
    [Obsolete("Use factory methods like Result.Success or Result.Failure instead. Any instance of public constructor will be considered empty.", true)]
#pragma warning restore S1133
    public Result() => throw new InvalidOperationException("Use factory methods like Result.Success or Result.Failure instead.");

    /// <summary>
    /// Implicitly converts a value to a successful <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A successful result containing the specified value.</returns>
    /// <example>
    /// <code><![CDATA[
    /// Result<int> result = 42; // Implicit conversion to successful result
    /// Console.WriteLine(result.IsSuccess); // true
    /// ]]></code>
    /// </example>
    [Pure]
    public static implicit operator Result<TValue>(TValue value) => Result.Success(value);

    /// <summary>
    /// Implicitly converts an error to a failed <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="resultError">The error to convert.</param>
    /// <returns>A failed result containing the specified error.</returns>
    /// <example>
    /// <code><![CDATA[
    /// var error = ResultError.New("Operation failed");
    /// Result<int> result = error; // Implicit conversion to failed result
    /// Console.WriteLine(result.IsFailure); // true
    /// ]]></code>
    /// </example>
    [Pure]
    public static implicit operator Result<TValue>(ResultError resultError) => Result.Failure<TValue>(resultError);

    /// <summary>
    /// Implicitly converts an exception to a failed <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="error">The exception to convert.</param>
    /// <returns>A failed result containing the exception as an error.</returns>
    /// <example>
    /// <code><![CDATA[
    /// Result<int> result = new InvalidOperationException("Invalid operation");
    /// Console.WriteLine(result.IsFailure); // true
    /// ]]></code>
    /// </example>
    [Pure]
    public static implicit operator Result<TValue>(Exception error) => Result.Failure<TValue>(error);
}

/// <summary>
/// Provides factory methods for creating <see cref="Result{TValue}"/> instances.
/// </summary>
/// <example>
/// Basic usage:
/// <code><![CDATA[
/// // Create a successful result
/// var success = Result.Success(42);
/// 
/// // Create a failure result
/// var failure = Result.Failure<int>(ResultError.New("Operation failed"));
/// 
/// // Create a failure from an exception
/// var exceptionResult = Result.FromException<int>(new InvalidOperationException());
/// ]]></code>
/// </example>
public static class Result
{
    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A successful result containing the specified value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code><![CDATA[
    /// var result = Result.Success(42);
    /// Console.WriteLine(result.IsSuccess); // true
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Success<TValue>(TValue value)
    {
        Guards.ThrowIfNull(value, nameof(value));
        return new Result<TValue>(true, value);
    }

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would have been returned on success.</typeparam>
    /// <param name="resultError">The error information.</param>
    /// <returns>A failed result containing the specified error.</returns>
    /// <example>
    /// <code><![CDATA[
    /// var result = Result.Failure<int>(ResultError.New("Operation failed"));
    /// Console.WriteLine(result.IsFailure); // true
    /// Console.WriteLine(result.Error.Message); // "Operation failed"
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Failure<TValue>(ResultError resultError) => new(false, default!, resultError);

    /// <summary>
    /// Creates a failed result from an exception.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would have been returned on success.</typeparam>
    /// <param name="error">The exception to convert to a failure result.</param>
    /// <returns>A failed result containing the exception information as an error.</returns>
    /// <example>
    /// <code><![CDATA[
    /// var result = Result.FromException<int>(new InvalidOperationException("Invalid operation"));
    /// Console.WriteLine(result.IsFailure); // true
    /// Console.WriteLine(result.Error.Message); // "Invalid operation"
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> FromException<TValue>(Exception error) => Failure<TValue>(ResultError.New(error));
}