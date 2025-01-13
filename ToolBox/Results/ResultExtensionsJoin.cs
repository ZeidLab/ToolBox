namespace ZeidLab.ToolBox.Results;

/// <summary>
/// provides extension methods for joining multiple <see cref="Result{TValue}"/>
/// with different value types.
/// </summary>
public static class ResultExtensionsJoin
{
    #region Join

    /// <summary>
    /// Joins multiple results into a single result by applying a specified function if both results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="func">The function to apply to the values of the results if both are successful.</param>
    /// <returns>A result containing the output of the function if both input results are successful; otherwise, a failed result with the error of the first failed result.</returns>
    public static Result<TOut> Join<TIn1, TIn2, TOut>(this Result<TIn1> result1, Result<TIn2> result2,
        Func<TIn1, TIn2, Result<TOut>> func)
    {
        if (result1.IsFailure)
            return Result<TOut>.Failure(result1.Error);
        if (result2.IsFailure)
            return Result<TOut>.Failure(result2.Error);

        return func(result1.Value, result2.Value);
    }

    /// <summary>
    /// Joins multiple results into a single result by applying a specified function if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A result containing the output of the function if all input results are successful; otherwise, a failed result with the error of the first failed result.</returns>
    public static Result<TOut> Join<TIn1, TIn2, TIn3, TOut>(this Result<TIn1> result1, Result<TIn2> result2,
        Result<TIn3> result3, Func<TIn1, TIn2, TIn3, Result<TOut>> func)
    {
        if (result1.IsFailure)
            return Result<TOut>.Failure(result1.Error);
        if (result2.IsFailure)
            return Result<TOut>.Failure(result2.Error);
        if (result3.IsFailure)
            return Result<TOut>.Failure(result3.Error);

        return func(result1.Value, result2.Value, result3.Value);
    }

    /// <summary>
    /// Joins multiple results into a single result by applying a specified function if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="result4">The fourth result.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A result containing the output of the function if all input results are successful; otherwise, a failed result with the error of the first failed result.</returns>
    public static Result<TOut> Join<TIn1, TIn2, TIn3, TIn4, TOut>(this Result<TIn1> result1, Result<TIn2> result2,
        Result<TIn3> result3, Result<TIn4> result4, Func<TIn1, TIn2, TIn3, TIn4, Result<TOut>> func)
    {
        if (result1.IsFailure)
            return Result<TOut>.Failure(result1.Error);
        if (result2.IsFailure)
            return Result<TOut>.Failure(result2.Error);
        if (result3.IsFailure)
            return Result<TOut>.Failure(result3.Error);
        if (result4.IsFailure)
            return Result<TOut>.Failure(result4.Error);

        return func(result1.Value, result2.Value, result3.Value, result4.Value);
    }

    /// <summary>
    /// Joins multiple results into a single result by applying a specified function if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="result4">The fourth result.</param>
    /// <param name="result5">The fifth result.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A result containing the output of the function if all input results are successful; otherwise, a failed result with the error of the first failed result.</returns>
    public static Result<TOut> Join<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(this Result<TIn1> result1, Result<TIn2> result2,
        Result<TIn3> result3, Result<TIn4> result4, Result<TIn5> result5,
        Func<TIn1, TIn2, TIn3, TIn4, TIn5, Result<TOut>> func)
    {
        if (result1.IsFailure)
            return Result<TOut>.Failure(result1.Error);
        if (result2.IsFailure)
            return Result<TOut>.Failure(result2.Error);
        if (result3.IsFailure)
            return Result<TOut>.Failure(result3.Error);
        if (result4.IsFailure)
            return Result<TOut>.Failure(result4.Error);
        if (result5.IsFailure)
            return Result<TOut>.Failure(result5.Error);

        return func(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value);
    }

