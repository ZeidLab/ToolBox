using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Options
{
    /// <summary>
    /// Provides extension methods for the <see cref="Maybe{TIn}"/> type that implement
    /// the optional monad pattern (railway–oriented programming). These methods allow you
    /// to transform a <see cref="Maybe{TIn}"/> into a result type
    /// by specifying functions to run when a value is present (the “some” branch) or absent
    /// (the “none” branch). Using the optional monad reduces explicit null checks, improves
    /// overall performance and code clarity, and eases the life of the programmer by enforcing
    /// a uniform error handling strategy.
    /// </summary>
    public static class MaybeExtensionsMatch
    {
        #region Maybe<TIn>

        /// <summary>
        /// Matches a <see cref="Maybe{TIn}"/> value by executing a specified function if a value is present
        /// or another function if the value is absent.
        /// </summary>
        /// <param name="self">
        /// The <see cref="Maybe{TIn}"/> instance to match.
        /// </param>
        /// <param name="some">
        /// A function that processes the contained value of type <typeparamref name="TIn"/> and returns a value of type <typeparamref name="TOut"/>.
        /// </param>
        /// <param name="none">
        /// A function that produces a default value of type <typeparamref name="TOut"/> when the <paramref name="self"/> is null.
        /// </param>
        /// <typeparam name="TIn">
        /// The type of the value contained in the optional monad; must be non-null.
        /// </typeparam>
        /// <typeparam name="TOut">
        /// The type of the output value; must be non-null.
        /// </typeparam>
        /// <returns>
        /// Returns the result of applying <paramref name="some"/> to the value if <paramref name="self"/> is not null;
        /// otherwise, returns the result of <paramref name="none"/>.
        /// </returns>
        /// <example>
        /// The following example shows how to use <c>Match</c> to extract an integer value from a <c>Maybe&lt;int&gt;</c>.
        /// <code><![CDATA[
        /// // Suppose maybeValue contains Some(10) or None.
        /// Maybe<int> maybeValue = GetMaybeValue();
        ///
        /// // Use Match to provide a default value (0) if maybeValue is None.
        /// int result = maybeValue.Match(
        ///     some: value => value * 2,
        ///     none: () => 0);
        ///
        /// // If maybeValue was Some(10), result becomes 20; if None, result is 0.
        /// Console.WriteLine(result);
        /// ]]></code>
        /// </example>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOut Match<TIn, TOut>(this Maybe<TIn> self, Func<TIn, TOut> some, Func<TOut> none)
            where TIn : notnull
            where TOut : notnull
        {
#pragma warning disable CA1062
#pragma warning disable CS8604 // Possible null reference argument.
            return !self.IsNull ? some(self.Value) : none();
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CA1062
        }

        /// <summary>
        /// Asynchronously matches a <see cref="Maybe{TIn}"/> value by executing an asynchronous function if a value is present
        /// or returning a specified asynchronous default value if absent.
        /// </summary>
        /// <param name="self">
        /// The <see cref="Maybe{TIn}"/> instance to match.
        /// </param>
        /// <param name="some">
        /// An asynchronous function that processes the contained value of type <typeparamref name="TIn"/>
        /// and returns a <see cref="Task"/> that produces a result of type <typeparamref name="TOut"/>.
        /// </param>
        /// <param name="none">
        /// A <see cref="Task"/> that produces a default result of type <typeparamref name="TOut"/> when the value is absent.
        /// </param>
        /// <typeparam name="TIn">
        /// The type of the contained value; must be non-null.
        /// </typeparam>
        /// <typeparam name="TOut">
        /// The type of the result; must be non-null.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Task"/> that produces the result of applying <paramref name="some"/> if a value is present,
        /// or the result from <paramref name="none"/> if absent.
        /// </returns>
        /// <example>
        /// The example demonstrates using <c>MatchAsync</c> with asynchronous functions.
        /// <code><![CDATA[
        /// // Assume GetMaybeValueAsync returns a Maybe<int>
        /// Maybe<int> maybeValue = await GetMaybeValueAsync();
        ///
        /// // Multiply the contained value by 3 asynchronously or return a default value.
        /// Task<int> resultTask = maybeValue.MatchAsync(
        ///     some: value => Task.FromResult(value * 3),
        ///     none: Task.FromResult(100));
        ///
        /// int result = await resultTask;
        /// // If maybeValue contains 10, result becomes 30; if None, result is 100.
        /// Console.WriteLine(result);
        /// ]]></code>
        /// </example>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<TOut> MatchAsync<TIn, TOut>(this Maybe<TIn> self, Func<TIn, Task<TOut>> some, Task<TOut> none)
            where TIn : notnull
            where TOut : notnull
        {
#pragma warning disable CA1062
#pragma warning disable CS8604 // Possible null reference argument.
            return !self.IsNull ? some(self.Value) : none;
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CA1062
        }

        /// <summary>
        /// Matches a <see cref="Maybe{TIn}"/> value and executes one of two actions: one if a value is present,
        /// and another if it is absent.
        /// </summary>
        /// <param name="self">
        /// The <see cref="Maybe{TIn}"/> instance to process.
        /// </param>
        /// <param name="some">
        /// An action to execute using the contained value of type <typeparamref name="TIn"/> when present.
        /// </param>
        /// <param name="none">
        /// An action to execute when the <paramref name="self"/> is null.
        /// </param>
        /// <typeparam name="TIn">
        /// The type of the contained value; must be non-null.
        /// </typeparam>
        /// <example>
        /// In the example below, <c>Match</c> is used to print a message depending on whether the optional contains a value.
        /// <code><![CDATA[
        /// Maybe<string> maybeName = GetMaybeName();
        ///
        /// maybeName.Match(
        ///     some: name => Console.WriteLine($"Hello, {name}!"),
        ///     none: () => Console.WriteLine("Hello, guest!"));
        /// ]]></code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Match<TIn>(this Maybe<TIn> self, Action<TIn> some, Action none)
            where TIn : notnull
        {
#pragma warning disable CA1062
#pragma warning disable CS8604 // Possible null reference argument.
            if (!self.IsNull)
                some(self.Value);
            else
                none();
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CA1062
        }

        /// <summary>
        /// Asynchronously matches a <see cref="Maybe{TIn}"/> value by executing an asynchronous action if a value is present
        /// or awaiting a specified asynchronous action if absent.
        /// </summary>
        /// <param name="self">
        /// The <see cref="Maybe{TIn}"/> instance to process.
        /// </param>
        /// <param name="some">
        /// An asynchronous function to execute with the contained value when present.
        /// </param>
        /// <param name="none">
        /// A <see cref="Task"/> representing the asynchronous action to perform when no value is present.
        /// </param>
        /// <typeparam name="TIn">
        /// The type of the contained value; must be non-null.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Task"/> representing the completion of the matching operation.
        /// </returns>
        /// <example>
        /// The following example uses <c>MatchAsync</c> to perform an asynchronous logging operation.
        /// <code><![CDATA[
        /// Maybe<int> maybeValue = GetMaybeNumber();
        ///
        /// // If maybeValue is present, log the value asynchronously; otherwise, log a default message.
        /// await maybeValue.MatchAsync(
        ///     some: value => LogAsync($"Value is: {value}"),
        ///     none: LogAsync("No value provided"));
        /// ]]></code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task MatchAsync<TIn>(this Maybe<TIn> self, Func<TIn, Task> some, Task none)
            where TIn : notnull
        {
#pragma warning disable CA1062
#pragma warning disable CS8604 // Possible null reference argument.
            return !self.IsNull ? some(self.Value) : none;
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CA1062
        }

        #endregion

        #region Task<Maybe<TIn>>

        /// <summary>
        /// Awaits a <see cref="Task"/> that produces a <see cref="Maybe{TIn}"/> and then matches its result synchronously.
        /// </summary>
        /// <param name="self">
        /// A <see cref="Task"/> producing a <see cref="Maybe{TIn}"/> value.
        /// </param>
        /// <param name="some">
        /// A function to apply to the contained value if it is present.
        /// </param>
        /// <param name="none">
        /// A function that returns a default value when the produced <see cref="Maybe{TIn}"/> is null.
        /// </param>
        /// <typeparam name="TIn">
        /// The type contained in the <see cref="Maybe{TIn}"/>; must be non-null.
        /// </typeparam>
        /// <typeparam name="TOut">
        /// The type of the output value; must be non-null.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Task"/> producing the result of type <typeparamref name="TOut"/>.
        /// </returns>
        /// <example>
        /// This example shows how to await a task that returns a <c>Maybe&lt;string&gt;</c> and then select an output.
        /// <code><![CDATA[
        /// // Assume GetMaybeNameAsync returns Task<Maybe<string>>
        /// Task<Maybe<string>> maybeNameTask = GetMaybeNameAsync();
        ///
        /// // If a name is found, convert it to uppercase; otherwise, use "UNKNOWN".
        /// string result = await maybeNameTask.MatchAsync(
        ///     some: name => name.ToUpper(),
        ///     none: () => "UNKNOWN");
        ///
        /// Console.WriteLine(result);
        /// ]]></code>
        /// </example>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<TOut> MatchAsync<TIn, TOut>(
            this Task<Maybe<TIn>> self,
            Func<TIn, TOut> some,
            Func<TOut> none)
            where TIn : notnull
            where TOut : notnull
        {
#pragma warning disable CA1062
            return (await self.ConfigureAwait(false)).Match(some: some, none: none);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Awaits a <see cref="Task"/> producing a <see cref="Maybe{TIn}"/> and asynchronously matches its result.
        /// </summary>
        /// <param name="self">
        /// A <see cref="Task"/> producing a <see cref="Maybe{TIn}"/> value.
        /// </param>
        /// <param name="some">
        /// An asynchronous function to apply to the contained value if it is present.
        /// </param>
        /// <param name="none">
        /// A <see cref="Task"/> representing the default asynchronous result when no value is present.
        /// </param>
        /// <typeparam name="TIn">
        /// The type contained in the <see cref="Maybe{TIn}"/>; must be non-null.
        /// </typeparam>
        /// <typeparam name="TOut">
        /// The type of the output value; must be non-null.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Task"/> producing a result of type <typeparamref name="TOut"/>.
        /// </returns>
        /// <example>
        /// In this example, we asynchronously process a <c>Maybe&lt;int&gt;</c> by doubling the value.
        /// <code><![CDATA[
        /// // Assume GetMaybeNumberAsync returns Task<Maybe<int>>
        /// Task<Maybe<int>> maybeNumberTask = GetMaybeNumberAsync();
        ///
        /// int result = await maybeNumberTask.MatchAsync(
        ///     some: value => Task.FromResult(value * 2),
        ///     none: Task.FromResult(-1));
        ///
        /// // If the number is present (e.g., 5), result is 10; if absent, result is -1.
        /// Console.WriteLine(result);
        /// ]]></code>
        /// </example>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<TOut> MatchAsync<TIn, TOut>(
            this Task<Maybe<TIn>> self,
            Func<TIn, Task<TOut>> some,
            Task<TOut> none)
            where TIn : notnull
            where TOut : notnull
        {
#pragma warning disable CA1062
            return await (await self.ConfigureAwait(false)).MatchAsync(some: some, none: none).ConfigureAwait(false);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Awaits a <see cref="Task"/> producing a <see cref="Maybe{TIn}"/> and matches its result by executing one of two actions.
        /// </summary>
        /// <param name="self">
        /// A <see cref="Task"/> that produces a <see cref="Maybe{TIn}"/>.
        /// </param>
        /// <param name="some">
        /// An action to perform if the produced <see cref="Maybe{TIn}"/> contains a value.
        /// </param>
        /// <param name="none">
        /// An action to perform if the produced <see cref="Maybe{TIn}"/> is null.
        /// </param>
        /// <typeparam name="TIn">
        /// The type contained in the <see cref="Maybe{TIn}"/>; must be non-null.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous matching operation.
        /// </returns>
        /// <example>
        /// The example demonstrates logging a message based on whether a task-returned optional value exists.
        /// <code><![CDATA[
        /// // Assume GetMaybeUserAsync returns Task<Maybe<User>>
        /// await GetMaybeUserAsync().MatchAsync(
        ///     some: user => { Console.WriteLine($"User: {user.Name}"); return Task.CompletedTask; },
        ///     none: () => { Console.WriteLine("User not found."); return Task.CompletedTask; });
        /// ]]></code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task MatchAsync<TIn>(
            this Task<Maybe<TIn>> self,
            Action<TIn> some,
            Action none)
            where TIn : notnull
        {
#pragma warning disable CA1062
            (await self.ConfigureAwait(false)).Match(some, none);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Awaits a <see cref="Task"/> producing a <see cref="Maybe{TIn}"/> and asynchronously executes
        /// one of two actions based on the presence of a value.
        /// </summary>
        /// <param name="self">
        /// A <see cref="Task"/> that yields a <see cref="Maybe{TIn}"/>.
        /// </param>
        /// <param name="some">
        /// An asynchronous function to execute if a value is present.
        /// </param>
        /// <param name="none">
        /// A <see cref="Task"/> representing the asynchronous action to execute if no value is present.
        /// </param>
        /// <typeparam name="TIn">
        /// The type contained in the <see cref="Maybe{TIn}"/>; must be non-null.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Task"/> representing the completion of the matching operation.
        /// </returns>
        /// <example>
        /// In this example, an asynchronous operation is performed based on the presence of a value.
        /// <code><![CDATA[
        /// // Assume GetMaybeOrderAsync returns Task<Maybe<Order>>
        /// await GetMaybeOrderAsync().MatchAsync(
        ///     some: order => ProcessOrderAsync(order),
        ///     none: Task.Run(() => Console.WriteLine("No order available")));
        /// ]]></code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task MatchAsync<TIn>(
            this Task<Maybe<TIn>> self,
            Func<TIn, Task> some,
            Task none)
            where TIn : notnull
        {
#pragma warning disable CA1062
            await (await self.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
#pragma warning restore CA1062
        }

        #endregion
    }
}
