using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Options;

/// <summary>
///
/// </summary>
public static class MaybeExtensionsTap
{
	#region Maybe<TIn>




	/// <summary>
	/// Applies the provided action to the content of the <see cref="Maybe{TIn}"/> instance if it is <see cref="Maybe{TIn}.IsSome"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the content of the <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <param name="self">The <see cref="Maybe{TIn}"/> instance to apply the action to.</param>
	/// <param name="action">The action to apply to the content of the <see cref="Maybe{TIn}"/> instance if it is <see cref="Maybe{TIn}.IsSome"/>.</param>
	/// <returns>The original <see cref="Maybe{TIn}"/> instance.</returns>

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
	/// Executes the provided action if the <see cref="Maybe{TIn}"/> instance is <see cref="Maybe{TIn}.IsNone"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the content of the <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <param name="self">The <see cref="Maybe{TIn}"/> instance to check.</param>
	/// <param name="action">The action to execute if the <see cref="Maybe{TIn}"/> instance is <see cref="Maybe{TIn}.IsNone"/>.</param>
	/// <returns>The original <see cref="Maybe{TIn}"/> instance.</returns>

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

	#region Obsolete

	/// <summary>
	/// Executes a given action if the <see cref="Maybe{TIn}"/> instance is <see cref="Maybe{TIn}.IsSome"/>,
	/// or another action if it is <see cref="Maybe{TIn}.IsNone"/>.
	/// </summary>
	/// <typeparam name="TIn">The type of the content of the <see cref="Maybe{TIn}"/> instance.</typeparam>
	/// <param name="self">The <see cref="Maybe{TIn}"/> instance to check.</param>
	/// <param name="some">The action to execute if the <see cref="Maybe{TIn}"/> instance is <see cref="Maybe{TIn}.IsSome"/>.</param>
	/// <param name="none">The action to execute if the <see cref="Maybe{TIn}"/> instance is <see cref="Maybe{TIn}.IsNone"/>.</param>
	/// <returns>The original <see cref="Maybe{TIn}"/> instance.</returns>
	/// <example>
	/// <code><![CDATA[
	/// Maybe<int> maybe = Maybe.Some(42);
	/// maybe.Tap(
	///		some: value => Console.WriteLine($"The value is {value}"),
	///		none: () => Console.WriteLine("No value"));
	/// ]]></code>
	/// </example>
#pragma warning disable S1133
	[Obsolete( "Use (TapIfSome/TapIfNone) or (TapIfSomeAsync/TapIfNoneAsync) instead. This is going to be removed on future versions",true)]
#pragma warning restore S1133
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