    /// <summary>
    /// Joins multiple results into a single result by applying a specified function if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="result4">The fourth result.</param>
    /// <param name="result5">The fifth result.</param>
    /// <param name="result6">The sixth result.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A result containing the output of the function if all input results are successful; otherwise, a failed result with the error of the first failed result.</returns>
    public static Result<TOut> Join<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>(this Result<TIn1> result1,
        Result<TIn2> result2, Result<TIn3> result3, Result<TIn4> result4, Result<TIn5> result5, Result<TIn6> result6,
        Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, Result<TOut>> func)
    {
        if (result1.IsFailure)
            return Result<TOut>.Failure(result1.Error);
        if (result2.IsFailure)
            return Result<TOut>.Failure(result2.Error);
        if (result3.IsFailure)
            return Result<TOut>.Failure(result3.Error);
        if (result4.IsFailure)
            return Result<TOut>.Failure(result4.Error);
        if (result5.IsFailure)
            return Result<TOut>.Failure(result5.Error);
        if (result6.IsFailure)
            return Result<TOut>.Failure(result6.Error);

        return func(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value);
    }

    /// <summary>
    /// Joins multiple results into a single result by applying a specified function if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn7">The type of the seventh input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="result4">The fourth result.</param>
    /// <param name="result5">The fifth result.</param>
    /// <param name="result6">The sixth result.</param>
    /// <param name="result7">The seventh result.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A result containing the output of the function if all input results are successful; otherwise, a failed result with the error of the first failed result.</returns>
    public static Result<TOut> Join<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>(this Result<TIn1> result1,
        Result<TIn2> result2, Result<TIn3> result3, Result<TIn4> result4, Result<TIn5> result5, Result<TIn6> result6,
        Result<TIn7> result7, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, Result<TOut>> func)
    {
        if (result1.IsFailure)
            return Result<TOut>.Failure(result1.Error);
        if (result2.IsFailure)
            return Result<TOut>.Failure(result2.Error);
        if (result3.IsFailure)
            return Result<TOut>.Failure(result3.Error);
        if (result4.IsFailure)
            return Result<TOut>.Failure(result4.Error);
        if (result5.IsFailure)
            return Result<TOut>.Failure(result5.Error);
        if (result6.IsFailure)
            return Result<TOut>.Failure(result6.Error);
        if (result7.IsFailure)
            return Result<TOut>.Failure(result7.Error);

        return func(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value,
            result7.Value);
    }

    /// <summary>
    /// Joins multiple results into a single result by applying a specified function if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn7">The type of the seventh input result value.</typeparam>
    /// <typeparam name="TIn8">The type of the eighth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="result4">The fourth result.</param>
    /// <param name="result5">The fifth result.</param>
    /// <param name="result6">The sixth result.</param>
    /// <param name="result7">The seventh result.</param>
    /// <param name="result8">The eighth result.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A result containing the output of the function if all input results are successful; otherwise, a failed result with the error of the first failed result.</returns>
    public static Result<TOut> Join<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>(this Result<TIn1> result1,
        Result<TIn2> result2, Result<TIn3> result3, Result<TIn4> result4, Result<TIn5> result5, Result<TIn6> result6,
        Result<TIn7> result7, Result<TIn8> result8,
        Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, Result<TOut>> func)
    {
        if (result1.IsFailure)
            return Result<TOut>.Failure(result1.Error);
        if (result2.IsFailure)
            return Result<TOut>.Failure(result2.Error);
        if (result3.IsFailure)
            return Result<TOut>.Failure(result3.Error);
        if (result4.IsFailure)
            return Result<TOut>.Failure(result4.Error);
        if (result5.IsFailure)
            return Result<TOut>.Failure(result5.Error);
        if (result6.IsFailure)
            return Result<TOut>.Failure(result6.Error);
        if (result7.IsFailure)
            return Result<TOut>.Failure(result7.Error);
        if (result8.IsFailure)
            return Result<TOut>.Failure(result8.Error);

        return func(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value,
            result7.Value, result8.Value);
    }

