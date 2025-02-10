using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Represents the result of an operation that may either succeed or fail. This type is a fundamental
/// building block in railway-oriented programming (ROP), where operations are modeled as a series of
/// steps that either continue on the "happy path" (success) or diverge to an "error track" (failure).
///
/// The <see cref="Result{TValue}"/> type encapsulates either a successful value of type <typeparamref name="TValue"/>
/// or an <see cref="Error"/> representing the failure. It provides a robust and predictable way to handle
/// errors without relying on exceptions for control flow.
///
/// Key Features:
/// - Immutable and thread-safe: Once created, a <see cref="Result{TValue}"/> instance cannot be modified.
/// - Value semantics: As a record struct, it is lightweight and copied by value.
/// - Success/Failure states: The <see cref="IsSuccess"/> and <see cref="IsFailure"/> properties
///   clearly indicate the outcome of the operation.
/// - Implicit conversions: Supports implicit conversions from <typeparamref name="TValue"/>, <see cref="Error"/>,
///   and <see cref="Exception"/> to simplify usage.
/// - Factory methods: Provides <see cref="Result.Success{TValue}(TValue)"/> and <see cref="Result.Failure{TValue}(ResultError)"/>
///   methods for creating instances with validation.
/// - Default value detection: The <see cref="IsDefault"/> property indicates whether the <see cref="Value"/>
///   is the default value for <typeparamref name="TValue"/>.
///
/// Example Usage:
/// <code>
/// // Successful result
/// Result&lt;int&gt; successResult = Result&lt;int&gt;.Success(42);
///
/// // Failed result with an error message
/// Result&lt;int&gt; failureResult = Result&lt;int&gt;.Failure(Error.New("Operation failed."));
///
/// // Failed result from an exception
/// Result&lt;int&gt; exceptionResult = new InvalidOperationException("Invalid operation");
///
/// // Check the result state
/// if (successResult.IsSuccess)
/// {
///     Console.WriteLine($"Success: {successResult.Value}");
/// }
/// else
/// {
///     Console.WriteLine($"Failure: {successResult.Error.Message}");
/// }
/// </code>
///
/// This type is designed to promote explicit and predictable error handling, making it easier to reason
/// about the flow of operations in functional-style C# applications.
/// </summary>
/// <typeparam name="TValue">The type of the value returned by a successful operation.</typeparam>
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
    /// or <see cref="Result.Failure{TValue}(ResultError)"/> instead. Using the public constructor will throw an exception.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown when the constructor is called directly.</exception>
#pragma warning disable S1133
    [Obsolete("Use factory methods like Result.Success or Result.Failure instead. Any instance of public constructor will be considered empty.", true)]
#pragma warning restore S1133
    public Result() => throw new InvalidOperationException("Use factory methods like Result.Success or Result.Failure instead.");

    /// <summary>
    /// Implicitly converts a value to a success of <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A successful <see cref="Result{TValue}"/> instance.</returns>
    [Pure]
    public static implicit operator Result<TValue>(TValue value) => Result.Success(value);

    /// <summary>
    /// Implicitly converts an error to a failure of <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="resultError">The error to convert.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> instance.</returns>
    [Pure]
    public static implicit operator Result<TValue>(ResultError resultError) => Result.Failure<TValue>(resultError);

    /// <summary>
    /// Implicitly converts an exception to a failure of <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="error">The exception to convert.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> instance.</returns>
    [Pure]
    public static implicit operator Result<TValue>(Exception error) => Result.Failure<TValue>(error);
}

/// <summary>
/// Static helper class for creating <see cref="Result{TValue}"/> instances.
/// </summary>
public static class Result
{
    /// <summary>
    /// Returns a successful result with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A successful <see cref="Result{TValue}"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Success<TValue>(TValue value)
    {
        Guards.ThrowIfNull(value, nameof(value));
        return new Result<TValue>(true, value);
    }

    /// <summary>
    /// Returns a failed result with the specified error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="resultError">The error to include in the result.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> instance.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Failure<TValue>(ResultError resultError) => new(false, default!, resultError);

    /// <summary>
    /// Returns a failed result for the given exception.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">The exception to convert to a failure result.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> instance.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> FromException<TValue>(Exception error) => Failure<TValue>(ResultError.New(error));
}