using System.Diagnostics.CodeAnalysis;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Options;

/// <summary>
/// Represents an optional value that may or may not be present.
/// This struct is similar to a monad that can hold a value or be in a 'None' state.
/// </summary>
/// <typeparam name="TIn">The type of the contained value.</typeparam>
[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
public readonly record struct Maybe<TIn> : IComparable<Maybe<TIn>>, IComparable
{
	/// <summary>
	/// keeping the value of the maybe, it can be null or default either
	/// </summary>
	internal readonly TIn? Value;

	/// <summary>
	/// Setting flag if the value is null
	/// </summary>
	internal readonly bool IsNull;

	/// <summary>
	/// Indicates whether the contained value is the default value of its type.
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
	/// Indicates whether the maybe contains a value.
	/// </summary>
	public bool IsSome => !IsNull;

	/// <summary>
	/// Indicates whether the maybe is in the 'None' state.
	/// </summary>
	public bool IsNone => IsNull;




	/// <summary>
	/// Implicitly converts a value of type <typeparamref name="TIn"/> to a <see cref="Maybe{TIn}"/> containing that value.
	/// </summary>
	/// <param name="value">The value to contain.</param>
	/// <returns>A <see cref="Maybe{TIn}"/> containing the specified value.</returns>
	public static implicit operator Maybe<TIn>(TIn? value)
		=> value.IsNull() ? new Maybe<TIn>()  : new Maybe<TIn>(value!) ;


	/// <summary>
	/// Compares this instance with another <see cref="Maybe{TIn}"/> instance.
	/// </summary>
	/// <param name="other">The other instance to compare with.</param>
	/// <returns>
	/// A value indicating the relative order of this instance and <paramref name="other"/>.
	/// </returns>
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
	/// </returns>
	/// <exception cref="ArgumentException">Thrown if <paramref name="obj"/> is not of type <see cref="Maybe{TIn}"/>.</exception>
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
public static class Maybe
{
	/// <summary>
	/// Creates a new Maybe containing the specified value.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="value">The non-null value to contain.</param>
	/// <returns>A new Maybe containing the specified value.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
	public static Maybe<T> Some<T>(T value)
	{
		Guards.ThrowIfNull(value  , nameof(value));
		return new Maybe<T>(value);
	}

	/// <summary>
	/// Creates a new Maybe in the None state.
	/// </summary>
	/// <typeparam name="T">The type of the Maybe.</typeparam>
	/// <returns>A new Maybe in the None state.</returns>
	public static Maybe<T> None<T>() => new();
}