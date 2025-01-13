using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// extension class for <see cref="Result{TValue}"/>
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts the provided object to a successful Result instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the object.</typeparam>
    /// <param name="self">The object to convert.</param>
    /// <returns>A successful Result instance containing the provided object.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> ToSuccess<TIn>(this TIn self)
        => Result<TIn>.Success(self);

    /// <summary>
    /// Converts the provided error to a failed Result instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the value that would have been contained in the successful result.</typeparam>
    /// <param name="self">The error to convert.</param>
    /// <returns>A failed Result instance containing the provided error.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> ToFailure<TIn>(this ResultError self)
        => Result<TIn>.Failure(self);

    /// <summary>
    /// Converts the provided exception to a failed Result instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the value that would have been contained in the successful result.</typeparam>
    /// <param name="self">The exception to convert.</param>
    /// <returns>A failed Result instance containing the provided exception as an error.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> ToFailure<TIn>(this Exception self)
        => Result<TIn>.Failure(ResultError.New(self));

    /// <summary>
    /// Converts a Result instance to a Maybe instance.
    /// </summary>
    /// <remarks>
    /// If the result is successful, the value of the result is returned as a <see cref="Maybe{TIn}.Some"/> instance.
    /// If the result is failed, a <see cref="Maybe{TIn}.None"/> instance is returned.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> ToMaybe<TIn>(this Result<TIn> self)
        => self.IsSuccess
            ? Maybe<TIn>.Some(self.Value)
            : Maybe<TIn>.None();

    /// <summary>
    /// Converts a <see cref="Try{TIn}"/> instance to a <see cref="Maybe{TIn}"/> instance.
    /// </summary>
    /// <remarks>
    /// If the try is successful, the value of the try is returned as a <see cref="Maybe{TIn}.Some"/> instance.
    /// If the try is failed, a <see cref="Maybe{TIn}.None"/> instance is returned.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> ToMaybe<TIn>(this Try<TIn> self)
        => self.Try() is {IsSuccess:true} result
            ? Maybe<TIn>.Some(result.Value)
            : Maybe<TIn>.None();

    /// <summary>
    /// Converts a Result instance to a Maybe instance asynchronously.
    /// </summary>
    /// <remarks>
    /// If the result is successful, the value of the result is returned as a <see cref="Maybe{TIn}.Some"/> instance.
    /// If the result is failed, a <see cref="Maybe{TIn}.None"/> instance is returned.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Maybe<TIn>> ToMaybeAsync<TIn>(this Task<Result<TIn>> self)
#pragma warning disable CA1062
        => (await self.ConfigureAwait(false)) is { IsSuccess: true } result
#pragma warning restore CA1062
            ? Maybe<TIn>.Some(result.Value)
            : Maybe<TIn>.None();

    /// <summary>
    /// Converts a <see cref="TryAsync{TIn}"/> instance to a <see cref="Maybe{TIn}"/> instance asynchronously.
    /// </summary>
    /// <remarks>
    /// If the try is successful, the value of the try is returned as a <see cref="Maybe{TIn}.Some"/> instance.
    /// If the try is failed, a <see cref="Maybe{TIn}.None"/> instance is returned.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Maybe<TIn>> ToMaybeAsync<TIn>(this TryAsync<TIn> self)
        => (await self.TryAsync().ConfigureAwait(false)) is { IsSuccess: true } result
            ? Maybe<TIn>.Some(result.Value)
            : Maybe<TIn>.None();

    /// <summary>
    /// Converts a Result instance to a Result{Unit} instance which in fact is a void result containing possible error.
    /// </summary>
    /// <remarks>
    /// If the result is successful, the value of the result is returned as a <see cref="Result{Unit}.Success"/> instance.
    /// If the result is failed, a <see><cref>Result{Unit}.failure</cref></see>
    /// instance is returned.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<Unit> ToUnitResult<TIn>(this Result<TIn> self)
        => self.IsSuccess
            ? Result<Unit>.Success(Unit.Default)
            : Result<Unit>.Failure(self.ResultError);

    /// <summary>
    /// Converts a Result instance to a Result{Unit} instance which in fact is a void result containing possible error.
    /// </summary>
    /// <remarks>
    /// If the result is successful, the value of the result is returned as a <see cref="Result{Unit}.Success"/> instance.
    /// If the result is failed, a <see><cref>Result{Unit}.failure</cref></see>
    /// instance is returned.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<Unit>> ToUnitResultAsync<TIn>(this Task<Result<TIn>> self)
    {
#pragma warning disable CA1062
        var result = await self.ConfigureAwait(false);
#pragma warning restore CA1062
        return result.IsSuccess
            ? Result<Unit>.Success(Unit.Default)
            : Result<Unit>.Failure(result.ResultError);
    }
}