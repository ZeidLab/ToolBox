using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Options
{
    /// <summary>
    /// Provides extension methods to perform side‐effects on a <see cref="Maybe{TIn}"/> instance,
    /// following a railway-oriented programming style.
    /// <para>
    /// The <see cref="Maybe{TIn}"/> type represents the Optional Monad, which encapsulates an optional value.
    /// Using this monad helps to improve software performance by avoiding null-reference errors,
    /// and it eases the programmer’s life by clearly separating the presence or absence of a value.
    /// </para>
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// // Example: Using TapIfSome to write the value to the console if it exists.
    /// Maybe<int> maybe = Maybe.Some(42);
    /// maybe.TapIfSome(value => Console.WriteLine($"Value: {value}"));
    /// // Output: Value: 42
    ///
    /// // Example: Using TapIfNone to perform an action when there is no value.
    /// Maybe<int> noValue = Maybe.None<int>();
    /// noValue.TapIfNone(() => Console.WriteLine("No value available"));
    /// // Output: No value available
    /// ]]></code>
    /// </example>
    public static class MaybeExtensionsTap
    {
        #region Maybe<TIn>

        /// <summary>
        /// Executes the specified <paramref name="action"/> on the content of the <see cref="Maybe{TIn}"/> instance
        /// if it contains a value (i.e. it is in a <see cref="Maybe{TIn}.IsSome"/> state).
        /// The method then returns the original instance, enabling fluent chaining.
        /// </summary>
        /// <typeparam name="TIn">
        /// The type of the content in the <see cref="Maybe{TIn}"/> instance.
        /// Must be a non-nullable type.
        /// </typeparam>
        /// <param name="self">
        /// The <see cref="Maybe{TIn}"/> instance on which to perform the action.
        /// </param>
        /// <param name="action">
        /// The <see cref="Action{TIn}"/> to execute if <paramref name="self"/> contains a value.
        /// The provided value is passed as an argument.
        /// </param>
        /// <returns>
        /// The original <see cref="Maybe{TIn}"/> instance.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// // Common use-case: performing a side-effect on a valid value.
        /// Maybe<int> maybe = Maybe.Some(42);
        /// maybe.TapIfSome(value => Console.WriteLine($"The value is {value}"));
        /// // Expected output: "The value is 42"
        ///
        /// // Edge-case: when no value is present, the action is not executed.
        /// Maybe<int> none = Maybe.None<int>();
        /// none.TapIfSome(value => Console.WriteLine("This will not be printed"));
        /// ]]></code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Maybe<TIn> TapIfSome<TIn>(this Maybe<TIn> self, Action<TIn> action)
            where TIn : notnull
        {
            if (!self.IsNull)
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
                action(self.Value);
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.
            return self;
        }

        /// <summary>
        /// Executes the specified <paramref name="action"/> if the <see cref="Maybe{TIn}"/> instance
        /// does not contain a value (i.e. it is in a <see cref="Maybe{TIn}.IsNone"/> state).
        /// The method then returns the original instance, facilitating fluent method chaining.
        /// </summary>
        /// <typeparam name="TIn">
        /// The type of the content in the <see cref="Maybe{TIn}"/> instance.
        /// Must be a non-nullable type.
        /// </typeparam>
        /// <param name="self">
        /// The <see cref="Maybe{TIn}"/> instance to evaluate.
        /// </param>
        /// <param name="action">
        /// The <see cref="Action"/> to execute if <paramref name="self"/> does not contain a value.
        /// </param>
        /// <returns>
        /// The original <see cref="Maybe{TIn}"/> instance.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// // Common use-case: handling the absence of a value.
        /// Maybe<int> none = Maybe.None<int>();
        /// none.TapIfNone(() => Console.WriteLine("No value found"));
        /// // Expected output: "No value found"
        ///
        /// // Edge-case: if a value exists, the action is not executed.
        /// Maybe<int> some = Maybe.Some(10);
        /// some.TapIfNone(() => Console.WriteLine("This will not be printed"));
        /// ]]></code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Maybe<TIn> TapIfNone<TIn>(this Maybe<TIn> self, Action action)
            where TIn : notnull
        {
            if (self.IsNull)
#pragma warning disable CA1062
                action();
#pragma warning restore CA1062
            return self;
        }

        #endregion

        #region Task<Maybe<TIn>>

        /// <summary>
        /// Asynchronously executes the specified <paramref name="action"/> on the value contained within a <see cref="Maybe{TIn}"/>
        /// instance that is produced by the provided <see cref="Task"/>. The action is executed only if the instance is in a
        /// <see cref="Maybe{TIn}.IsSome"/> state. The method returns the resulting <see cref="Maybe{TIn}"/> instance.
        /// </summary>
        /// <typeparam name="TIn">
        /// The type of the content in the <see cref="Maybe{TIn}"/> instance.
        /// Must be a non-nullable type.
        /// </typeparam>
        /// <param name="self">
        /// A <see cref="Task"/> that produces a <see cref="Maybe{TIn}"/> instance.
        /// </param>
        /// <param name="action">
        /// A function that takes the value (if present) and returns a <see cref="Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that resolves to the original <see cref="Maybe{TIn}"/> instance.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// // Example: Asynchronously tapping a value when it is present.
        /// Task<Maybe<int>> maybeTask = Task.FromResult(Maybe.Some(42));
        /// await maybeTask.TapIfSomeAsync(async value => {
        ///     await Task.Delay(10); // Simulate asynchronous work.
        ///     Console.WriteLine($"Async value: {value}");
        /// });
        /// // Expected output: "Async value: 42"
        ///
        /// // Edge-case: If no value is present, the action is skipped.
        /// Task<Maybe<int>> noneTask = Task.FromResult(Maybe.None<int>());
        /// await noneTask.TapIfSomeAsync(async value => Console.WriteLine("This will not be printed"));
        /// ]]></code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<Maybe<TIn>> TapIfSomeAsync<TIn>(this Task<Maybe<TIn>> self, Func<TIn, Task> action)
            where TIn : notnull
        {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
            var result = await self.ConfigureAwait(false);
            if (!result.IsNull)
                await action(result.Value).ConfigureAwait(false);
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.
            return result;
        }

        /// <summary>
        /// Asynchronously executes the specified <paramref name="action"/> if the <see cref="Maybe{TIn}"/> instance
        /// produced by the provided <see cref="Task"/> does not contain a value (i.e. is in a <see cref="Maybe{TIn}.IsNone"/> state).
        /// The method then returns the original <see cref="Maybe{TIn}"/> instance.
        /// </summary>
        /// <typeparam name="TIn">
        /// The type of the content in the <see cref="Maybe{TIn}"/> instance.
        /// Must be a non-nullable type.
        /// </typeparam>
        /// <param name="self">
        /// A <see cref="Task"/> that produces a <see cref="Maybe{TIn}"/> instance.
        /// </param>
        /// <param name="action">
        /// A function that returns a <see cref="Task"/>, to be executed if <paramref name="self"/> contains no value.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that resolves to the original <see cref="Maybe{TIn}"/> instance.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// // Example: Asynchronously handling the absence of a value.
        /// Task<Maybe<int>> noneTask = Task.FromResult(Maybe.None<int>());
        /// await noneTask.TapIfNoneAsync(async () => {
        ///     await Task.Delay(10); // Simulate asynchronous work.
        ///     Console.WriteLine("No async value found");
        /// });
        /// // Expected output: "No async value found"
        ///
        /// // Edge-case: If a value exists, the action is not executed.
        /// Task<Maybe<int>> someTask = Task.FromResult(Maybe.Some(99));
        /// await someTask.TapIfNoneAsync(async () => Console.WriteLine("This will not be printed"));
        /// ]]></code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<Maybe<TIn>> TapIfNoneAsync<TIn>(this Task<Maybe<TIn>> self, Func<Task> action)
            where TIn : notnull
        {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1062
            var result = await self.ConfigureAwait(false);
            if (result.IsNull)
                await action().ConfigureAwait(false);
#pragma warning restore CA1062
#pragma warning restore CS8604 // Possible null reference argument.
            return result;
        }

        #endregion

        #region Obsolete

        /// <summary>
        /// Executes either the <paramref name="some"/> action if the <see cref="Maybe{TIn}"/> instance contains a value,
        /// or the <paramref name="none"/> action if it does not.
        /// <para>
        /// <b>Note:</b> This method is obsolete.
        /// It is recommended to use <see cref="TapIfSome{TIn}"/> and <see cref="TapIfNone{TIn}"/>
        /// (or their asynchronous counterparts) instead.
        /// </para>
        /// </summary>
        /// <typeparam name="TIn">
        /// The type of the content in the <see cref="Maybe{TIn}"/> instance.
        /// Must be a non-nullable type.
        /// </typeparam>
        /// <param name="self">
        /// The <see cref="Maybe{TIn}"/> instance to evaluate.
        /// </param>
        /// <param name="some">
        /// The <see cref="Action{TIn}"/> to execute if <paramref name="self"/> contains a value.
        /// </param>
        /// <param name="none">
        /// The <see cref="Action"/> to execute if <paramref name="self"/> does not contain a value.
        /// </param>
        /// <returns>
        /// The original <see cref="Maybe{TIn}"/> instance.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// // Example: Handling both cases with the obsolete Tap method.
        /// Maybe<int> maybe = Maybe.Some(42);
        /// maybe.Tap(
        ///     some: value => Console.WriteLine($"Value exists: {value}"),
        ///     none: () => Console.WriteLine("No value available")
        /// );
        /// // Expected output when value exists: "Value exists: 42"
        ///
        /// // Example with no value:
        /// Maybe<int> none = Maybe.None<int>();
        /// none.Tap(
        ///     some: value => Console.WriteLine($"Value exists: {value}"),
        ///     none: () => Console.WriteLine("No value available")
        /// );
        /// // Expected output: "No value available"
        /// ]]></code>
        /// </example>
#pragma warning disable S1133
        [Obsolete(
            "Use (TapIfSome/TapIfNone) or (TapIfSomeAsync/TapIfNoneAsync) instead. This is going to be removed on future versions",
            true)]
#pragma warning restore S1133
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Maybe<TIn> Tap<TIn>(this Maybe<TIn> self, Action<TIn> some, Action none)
            where TIn : notnull
        {
            if (self.IsNull)
#pragma warning disable CA1062
                none();
#pragma warning restore CA1062
            else
#pragma warning disable CA1062
#pragma warning disable CS8604 // Possible null reference argument.
                some(self.Value);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CA1062
            return self;
        }

        #endregion
    }
}
