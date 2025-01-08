using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

internal static class TestHelper
{
    public static readonly Error DefaultError = Error.New("Error");
    public static Result<T> CreateSuccessResult<T>(T value) => Result<T>.Success(value);
    public static Result<T> CreateFailureResult<T>(Error error) => Result<T>.Failure(error);

    public static IEnumerable<Result<T>> CreateResults<T>(int count ,T value, int? failPosition = null ) 
        => failPosition is null ?
            Enumerable.Range(0, count).Select(x => CreateSuccessResult(value)) :
            Enumerable.Range(0, count).Select(x => x == failPosition 
                ? CreateFailureResult<T>(DefaultError) 
                : CreateSuccessResult(value));
    

    public static IEnumerable<Task<Result<T>>> CreateAsyncResults<T>(int count ,T value, int? failPosition = null ) 
        => failPosition is null ?
            Enumerable.Range(0, count).Select(x => CreateSuccessResult(value).AsTask()) :
            Enumerable.Range(0, count).Select(x => x == failPosition 
                ? CreateFailureResult<T>(DefaultError).AsTask() 
                : CreateSuccessResult(value).AsTask());
}