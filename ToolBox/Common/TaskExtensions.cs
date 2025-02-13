using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Common;

/// <summary>
/// Provides extension methods for <see cref="Task{TIn}"/>.
/// </summary>
/// <example>
/// Basic usage of TaskExtensions:
/// <code><![CDATA[
/// // Example for IsCompletedSuccessfully
/// Task<int> completedTask = Task.FromResult(5);
/// bool isSuccessful = completedTask.IsCompletedSuccessfully(); // Output: True
///
/// Task<int> faultedTask = Task.FromException<int>(new Exception("Something went wrong"));
/// bool isFaultedSuccessful = faultedTask.IsCompletedSuccessfully(); // Output: False
///
/// // Example for AsFailedTaskAsync
/// var exception = new InvalidOperationException("Operation failed");
/// Task<int> failedTask = exception.AsFailedTaskAsync<int>();
/// await failedTask.ContinueWith(task => {
///     if (task.IsFaulted)
///     {
///         Console.WriteLine($"Task faulted with exception: {task.Exception.InnerException.Message}");
///     }
/// });
///
/// // Example for AsTaskAsync
/// int value = 10;
/// Task<int> valueTask = value.AsTaskAsync();
/// Console.WriteLine($"Task value: {await valueTask}"); // Output: Task value: 10
///
/// // Example for FlattenAsync
/// Task<Task<int>> nestedTask = Task.FromResult(Task.FromResult(20));
/// Task<int> flattenedTask = nestedTask.FlattenAsync();
/// Console.WriteLine($"Flattened task result: {await flattenedTask}"); // Output: Flattened task result: 20
///
/// Task<Task<Task<string>>> deeplyNestedTask = Task.FromResult(Task.FromResult(Task.FromResult("Hello")));
/// Task<string> deeplyFlattenedTask = deeplyNestedTask.FlattenAsync();
/// Console.WriteLine($"Deeply flattened task result: {await deeplyFlattenedTask}"); // Output: Deeply flattened task result: Hello
/// ]]></code>
/// </example>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class TaskExtensions
{
	/// <summary>
	/// Determines whether the task has completed successfully,
	/// meaning it completed and was not canceled or faulted.
	/// </summary>
	/// <typeparam name="TIn">The type of the result of the task.</typeparam>
	/// <param name="task">The task to check.</param>
	/// <returns><see langword="true"/> if completed successfully, <see langword="false"/> otherwise.</returns>
	/// <example>
	/// Basic completed task:
	/// <code><![CDATA[
	/// Task<int> completedTask = Task.FromResult(5);
	/// bool isSuccessful = completedTask.IsCompletedSuccessfully(); // Output: True
	/// ]]></code>
	/// Faulted task:
	/// <code><![CDATA[
	/// Task<int> faultedTask = Task.FromException<int>(new Exception("Something went wrong"));
	/// bool isFaultedSuccessful = faultedTask.IsCompletedSuccessfully(); // Output: False
	/// ]]></code>
	/// Canceled task:
	/// <code><![CDATA[
	/// Task<int> canceledTask = new TaskCompletionSource<int>().Task;
	/// canceledTask.Cancel();
	/// bool isCanceledSuccessful = canceledTask.IsCompletedSuccessfully(); // Output: False
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsCompletedSuccessfully<TIn>(this Task<TIn> task)
	{
		return task is { IsCompleted: true, IsFaulted: false, IsCanceled: false };
	}

	/// <summary>
	/// Converts the provided exception to a <see cref="Task{TIn}"/> that is faulted
	/// with the exception.
	/// </summary>
	/// <typeparam name="TIn">The type of the result of the task.</typeparam>
	/// <param name="ex">The exception to use as the faulted state.</param>
	/// <returns>A <see cref="Task{TIn}"/> that has already completed with the exception.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var exception = new InvalidOperationException("Operation failed");
	/// Task<int> failedTask = exception.AsFailedTaskAsync<int>();
	/// await failedTask.ContinueWith(task => {
	///     if (task.IsFaulted)
	///     {
	///         Console.WriteLine($"Task faulted with exception: {task.Exception.InnerException.Message}");
	///     }
	/// });
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<TIn> AsFailedTaskAsync<TIn>(this Exception ex)
	{
		TaskCompletionSource<TIn> completionSource = new();
		completionSource.SetException(ex);
		return completionSource.Task;
	}

	/// <summary>Convert a value to a <see cref="Task{TIn}"/> that completes immediately</summary>
	/// <typeparam name="TIn">The type of the value.</typeparam>
	/// <param name="self">The value to convert to a task.</param>
	/// <returns>A <see cref="Task{TIn}"/> that is already completed with the value.</returns>
	/// <example>
	/// <code><![CDATA[
	/// int value = 10;
	/// Task<int> valueTask = value.AsTaskAsync();
	/// Console.WriteLine($"Task value: {await valueTask}"); // Output: Task value: 10
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<TIn> AsTaskAsync<TIn>(this TIn self) => Task.FromResult(self);

	/// <summary>FlattenAsync the nested <see cref="Task{TIn}"/> type <see cref="Task{T}"/> of <see cref="Task{TIn}"/></summary>
	/// <typeparam name="TIn">The type of the result of the inner task.</typeparam>
	/// <param name="self">The nested task to flatten.</param>
	/// <returns>A flattened <see cref="Task{TIn}"/>.</returns>
	/// <example>
	/// <code><![CDATA[
	/// Task<Task<int>> nestedTask = Task.FromResult(Task.FromResult(20));
	/// Task<int> flattenedTask = nestedTask.FlattenAsync();
	/// Console.WriteLine($"Flattened task result: {await flattenedTask}"); // Output: Flattened task result: 20
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TIn> FlattenAsync<TIn>(this Task<Task<TIn>> self)
	{
		return await (await self.ConfigureAwait(false)).ConfigureAwait(false);
	}

	/// <summary>FlattenAsync the deeply nested <see cref="Task{TIn}"/> type <see cref="Task{T}"/> of <see cref="Task{T}"/> of <see cref="Task{TIn}"/> to <see cref="Task{TIn}"/></summary>
	/// <typeparam name="TIn">The type of the result of the inner task.</typeparam>
	/// <param name="self">The deeply nested task to flatten.</param>
	/// <returns>A flattened <see cref="Task{TIn}"/>.</returns>
	/// <example>
	/// <code><![CDATA[
	/// Task<Task<Task<string>>> deeplyNestedTask = Task.FromResult(Task.FromResult(Task.FromResult("Hello")));
	/// Task<string> deeplyFlattenedTask = deeplyNestedTask.FlattenAsync();
	/// Console.WriteLine($"Deeply flattened task result: {await deeplyFlattenedTask}"); // Output: Deeply flattened task result: Hello
	/// ]]></code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TIn> FlattenAsync<TIn>(this Task<Task<Task<TIn>>> self)
	{
		return await (await (await self.ConfigureAwait(false)).ConfigureAwait(false)).ConfigureAwait(false);
	}

}