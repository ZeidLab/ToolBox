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
    public readonly bool IsFailure;
    
    private Result(bool isSuccess, TValue value, Error? error = null)
    {
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
        IsFailure = !isSuccess;
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

public delegate Result<TIn> Try<TIn>();

public delegate Task<Result<TIn>> TryAsync<TIn>();