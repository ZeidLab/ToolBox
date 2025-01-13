using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Common;

/// <summary>
/// an elegant way to find out if the value is null or default,
/// I have learned this from LanguageExt package
/// </summary>
/// <typeparam name="TIn">Type of the value</typeparam>
internal static class Check<TIn>
{
    // ReSharper disable once StaticMemberInGenericType
#pragma warning disable S2743 // A static field in a generic type is not shared among instances of different close constructed types
    private static readonly bool IsReferenceType;
#pragma warning restore S2743 // A static field in a generic type is not shared among instances of different close constructed types
    private static readonly bool IsNullable = Nullable.GetUnderlyingType(typeof(TIn)) is not null;
#pragma warning disable S3963 // Initialize all 'static fields' inline and remove the 'static constructor'.
#pragma warning disable CA1810
	static Check() => IsReferenceType = !typeof(TIn).GetTypeInfo().IsValueType;
#pragma warning restore CA1810
#pragma warning restore S3963 // Initialize all 'static fields' inline and remove the 'static constructor'.

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CS8604 // Possible null reference argument.
    internal static bool IsDefault(TIn value) => EqualityComparer<TIn>.Default.Equals(value, default(TIn));
#pragma warning restore CS8604 // Possible null reference argument.

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsNull(TIn value)
    {
        if (IsReferenceType && value is null)
            return true;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        return IsNullable && value.Equals(null);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}