    /// <summary>
    /// Joins multiple results into a single result by applying a specified function if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn7">The type of the seventh input result value.</typeparam>
    /// <typeparam name="TIn8">The type of the eighth input result value.</typeparam>
    /// <typeparam name="TIn9">The type of the ninth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="result4">The fourth result.</param>
    /// <param name="result5">The fifth result.</param>
    /// <param name="result6">The sixth result.</param>
    /// <param name="result7">The seventh result.</param>
    /// <param name="result8">The eighth result.</param>
    /// <param name="result9">The ninth result.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A result containing the output of the function if all input results are successful; otherwise, a failed result with the error of the first failed result.</returns>
    public static Result<TOut> Join<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>(
        this Result<TIn1> result1, Result<TIn2> result2, Result<TIn3> result3, Result<TIn4> result4,
        Result<TIn5> result5, Result<TIn6> result6, Result<TIn7> result7, Result<TIn8> result8, Result<TIn9> result9,
        Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, Result<TOut>> func)
    {
        if (result1.IsFailure)
            return Result<TOut>.Failure(result1.Error);
        if (result2.IsFailure)
            return Result<TOut>.Failure(result2.Error);
        if (result3.IsFailure)
            return Result<TOut>.Failure(result3.Error);
        if (result4.IsFailure)
            return Result<TOut>.Failure(result4.Error);
        if (result5.IsFailure)
            return Result<TOut>.Failure(result5.Error);
        if (result6.IsFailure)
            return Result<TOut>.Failure(result6.Error);
        if (result7.IsFailure)
            return Result<TOut>.Failure(result7.Error);
        if (result8.IsFailure)
            return Result<TOut>.Failure(result8.Error);
        if (result9.IsFailure)
            return Result<TOut>.Failure(result9.Error);

        return func(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value,
            result7.Value, result8.Value, result9.Value);
    }

    /// <summary>
    /// Joins multiple results into a single result by applying a specified function if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn7">The type of the seventh input result value.</typeparam>
    /// <typeparam name="TIn8">The type of the eighth input result value.</typeparam>
    /// <typeparam name="TIn9">The type of the ninth input result value.</typeparam>
    /// <typeparam name="TIn10">The type of the tenth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="result4">The fourth result.</param>
    /// <param name="result5">The fifth result.</param>
    /// <param name="result6">The sixth result.</param>
    /// <param name="result7">The seventh result.</param>
    /// <param name="result8">The eighth result.</param>
    /// <param name="result9">The ninth result.</param>
    /// <param name="result10">The tenth result.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A result containing the output of the function if all input results are successful; otherwise, a failed result with the error of the first failed result.</returns>
    public static Result<TOut> Join<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>(
        this Result<TIn1> result1, Result<TIn2> result2, Result<TIn3> result3, Result<TIn4> result4,
        Result<TIn5> result5, Result<TIn6> result6, Result<TIn7> result7, Result<TIn8> result8, Result<TIn9> result9,
        Result<TIn10> result10, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, Result<TOut>> func)
    {
        if (result1.IsFailure)
            return Result<TOut>.Failure(result1.Error);
        if (result2.IsFailure)
            return Result<TOut>.Failure(result2.Error);
        if (result3.IsFailure)
            return Result<TOut>.Failure(result3.Error);
        if (result4.IsFailure)
            return Result<TOut>.Failure(result4.Error);
        if (result5.IsFailure)
            return Result<TOut>.Failure(result5.Error);
        if (result6.IsFailure)
            return Result<TOut>.Failure(result6.Error);
        if (result7.IsFailure)
            return Result<TOut>.Failure(result7.Error);
        if (result8.IsFailure)
            return Result<TOut>.Failure(result8.Error);
        if (result9.IsFailure)
            return Result<TOut>.Failure(result9.Error);
        if (result10.IsFailure)
            return Result<TOut>.Failure(result10.Error);

        return func(result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value,
            result7.Value, result8.Value, result9.Value, result10.Value);
    }

    #endregion

    #region JoinAsync

