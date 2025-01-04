#nullable disable
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace ZeidLab.ToolBox.Common;

/// <summary>
/// A unit type is a type that allows only one value (and thus can hold no information)
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly record struct Unit : IComparable<Unit>
{
    public static readonly Unit Default = new();

    [Pure]
    public override string ToString() => "()";

    [Pure]
    public static bool operator >(Unit lhs, Unit rhs) => false;

    [Pure]
    public static bool operator >=(Unit lhs, Unit rhs) => true;

    [Pure]
    public static bool operator <(Unit lhs, Unit rhs) => false;

    [Pure]
    public static bool operator <=(Unit lhs, Unit rhs) => true;

    /// <summary>Provide an alternative value to unit</summary>
    /// <typeparam name="T">Alternative value type</typeparam>
    /// <param name="anything">Alternative value</param>
    /// <returns>Alternative value</returns>
    [Pure]
    public T Return<T>(T anything) => anything;

    /// <summary>Provide an alternative value to unit</summary>
    /// <typeparam name="T">Alternative value type</typeparam>
    /// <param name="anything">Alternative value</param>
    /// <returns>Alternative value</returns>
    [Pure]
    public T Return<T>(Func<T> anything) => anything();

    /// <summary>Always equal</summary>
    [Pure]
    public int CompareTo(Unit other) => 0;

    [Pure]
    public static Unit operator +(Unit a, Unit b) => Unit.Default;

    [Pure]
    public static implicit operator ValueTuple(Unit _) => new();

    [Pure]
    public static implicit operator Unit(ValueTuple _) => new();
}