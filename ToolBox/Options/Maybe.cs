// ReSharper disable once CheckNamespace
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Options;

/// <summary>
/// Represents an optional value that may or may not be present.
/// This struct is similar to a monad that can hold a value or be in a 'None' state.
/// </summary>
/// <typeparam name="TIn">The type of the contained value.</typeparam>
public readonly record struct Maybe<TIn> : IComparable<Maybe<TIn>>, IComparable
{
    /// <summary>
    /// keeping the value of the maybe, it can be null or default either
    /// </summary>
    internal readonly TIn? Content;
    
    /// <summary>
    /// Setting flag if the value is null
    /// </summary>
    internal readonly bool IsNull;
    
    /// <summary>
    /// Indicates whether the contained value is the default value of its type.
    /// </summary>
    public readonly bool IsDefault;

    /// <summary>
    /// Initializes a new instance of the <see cref="Maybe{TIn}"/> struct with the specified content.
    /// This constructor is intended for internal use.
    /// </summary>
    /// <param name="content">The content of the maybe.</param>
    private Maybe(TIn? content)
    {
        Content = content;
        IsNull = false;
        IsDefault = content.IsDefault();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Maybe{TIn}"/> struct representing the 'None' state.
    /// </summary>
    public Maybe()
    {
        IsNull = true;
        IsDefault = true;
        Content = default;
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
    /// Creates a new <see cref="Maybe{TIn}"/> containing the specified value.
    /// this is not going to automatically convert null or default to none.
    /// </summary>
    /// <param name="value">The non-null value to contain, and You should not pass default as value either.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    /// <returns>A new <see cref="Maybe{TIn}"/> containing the specified value.</returns>
    public static Maybe<TIn> Some(TIn value)
    {
        if (value.IsNull())
            throw new ArgumentNullException(nameof(value));
        return new Maybe<TIn>(value);
    }

    /// <summary>
    /// Creates a new <see cref="Maybe{TIn}"/> in the 'None' state.
    /// </summary>
    /// <returns>A new <see cref="Maybe{TIn}"/> in the 'None' state.</returns>
    public static Maybe<TIn> None() => new();

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="TIn"/> to a <see cref="Maybe{TIn}"/> containing that value.
    /// </summary>
    /// <param name="value">The value to contain.</param>
    /// <returns>A <see cref="Maybe{TIn}"/> containing the specified value.</returns>
    public static implicit operator Maybe<TIn>(TIn value) => Some(value);

    /// <summary>
    /// Compares this instance with another <see cref="Maybe{TIn}"/> instance.
    /// </summary>
    /// <param name="other">The other instance to compare with.</param>
    /// <returns>
    /// A value indicating the relative order of this instance and <paramref name="other"/>.
    /// </returns>
    public int CompareTo(Maybe<TIn> other)
    {
        // Handle null states
        if (IsNull && other.IsNull) return 0;
        if (IsNull) return -1;
        if (other.IsNull) return 1;

        // Compare content
#pragma warning disable CS8604 // Possible null reference argument.
        return Comparer<TIn>.Default.Compare(Content, other.Content);
#pragma warning restore CS8604 // Possible null reference argument.
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
        if (obj is null) return 1;
        if (obj is Maybe<TIn> other)
            return CompareTo(other);
        throw new ArgumentException($"Object must be of type {typeof(Maybe<TIn>)}");
    }
}
