using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for <see cref="Result{TValue}"/> to enhance its functionality
/// with conversions and transformations.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Creates a successful <see cref="Result{TIn}"/> instance containing the provided value.
    /// </summary>
    /// <typeparam name="TIn">The type of the value to be wrapped in the Result.</typeparam>
    /// <param name="self">The value to convert into a successful Result.</param>
    /// <returns>
    /// A new instance of <see cref="Result{TIn}"/> in a success state containing
    /// the provided value of type <typeparamref name="TIn"/>.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> ToSuccess<TIn>(this TIn self)
        => Result.Success(self);

    /// <summary>
    /// Creates a failed <see cref="Result{TIn}"/> instance containing the provided error.
    /// </summary>
    /// <typeparam name="TIn">The type that would have been contained in a successful result.</typeparam>
    /// <param name="self">The error to be wrapped in the failed Result.</param>
    /// <returns>
    /// A new instance of <see cref="Result{TIn}"/> in a failure state containing
    /// the provided <see cref="ResultError"/>.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> ToFailure<TIn>(this ResultError self)
        => Result.Failure<TIn>(self);

    /// <summary>
    /// Creates a failed <see cref="Result{TIn}"/> instance from the provided exception.
    /// </summary>
    /// <typeparam name="TIn">The type that would have been contained in a successful result.</typeparam>
    /// <param name="self">The exception to be converted into a ResultError and wrapped in the failed Result.</param>
    /// <returns>
    /// A new instance of <see cref="Result{TIn}"/> in a failure state containing
    /// a <see cref="ResultError"/> created from the provided <see cref="Exception"/>.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> ToFailure<TIn>(this Exception self)
        => Result.Failure<TIn>(ResultError.New(self));

    /// <summary>
    /// Converts a <see cref="Result{TIn}"/> to a <see cref="Maybe{TIn}"/> instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the Result.</typeparam>
    /// <param name="self">The Result instance to convert.</param>
    /// <returns>
    /// A new <see cref="Maybe{TIn}"/> instance that will be:
    /// <list type="bullet">
    /// <item><description>Some(<typeparamref name="TIn"/>) if the Result is successful</description></item>
    /// <item><description>None if the Result is a failure</description></item>
    /// </list>
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> ToMaybe<TIn>(this Result<TIn> self)
        => self.IsSuccess
            ? Maybe.Some(self.Value)
            : Maybe.None<TIn>();

    /// <summary>
    /// Converts a <see cref="Try{TIn}"/> to a <see cref="Maybe{TIn}"/> instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the value that might be produced by the Try operation.</typeparam>
    /// <param name="self">The Try instance to convert.</param>
    /// <returns>
    /// A new <see cref="Maybe{TIn}"/> instance that will be:
    /// <list type="bullet">
    /// <item><description>Some(<typeparamref name="TIn"/>) if the Try operation succeeds</description></item>
    /// <item><description>None if the Try operation fails</description></item>
    /// </list>
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> ToMaybe<TIn>(this Try<TIn> self)
        => self.Try() is {IsSuccess:true} result
            ? Maybe.Some(result.Value)
            : Maybe.None<TIn>();

    /// <summary>
    /// Asynchronously converts a <see cref="Result{TIn}"/> to a <see cref="Maybe{TIn}"/> instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the Result.</typeparam>
    /// <param name="self">The Task containing the Result instance to convert.</param>
    /// <returns>
    /// A Task that resolves to a new <see cref="Maybe{TIn}"/> instance that will be:
    /// <list type="bullet">
    /// <item><description>Some(<typeparamref name="TIn"/>) if the Result is successful</description></item>
    /// <item><description>None if the Result is a failure</description></item>
    /// </list>
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Maybe<TIn>> ToMaybeAsync<TIn>(this Task<Result<TIn>> self)
#pragma warning disable CA1062
        => (await self.ConfigureAwait(false)) is { IsSuccess: true } result
#pragma warning restore CA1062
            ? Maybe.Some<TIn>(result.Value)
            : Maybe.None<TIn>();

    /// <summary>
    /// Asynchronously converts a <see cref="TryAsync{TIn}"/> to a <see cref="Maybe{TIn}"/> instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the value that might be produced by the asynchronous Try operation.</typeparam>
    /// <param name="self">The TryAsync instance to convert.</param>
    /// <returns>
    /// A Task that resolves to a new <see cref="Maybe{TIn}"/> instance that will be:
    /// <list type="bullet">
    /// <item><description>Some(<typeparamref name="TIn"/>) if the Try operation succeeds</description></item>
    /// <item><description>None if the Try operation fails</description></item>
    /// </list>
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Maybe<TIn>> ToMaybeAsync<TIn>(this TryAsync<TIn> self)
        => (await self.TryAsync().ConfigureAwait(false)) is { IsSuccess: true } result
            ? Maybe.Some(result.Value)
            : Maybe.None<TIn>();

    /// <summary>
    /// Converts a <see cref="Result{TIn}"/> to a <see cref="Result{Unit}"/> instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the original Result.</typeparam>
    /// <param name="self">The Result instance to convert.</param>
    /// <returns>
    /// A new <see cref="Result{Unit}"/> instance that will be:
    /// <list type="bullet">
    /// <item><description>Success(<see cref="Unit"/>) if the original Result is successful</description></item>
    /// <item><description>Failure with the same error if the original Result is a failure</description></item>
    /// </list>
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<Unit> ToUnitResult<TIn>(this Result<TIn> self)
        => self.IsSuccess
            ? Result.Success(Unit.Default)
            : Result.Failure<Unit>(self.Error);

    /// <summary>
    /// Asynchronously converts a <see cref="Result{TIn}"/> to a <see cref="Result{Unit}"/> instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the original Result.</typeparam>
    /// <param name="self">The Task containing the Result instance to convert.</param>
    /// <returns>
    /// A Task that resolves to a new <see cref="Result{Unit}"/> instance that will be:
    /// <list type="bullet">
    /// <item><description>Success(<see cref="Unit"/>) if the original Result is successful</description></item>
    /// <item><description>Failure with the same error if the original Result is a failure</description></item>
    /// </list>
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<Unit>> ToUnitResultAsync<TIn>(this Task<Result<TIn>> self)
    {
#pragma warning disable CA1062
        var result = await self.ConfigureAwait(false);
#pragma warning restore CA1062
        return result.IsSuccess
            ? Result.Success(Unit.Default)
            : Result.Failure<Unit>(result.Error);
    }
}