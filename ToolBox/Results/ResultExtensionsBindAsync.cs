using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for asynchronous binding operations on <see cref="Result{TValue}"/> objects.
/// These methods enable monadic composition of Result types with async operations.
/// </summary>
/// <remarks>
/// <para>
/// These methods provide asynchronous variants of the binding operations, allowing seamless
/// integration with async/await patterns while maintaining proper error propagation.
/// </para>
/// <example>
/// Basic async usage:
/// <code><![CDATA[
/// async Task<Result<int>> FetchDataAsync(int id) =>
///     await Task.FromResult(Result.Success(id * 2));
///
/// var result = await Result.Success(5)
///     .BindAsync(async x => await FetchDataAsync(x))
///     .BindAsync(async x => await FetchDataAsync(x));
/// // result is Success(20)
/// ]]></code>
/// </example>
/// <example>
/// Error propagation in async operations:
/// <code><![CDATA[
/// async Task<Result<int>> ValidateAsync(int x) =>
///     x <= 0 
///         ? Result.Failure<int>(ResultError.New("Value must be positive"))
///         : Result.Success(x);
///
/// var result = await Result.Success(-5)
///     .BindAsync(async x => await ValidateAsync(x))
///     .BindAsync(async x => await FetchDataAsync(x));
/// // result contains Failure("Value must be positive")
/// ]]></code>
/// </example>
/// </remarks>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsBindAsync
{
    /// <summary>
    /// Asynchronously transforms a successful result into a new result using the provided async function.
    /// If the input result is a failure, the failure is propagated without executing the function.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the input result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the output result.</typeparam>
    /// <param name="self">The result to transform.</param>
    /// <param name="func">The asynchronous function that maps a successful value to a new result.</param>
    /// <returns>
    /// A task containing either:
    /// - A successful result with the transformed value if both the input result and the transformation succeed
    /// - A failure result containing the error from either the input result or the transformation
    /// </returns>
    /// <example>
    /// Basic usage demonstrating successful transformation:
    /// <code><![CDATA[
    /// async Task<Result<int>> MultiplyByTwoAsync(int x) =>
    ///     await Task.FromResult(Result.Success(x * 2));
    ///
    /// var result = await Result.Success(21)
    ///     .BindAsync(async x => await MultiplyByTwoAsync(x));
    /// // result is Success(42)
    /// ]]></code>
    ///
    /// Error propagation example:
    /// <code><![CDATA[
    /// async Task<Result<int>> ValidatePositiveAsync(int x) =>
    ///     x <= 0
    ///         ? Result.Failure<int>(ResultError.New("Number must be positive"))
    ///         : Result.Success(x);
    ///
    /// var result = await Result.Success(-5)
    ///     .BindAsync(async x => await ValidatePositiveAsync(x));
    /// // result contains Failure("Number must be positive")
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.IsSuccess
            ? func(self.Value)
            : Result.Failure<TOut>(self.Error).AsTaskAsync();

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload handles the case where the input Result is wrapped in a Task, but the binding
    /// function is synchronous. This is useful when you have an asynchronous Result that you want
    /// to transform using a synchronous operation.
    /// </remarks>
    /// <example>
    /// Transforming an async result with validation:
    /// <code><![CDATA[
    /// async Task<Result<decimal>> FetchPriceAsync() =>
    ///     await Task.FromResult(Result.Success(42.50m));
    ///
    /// Result<string> FormatPrice(decimal price) =>
    ///     price < 0
    ///         ? Result.Failure<string>(ResultError.New("Price cannot be negative"))
    ///         : Result.Success($"${price:F2}");
    ///
    /// // Success case
    /// var result1 = await FetchPriceAsync()
    ///     .BindAsync(price => FormatPrice(price));
    /// // result1 is Success("$42.50")
    ///
    /// // Failure case
    /// var result2 = await Task.FromResult(Result.Success(-10.00m))
    ///     .BindAsync(price => FormatPrice(price));
    /// // result2 contains Failure("Price cannot be negative")
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Result<TOut>> func)
        => (await self.ConfigureAwait(false)).Bind(func);

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload handles both an asynchronous input Result and an asynchronous binding function.
    /// This is particularly useful when chaining multiple asynchronous operations that each return Results.
    /// </remarks>
    /// <example>
    /// Chaining validation with data processing:
    /// <code><![CDATA[
    /// async Task<Result<UserData>> ValidateEmailAsync(string email) =>
    ///     string.IsNullOrEmpty(email)
    ///         ? Result.Failure<UserData>(ResultError.New("Email cannot be empty"))
    ///         : !email.Contains("@")
    ///             ? Result.Failure<UserData>(ResultError.New("Invalid email format"))
    ///             : Result.Success(new UserData { Email = email });
    ///
    /// async Task<Result<UserProfile>> CreateProfileAsync(UserData data) =>
    ///     await Task.FromResult(Result.Success(new UserProfile { Email = data.Email }));
    ///
    /// // Success case
    /// var result1 = await Task.FromResult(Result.Success("user@example.com"))
    ///     .BindAsync(async email => await ValidateEmailAsync(email))
    ///     .BindAsync(async data => await CreateProfileAsync(data));
    /// // result1 is Success(UserProfile)
    ///
    /// // Failure case - empty email
    /// var result2 = await Task.FromResult(Result.Success(""))
    ///     .BindAsync(async email => await ValidateEmailAsync(email))
    ///     .BindAsync(async data => await CreateProfileAsync(data));
    /// // result2 contains Failure("Email cannot be empty")
    ///
    /// // Failure case - invalid format
    /// var result3 = await Task.FromResult(Result.Success("invalid-email"))
    ///     .BindAsync(async email => await ValidateEmailAsync(email))
    ///     .BindAsync(async data => await CreateProfileAsync(data));
    /// // result3 contains Failure("Invalid email format")
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Task<Result<TOut>>> func)
    {
        var result = await self.ConfigureAwait(false);
        return result.IsSuccess
            ? await func(result.Value).ConfigureAwait(false)
            : Result.Failure<TOut>(result.Error);
    }

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload converts a synchronous Try operation to an asynchronous Result by applying
    /// an asynchronous Try operation. It safely handles exceptions before proceeding with the
    /// asynchronous processing.
    /// </remarks>
    /// <example>
    /// Processing file content asynchronously:
    /// <code><![CDATA[
    /// // Synchronous operation that might throw
    /// Try<string[]> readLines = () => {
    ///     var path = "example.txt";
    ///     return Result.Success(File.ReadAllLines(path));
    /// };
    ///
    /// // Async operation to process the lines
    /// TryAsync<int> countWords = async () => {
    ///     await Task.Delay(100); // Simulate processing
    ///     return Result.Success(42); // Example result
    /// };
    ///
    /// // Success case
    /// var result1 = await readLines
    ///     .BindAsync(lines => countWords);
    /// // For existing file: result1 is Success(wordCount)
    ///
    /// // Failure case - file not found
    /// var result2 = await readLines
    ///     .BindAsync(lines => countWords);
    /// // For non-existent file: result2 contains FileNotFoundException
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, TryAsync<TOut>> func)
        => self.Try().BindAsync(input => func(input).TryAsync());

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload converts a synchronous Try operation to an asynchronous Result by applying
    /// an asynchronous Result operation. It handles exceptions from the synchronous operation
    /// and converts them to Result failures before proceeding with the async operation.
    /// </remarks>
    /// <example>
    /// Data parsing and API validation:
    /// <code><![CDATA[
    /// // Synchronous operation that might throw
    /// Try<int> parseData = () => Result.Success(int.Parse("42"));
    ///
    /// // Async operation returning Result
    /// async Task<Result<int>> ValidateNumberAsync(int num) =>
    ///     num <= 0
    ///         ? Result.Failure<int>(ResultError.New("Number must be positive"))
    ///         : await Task.FromResult(Result.Success(num));
    ///
    /// // Success case
    /// var result1 = await parseData
    ///     .BindAsync(async num => await ValidateNumberAsync(num));
    /// // For "42": result1 is Success(42)
    ///
    /// // Failure case - parse error
    /// Try<int> invalidParse = () => Result.Success(int.Parse("invalid"));
    /// var result2 = await invalidParse
    ///     .BindAsync(async num => await ValidateNumberAsync(num));
    /// // For invalid input: result2 contains FormatException
    ///
    /// // Failure case - validation error
    /// Try<int> negativeNumber = () => Result.Success(-1);
    /// var result3 = await negativeNumber
    ///     .BindAsync(async num => await ValidateNumberAsync(num));
    /// // For "-1": result3 contains Failure("Number must be positive")
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.Try().BindAsync(func);

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload chains two asynchronous Try operations together. Both operations must complete
    /// successfully for the result to be successful. Any exception in either operation will result
    /// in a failure.
    /// </remarks>
    /// <example>
    /// Data fetching and processing:
    /// <code><![CDATA[
    /// // First async operation
    /// TryAsync<string> fetchData = async () => {
    ///     await Task.Delay(100); // Simulate network delay
    ///     return Result.Success("sample,data");
    /// };
    ///
    /// // Second async operation
    /// TryAsync<int> processData = async () => {
    ///     await Task.Delay(100); // Simulate processing
    ///     return Result.Success(42);
    /// };
    ///
    /// // Success case
    /// var result1 = await fetchData
    ///     .BindAsync(_ => processData);
    /// // For valid data: result1 is Success(42)
    ///
    /// // Failure case - fetch error
    /// TryAsync<string> failedFetch = async () => {
    ///     throw new Exception("Network error");
    /// };
    /// var result2 = await failedFetch
    ///     .BindAsync(_ => processData);
    /// // For network error: result2 contains Exception
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, TryAsync<TOut>> func)
        => self.TryAsync().BindAsync(input => func(input).TryAsync());

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload combines an asynchronous Try operation with a synchronous Try operation.
    /// The async operation must complete successfully before the synchronous Try operation is attempted.
    /// </remarks>
    /// <example>
    /// Data fetching and parsing:
    /// <code><![CDATA[
    /// // Async operation to fetch raw data
    /// TryAsync<string> fetchData = async () => {
    ///     await Task.Delay(100); // Simulate network delay
    ///     return Result.Success("42,13,7");
    /// };
    ///
    /// // Sync operation to parse numbers
    /// Try<int[]> parseNumbers = () => {
    ///     string csv = "42,13,7";
    ///     return Result.Success(csv.Split(',').Select(int.Parse).ToArray());
    /// };
    ///
    /// // Success case
    /// var result1 = await fetchData
    ///     .BindAsync(_ => parseNumbers);
    /// // For valid data: result1 is Success([42, 13, 7])
    ///
    /// // Failure case - fetch error
    /// TryAsync<string> failedFetch = async () => {
    ///     throw new Exception("Network error");
    /// };
    /// var result2 = await failedFetch
    ///     .BindAsync(_ => parseNumbers);
    /// // For network error: result2 contains Exception
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Try<TOut>> func)
        => self.TryAsync().BindAsync(input => func(input).Try());

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload combines an asynchronous Try operation with a synchronous Result operation.
    /// The async Try must complete successfully before the Result operation is attempted.
    /// </remarks>
    /// <example>
    /// Remote config validation:
    /// <code><![CDATA[
    /// // Async operation that might throw
    /// TryAsync<Config> fetchConfig = async () => {
    ///     await Task.Delay(100); // Simulate API call
    ///     return Result.Success(new Config { Timeout = 30 });
    /// };
    ///
    /// // Sync validation returning Result
    /// Result<Config> ValidateConfig(Config config) =>
    ///     config.Timeout <= 0
    ///         ? Result.Failure<Config>(ResultError.New("Timeout must be positive"))
    ///         : Result.Success(config);
    ///
    /// // Success case
    /// var result1 = await fetchConfig
    ///     .BindAsync(config => ValidateConfig(config));
    /// // For valid config: result1 is Success(Config)
    ///
    /// // Failure case - fetch error
    /// TryAsync<Config> failedFetch = async () => {
    ///     throw new Exception("Network error");
    /// };
    /// var result2 = await failedFetch
    ///     .BindAsync(config => ValidateConfig(config));
    /// // For network error: result2 contains Exception
    ///
    /// // Failure case - validation error
    /// TryAsync<Config> invalidConfig = async () => {
    ///     return Result.Success(new Config { Timeout = -1 });
    /// };
    /// var result3 = await invalidConfig
    ///     .BindAsync(config => ValidateConfig(config));
    /// // For invalid config: result3 contains Failure("Timeout must be positive")
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Result<TOut>> func)
        => (await self.TryAsync().ConfigureAwait(false)).Bind(func);

    /// <inheritdoc cref="BindAsync{TIn,TOut}(Result{TIn},Func{TIn,Task{Result{TOut}}})"/>
    /// <remarks>
    /// This overload combines an asynchronous Try operation with an asynchronous Result operation.
    /// The Try operation must complete successfully before the async Result operation is attempted.
    /// </remarks>
    /// <example>
    /// Authentication and profile retrieval:
    /// <code><![CDATA[
    /// // First async operation that might throw
    /// TryAsync<AuthToken> authenticate = async () => {
    ///     await Task.Delay(100); // Simulate auth request
    ///     if (string.IsNullOrEmpty("token"))
    ///         throw new ArgumentException("Token required");
    ///     return Result.Success(new AuthToken { UserId = 42 });
    /// };
    ///
    /// // Second async operation returning Result
    /// async Task<Result<UserProfile>> FetchProfileAsync(AuthToken token) =>
    ///     token.UserId <= 0
    ///         ? Result.Failure<UserProfile>(ResultError.New("Invalid user ID"))
    ///         : await Task.FromResult(Result.Success(new UserProfile { Id = token.UserId }));
    ///
    /// // Success case
    /// var result1 = await authenticate
    ///     .BindAsync(async token => await FetchProfileAsync(token));
    /// // For valid credentials: result1 is Success(UserProfile)
    ///
    /// // Failure case - auth error
    /// TryAsync<AuthToken> failedAuth = async () => {
    ///     throw new Exception("Auth failed");
    /// };
    /// var result2 = await failedAuth
    ///     .BindAsync(async token => await FetchProfileAsync(token));
    /// // For auth error: result2 contains Exception
    ///
    /// // Failure case - profile error
    /// TryAsync<AuthToken> invalidToken = async () => {
    ///     return Result.Success(new AuthToken { UserId = -1 });
    /// };
    /// var result3 = await invalidToken
    ///     .BindAsync(async token => await FetchProfileAsync(token));
    /// // For invalid user ID: result3 contains Failure("Invalid user ID")
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Task<Result<TOut>>> func)
        => self.TryAsync().BindAsync(func);
}