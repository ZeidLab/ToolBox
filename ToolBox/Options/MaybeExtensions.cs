﻿using System.Diagnostics.Contracts;
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
		=> Maybe<TIn>.Some(self);

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
	/// <code>
	/// // Both calls will return a None state Maybe
	/// var none1 = 42.ToNone();
	/// var none2 = ((string)null).ToNone();
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<TIn> ToNone<TIn>(this TIn? self)
		=> Maybe<TIn>.None();

	/// <summary>
	/// Maps the content of a <see cref="Maybe{TIn}"/> instance to a new <see cref="Maybe{TOut}"/> instance
	/// using the provided mapping function.
	/// </summary>
	/// <typeparam name="TIn">The type of the content in the original <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <typeparam name="TOut">The type of the content in the resulting <see cref="Maybe{TOut}"/> instance.</typeparam>
	/// <param name="self">The original <see cref="Maybe{TIn}"/> instance to map.</param>
	/// <param name="map">
	/// The mapping function to apply if <paramref name="self"/> is Some.
	/// It takes a value of type <typeparamref name="TIn"/> and returns a <see cref="Maybe{TOut}"/>.
	/// </param>
	/// <returns>
	/// A new <see cref="Maybe{TOut}"/> instance with the mapped content if <paramref name="self"/> is Some,
	/// or a new None instance if <paramref name="self"/> is None.
	/// </returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<TOut> Bind<TIn, TOut>(this Maybe<TIn> self, Func<TIn, Maybe<TOut>> map)
		where TOut : notnull
		where TIn : notnull =>
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
		!self.IsNull ? map(self.Value) : Maybe<TOut>.None();
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.


	/// <summary>
	/// Maps the content of a <see cref="Maybe{TIn}"/> instance to a new value using two functions:
	/// one for handling the Some case and another for handling the None case. This method always returns
	/// a value, making it useful for providing default values or handling both states of a Maybe instance.
	/// </summary>
	/// <typeparam name="TIn">The type of the content in the original <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <typeparam name="TOut">The type of the result value after mapping.</typeparam>
	/// <param name="self">The original <see cref="Maybe{TIn}"/> instance to map.</param>
	/// <param name="some">
	/// The mapping function to apply if <paramref name="self"/> is Some.
	/// Takes a value of type <typeparamref name="TIn"/> and returns a value of type <typeparamref name="TOut"/>.
	/// </param>
	/// <param name="none">
	/// The function to execute if <paramref name="self"/> is None.
	/// Returns a default or fallback value of type <typeparamref name="TOut"/>.
	/// </param>
	/// <returns>
	/// A value of type <typeparamref name="TOut"/>: either the result of applying <paramref name="some"/>
	/// to the content if <paramref name="self"/> is Some, or the result of <paramref name="none"/> if it is None.
	/// </returns>
	/// <example>
	/// <code>
	/// var maybeAge = Maybe&lt;int&gt;.Some(25);
	/// var description = maybeAge.Map(
	///     some: age => $"Age is {age}",
	///     none: () => "Age unknown"
	/// ); // Returns "Age is 25"
	///
	/// var maybeNone = Maybe&lt;int&gt;.None();
	/// var result = maybeNone.Map(
	///     some: age => age * 2,
	///     none: () => 0
	/// ); // Returns 0
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TOut Map<TIn, TOut>(this Maybe<TIn> self, Func<TIn, TOut> some, Func<TOut> none)
		where TIn : notnull =>
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
		!self.IsNull ? some(self.Value) : none();
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.

	/// <summary>
	/// Matches the content of a <see cref="Maybe{TIn}"/> instance to a new <see cref="Maybe{TOut}"/> instance
	/// using one of two provided functions.
	/// </summary>
	/// <typeparam name="TIn">The type of the content in the original <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <typeparam name="TOut">The type of the content in the resulting <see cref="Maybe{TOut}"/> instance.</typeparam>
	/// <param name="self">The original <see cref="Maybe{TIn}"/> instance to match.</param>
	/// <param name="some">
	/// A function to apply if <paramref name="self"/> is some.
	/// It takes a value of type <typeparamref name="TIn"/> and returns a <see cref="Maybe{TOut}"/>.
	/// </param>
	/// <param name="none">
	/// A function to apply if <paramref name="self"/> is none.
	/// It returns a <see cref="Maybe{TOut}"/>.
	/// </param>
	/// <returns>
	/// A new <see cref="Maybe{TOut}"/> instance resulting from applying the appropriate function
	/// based on the state of <paramref name="self"/>.
	/// </returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<TOut> Match<TIn, TOut>(this Maybe<TIn> self,
		Func<TIn, Maybe<TOut>> some,
		Func<Maybe<TOut>> none)
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
	/// <code>
	/// List&lt;Maybe&lt;int&gt;&gt; maybeList = new List&lt;Maybe&lt;int&gt;&gt;
	/// {
	///     Maybe&lt;int&gt;.Some(2),
	///     Maybe&lt;int&gt;.Some(10),
	///     Maybe&lt;int&gt;.None(),
	///     Maybe&lt;int&gt;.Some(42)
	/// };
	///
	/// var filtered = maybeList
	///     .Where(x => x.If(value => value &gt; 12))
	///     .ToList();
	///
	/// Console.WriteLine(filtered.Count); // Output: 2
	/// var result = filtered[0].if(value => value &gt; 0) ? filtered[0].Reduce(() => 0) : 42;
	/// Console.WriteLine(result); // Output: 2
	/// </code>
	/// </example>
	/// <typeparam name="TIn">The type of the content of the <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <param name="self">The <see cref="Maybe{TIn}"/> instance whose content to check.</param>
	/// <param name="predicate">
	/// The predicate to evaluate. It takes a value of type <typeparamref name="TIn"/> and returns a <see cref="bool"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="Maybe{TIn}"/> instance is not <see cref="Maybe{TIn}.None"/>
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
	/// Filters a sequence of Maybe instances based on a predicate applied to their content.
	/// </summary>
	/// <typeparam name="TIn">The type of the content within the Maybe instances.</typeparam>
	/// <param name="self">The sequence of Maybe instances to filter.</param>
	/// <param name="predicate">The predicate function to apply to the content of each Maybe instance.</param>
	/// <returns>
	/// An IEnumerable of Maybe instances where the content satisfies the predicate.
	/// </returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<Maybe<TIn>> Where<TIn>(this IEnumerable<Maybe<TIn>> self, Func<TIn, bool> predicate)
