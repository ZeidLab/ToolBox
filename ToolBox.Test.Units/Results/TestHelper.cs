using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

internal static class TestHelper
{
    public static readonly ResultError DefaultResultError = ResultError.New("Error");
    public static Result<T> CreateSuccessResult<T>(T value) => Result<T>.Success(value);
    public static Result<T> CreateFailureResult<T>(ResultError resultError) => Result<T>.Failure(resultError);

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
}