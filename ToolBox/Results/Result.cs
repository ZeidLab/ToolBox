using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Represents the result of an operation that may or may not be successful.
/// <para>
/// This type is commonly used to represent the result of an operation that may
/// or may not be successful, such as a database query or a web service call.
/// </para>
/// <typeparam name="TValue">The value type of the result.</typeparam>
/// </summary>
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
    /// <typeparam name="TValue">The type of the value.</typeparam>
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
    public static Result<TValue> Failure(Error error) => new(false, default!, error);
}

