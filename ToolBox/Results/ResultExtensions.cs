using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for <see cref="Result{TValue}"/> to enhance its functionality
/// with conversions and transformations between different monadic types and result states.
/// </summary>
/// <remarks>
/// This class provides a rich set of extension methods to work with Result types, enabling
/// seamless conversions between Results, Maybe types, and other monadic structures.
/// </remarks>
/// <example>
/// Basic Result conversion examples:
/// <code><![CDATA[
/// // Converting a value to a successful Result
/// var number = 42;
/// var successResult = number.ToSuccess(); // Result.Success(42)
/// Console.WriteLine(successResult.IsSuccess); // Output: True
///
/// // Converting an error to a failed Result
/// var error = ResultError.New("Not found");
/// var failedResult = error.ToFailure<int>(); // Result.Failure<int>(error)
/// Console.WriteLine(failedResult.IsFailure); // Output: True
/// Console.WriteLine(failedResult.Error.Message); // Output: "Not found"
///
/// // Converting a Result to Maybe
/// var result = Result.Success(42);
/// var maybe = result.ToMaybe(); // Maybe.Some(42)
/// Console.WriteLine(maybe.IsSome); // Output: True
///
/// // Converting to Unit Result
/// var strResult = Result.Success("hello");
/// var unitResult = strResult.ToUnitResult();
/// Console.WriteLine(unitResult.IsSuccess); // Output: True
/// ]]></code>
/// </example>
public static class ResultExtensions
{
    /// <summary>
    /// Creates a successful <see cref="Result{TIn}"/> instance containing the provided value.
    /// </summary>
    /// <typeparam name="TIn">The type of the value to be wrapped in the Result.</typeparam>
    /// <param name="self">The value to convert into a successful Result.</param>
    /// <returns>A new successful <see cref="Result{TIn}"/> containing the provided value.</returns>
    /// <example>
    /// <code><![CDATA[
    /// var number = 42;
    /// Result<int> result = number.ToSuccess();
    /// Console.WriteLine(result.IsSuccess); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> ToSuccess<TIn>(this TIn self)
        => Result.Success(self);

    /// <summary>
    /// Creates a failed <see cref="Result{TIn}"/> instance containing the provided error.
    /// </summary>
    /// <typeparam name="TIn">The type that would have been contained in a successful result.</typeparam>
    /// <param name="self">The error to be wrapped in the failed Result.</param>
    /// <returns>A new failed <see cref="Result{TIn}"/> containing the provided error.</returns>
    /// <example>
    /// <code><![CDATA[
    /// var error = ResultError.New("Not found");
    /// Result<int> result = error.ToFailure<int>();
    /// Console.WriteLine(result.IsFailure); // Output: True
    /// Console.WriteLine(result.Error.Message); // Output: "Not found"
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TIn> ToFailure<TIn>(this ResultError self)
        => Result.Failure<TIn>(self);

    /// <inheritdoc cref="ToFailure{TIn}(ResultError)"/>
    /// <remarks>
    /// This overload automatically converts the exception to a <see cref="ResultError"/> instance.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var ex = new InvalidOperationException("Invalid operation");
    /// Result<int> result = ex.ToFailure<int>();
    /// Console.WriteLine(result.IsFailure); // Output: True
    /// Console.WriteLine(result.Error.Exception == ex); // Output: True
    /// ]]></code>
    /// </example>
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
    /// A new <see cref="Maybe{TIn}"/> that will be:
    /// <list type="bullet">
    /// <item><description><see cref="Maybe{T}.IsSome"/> if the Result is successful</description></item>
    /// <item><description><see cref="Maybe{T}.IsNone"/> if the Result is a failure</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// <code><![CDATA[
    /// // Success case
    /// var successResult = Result.Success(42);
    /// var someValue = successResult.ToMaybe();
    /// Console.WriteLine(someValue.IsSome); // Output: True
    /// Console.WriteLine(someValue.Match(
    ///     some: x => x,
    ///     none: () => 0)); // Output: 42
    ///
    /// // Failure case
    /// var failedResult = Result.Failure<int>(ResultError.New("Error"));
    /// var noneValue = failedResult.ToMaybe();
    /// Console.WriteLine(noneValue.IsNone); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> ToMaybe<TIn>(this Result<TIn> self)
        => self.Match(
            success: Maybe.Some,
            failure: _ => Maybe.None<TIn>());

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
    /// <example>
    /// <code><![CDATA[
    /// // Success case
    /// Try<int> tryParse = () => Result.Success(int.Parse("42"));
    /// Maybe<int> maybeNumber = tryParse.ToMaybe();
    /// Console.WriteLine(maybeNumber.IsSome); // Output: True
    /// Console.WriteLine(maybeNumber.Match(
    ///     some: x => x,
    ///     none: () => 0)); // Output: 42
    ///
    /// // Failure case
    /// Try<int> tryParseInvalid = () => Result.Success(int.Parse("invalid"));
    /// Maybe<int> maybeInvalid = tryParseInvalid.ToMaybe();
    /// Console.WriteLine(maybeInvalid.IsNone); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> ToMaybe<TIn>(this Try<TIn> self)
        => self.Try() is { IsSuccess: true } result
            ? Maybe.Some(result.Value)
            : Maybe.None<TIn>();

