using System.Diagnostics.CodeAnalysis;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Options;

/// <summary>
/// Represents an optional value that may or may not be present.
/// This struct is similar to a monad that can hold a value or be in a 'None' state.
/// </summary>
/// <typeparam name="TIn">The type of the contained value.</typeparam>
/// <example>
/// Basic usage:
/// <code><![CDATA[
/// // Creating a Maybe with a value
/// Maybe<int> someValue = Maybe.Some(10);
///
/// // Creating a Maybe in the 'None' state
/// Maybe<string> noneValue = Maybe.None<string>();
/// ]]></code>
/// </example>
[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
public readonly record struct Maybe<TIn> : IComparable<Maybe<TIn>>, IComparable
{
	/// <summary>
	/// Gets the value of the maybe, it can be null or default either.
	/// </summary>
	internal readonly TIn? Value;

	/// <summary>
	/// Gets a value indicating whether the value is null.
	/// </summary>
	internal readonly bool IsNull;

	/// <summary>
	/// Gets a value indicating whether the contained value is the default value of its type.
	/// </summary>
#pragma warning disable CA1051
	public readonly bool IsDefault;
#pragma warning restore CA1051

	/// <summary>
	/// Initializes a new instance of the <see cref="Maybe{TIn}"/> struct with the specified content.
	/// This constructor is intended for internal use.
	/// </summary>
	/// <param name="value">The content of the maybe.</param>
	internal Maybe(TIn value)
	{
		Value = value;
		IsNull = false;
		IsDefault = value.IsDefault();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Maybe{TIn}"/> struct representing the 'None' state.
	/// </summary>
	public Maybe()
	{
		IsNull = true;
		IsDefault = true;
		Value = default;
	}

	/// <summary>
	/// Gets a value indicating whether the maybe contains a value.
	/// </summary>
	public bool IsSome => !IsNull;

	/// <summary>
	/// Gets a value indicating whether the maybe is in the 'None' state.
	/// </summary>
	public bool IsNone => IsNull;



	/// <summary>
	/// Implicitly converts a value of type <typeparamref name="TIn"/> to a <see cref="Maybe{TIn}"/> containing that value.
	/// </summary>
	/// <param name="value">The value to contain.</param>
	/// <returns>A <see cref="Maybe{TIn}"/> containing the specified value or a 'None' Maybe if the value is null.</returns>
	/// <example>
	/// <code><![CDATA[
	/// Maybe<int> maybeInt = 5; // Implicit conversion from int to Maybe<int>
	/// string? nullableString = null;
	/// Maybe<string> maybeString = nullableString; // Implicit conversion from null to Maybe<string> (None)
	/// ]]></code>
	/// </example>
	public static implicit operator Maybe<TIn>(TIn? value)
#pragma warning disable CS8604 // Possible null reference argument.
		=> value.IsNull() ? new Maybe<TIn>() : new Maybe<TIn>(value);
#pragma warning restore CS8604 // Possible null reference argument.


	/// <summary>
	/// Compares this instance with another <see cref="Maybe{TIn}"/> instance.
	/// </summary>
	/// <param name="other">The other instance to compare with.</param>
	/// <returns>
	/// A value indicating the relative order of this instance and <paramref name="other"/>.
	/// Returns 0 if both are None or if the values are equal.
	/// Returns -1 if this instance is None or its value is less than the other.
	/// Returns 1 if this instance is Some and its value is greater than the other.
	/// </returns>
	/// <example>
	/// <code><![CDATA[
	/// Maybe<int> a = Maybe.Some(5);
	/// Maybe<int> b = Maybe.Some(10);
	/// Maybe<int> c = Maybe.None<int>();
	///
	/// Console.WriteLine(a.CompareTo(b)); // Output: -1
	/// Console.WriteLine(a.CompareTo(a)); // Output: 0
	/// Console.WriteLine(a.CompareTo(c)); // Output: 1
	/// Console.WriteLine(c.CompareTo(a)); // Output: -1
	/// ]]></code>
	/// </example>
	public int CompareTo(Maybe<TIn> other)
	{
		return (IsNull, other.IsNull) switch
		{
			// Handle null states
			(true, true) => 0,
			(true, false) => -1,
			(false, true) => 1,
			// handle non-null states
#pragma warning disable CS8604 // Possible null reference argument.
			(false, false) => Comparer<TIn>.Default.Compare(Value, other.Value)
#pragma warning restore CS8604 // Possible null reference argument.
		};
	}

	/// <summary>
	/// Compares this instance with another object.
	/// </summary>
	/// <param name="obj">The object to compare with.</param>
	/// <returns>
	/// A value indicating the relative order of this instance and <paramref name="obj"/>.
	/// Returns 1 if <paramref name="obj"/> is null.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown if <paramref name="obj"/> is not of type <see cref="Maybe{TIn}"/>.</exception>
	/// <example>
	/// <code><![CDATA[
	/// Maybe<int> a = Maybe.Some(5);
	/// Maybe<int> b = Maybe.Some(10);
	///
	/// Console.WriteLine(a.CompareTo(b)); // Output: -1
	/// Console.WriteLine(a.CompareTo(5)); // Throws ArgumentException
	/// Console.WriteLine(a.CompareTo(null)); // Output: 1
	/// ]]></code>
	/// </example>
	public int CompareTo(object? obj)
	{
		return obj switch
		{
			null => 1,
			Maybe<TIn> other => CompareTo(other),
			_ => throw new ArgumentException($"Object must be of type {typeof(Maybe<TIn>)}")
		};
	}

	/// <summary>
	/// Determines whether the left <see cref="Maybe{TIn}"/> is less than the right <see cref="Maybe{TIn}"/>.
	/// </summary>
	/// <param name="left">The left <see cref="Maybe{TIn}"/> to compare.</param>
	/// <param name="right">The right <see cref="Maybe{TIn}"/> to compare.</param>
	/// <returns><see langword="true"/> if the left <see cref="Maybe{TIn}"/> is less than the right <see cref="Maybe{TIn}"/>, otherwise <see langword="false"/>.</returns>
	public static bool operator <(Maybe<TIn> left, Maybe<TIn> right)
	{
		return left.CompareTo(right) < 0;
	}

	/// <summary>
	/// Determines whether the left <see cref="Maybe{TIn}"/> is less than or equal to the right <see cref="Maybe{TIn}"/>.
	/// </summary>
	/// <param name="left">The left <see cref="Maybe{TIn}"/> to compare.</param>
	/// <param name="right">The right <see cref="Maybe{TIn}"/> to compare.</param>
	/// <returns><see langword="true"/> if the left <see cref="Maybe{TIn}"/> is less than or equal to the right <see cref="Maybe{TIn}"/>, otherwise <see langword="false"/>.</returns>
	public static bool operator <=(Maybe<TIn> left, Maybe<TIn> right)
	{
		return left.CompareTo(right) <= 0;
	}

	/// <summary>
	/// Determines whether the left <see cref="Maybe{TIn}"/> is greater than the right <see cref="Maybe{TIn}"/>.
	/// </summary>
	/// <param name="left">The left <see cref="Maybe{TIn}"/> to compare.</param>
	/// <param name="right">The right <see cref="Maybe{TIn}"/> to compare.</param>
	/// <returns><see langword="true"/> if the left <see cref="Maybe{TIn}"/> is greater than the right <see cref="Maybe{TIn}"/>, otherwise <see langword="false"/>.</returns>
	public static bool operator >(Maybe<TIn> left, Maybe<TIn> right)
	{
		return left.CompareTo(right) > 0;
	}

	/// <summary>
	/// Determines whether the left <see cref="Maybe{TIn}"/> is greater than or equal to the right <see cref="Maybe{TIn}"/>.
	/// </summary>
	/// <param name="left">The left <see cref="Maybe{TIn}"/> to compare.</param>
	/// <param name="right">The right <see cref="Maybe{TIn}"/> to compare.</param>
	/// <returns><see langword="true"/> if the left <see cref="Maybe{TIn}"/> is greater than or equal to the right <see cref="Maybe{TIn}"/>, otherwise <see langword="false"/>.</returns>
	public static bool operator >=(Maybe<TIn> left, Maybe<TIn> right)
	{
		return left.CompareTo(right) >= 0;
	}
}


/// <summary>
/// Provides factory methods for creating Maybe instances.
/// </summary>
/// <example>
/// <code><![CDATA[
/// // Create a Maybe with a value
/// Maybe<int> maybeInt = Maybe.Some(10);
///
/// // Create a Maybe in the None state, explicitly specifying the type as string
/// Maybe<string> maybeString = Maybe.None<string>();
/// ]]></code>
/// </example>
public static class Maybe
{
	/// <summary>
	/// Creates a new Maybe containing the specified value.
	/// </summary>
	/// <typeparam name="TIn">The type of the value.</typeparam>
	/// <param name="value">The non-null value to contain.</param>
	/// <returns>An instance of <see cref="Maybe{TIn}"/> containing the specified value of type <typeparamref name="TIn"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
	/// <example>
	/// <code><![CDATA[
	/// Maybe<int> maybeInt = Maybe.Some(5);
	/// ]]></code>
	/// </example>
	public static Maybe<TIn> Some<TIn>(TIn value)
	{
		Guards.ThrowIfNull(value  , nameof(value));
		return new Maybe<TIn>(value);
	}

	/// <summary>
	/// Creates a new Maybe in the None state.
	/// </summary>
	/// <typeparam name="TIn">The type of the Maybe.</typeparam>
	/// <returns>A new Maybe in the None state.</returns>
	public static Maybe<TIn> None<TIn>() => new();
}