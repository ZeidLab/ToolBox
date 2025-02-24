using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Options;

/// <summary>
/// Provides extension methods to reduce a <see cref="Maybe{TIn}"/> instance
/// to a concrete value. This is useful in railway-oriented programming where
/// the optional monad pattern improves overall performance by eliminating null
/// reference issues and easing the programmer’s life by clearly defining when a value
/// is present (<see cref="Maybe{TIn}.IsSome"/>) or absent (<see cref="Maybe{TIn}.IsNone"/>).
/// </summary>
/// <example>
/// <code>
/// <![CDATA[
/// // Example: Reducing a Maybe<int> to a concrete value using a default value.
/// var maybeNumber = new Maybe<int>(5);
/// int result = maybeNumber.Reduce(0);
/// // result is 5
///
/// var noneNumber = Maybe<int>.None;
/// int defaultResult = noneNumber.Reduce(0);
/// // defaultResult is 0
/// ]]>
/// </code>
/// </example>
public static class MaybeExtensionsReduce
{
	#region Maybe<TIn>

	/// <summary>
	/// Reduces the content of a <see cref="Maybe{TIn}"/> instance to a single value
	/// using the provided default value if the instance is empty.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the content held by the <see cref="Maybe{TIn}"/> instance.
	/// </typeparam>
	/// <param name="self">
	/// The <see cref="Maybe{TIn}"/> instance to reduce.
	/// </param>
	/// <param name="substitute">
	/// The default value to return if <paramref name="self"/> is empty (<see cref="Maybe{TIn}.IsNone"/>).
	/// </param>
	/// <returns>
	/// Returns the contained value if <paramref name="self"/> has one (<see cref="Maybe{TIn}.IsSome"/>),
	/// otherwise returns <paramref name="substitute"/>.
	/// </returns>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Common use case: Using a default value when no value is present.
	/// var maybeNumber = new Maybe<int>(5);
	/// int result = maybeNumber.Reduce(0);
	/// // result is 5
	///
	/// // Edge case: When the Maybe is empty, the default value is used.
	/// var noneNumber = Maybe<int>.None;
	/// int defaultResult = noneNumber.Reduce(0);
	/// // defaultResult is 0
	/// ]]>
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TIn Reduce<TIn>(this Maybe<TIn> self, TIn substitute)
		where TIn : notnull =>
#pragma warning disable CS8603 // Possible null reference return.
		!self.IsNull ? self.Value : substitute;
#pragma warning restore CS8603 // Possible null reference return.

	/// <summary>
	/// Reduces the content of a <see cref="Maybe{TIn}"/> instance to a single value
	/// using the provided function to generate a default value when the instance is empty.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the content held by the <see cref="Maybe{TIn}"/> instance.
	/// </typeparam>
	/// <param name="self">
	/// The <see cref="Maybe{TIn}"/> instance to reduce.
	/// </param>
	/// <param name="substitute">
	/// A function that returns a default value if <paramref name="self"/> is empty (<see cref="Maybe{TIn}.IsNone"/>).
	/// </param>
	/// <returns>
	/// Returns the contained value if <paramref name="self"/> is non-empty (<see cref="Maybe{TIn}.IsSome"/>),
	/// otherwise returns the result of <paramref name="substitute"/>.
	/// </returns>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Common use case: Using a function to supply the default value.
	/// var maybeString = new Maybe<string>("Hello");
	/// string result = maybeString.Reduce(() => "Default");
	/// // result is "Hello"
	///
	/// // Edge case: When the Maybe is empty, the function supplies the default.
	/// var noneString = Maybe<string>.None;
	/// string defaultResult = noneString.Reduce(() => "Default");
	/// // defaultResult is "Default"
	/// ]]>
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TIn Reduce<TIn>(this Maybe<TIn> self, Func<TIn> substitute)
		where TIn : notnull =>
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CA1062
		!self.IsNull ? self.Value : substitute();
#pragma warning restore CA1062
#pragma warning restore CS8603 // Possible null reference return.

	/// <summary>
	/// Asynchronously reduces the content of a <see cref="Maybe{TIn}"/> instance to a single value
	/// using the provided asynchronous default value when the instance is empty.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the content held by the <see cref="Maybe{TIn}"/> instance.
	/// </typeparam>
	/// <param name="self">
	/// The <see cref="Maybe{TIn}"/> instance to reduce.
	/// </param>
	/// <param name="substitute">
	/// A <see cref="Task"/> returning the default value to use if <paramref name="self"/> is empty (<see cref="Maybe{TIn}.IsNone"/>).
	/// </param>
	/// <returns>
	/// Returns a <see cref="Task"/> containing either the contained value if <paramref name="self"/> is non-empty
	/// (<see cref="Maybe{TIn}.IsSome"/>), or the result of <paramref name="substitute"/> if it is empty.
	/// </returns>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Common use case: Awaiting the asynchronous default value.
	/// var maybeValue = new Maybe<int>(10);
	/// Task<int> asyncDefault = Task.FromResult(0);
	/// int result = await maybeValue.ReduceAsync(asyncDefault);
	/// // result is 10
	///
	/// // Edge case: When the Maybe is empty, the asynchronous default is returned.
	/// var noneValue = Maybe<int>.None;
	/// int defaultResult = await noneValue.ReduceAsync(asyncDefault);
	/// // defaultResult is 0
	/// ]]>
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<TIn> ReduceAsync<TIn>(this Maybe<TIn> self, Func<Task<TIn>> substitute)
		where TIn : notnull =>
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CA1062
		!self.IsNull ? self.Value!.AsTaskAsync() : substitute();
#pragma warning restore CA1062
#pragma warning restore CS8603 // Possible null reference return.