    /// <inheritdoc cref="ToMaybe{TIn}(Result{TIn})"/>
    /// <remarks>
    /// This asynchronous version works with Task-wrapped Results.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// // Success case
    /// Task<Result<int>> asyncSuccess = Task.FromResult(Result.Success(42));
    /// Maybe<int> maybeNumber = await asyncSuccess.ToMaybeAsync();
    /// Console.WriteLine(maybeNumber.IsSome); // Output: True
    /// Console.WriteLine(maybeNumber.Match(
    ///     some: x => x,
    ///     none: () => 0)); // Output: 42
    ///
    /// // Failure case
    /// Task<Result<int>> asyncFailure = Task.FromResult(
    ///     Result.Failure<int>(ResultError.New("Failed")));
    /// Maybe<int> maybeFailure = await asyncFailure.ToMaybeAsync();
    /// Console.WriteLine(maybeFailure.IsNone); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Maybe<TIn>> ToMaybeAsync<TIn>(this Task<Result<TIn>> self)
#pragma warning disable CA1062
        => (await self.ConfigureAwait(false)).Match(
            success: Maybe.Some,
            failure: _ => Maybe.None<TIn>());
#pragma warning restore CA1062

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
    /// <example>
    /// <code><![CDATA[
    /// // Success case
    /// TryAsync<int> asyncTry = async () => {
    ///     await Task.Delay(100); // Simulate async work
    ///     return Result.Success(42);
    /// };
    /// Maybe<int> maybeNumber = await asyncTry.ToMaybeAsync();
    /// Console.WriteLine(maybeNumber.IsSome); // Output: True
    /// Console.WriteLine(maybeNumber.Match(
    ///     some: x => x,
    ///     none: () => 0)); // Output: 42
    ///
    /// // Failure case
    /// TryAsync<int> asyncFailure = async () => {
    ///     await Task.Delay(100); // Simulate async work
    ///     return Result.Failure<int>(ResultError.New("Failed"));
    /// };
    /// Maybe<int> maybeFailure = await asyncFailure.ToMaybeAsync();
    /// Console.WriteLine(maybeFailure.IsNone); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Maybe<TIn>> ToMaybeAsync<TIn>(this TryAsync<TIn> self)
        => (await self.TryAsync().ConfigureAwait(false)).Match(
            success: Maybe.Some,
            failure: _ => Maybe.None<TIn>());

    /// <summary>
    /// Converts a <see cref="Result{TIn}"/> to a <see cref="Result{Unit}"/> instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the value contained in the original Result.</typeparam>
    /// <param name="self">The Result instance to convert.</param>
    /// <returns>
    /// A new <see cref="Result{Unit}"/> that will be:
    /// <list type="bullet">
    /// <item><description><c>Success(<see cref="Unit"/>)</c> if the original Result is successful</description></item>
    /// <item><description>Failure with the same error if the original Result is a failure</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// <code><![CDATA[
    /// // Success case
    /// var successResult = Result.Success("hello");
    /// var unitResult = successResult.ToUnitResult();
    /// Console.WriteLine(unitResult.IsSuccess); // Output: True
    ///
    /// // Failure case
    /// var failedResult = Result.Failure<string>(ResultError.New("Error"));
    /// var failedUnitResult = failedResult.ToUnitResult();
    /// Console.WriteLine(failedUnitResult.IsFailure); // Output: True
    /// Console.WriteLine(failedUnitResult.Error.Message); // Output: "Error"
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<Unit> ToUnitResult<TIn>(this Result<TIn> self)
        => self.IsSuccess
            ? Result.Success(Unit.Default)
            : Result.Failure<Unit>(self.Error);

    /// <inheritdoc cref="ToUnitResult{TIn}(Result{TIn})"/>
    /// <remarks>
    /// This asynchronous version works with Task-wrapped Results.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// // Success case with string result
    /// Task<Result<string>> asyncSuccess = Task.FromResult(Result.Success("hello"));
    /// Result<Unit> unitResult = await asyncSuccess.ToUnitResultAsync();
    /// Console.WriteLine(unitResult.IsSuccess); // Output: True
    ///
    /// // Failure case with custom error
    /// var error = ResultError.New("Operation failed");
    /// Task<Result<int>> asyncFailure = Task.FromResult(Result.Failure<int>(error));
    /// Result<Unit> failedResult = await asyncFailure.ToUnitResultAsync();
    /// Console.WriteLine(failedResult.IsFailure); // Output: True
    /// Console.WriteLine(failedResult.Error.Message); // Output: "Operation failed"
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<Unit>> ToUnitResultAsync<TIn>(this Task<Result<TIn>> self)
    {
#pragma warning disable CA1062
        var result = await self.ConfigureAwait(false);
        return result.IsSuccess
            ? Result.Success(Unit.Default)
            : Result.Failure<Unit>(result.Error);
#pragma warning restore CA1062
    }
}