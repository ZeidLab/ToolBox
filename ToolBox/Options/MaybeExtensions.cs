using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
// ReSharper disable once CheckNamespace
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Options;

/// <summary>
/// Contains extension methods for the Maybe type.
/// </summary>
public static class MaybeExtensions
{
    /// <summary>
    /// Converts the provided object to a Maybe instance.
    /// </summary>
    /// <typeparam name="TIn">The type of the object.</typeparam>
    /// <param name="self">The object to convert.</param>
    /// <returns>A Maybe instance with the provided object as its content.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> ToSome<TIn>(this TIn? self) where TIn : notnull
        => Maybe<TIn>.Some(self!);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> ToNone<TIn>(this TIn? self)
        => Maybe<TIn>.None();

    /// <summary>
    /// Maps the content of a Maybe instance to a new Maybe instance using the provided mapping function.
    /// </summary>
    /// <typeparam name="TIn">The type of the content of the original Maybe instance.</typeparam>
    /// <typeparam name="TOut">The type of the content of the new Maybe instance.</typeparam>
    /// <param name="self">The original Maybe instance.</param>
    /// <param name="map">The mapping function.</param>
    /// <returns>A new Maybe instance with the mapped content, or a new None instance if the original Maybe instance is None.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TOut> Map<TIn, TOut>(this Maybe<TIn> self, Func<TIn, TOut> map)
        where TOut : notnull
        where TIn : notnull =>
        !self.IsNull ? Maybe<TOut>.Some(map(self.Content!)) : Maybe<TOut>.None();


    /// <summary>
    /// Ensures that the content of a Maybe instance satisfies a predicate.
    /// </summary>
    /// <example>
    /// <code>
    /// List&lt;Maybe&lt;int&gt;&gt; maybeList = new List&lt;Maybe&lt;int&gt;&gt;
    /// {
    ///     Maybe&lt;int&gt;.Some(2),
    ///     Maybe&lt;int&gt;.Some(10),
    ///     Maybe&lt;int&gt;.None(),
    ///     Maybe&lt;int&gt;.Some(42)
    /// };
    /// 
    /// var filtered = maybeList
    ///     .Where(x => x.If(value => value &gt; 12))
    ///     .ToList();
    /// 
    /// Console.WriteLine(filtered.Count); // Output: 2
    /// var result = filtered[0].if(value => value &gt; 0) ? filtered[0].Reduce(() => 0) : 42;
    /// Console.WriteLine(result); // Output: 2
    /// </code>
    /// </example>
    /// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
    /// <param name="self">The Maybe instance whose content to check.</param>
    /// <param name="predicate">The predicate to evaluate.</param>
    /// <returns>
    /// True if the Maybe instance is not None and its content satisfies the predicate.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool If<TIn>(this Maybe<TIn> self, Func<TIn, bool> predicate)
        where TIn : notnull => !self.IsNull && predicate(self.Content!);

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
        where TIn : notnull => self.Where(x => !x.IsNull && predicate(x.Content!));

    /// <summary>
    /// Converts a sequence of Maybe instances to an IEnumerable of their content, excluding None instances.
    /// </summary>
    /// <example>
    /// <code>
    /// var maybeList = new List&lt;Maybe&lt;int&gt;&gt;
    /// {
    ///     Maybe&lt;int&gt;.Some(1),
    ///     Maybe&lt;int&gt;.None(),
    ///     Maybe&lt;int&gt;.Some(3)
    /// };
    ///
    /// var result = maybeList.ToEnumerable(); // Output: [1, 3]
    /// </code>
    /// </example>
    /// <typeparam name="TIn">The type of the content within the Maybe instances.</typeparam>
    /// <param name="self">The sequence of Maybe instances to convert.</param>
    /// <returns>An IEnumerable of the content of the Maybe instances, excluding None instances.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<TIn> ToEnumerable<TIn>(this IEnumerable<Maybe<TIn>> self)
        where TIn : notnull => self.Where(x => !x.IsNull).Select(x => x.Content!);

    /// <summary>
    /// Converts a sequence of Maybe instances to an IEnumerable of their content,
    /// replacing None instances with a specified substitute value.
    /// </summary>
    /// <typeparam name="TIn">The type of the content within the Maybe instances.</typeparam>
    /// <param name="self">The sequence of Maybe instances to convert.</param>
    /// <param name="substitute">The substitute value to use for None instances.</param>
    /// <returns>An IEnumerable of the content of the Maybe instances, with None instances replaced by the substitute value.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<TIn> ToEnumerable<TIn>(this IEnumerable<Maybe<TIn>> self, TIn substitute)
        where TIn : notnull => self.Select(x => x.Reduce(substitute));

    /// <summary>
    /// Converts a sequence of Maybe instances to an IEnumerable of their content using a substitute function for None instances.
    /// </summary>
    /// <typeparam name="TIn">The type of the content within the Maybe instances.</typeparam>
    /// <param name="self">The sequence of Maybe instances to convert.</param>
    /// <param name="substitute">The function to provide a substitute value for None instances.</param>
    /// <returns>An IEnumerable of the content of the Maybe instances, replacing None instances with the result of the substitute function.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<TIn> ToEnumerable<TIn>(this IEnumerable<Maybe<TIn>> self, Func<TIn> substitute)
        where TIn : notnull => self.Select(x => x.Reduce(substitute));

    /// <summary>
    /// Reduces the content of a Maybe instance to a single value using the provided default value or function.
    /// </summary>
    /// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
    /// <param name="self">The Maybe instance to reduce.</param>
    /// <param name="substitute">The default value to use if the Maybe instance is None.</param>
    /// <returns>The content of the Maybe instance, or the default value if the Maybe instance is None.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TIn Reduce<TIn>(this Maybe<TIn> self, TIn substitute)
        where TIn : notnull =>
        !self.IsNull ? self.Content! : substitute;

    /// <summary>
    /// Reduces the content of a Maybe instance to a single value using the provided function.
    /// </summary>
    /// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
    /// <param name="self">The Maybe instance to reduce.</param>
    /// <param name="substitute">The function to use if the Maybe instance is None.</param>
    /// <returns>The content of the Maybe instance, or the result of the provided function if the Maybe instance is None.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TIn Reduce<TIn>(this Maybe<TIn> self, Func<TIn> substitute)
        where TIn : notnull =>
        !self.IsNull ? self.Content! : substitute();

    /// <summary>
    /// Applies the provided action to the content of the Maybe instance if it is Some.
    /// </summary>
    /// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
    /// <param name="self">The Maybe instance to apply the action to.</param>
    /// <param name="action">The action to apply to the content of the Maybe instance if it is Some.</param>
    /// <returns>The original Maybe instance.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> DoIfSome<TIn>(this Maybe<TIn> self, Action<TIn> action)
        where TIn : notnull
    {
        if (!self.IsNull)
            action(self.Content!);
        return self;
    }

    /// <summary>
    /// Executes the provided action if the Maybe instance is None.
    /// </summary>
    /// <typeparam name="TIn">The type of the content of the Maybe instance.</typeparam>
    /// <param name="self">The Maybe instance to check.</param>
    /// <param name="action">The action to execute if the Maybe instance is None.</param>
    /// <returns>The original Maybe instance.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TIn> DoIfNone<TIn>(this Maybe<TIn> self, Action action)
        where TIn : notnull
    {
        if (self.IsNull)
            action();
        return self;
    }
}