	#endregion

	#region Task<Maybe<TIn>>

	/// <summary>
	/// Asynchronously reduces the content of a <see cref="Maybe{TIn}"/> instance wrapped in a <see cref="Task"/>
	/// to a single value using the provided default value when the instance is empty.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the content held by the <see cref="Maybe{TIn}"/> instance.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Task"/> that returns a <see cref="Maybe{TIn}"/> instance.
	/// </param>
	/// <param name="substitute">
	/// The default value to use if the resolved <see cref="Maybe{TIn}"/> is empty (<see cref="Maybe{TIn}.IsNone"/>).
	/// </param>
	/// <returns>
	/// Returns a <see cref="Task"/> containing the reduced value:
	/// either the contained value if the <see cref="Maybe{TIn}"/> is non-empty (<see cref="Maybe{TIn}.IsSome"/>)
	/// or the provided default value.
	/// </returns>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Example: Reducing an asynchronously retrieved Maybe<int>.
	/// Task<Maybe<int>> maybeTask = Task.FromResult(new Maybe<int>(20));
	/// int result = await maybeTask.ReduceAsync(0);
	/// // result is 20
	///
	/// maybeTask = Task.FromResult(Maybe<int>.None);
	/// int defaultResult = await maybeTask.ReduceAsync(0);
	/// // defaultResult is 0
	/// ]]>
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TIn> ReduceAsync<TIn>(this Task<Maybe<TIn>> self, TIn substitute)
		where TIn : notnull =>
#pragma warning disable CA1062
		(await self.ConfigureAwait(false)).Reduce(substitute: substitute);
#pragma warning restore CA1062

	/// <summary>
	/// Asynchronously reduces the content of a <see cref="Maybe{TIn}"/> instance wrapped in a <see cref="Task"/>
	/// to a single value using the provided function to generate a default value when the instance is empty.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the content held by the <see cref="Maybe{TIn}"/> instance.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Task"/> that returns a <see cref="Maybe{TIn}"/> instance.
	/// </param>
	/// <param name="substitute">
	/// A function that returns a default value if the resolved <see cref="Maybe{TIn}"/> is empty (<see cref="Maybe{TIn}.IsNone"/>).
	/// </param>
	/// <returns>
	/// Returns a <see cref="Task"/> containing the reduced value:
	/// either the contained value if the <see cref="Maybe{TIn}"/> is non-empty (<see cref="Maybe{TIn}.IsSome"/>)
	/// or the result of the provided function.
	/// </returns>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Example: Using a function to provide a default for an asynchronously retrieved Maybe<string>.
	/// Task<Maybe<string>> maybeTask = Task.FromResult(new Maybe<string>("Async"));
	/// string result = await maybeTask.ReduceAsync(() => "Default");
	/// // result is "Async"
	///
	/// maybeTask = Task.FromResult(Maybe<string>.None);
	/// string defaultResult = await maybeTask.ReduceAsync(() => "Default");
	/// // defaultResult is "Default"
	/// ]]>
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TIn> ReduceAsync<TIn>(this Task<Maybe<TIn>> self, Func<TIn> substitute)
		where TIn : notnull =>
#pragma warning disable CA1062
		(await self.ConfigureAwait(false)).Reduce(substitute: substitute);
#pragma warning restore CA1062

	/// <summary>
	/// Asynchronously reduces the content of a <see cref="Maybe{TIn}"/> instance wrapped in a <see cref="Task"/>
	/// to a single value using the provided asynchronous default value when the instance is empty.
	/// </summary>
	/// <typeparam name="TIn">
	/// The type of the content held by the <see cref="Maybe{TIn}"/> instance.
	/// </typeparam>
	/// <param name="self">
	/// A <see cref="Task"/> that returns a <see cref="Maybe{TIn}"/> instance.
	/// </param>
	/// <param name="substitute">
	/// A <see cref="Task"/> returning the default value to use if the resolved <see cref="Maybe{TIn}"/> is empty (<see cref="Maybe{TIn}.IsNone"/>).
	/// </param>
	/// <returns>
	/// Returns a <see cref="Task"/> containing the reduced value:
	/// either the contained value if the <see cref="Maybe{TIn}"/> is non-empty (<see cref="Maybe{TIn}.IsSome"/>)
	/// or the result of <paramref name="substitute"/> if it is empty.
	/// </returns>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// // Example: Asynchronously reducing a Task<Maybe<int>> with an asynchronous default.
	/// Task<Maybe<int>> maybeTask = Task.FromResult(new Maybe<int>(30));
	/// Task<int> asyncDefault = Task.FromResult(0);
	/// int result = await maybeTask.ReduceAsync(asyncDefault);
	/// // result is 30
	///
	/// maybeTask = Task.FromResult(Maybe<int>.None);
	/// int defaultResult = await maybeTask.ReduceAsync(asyncDefault);
	/// // defaultResult is 0
	/// ]]>
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TIn> ReduceAsync<TIn>(this Task<Maybe<TIn>> self, Func<Task<TIn>> substitute)
		where TIn : notnull =>
#pragma warning disable CA1062
		await (await self.ConfigureAwait(false)).ReduceAsync(substitute: substitute).ConfigureAwait(false);
#pragma warning restore CA1062

	#endregion
}