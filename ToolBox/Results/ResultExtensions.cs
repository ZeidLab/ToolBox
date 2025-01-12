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
    public static Result<TIn> ToFailure<TIn>(this Error self)
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
        => Result<TIn>.Failure(Error.New(self));

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
            ? Maybe<TIn>.Some(self.Value!)
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
            : Result<Unit>.Failure(self.Error.GetValueOrDefault());

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
        var result = await self;
        return result.IsSuccess
            ? Result<Unit>.Success(Unit.Default)
            : Result<Unit>.Failure(result.Error.GetValueOrDefault());
    }
}