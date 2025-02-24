using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

internal static class TestHelper
{
    public static readonly ResultError DefaultResultError = ResultError.New("Error");
    public static Result<T> CreateSuccessResult<T>(T value) => Result.Success(value);
    public static Task<Result<T>> CreateSuccessResultTaskAsync<T>(T value) => Result.Success(value).AsTaskAsync();
    public static Result<T> CreateFailureResult<T>(ResultError resultError) => Result.Failure<T>(resultError);
    public static Task<Result<T>> CreateFailureResultTaskAsync<T>(ResultError resultError) => Result.Failure<T>(resultError).AsTaskAsync();

    public static IEnumerable<Result<T>> CreateResults<T>(int count ,T value, int? failPosition = null )
        => failPosition is null ?
            Enumerable.Range(0, count).Select(x => CreateSuccessResult(value)) :
            Enumerable.Range(0, count).Select(x => x == failPosition
                ? CreateFailureResult<T>(DefaultResultError)
                : CreateSuccessResult(value));


    public static IEnumerable<Task<Result<T>>> CreateAsyncResults<T>(int count ,T value, int? failPosition = null )
        => failPosition is null ?
            Enumerable.Range(0, count).Select(x => CreateSuccessResult(value).AsTaskAsync()) :
            Enumerable.Range(0, count).Select(x => x == failPosition
                ? CreateFailureResult<T>(DefaultResultError).AsTaskAsync()
                : CreateSuccessResult(value).AsTaskAsync());

    public static TryAsync<TIn> CreateTryAsyncFuncWithFailure<TIn>(ResultError resultError)
	    => () => CreateFailureResult<TIn>(resultError).AsTaskAsync();

    public static Try<TIn> CreateTryFuncWithFailure<TIn>(ResultError resultError)
	    => () => CreateFailureResult<TIn>(resultError);

    public static TryAsync<TIn> CreateTryAsyncFuncWithSuccess<TIn>(TIn successValue)
	    => () => CreateSuccessResult<TIn>(successValue).AsTaskAsync();

	public static Try<TIn> CreateTryFuncWithSuccess<TIn>(TIn successValue)
		=> () => CreateSuccessResult<TIn>(successValue);
}