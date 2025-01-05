using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

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
    [Pure]
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, TValue value, Error? error)
    {
        switch (isSuccess)
        {
            case true when (error is not null || value is null):
                throw new ArgumentException("Value cannot be null and error should be null when IsSuccess is true.");
            case false when (error is null || value is not null):
                throw new ArgumentException(
                    "Value should be null and error should not be null when IsSuccess is false.");
        }
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
        IsDefault = Value.IsDefault();
    }

    [Pure]
    public static implicit operator Result<TValue>(TValue value) => Success(value);

    [Pure]
    public static implicit operator Result<TValue>(Error error) => Failure(error);

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
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return new Result<TValue>(true, value, null);
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