    /// <summary>
    /// Asynchronously joins multiple result objects and applies a function to their values if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result to join.</param>
    /// <param name="result2">The second result to join.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A task representing the asynchronous operation, containing a successful result with the output value if all inputs are successful, otherwise a failed result with the error of the first failed input.</returns>
    public static async Task<Result<TOut>> JoinAsync<TIn1, TIn2, TOut>(
        this Task<Result<TIn1>> result1, Task<Result<TIn2>> result2,
        Func<TIn1, TIn2, Result<TOut>> func)
    {
        await Task.WhenAll(result1, result2).ConfigureAwait(false);
        if (result1.Result.IsFailure)
            return Result<TOut>.Failure(result1.Result.Error);
        if (result2.Result.IsFailure)
            return Result<TOut>.Failure(result2.Result.Error);

        return func(result1.Result.Value, result2.Result.Value);
    }

    /// <summary>
    /// Asynchronously joins multiple result objects and applies a function to their values if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result to join.</param>
    /// <param name="result2">The second result to join.</param>
    /// <param name="result3">The third result to join.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A task representing the asynchronous operation, containing a successful result with the output value if all inputs are successful, otherwise a failed result with the error of the first failed input.</returns>
    public static async Task<Result<TOut>> JoinAsync<TIn1, TIn2, TIn3, TOut>(
        this Task<Result<TIn1>> result1, Task<Result<TIn2>> result2, Task<Result<TIn3>> result3,
        Func<TIn1, TIn2, TIn3, Result<TOut>> func)
    {
        await Task.WhenAll(result1, result2, result3).ConfigureAwait(false);
        if (result1.Result.IsFailure)
            return Result<TOut>.Failure(result1.Result.Error);
        if (result2.Result.IsFailure)
            return Result<TOut>.Failure(result2.Result.Error);
        if (result3.Result.IsFailure)
            return Result<TOut>.Failure(result3.Result.Error);

        return func(result1.Result.Value, result2.Result.Value, result3.Result.Value);
    }

    /// <summary>
    /// Asynchronously joins multiple result objects and applies a function to their values if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result to join.</param>
    /// <param name="result2">The second result to join.</param>
    /// <param name="result3">The third result to join.</param>
    /// <param name="result4">The fourth result to join.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A task representing the asynchronous operation, containing a successful result with the output value if all inputs are successful, otherwise a failed result with the error of the first failed input.</returns>
    public static async Task<Result<TOut>> JoinAsync<TIn1, TIn2, TIn3, TIn4, TOut>(
        this Task<Result<TIn1>> result1, Task<Result<TIn2>> result2, Task<Result<TIn3>> result3,
        Task<Result<TIn4>> result4,
        Func<TIn1, TIn2, TIn3, TIn4, Result<TOut>> func)
    {
        await Task.WhenAll(result1, result2, result3, result4).ConfigureAwait(false);
        if (result1.Result.IsFailure)
            return Result<TOut>.Failure(result1.Result.Error);
        if (result2.Result.IsFailure)
            return Result<TOut>.Failure(result2.Result.Error);
        if (result3.Result.IsFailure)
            return Result<TOut>.Failure(result3.Result.Error);
        if (result4.Result.IsFailure)
            return Result<TOut>.Failure(result4.Result.Error);

        return func(result1.Result.Value, result2.Result.Value, result3.Result.Value, result4.Result.Value);
    }

    /// <summary>
    /// Asynchronously joins multiple result objects and applies a function to their values if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result to join.</param>
    /// <param name="result2">The second result to join.</param>
    /// <param name="result3">The third result to join.</param>
    /// <param name="result4">The fourth result to join.</param>
    /// <param name="result5">The fifth result to join.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A task representing the asynchronous operation, containing a successful result with the output value if all inputs are successful, otherwise a failed result with the error of the first failed input.</returns>
    public static async Task<Result<TOut>> JoinAsync<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(
        this Task<Result<TIn1>> result1, Task<Result<TIn2>> result2, Task<Result<TIn3>> result3,
        Task<Result<TIn4>> result4, Task<Result<TIn5>> result5,
        Func<TIn1, TIn2, TIn3, TIn4, TIn5, Result<TOut>> func)
    {
        await Task.WhenAll(result1, result2, result3, result4, result5).ConfigureAwait(false);
        if (result1.Result.IsFailure)
            return Result<TOut>.Failure(result1.Result.Error);
        if (result2.Result.IsFailure)
            return Result<TOut>.Failure(result2.Result.Error);
        if (result3.Result.IsFailure)
            return Result<TOut>.Failure(result3.Result.Error);
        if (result4.Result.IsFailure)
            return Result<TOut>.Failure(result4.Result.Error);
        if (result5.Result.IsFailure)
            return Result<TOut>.Failure(result5.Result.Error);

        return func(result1.Result.Value, result2.Result.Value, result3.Result.Value, result4.Result.Value,
            result5.Result.Value);
    }

