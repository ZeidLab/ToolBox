using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Options
{
    /// <summary>
    /// Provides extension methods for binding operations on the optional monad type
    /// <see cref="Maybe{TIn}"/> and on asynchronous tasks returning <see cref="Maybe{TIn}"/>.
    /// <para>
    /// These methods implement the railway-oriented programming paradigm, allowing fluent
    /// chaining of operations while handling the presence or absence of a value safely.
    /// The optional monad improves overall performance by reducing null reference errors
    /// and eases the life of a programmer by making error handling and value transformations
    /// more predictable.
    /// </para>
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// // Example usage with a synchronous Maybe<T>:
    /// var maybeNumber = Maybe.Some(10);
    /// var maybeString = maybeNumber.Bind(num => Maybe.Some(num.ToString()));
    /// // maybeString now contains "10"
    ///
    /// // Example with a None value:
    /// var maybeNone = Maybe.None<int>();
    /// var result = maybeNone.Bind(num => Maybe.Some(num.ToString()));
    /// // result is None
    /// ]]></code>
    /// </example>
    public static class MaybeExtensionsBind
    {
        #region Maybe<TIn>

        /// <summary>
        /// Maps the content of a <see cref="Maybe{TIn}"/> instance to a new <see cref="Maybe{TOut}"/>
        /// instance using the provided synchronous mapping function.
        /// <para>
        /// If <paramref name="self"/> contains a value, the function <paramref name="map"/>
        /// is invoked to transform the value to type <typeparamref name="TOut"/>; otherwise,
        /// a None instance is returned.
        /// </para>
        /// </summary>
        /// <typeparam name="TIn">
        /// The type of the content in the original <see cref="Maybe{TIn}"/> instance.
        /// </typeparam>
        /// <typeparam name="TOut">
        /// The type of the content in the resulting <see cref="Maybe{TOut}"/> instance.
        /// </typeparam>
        /// <param name="self">
        /// The original <see cref="Maybe{TIn}"/> instance to be mapped.
        /// </param>
        /// <param name="map">
        /// The mapping function to apply if <paramref name="self"/> contains a value.
        /// It takes a value of type <typeparamref name="TIn"/> and returns a <see cref="Maybe{TOut}"/>.
        /// </param>
        /// <returns>
        /// A new <see cref="Maybe{TOut}"/> instance with the mapped content if <paramref name="self"/>
        /// is Some; otherwise, a new None instance.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// // Given a Maybe containing an integer:
        /// var maybeNumber = Maybe.Some(42);
        ///
        /// // Transform the integer to its string representation:
        /// var maybeString = maybeNumber.Bind(num => Maybe.Some(num.ToString()));
        ///
        /// // Expected output: maybeString contains "42"
        /// Console.WriteLine(maybeString); // Output depends on the implementation of Maybe
        /// ]]></code>
        /// </example>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Maybe<TOut> Bind<TIn, TOut>(this Maybe<TIn> self, Func<TIn, Maybe<TOut>> map)
            where TOut : notnull
            where TIn : notnull =>
    #pragma warning disable CS8604 // Possible null reference argument.
    #pragma warning disable CA1062
                !self.IsNull ? map(self.Value) : Maybe.None<TOut>();
    #pragma warning restore CA1062
    #pragma warning restore CS8604 // Possible null reference argument.

        /// <summary>
        /// Asynchronously maps the content of a <see cref="Maybe{TIn}"/> instance to a new
        /// <see cref="Maybe{TOut}"/> instance using the provided asynchronous mapping function.
        /// <para>
        /// If <paramref name="self"/> contains a value, the asynchronous function <paramref name="map"/>
        /// is invoked; otherwise, a None instance wrapped in a <see cref="Task"/> is returned.
        /// </para>
        /// </summary>
        /// <typeparam name="TIn">
        /// The type of the content in the original <see cref="Maybe{TIn}"/> instance.
        /// </typeparam>
        /// <typeparam name="TOut">
        /// The type of the content in the resulting <see cref="Maybe{TOut}"/> instance.
        /// </typeparam>
        /// <param name="self">
        /// The original <see cref="Maybe{TIn}"/> instance to be mapped.
        /// </param>
        /// <param name="map">
        /// The asynchronous mapping function to apply if <paramref name="self"/> contains a value.
        /// It takes a value of type <typeparamref name="TIn"/> and returns a <see cref="Task"/> containing
        /// a <see cref="Maybe{TOut}"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that resolves to a new <see cref="Maybe{TOut}"/> instance with the mapped content
        /// if <paramref name="self"/> is Some; otherwise, a new None instance.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// // Given a Maybe containing an integer:
        /// var maybeNumber = Maybe.Some(5);
        ///
        /// // Asynchronously transform the integer to its string representation:
        /// Task<Maybe<string>> maybeStringTask = maybeNumber.BindAsync(async num => {
        ///     await Task.Delay(100); // Simulate asynchronous work
        ///     return Maybe.Some(num.ToString());
        /// });
        ///
        /// // Expected output: Task resolves to a Maybe containing "5"
        /// var maybeString = await maybeStringTask;
        /// Console.WriteLine(maybeString); // Output depends on the implementation of Maybe
        /// ]]></code>
        /// </example>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<Maybe<TOut>> BindAsync<TIn, TOut>(this Maybe<TIn> self, Func<TIn, Task<Maybe<TOut>>> map)
            where TOut : notnull
            where TIn : notnull =>
    #pragma warning disable CA1062
    #pragma warning disable CS8604 // Possible null reference argument.
                !self.IsNull ? map(self.Value) : Maybe.None<TOut>().AsTaskAsync();
    #pragma warning restore CS8604 // Possible null reference argument.
    #pragma warning restore CA1062

        #endregion

        #region Task<Maybe<TIn>>

        /// <summary>
        /// Asynchronously maps the content of a <see cref="Task"/> returning a <see cref="Maybe{TIn}"/>
        /// instance to a new <see cref="Maybe{TOut}"/> instance using the provided synchronous mapping function.
        /// <para>
        /// This extension method awaits the task to retrieve the <see cref="Maybe{TIn}"/> instance and
        /// then applies the synchronous <see cref="Bind{TIn, TOut}"/> method.
        /// </para>
        /// </summary>
        /// <typeparam name="TIn">
        /// The type of the content in the original <see cref="Maybe{TIn}"/> instance.
        /// </typeparam>
        /// <typeparam name="TOut">
        /// The type of the content in the resulting <see cref="Maybe{TOut}"/> instance.
        /// </typeparam>
        /// <param name="self">
        /// A <see cref="Task"/> that resolves to a <see cref="Maybe{TIn}"/> instance.
        /// </param>
        /// <param name="map">
        /// The mapping function to apply if the resolved <paramref name="self"/> contains a value.
        /// It takes a value of type <typeparamref name="TIn"/> and returns a <see cref="Maybe{TOut}"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that resolves to a new <see cref="Maybe{TOut}"/> instance with the mapped content
        /// if the original task's result is Some; otherwise, a new None instance.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// // Given a task returning a Maybe containing an integer:
        /// Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.Some(20));
        ///
        /// // Map the integer to its string representation synchronously after awaiting:
        /// Task<Maybe<string>> maybeStringTask = maybeTask.BindAsync(num => Maybe.Some(num.ToString()));
        ///
        /// // Expected output: Task resolves to a Maybe containing "20"
        /// var maybeString = await maybeStringTask;
        /// Console.WriteLine(maybeString); // Output depends on the implementation of Maybe
        /// ]]></code>
        /// </example>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<Maybe<TOut>> BindAsync<TIn, TOut>(this Task<Maybe<TIn>> self, Func<TIn, Maybe<TOut>> map)
            where TOut : notnull
            where TIn : notnull =>
    #pragma warning disable CS8604 // Possible null reference argument.
    #pragma warning disable CA1062
                (await self.ConfigureAwait(false)).Bind(map);
    #pragma warning restore CA1062
    #pragma warning restore CS8604 // Possible null reference argument.

        /// <summary>
        /// Asynchronously maps the content of a <see cref="Task"/> returning a <see cref="Maybe{TIn}"/>
        /// instance to a new <see cref="Maybe{TOut}"/> instance using the provided asynchronous mapping function.
        /// <para>
        /// This extension method awaits the task to retrieve the <see cref="Maybe{TIn}"/> instance
        /// and then applies the asynchronous mapping function.
        /// </para>
        /// </summary>
        /// <typeparam name="TIn">
        /// The type of the content in the original <see cref="Maybe{TIn}"/> instance.
        /// </typeparam>
        /// <typeparam name="TOut">
        /// The type of the content in the resulting <see cref="Maybe{TOut}"/> instance.
        /// </typeparam>
        /// <param name="self">
        /// A <see cref="Task"/> that resolves to a <see cref="Maybe{TIn}"/> instance.
        /// </param>
        /// <param name="map">
        /// The asynchronous mapping function to apply if the resolved <paramref name="self"/> contains a value.
        /// It takes a value of type <typeparamref name="TIn"/> and returns a <see cref="Task"/> containing a
        /// <see cref="Maybe{TOut}"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that resolves to a new <see cref="Maybe{TOut}"/> instance with the mapped content
        /// if the original task's result is Some; otherwise, a new None instance.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// // Given a task returning a Maybe containing an integer:
        /// Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.Some(30));
        ///
        /// // Asynchronously map the integer to its string representation:
        /// Task<Maybe<string>> maybeStringTask = maybeTask.BindAsync(async num => {
        ///     await Task.Delay(50); // Simulate asynchronous work
        ///     return Maybe.Some(num.ToString());
        /// });
        ///
        /// // Expected output: Task resolves to a Maybe containing "30"
        /// var maybeString = await maybeStringTask;
        /// Console.WriteLine(maybeString); // Output depends on the implementation of Maybe
        /// ]]></code>
        /// </example>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<Maybe<TOut>> BindAsync<TIn, TOut>(this Task<Maybe<TIn>> self, Func<TIn, Task<Maybe<TOut>>> map)
            where TOut : notnull
            where TIn : notnull =>
    #pragma warning disable CA1062
    #pragma warning disable CS8604 // Possible null reference argument.
                await (await self.ConfigureAwait(false)).BindAsync(map).ConfigureAwait(false);
    #pragma warning restore CS8604 // Possible null reference argument.
    #pragma warning restore CA1062

        #endregion
    }
}
