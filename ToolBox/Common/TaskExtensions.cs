using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Common;

public static class TaskExtensions
{
    /// <summary>
    /// Determines whether the task has completed successfully,
    /// meaning it completed and was not canceled or faulted.
    /// </summary>
    /// <param name="task">The task to check.</param>
    /// <returns>True if completed successfully, false otherwise.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCompletedSuccessfully<TIn>(this Task<TIn> task)
    {
        return task is { IsCompleted: true, IsFaulted: false, IsCanceled: false };
    }
    
    /// <summary>
    /// Converts the provided exception to a Task that is faulted
    /// with the exception.
    /// </summary>
    /// <param name="ex">The exception to use as the faulted state.</param>
    /// <returns>A Task that has already completed with the exception.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<TIn> AsFailedTask<TIn>(this Exception ex)
    {
        TaskCompletionSource<TIn> completionSource = new();
        completionSource.SetException(ex);
        return completionSource.Task;
    }

    /// <summary>Convert a value to a Task that completes immediately</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<TIn> AsTask<TIn>(this TIn self) => Task.FromResult(self);

    /// <summary>Flatten the nested Task type</summary>
    [Pure]
    public static async Task<TIn> Flatten<TIn>(this Task<Task<TIn>> self)
    {
        return await (await self.ConfigureAwait(false)).ConfigureAwait(false);
    }

    /// <summary>Flatten the nested Task type</summary>
    [Pure]
    public static async Task<TIn> Flatten<TIn>(this Task<Task<Task<TIn>>> self)
    {
        return await (await (await self.ConfigureAwait(false)).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
}