    /// <summary>
    /// Asynchronously joins multiple result objects and applies a function to their values if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result to join.</param>
    /// <param name="result2">The second result to join.</param>
    /// <param name="result3">The third result to join.</param>
    /// <param name="result4">The fourth result to join.</param>
    /// <param name="result5">The fifth result to join.</param>
    /// <param name="result6">The sixth result to join.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A task representing the asynchronous operation, containing a successful result with the output value if all inputs are successful, otherwise a failed result with the error of the first failed input.</returns>
    public static async Task<Result<TOut>> JoinAsync<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut>(
        this Task<Result<TIn1>> result1, Task<Result<TIn2>> result2, Task<Result<TIn3>> result3,
        Task<Result<TIn4>> result4, Task<Result<TIn5>> result5, Task<Result<TIn6>> result6,
        Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, Result<TOut>> func)
    {
        await Task.WhenAll(result1, result2, result3, result4, result5, result6).ConfigureAwait(false);
        if (result1.Result.IsFailure)
            return Result<TOut>.Failure(result1.Result.Error);
        if (result2.Result.IsFailure)
            return Result<TOut>.Failure(result2.Result.Error);
        if (result3.Result.IsFailure)
            return Result<TOut>.Failure(result3.Result.Error);
        if (result4.Result.IsFailure)
            return Result<TOut>.Failure(result4.Result.Error);
        if (result5.Result.IsFailure)
            return Result<TOut>.Failure(result5.Result.Error);
        if (result6.Result.IsFailure)
            return Result<TOut>.Failure(result6.Result.Error);

        return func(result1.Result.Value, result2.Result.Value, result3.Result.Value, result4.Result.Value,
            result5.Result.Value, result6.Result.Value);
    }

    /// <summary>
    /// Asynchronously joins multiple result objects and applies a function to their values if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn7">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result to join.</param>
    /// <param name="result2">The second result to join.</param>
    /// <param name="result3">The third result to join.</param>
    /// <param name="result4">The fourth result to join.</param>
    /// <param name="result5">The fifth result to join.</param>
    /// <param name="result6">The sixth result to join.</param>
    /// <param name="result7">The sixth result to join.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A task representing the asynchronous operation, containing a successful result with the output value if all inputs are successful, otherwise a failed result with the error of the first failed input.</returns>
    public static async Task<Result<TOut>>
        JoinAsync<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut>(
            this Task<Result<TIn1>> result1, Task<Result<TIn2>> result2,
            Task<Result<TIn3>> result3, Task<Result<TIn4>> result4,
            Task<Result<TIn5>> result5, Task<Result<TIn6>> result6,
            Task<Result<TIn7>> result7,
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, Result<TOut>> func)
    {
        await Task.WhenAll(result1, result2, result3, result4, result5, result6, result7).ConfigureAwait(false);
        if (result1.Result.IsFailure)
            return Result<TOut>.Failure(result1.Result.Error);
        if (result2.Result.IsFailure)
            return Result<TOut>.Failure(result2.Result.Error);
        if (result3.Result.IsFailure)
            return Result<TOut>.Failure(result3.Result.Error);
        if (result4.Result.IsFailure)
            return Result<TOut>.Failure(result4.Result.Error);
        if (result5.Result.IsFailure)
            return Result<TOut>.Failure(result5.Result.Error);
        if (result6.Result.IsFailure)
            return Result<TOut>.Failure(result6.Result.Error);
        if (result7.Result.IsFailure)
            return Result<TOut>.Failure(result7.Result.Error);

        return func(result1.Result.Value, result2.Result.Value, result3.Result.Value, result4.Result.Value,
            result5.Result.Value, result6.Result.Value, result7.Result.Value);
    }

