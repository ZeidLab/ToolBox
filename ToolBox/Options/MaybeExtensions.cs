using System.Diagnostics.CodeAnalysis;
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
	/// <code><![CDATA[
	/// var maybeAge = Maybe.Some(25);
	/// var description = maybeAge.Map(
	///     some: age => $"Age is {age}",
	///     none: () => "Age unknown"
	/// ); // Returns "Age is 25"
	///
	/// var maybeNone = Maybe.<int>None();
	/// var result = maybeNone.Map(
	///     some: age => age * 2,
	///     none: () => 0
	/// ); // Returns 0
	/// ]]></code>
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
	/// Filters the Maybe instance based on a predicate applied to its content.
	/// </summary>
	/// <example>
	/// <code><![CDATA[
	/// var maybe = Maybe.Some(42);
	/// var filtered = maybe.Filter(value => value > 0);
	/// Console.WriteLine(filtered.Reduce(() => 0)); // Output: 42
	/// ]]></code>
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