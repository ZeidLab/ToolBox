﻿using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace

namespace ZeidLab.ToolBox.Options;

/// <summary>
/// Contains extension methods for working with <see cref="Maybe{T}"/> instances, providing functional programming operations
/// such as binding, mapping, filtering, and pattern matching.
/// </summary>
public static class MaybeExtensions
{
	/// <summary>
	/// Converts a non-null object of type <typeparamref name="TIn"/> to a <see cref="Maybe{TIn}"/> instance in the 'Some' state.
	/// </summary>
	/// <typeparam name="TIn">The type of the object to be wrapped in the <see cref="Maybe{TIn}"/> instance. Must be a non-nullable type.</typeparam>
	/// <param name="self">The non-null object to convert into a <see cref="Maybe{TIn}"/> instance.</param>
	/// <returns>
	/// A <see cref="Maybe{TIn}"/> instance in the 'Some' state containing the provided object as its content.
	/// If <paramref name="self"/> is null, this method will throw an exception.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> is null.</exception>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<TIn> ToSome<TIn>(this TIn self) where TIn : notnull
		=> Maybe.Some(self);

	/// <summary>
	/// Creates a <see cref="Maybe{TIn}"/> instance in the None state, regardless of the input value.
	/// </summary>
	/// <typeparam name="TIn">The type parameter for the resulting <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <param name="self">The input object, which can be null or non-null. This value is ignored.</param>
	/// <returns>
	/// A new <see cref="Maybe{TIn}"/> instance in the None state, representing the absence of a value.
	/// </returns>
	/// <remarks>
	/// This method is useful when you need to explicitly create a None state <see cref="Maybe{TIn}"/> instance,
	/// regardless of whether you have a value or not. The input parameter is ignored entirely.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// // Both calls will return a None state Maybe
	/// var none1 = 42.ToNone();
	/// var none2 = ((string)null).ToNone();
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<TIn> ToNone<TIn>(this TIn? self)
		=> Maybe.None<TIn>();



	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[ExcludeFromCodeCoverage]
#pragma warning disable S1133
	[Obsolete("Use .Match and .MatchAsync instead",true)]
#pragma warning restore S1133
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public static TOut Map<TIn, TOut>(this Maybe<TIn> self, Func<TIn, TOut> some, Func<TOut> none)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
		where TIn : notnull =>
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
		!self.IsNull ? some(self.Value) : none();
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.


	/// <summary>
	/// Ensures that the content of a <see cref="Maybe{TIn}"/> instance satisfies a predicate.
	/// </summary>
	/// <example>
	/// <code><![CDATA[
	/// List<Maybe<int>> maybeList = new List<Maybe<int>>
	/// {
	///     Maybe.Some(2),
	///     Maybe.Some(10),
	///     Maybe.None<int>(),
	///     Maybe.Some(42)
	/// };
	///
	/// var filtered = maybeList
	///     .Where(x => x.If(value => value > 12))
	///     .ToList();
	///
	/// Console.WriteLine(filtered.Count); // Output: 2
	/// var result = filtered[0].if(value => value > 0) ? filtered[0].Reduce(() => 0) : 42;
	/// Console.WriteLine(result); // Output: 2
	/// ]]></code>
	/// </example>
	/// <typeparam name="TIn">The type of the content of the <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <param name="self">The <see cref="Maybe{TIn}"/> instance whose content to check.</param>
	/// <param name="predicate">
	/// The predicate to evaluate. It takes a value of type <typeparamref name="TIn"/> and returns a <see cref="bool"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="Maybe{TIn}"/> instance is not <see cref="Maybe{TIn}.IsNone"/>
	/// and its content satisfies the <paramref name="predicate"/>; otherwise, <see langword="false"/>.
	/// </returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool If<TIn>(this Maybe<TIn> self, Func<TIn, bool> predicate)
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
		where TIn : notnull => !self.IsNull && predicate(self.Value);
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.



	/// <summary>
	/// Filters the <see cref="Maybe{TIn}"/> instance based on a predicate applied to its content.
	/// </summary>
	/// <typeparam name="TIn">The type of the content within the <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <param name="self">The <see cref="Maybe{TIn}"/> instance to filter.</param>
	/// <param name="predicate">The predicate function to apply to the content of the <see cref="Maybe{TIn}"/> instance.</param>
	/// <returns>
	/// A <see cref="Maybe{TIn}"/> instance with the same content if the predicate is true, or None if the predicate is false.
	/// </returns>
	/// <example>
	/// <code><![CDATA[
	/// var maybe = Maybe.Some(42);
	/// var filtered = maybe.Filter(value => value > 0);
	/// Console.WriteLine(filtered.Reduce(() => 0)); // Output: 42
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<TIn> Filter<TIn>(this Maybe<TIn> self, Func<TIn, bool> predicate)
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
		where TIn : notnull => !self.IsNull && predicate(self.Value) ? Maybe.Some(self.Value) : Maybe.None<TIn>();
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.





	/// <summary>
	/// Returns the value of the <see cref="Maybe{TIn}"/> instance if it is <see cref="Maybe{TIn}.IsSome"/>,
	/// otherwise throws an <see cref="InvalidOperationException"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the content of the <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <param name="self">The <see cref="Maybe{TIn}"/> instance whose value to return.</param>
	/// <returns>
	/// The value of the <see cref="Maybe{TIn}"/> instance if it is <see cref="Maybe{TIn}.IsSome"/>,
	/// or throws an <see cref="InvalidOperationException"/> if it is <see cref="Maybe{TIn}.IsNone"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TIn ValueOrThrow<TIn>(this Maybe<TIn> self)
		where TIn : notnull
	{
		if (self.IsNull)
			throw new InvalidOperationException("The value is not set.");
#pragma warning disable CS8603 // Possible null reference return.
		return self.Value;
#pragma warning restore CS8603 // Possible null reference return.
	}



}