    /// <summary>
    /// Asynchronously joins multiple result objects and applies a function to their values if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn7">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn8">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result to join.</param>
    /// <param name="result2">The second result to join.</param>
    /// <param name="result3">The third result to join.</param>
    /// <param name="result4">The fourth result to join.</param>
    /// <param name="result5">The fifth result to join.</param>
    /// <param name="result6">The sixth result to join.</param>
    /// <param name="result7">The sixth result to join.</param>
    /// <param name="result8">The sixth result to join.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A task representing the asynchronous operation, containing a successful result with the output value if all inputs are successful, otherwise a failed result with the error of the first failed input.</returns>
    public static async Task<Result<TOut>>
        JoinAsync<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut>(
            this Task<Result<TIn1>> result1, Task<Result<TIn2>> result2,
            Task<Result<TIn3>> result3, Task<Result<TIn4>> result4,
            Task<Result<TIn5>> result5, Task<Result<TIn6>> result6,
            Task<Result<TIn7>> result7, Task<Result<TIn8>> result8,
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, Result<TOut>> func)
    {
        await Task.WhenAll(result1, result2, result3, result4, result5, result6, result7, result8)
            .ConfigureAwait(false);
        if (result1.Result.IsFailure)
            return Result<TOut>.Failure(result1.Result.Error);
        if (result2.Result.IsFailure)
            return Result<TOut>.Failure(result2.Result.Error);
        if (result3.Result.IsFailure)
            return Result<TOut>.Failure(result3.Result.Error);
        if (result4.Result.IsFailure)
            return Result<TOut>.Failure(result4.Result.Error);
        if (result5.Result.IsFailure)
            return Result<TOut>.Failure(result5.Result.Error);
        if (result6.Result.IsFailure)
            return Result<TOut>.Failure(result6.Result.Error);
        if (result7.Result.IsFailure)
            return Result<TOut>.Failure(result7.Result.Error);
        if (result8.Result.IsFailure)
            return Result<TOut>.Failure(result8.Result.Error);

        return func(result1.Result.Value, result2.Result.Value, result3.Result.Value, result4.Result.Value,
            result5.Result.Value, result6.Result.Value, result7.Result.Value, result8.Result.Value);
    }

    /// <summary>
    /// Asynchronously joins multiple result objects and applies a function to their values if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn7">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn8">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn9">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result to join.</param>
    /// <param name="result2">The second result to join.</param>
    /// <param name="result3">The third result to join.</param>
    /// <param name="result4">The fourth result to join.</param>
    /// <param name="result5">The fifth result to join.</param>
    /// <param name="result6">The sixth result to join.</param>
    /// <param name="result7">The sixth result to join.</param>
    /// <param name="result8">The sixth result to join.</param>
    /// <param name="result9">The sixth result to join.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A task representing the asynchronous operation, containing a successful result with the output value if all inputs are successful, otherwise a failed result with the error of the first failed input.</returns>
    public static async Task<Result<TOut>>
        JoinAsync<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TOut>(
            this Task<Result<TIn1>> result1, Task<Result<TIn2>> result2,
            Task<Result<TIn3>> result3, Task<Result<TIn4>> result4,
            Task<Result<TIn5>> result5, Task<Result<TIn6>> result6,
            Task<Result<TIn7>> result7, Task<Result<TIn8>> result8,
            Task<Result<TIn9>> result9,
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, Result<TOut>> func)
    {
        await Task.WhenAll(result1, result2, result3, result4, result5, result6, result7, result8, result9)
            .ConfigureAwait(false);
        if (result1.Result.IsFailure)
            return Result<TOut>.Failure(result1.Result.Error);
        if (result2.Result.IsFailure)
            return Result<TOut>.Failure(result2.Result.Error);
        if (result3.Result.IsFailure)
            return Result<TOut>.Failure(result3.Result.Error);
        if (result4.Result.IsFailure)
            return Result<TOut>.Failure(result4.Result.Error);
        if (result5.Result.IsFailure)
            return Result<TOut>.Failure(result5.Result.Error);
        if (result6.Result.IsFailure)
            return Result<TOut>.Failure(result6.Result.Error);
        if (result7.Result.IsFailure)
            return Result<TOut>.Failure(result7.Result.Error);
        if (result8.Result.IsFailure)
            return Result<TOut>.Failure(result8.Result.Error);
        if (result9.Result.IsFailure)
            return Result<TOut>.Failure(result9.Result.Error);

        return func(result1.Result.Value, result2.Result.Value, result3.Result.Value, result4.Result.Value,
            result5.Result.Value, result6.Result.Value, result7.Result.Value, result8.Result.Value,
            result9.Result.Value);
    }


