using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Options;

/// <summary>
///
/// </summary>
public static class MaybeExtensionsEnumerable
{
	/// <summary>
	/// Converts a sequence of Maybe instances to an IEnumerable of their content, excluding None instances.
	/// </summary>
	/// <example>
	/// <code><![CDATA[
	/// var maybeList = new List<Maybe<int>>
	/// {
	///     Maybe.Some(1),
	///     Maybe.<int>None(),
	///     Maybe.Some(3)
	/// };
	///
	/// var result = maybeList.Flatten(); // Output: [1, 3]
	/// ]]></code>
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
	/// Converts a sequence of <see cref="Maybe{TIn}"/> instances to an <see cref="IEnumerable{TIn}"/> of their content,
	/// replacing <see cref="Maybe{TIn}.IsNone"/> instances with a specified substitute value.
	/// </summary>
	/// <typeparam name="TIn">The type of the content within the <see cref="Maybe{TIn}"/> instances.</typeparam>
	/// <param name="self">The sequence of <see cref="Maybe{TIn}"/> instances to convert.</param>
	/// <param name="substitute">The substitute value to use for <see cref="Maybe{TIn}.IsNone"/> instances.</param>
	/// <returns>
	/// An <see cref="IEnumerable{TIn}"/> of the content of the <see cref="Maybe{TIn}"/> instances,
	/// with <see cref="Maybe{TIn}.IsNone"/> instances replaced by the substitute value.
	/// </returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<TIn> Flatten<TIn>(this IEnumerable<Maybe<TIn>> self, TIn substitute)
		where TIn : notnull => self.Select(x => x.Reduce(substitute));

	/// <summary>
	/// Converts a sequence of <see cref="Maybe{TIn}"/> instances to an <see cref="IEnumerable{TIn}"/> of their content,
	/// using a substitute function for <see cref="Maybe{TIn}.IsNone"/> instances.
	/// </summary>
	/// <typeparam name="TIn">The type of the content within the <see cref="Maybe{TIn}"/> instances.</typeparam>
	/// <param name="self">The sequence of <see cref="Maybe{TIn}"/> instances to convert.</param>
	/// <param name="substitute">The function <see cref="Func{TIn}"/> to provide a substitute value for <see cref="Maybe{TIn}.IsNone"/> instances.</param>
	/// <returns>
	/// An <see cref="IEnumerable{TIn}"/> of the content of the <see cref="Maybe{TIn}"/> instances,
	/// replacing <see cref="Maybe{TIn}.IsNone"/> instances with the result of the substitute function.
	/// </returns>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<TIn> Flatten<TIn>(this IEnumerable<Maybe<TIn>> self, Func<TIn> substitute)
		where TIn : notnull => self.Select(x => x.Reduce(substitute));

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
}