#pragma warning disable CS8604 // Possible null reference argument.
		where TIn : notnull => self.Where(x => !x.IsNull && predicate(x.Value));
#pragma warning restore CS8604 // Possible null reference argument.

	/// <summary>
	/// Filters the Maybe instance based on a predicate applied to its content.
	/// </summary>
	/// <example>
	/// <code>
	/// var maybe = Maybe&lt;int&gt;.Some(42);
	/// var filtered = maybe.Filter(value => value &gt; 0);
	/// Console.WriteLine(filtered.Reduce(() => 0)); // Output: 42
	/// </code>
	/// </example>
	/// <typeparam name="TIn">The type of the content within the Maybe instance.</typeparam>
	/// <param name="self">The Maybe instance to filter.</param>
	/// <param name="predicate">The predicate function to apply to the content of the Maybe instance.</param>
	/// <returns>
	/// A Maybe instance with the same content if the predicate is true, or None if the predicate is false.
	/// </returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<TIn> Filter<TIn>(this Maybe<TIn> self, Func<TIn, bool> predicate)
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
		where TIn : notnull => !self.IsNull && predicate(self.Value) ? Maybe<TIn>.Some(self.Value) : Maybe<TIn>.None();
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.

	/// <summary>
	/// Converts a sequence of Maybe instances to an IEnumerable of their content, excluding None instances.
	/// </summary>
	/// <example>
	/// <code>
	/// var maybeList = new List&lt;Maybe&lt;int&gt;&gt;
	/// {
	///     Maybe&lt;int&gt;.Some(1),
	///     Maybe&lt;int&gt;.None(),
	///     Maybe&lt;int&gt;.Some(3)
	/// };
	///
	/// var result = maybeList.ToEnumerable(); // Output: [1, 3]
	/// </code>
	/// </example>
	/// <typeparam name="TIn">The type of the content within the Maybe instances.</typeparam>
	/// <param name="self">The sequence of Maybe instances to convert.</param>
	/// <returns>An IEnumerable of the content of the Maybe instances <see cref="IEnumerable{TIn}"/>, excluding None instances.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<TIn> Flatten<TIn>(this IEnumerable<Maybe<TIn>> self)
		// ReSharper disable once NullableWarningSuppressionIsUsed
		where TIn : notnull => self.Where(x => !x.IsNull).Select(x => x.Value!);

	/// <summary>
	/// Converts a sequence of Maybe instances to an IEnumerable of their content,
	/// replacing None instances with a specified substitute value.
	/// </summary>
	/// <typeparam name="TIn">The type of the content within the Maybe instances.</typeparam>
	/// <param name="self">The sequence of Maybe instances to convert.</param>
	/// <param name="substitute">The substitute value to use for None instances.</param>
	/// <returns>An IEnumerable of the content of the Maybe instances <see cref="IEnumerable{TIn}"/>, with None instances replaced by the substitute value.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<TIn> Flatten<TIn>(this IEnumerable<Maybe<TIn>> self, TIn substitute)
		where TIn : notnull => self.Select(x => x.Reduce(substitute));

	/// <summary>
	/// Converts a sequence of Maybe instances to an IEnumerable of their content using a substitute function for None instances.
	/// </summary>
	/// <typeparam name="TIn">The type of the content within the Maybe instances.</typeparam>
	/// <param name="self">The sequence of Maybe instances to convert.</param>
	/// <param name="substitute">The function <see cref="Func{TIn}"/> to provide a substitute value for None instances.</param>
	/// <returns>An IEnumerable of the content of the Maybe instances <see cref="IEnumerable{TIn}"/>, replacing None instances with the result of the substitute function.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<TIn> Flatten<TIn>(this IEnumerable<Maybe<TIn>> self, Func<TIn> substitute)
		where TIn : notnull => self.Select(x => x.Reduce(substitute));

	/// <summary>
	/// Reduces the content of a Maybe instance to a single value using the provided default value or function.
	/// </summary>
	/// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
	/// <param name="self">The Maybe instance to reduce.</param>
	/// <param name="substitute">The default value to use if the Maybe instance is None.</param>
	/// <returns>The content of the Maybe instance, or the default value if the Maybe instance is None.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TIn Reduce<TIn>(this Maybe<TIn> self, TIn substitute)
		where TIn : notnull =>
