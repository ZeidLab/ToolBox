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
/// - Value semantics: As a <see><cref>record struct</cref></see> , it is lightweight and copied by value.
/// - Success/Failure states: The <see cref="IsSuccess"/> and <see cref="IsFailure"/> properties
///   clearly indicate the outcome of the operation.
/// - Implicit conversions: Supports implicit conversions from <typeparamref name="TValue"/>, <see cref="Error"/>,
///   and <see cref="Exception"/> to simplify usage.
/// - Factory methods: Provides <see cref="Success(TValue)"/> and <see><cref>Failure(Error)</cref></see>
/// methods
///   for creating instances with validation.
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
public readonly record struct Result<TValue>
{
    internal readonly TValue Value;
    internal readonly Error? Error;

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public readonly bool IsSuccess;

    /// <summary>
    /// Gets a value indicating whether the Value is default.
    /// </summary>
    public readonly bool IsDefault;

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    public readonly bool IsFailure;
    
    private Result(bool isSuccess, TValue value, Error? error = null)
    {
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
        IsFailure = !isSuccess;
        IsDefault = Value.IsDefault();
    }

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// <returns>It will throw an <exception cref="InvalidOperationException"></exception></returns>
    /// </summary>
    /// <remarks>
    /// Use factory methods like <see cref="Success(TValue)"/>
    /// or <see cref="Failure(Results.Error)"/> instead. Using public constructor will throw exception.
    /// </remarks>
    [Obsolete("Use factory methods like Result<TIn>.Success or Result<TIn>.Failure instead. Any instance of public constructor will be considered empty.",true)]
    public Result() => throw new InvalidOperationException("Use factory methods like Result<TIn>.Success or Result<TIn>.Failure instead.");


    /// <summary>
    /// Implicitly converts a value to a success of <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">specified value</param>
    /// <returns>returns an immutable record struct of <see cref="Result{TValue}"/></returns>
    [Pure]
    public static implicit operator Result<TValue>(TValue value) => Success(value);


    /// <summary>
    /// Implicitly converts an error to a failure of <see cref="Result{TValue}"/>
    /// </summary>
    /// <param name="error">specified error</param>
    /// <returns>returns an immutable record struct of <see cref="Result{TValue}"/> with error</returns>
    [Pure]
    public static implicit operator Result<TValue>(Error error) => Failure(error);

  
    /// <summary>
    /// Implicitly converts an exception to a failure of <see cref="Result{TValue}"/>
    /// </summary>
    /// <param name="error">specified exception</param>
    /// <returns>returns an immutable record struct of <see cref="Result{TValue}"/> with error including exception</returns>
    [Pure]
    public static implicit operator Result<TValue>(Exception error) => Failure(error);

    /// <summary>
    /// Returns a successful result with a value.
    /// </summary>
    /// <param name="value">The value to include in the result.</param>
    /// <returns>A successful <see cref="Result{TValue}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Success(TValue value)
    {
        Guards.ThrowIfNull(value, nameof(value));
        return new Result<TValue>(true, value);
    }

    /// <summary>
    /// Returns a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error to include in the result.</param>
    /// <returns>A failed <see cref="Result{TValue}"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CS8604 // Possible null reference argument.
    public static Result<TValue> Failure(Error error) => new(false, default, error);
#pragma warning restore CS8604 // Possible null reference argument.
}

