#nullable disable
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Common;

/// <summary>
/// Represents a type that has only one possible value, often used to signify the absence of a meaningful value.
/// The <see cref="Unit"/> type is analogous to <see langword="void"/> in imperative programming but is used
/// in functional programming and railway-oriented programming (ROP) to provide a concrete type when no
/// meaningful value is returned. This is particularly useful in scenarios where methods must return a value
/// for chaining or composition, even when the operation itself does not produce a meaningful result.
///
/// In railway-oriented programming, the <see cref="Unit"/> type is commonly used with <see cref="Result{TValue}"/>
/// to represent successful operations that do not return a value. For example, a method that performs a side
/// effect (e.g., logging) can return <see cref="Result{Unit}"/> to indicate success or failure without needing
/// to return a specific value.
///
/// Key Features:
/// - Single Value: The <see cref="Unit"/> type has only one value, <see cref="Default"/>, which represents the
///   absence of a meaningful value.
/// - Lightweight: As a <see><cref>record struct</cref></see>
/// , it is a lightweight and efficient type.
/// - Compatibility: Can be implicitly converted to and from <see cref="ValueTuple"/> for interoperability with
///   other functional programming constructs.
///
/// Example Usage:
/// <code>
/// // A method that performs a side effect and returns Result&lt;Unit&gt;
/// Result&lt;Unit&gt; LogMessage(string message)
/// {
///     Console.WriteLine(message);
///     return Result&lt;Unit&gt;.Success(Unit.Default);
/// }
///
/// // Chaining methods in railway-oriented programming
/// Result&lt;Unit&gt; result = LogMessage("Hello, World!")
///     .Bind(_ => LogMessage("Another message."));
/// </code>
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public readonly record struct Unit : IComparable<Unit>
{
    /// <summary>
    /// The single instance of the <see cref="Unit"/> type. This is the only value that a <see cref="Unit"/>
    /// can hold, representing the absence of a meaningful value.
    /// </summary>
#pragma warning disable CA1805
    public static readonly Unit Default = new();
#pragma warning restore CA1805

    /// <summary>
    /// Returns a string representation of the current unit. This is always "()", which is a common
    /// representation of the unit type in functional programming.
    /// </summary>
    /// <returns>A string representation of the current unit.</returns>
    [Pure]
    public override string ToString() => "()";

    /// <summary>
    /// Always returns <see langword="false"/>. This operator is provided for completeness but has no
    /// meaningful behavior for the <see cref="Unit"/> type, as there is only one possible value.
    /// </summary>
    /// <param name="lhs">The left-hand side of the operator.</param>
    /// <param name="rhs">The right-hand side of the operator.</param>
    /// <returns><see langword="false"/>.</returns>
    [Pure]
    public static bool operator >(Unit lhs, Unit rhs) => false;

    /// <summary>
    /// Always returns <see langword="true"/>. This operator is provided for completeness but has no
    /// meaningful behavior for the <see cref="Unit"/> type, as there is only one possible value.
    /// </summary>
    /// <param name="lhs">The left-hand side of the operator.</param>
    /// <param name="rhs">The right-hand side of the operator.</param>
    /// <returns><see langword="true"/>.</returns>
    [Pure]
    public static bool operator >=(Unit lhs, Unit rhs) => true;

    /// <summary>
    /// Always returns <see langword="false"/>. This operator is provided for completeness but has no
    /// meaningful behavior for the <see cref="Unit"/> type, as there is only one possible value.
    /// </summary>
    /// <param name="lhs">The left-hand side of the operator.</param>
    /// <param name="rhs">The right-hand side of the operator.</param>
    /// <returns><see langword="false"/>.</returns>
    [Pure]
    public static bool operator <(Unit lhs, Unit rhs) => false;

    /// <summary>
    /// Always returns <see langword="true"/>. This operator is provided for completeness but has no
    /// meaningful behavior for the <see cref="Unit"/> type, as there is only one possible value.
    /// </summary>
    /// <param name="lhs">The left-hand side of the operator.</param>
    /// <param name="rhs">The right-hand side of the operator.</param>
    /// <returns><see langword="true"/>.</returns>
    [Pure]
    public static bool operator <=(Unit lhs, Unit rhs) => true;

    /// <summary>
    /// Provides an alternative value to <see cref="Unit"/>. This method is useful for transforming
    /// a <see cref="Unit"/> into a meaningful value when needed.
    /// </summary>
    /// <typeparam name="T">The type of the alternative value.</typeparam>
    /// <param name="anything">The alternative value to return.</param>
    /// <returns>The alternative value.</returns>
    [Pure]
    public static T Return<T>(T anything) => anything;


    /// <summary>
    /// Provides an alternative value to <see cref="Unit"/> by invoking a function. This method is useful
    /// for lazily producing a meaningful value when needed.
    /// </summary>
    /// <typeparam name="T">The type of the alternative value.</typeparam>
    /// <param name="anything">A function that produces the alternative value.</param>
    /// <returns>The alternative value produced by the function.</returns>
    [Pure]

#pragma warning disable CA1062
    public T Return<T>(Func<T> anything) => anything();
#pragma warning restore CA1062


    /// <summary>
    /// Always returns 0, as all instances of <see cref="Unit"/> are equal.
    /// </summary>
    /// <param name="other">The other <see cref="Unit"/> to compare to.</param>
    /// <returns>0, indicating equality.</returns>
    [Pure]
    public int CompareTo(Unit other) => 0;

    /// <summary>
    /// Combines two <see cref="Unit"/> values. Since <see cref="Unit"/> has only one possible value,
    /// this operation always returns <see cref="Default"/>.
    /// </summary>
    /// <param name="a">The first <see cref="Unit"/>.</param>
    /// <param name="b">The second <see cref="Unit"/>.</param>
    /// <returns><see cref="Default"/>.</returns>
    [Pure]
    public static Unit operator +(Unit a, Unit b) => Unit.Default;

    /// <summary>
    /// Implicitly converts a <see cref="Unit"/> to a <see cref="ValueTuple"/>. This allows for
    /// interoperability with other functional programming constructs that use <see cref="ValueTuple"/>.
    /// </summary>
    /// <param name="_">The <see cref="Unit"/> to convert.</param>
    /// <returns>An empty <see cref="ValueTuple"/>.</returns>
    [Pure]
    public static implicit operator ValueTuple(Unit _) => new();

    /// <summary>
    /// Implicitly converts a <see cref="ValueTuple"/> to a <see cref="Unit"/>. This allows for
    /// interoperability with other functional programming constructs that use <see cref="ValueTuple"/>.
    /// </summary>
    /// <param name="_">The <see cref="ValueTuple"/> to convert.</param>
    /// <returns>A <see cref="Unit"/>.</returns>
    [Pure]
    public static implicit operator Unit(ValueTuple _) => new();
}