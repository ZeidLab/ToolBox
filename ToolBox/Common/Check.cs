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
    private static readonly bool IsReferenceType;
    private static readonly bool IsNullable = Nullable.GetUnderlyingType(typeof (TIn)) is not null;

    static Check() => IsReferenceType = !typeof (TIn).GetTypeInfo().IsValueType;

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