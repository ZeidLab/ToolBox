using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Common;

internal static class Check<TIn>
{
    private static readonly bool IsReferenceType;
    private static readonly bool IsNullable = Nullable.GetUnderlyingType(typeof (TIn)) is not null;

    static Check() => IsReferenceType = !typeof (TIn).GetTypeInfo().IsValueType;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsDefault(TIn value) => EqualityComparer<TIn>.Default.Equals(value, default (TIn));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsNull(TIn value)
    {
        if (IsReferenceType && value is null)
            return true;
        return IsNullable && value!.Equals(null);
    }
}