    /// <summary>
    /// Asynchronously joins multiple result objects and applies a function to their values if all results are successful.
    /// </summary>
    /// <typeparam name="TIn1">The type of the first input result value.</typeparam>
    /// <typeparam name="TIn2">The type of the second input result value.</typeparam>
    /// <typeparam name="TIn3">The type of the third input result value.</typeparam>
    /// <typeparam name="TIn4">The type of the fourth input result value.</typeparam>
    /// <typeparam name="TIn5">The type of the fifth input result value.</typeparam>
    /// <typeparam name="TIn6">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn7">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn8">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn9">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TIn10">The type of the sixth input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result1">The first result to join.</param>
    /// <param name="result2">The second result to join.</param>
    /// <param name="result3">The third result to join.</param>
    /// <param name="result4">The fourth result to join.</param>
    /// <param name="result5">The fifth result to join.</param>
    /// <param name="result6">The sixth result to join.</param>
    /// <param name="result7">The sixth result to join.</param>
    /// <param name="result8">The sixth result to join.</param>
    /// <param name="result9">The sixth result to join.</param>
    /// <param name="result10">The sixth result to join.</param>
    /// <param name="func">The function to apply to the values of the results if all are successful.</param>
    /// <returns>A task representing the asynchronous operation, containing a successful result with the output value if all inputs are successful, otherwise a failed result with the error of the first failed input.</returns>
    public static async Task<Result<TOut>>
        JoinAsync<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TOut>(
            this Task<Result<TIn1>> result1, Task<Result<TIn2>> result2,
            Task<Result<TIn3>> result3, Task<Result<TIn4>> result4,
            Task<Result<TIn5>> result5, Task<Result<TIn6>> result6,
            Task<Result<TIn7>> result7, Task<Result<TIn8>> result8,
            Task<Result<TIn9>> result9, Task<Result<TIn10>> result10,
            Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, Result<TOut>> func)
    {
        await Task.WhenAll(result1, result2, result3, result4, result5, result6, result7, result8, result9, result10)
            .ConfigureAwait(false);

        if (result1.Result.IsFailure)
            return Result<TOut>.Failure(result1.Result.Error);
        if (result2.Result.IsFailure)
            return Result<TOut>.Failure(result2.Result.Error);
        if (result3.Result.IsFailure)
            return Result<TOut>.Failure(result3.Result.Error);
        if (result4.Result.IsFailure)
            return Result<TOut>.Failure(result4.Result.Error);
        if (result5.Result.IsFailure)
            return Result<TOut>.Failure(result5.Result.Error);
        if (result6.Result.IsFailure)
            return Result<TOut>.Failure(result6.Result.Error);
        if (result7.Result.IsFailure)
            return Result<TOut>.Failure(result7.Result.Error);
        if (result8.Result.IsFailure)
            return Result<TOut>.Failure(result8.Result.Error);
        if (result9.Result.IsFailure)
            return Result<TOut>.Failure(result9.Result.Error);
        if (result10.Result.IsFailure)
            return Result<TOut>.Failure(result10.Result.Error);

        return func(result1.Result.Value, result2.Result.Value, result3.Result.Value, result4.Result.Value,
            result5.Result.Value, result6.Result.Value, result7.Result.Value, result8.Result.Value,
            result9.Result.Value, result10.Result.Value);
    }

    #endregion
}