#pragma warning disable CS8603 // Possible null reference return.
		!self.IsNull ? self.Value : substitute;
#pragma warning restore CS8603 // Possible null reference return.

	/// <summary>
	/// Reduces the content of a Maybe instance to a single value using the provided function.
	/// </summary>
	/// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
	/// <param name="self">The Maybe instance to reduce.</param>
	/// <param name="substitute">The function to use if the Maybe instance is None.</param>
	/// <returns>The content of the Maybe instance, or the result of the provided function if the Maybe instance is None.</returns>
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
	/// Applies the provided action to the content of the Maybe instance if it is Some.
	/// </summary>
	/// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
	/// <param name="self">The Maybe instance to apply the action to.</param>
	/// <param name="action">The action to apply to the content of the Maybe instance if it is Some.</param>
	/// <returns>The original Maybe instance.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<TIn> DoIfSome<TIn>(this Maybe<TIn> self, Action<TIn> action)
		where TIn : notnull
	{
		if (!self.IsNull)
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
			action(self.Value);
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.
		return self;
	}

	/// <summary>
	/// Executes the provided action if the Maybe instance is None.
	/// </summary>
	/// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
	/// <param name="self">The Maybe instance to check.</param>
	/// <param name="action">The action to execute if the Maybe instance is None.</param>
	/// <returns>The original Maybe instance.</returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<TIn> DoIfNone<TIn>(this Maybe<TIn> self, Action action)
		where TIn : notnull
	{
		if (self.IsNull)
#pragma warning disable CA1062
			action();
#pragma warning restore CA1062
		return self;
	}

	/// <summary>
	/// Returns the value of the Maybe instance if it is Some, otherwise throws an InvalidOperationException.
	/// </summary>
	/// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
	/// <param name="self">The Maybe instance whose value to return.</param>
	/// <returns>The value of the Maybe instance if it is Some, or throws an InvalidOperationException if it is None.</returns>
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
