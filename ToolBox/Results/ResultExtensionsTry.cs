using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

public delegate Result<TIn> Try<TIn>();
public delegate Task<Result<TIn>> TryAsync<TIn>();
public static class ResultExtensionsTry
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TryAsync<TIn> ToAsync<TIn>(this Try<TIn> self)
        => () => self.Try().AsTask();


    [Pure]
    public static Result<TIn> Try<TIn>(this Try<TIn> self)
    {
        try
        {
            return self.Invoke();
        }
        catch (Exception ex)
        {
            return Result<TIn>.Failure(ex);
        }
    }

    [Pure]
    public static async Task<Result<T>> Try<T>(this TryAsync<T> self)
    {
        try
        {
            return await self().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex);
        }
    }
}