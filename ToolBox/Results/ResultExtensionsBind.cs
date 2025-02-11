using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for binding operations on <see cref="Result{TValue}"/> objects.
/// These methods enable monadic composition of Result types, allowing for clean error handling
/// and functional programming patterns.
/// </summary>
/// <remarks>
/// <para>
/// Binding operations allow you to chain multiple operations that return Results while
/// maintaining proper error propagation.
/// </para>
/// <example>
/// Basic usage with successful results:
/// <code><![CDATA[
/// // Create a function that validates a positive number
/// Result<int> ValidatePositive(int x) =>
///     x > 0 ? Result.Success(x)
///           : Result.Failure<int>(ResultError.New("Number must be positive"));
///
/// // Create a function that validates if a number is even
/// Result<int> ValidateEven(int x) =>
///     x % 2 == 0 ? Result.Success(x)
///                : Result.Failure<int>(ResultError.New("Number must be even"));
///
/// // Chain validations using Bind
/// var result = Result.Success(4)
///     .Bind(ValidatePositive)
///     .Bind(ValidateEven);
/// Console.WriteLine(result.IsSuccess); // Output: True
///
/// // Chain that will fail
/// var failedResult = Result.Success(-2)
///     .Bind(ValidatePositive)
///     .Bind(ValidateEven);
/// Console.WriteLine(failedResult.IsFailure); // Output: True
/// Console.WriteLine(failedResult.Error.Message); // Output: "Number must be positive"
/// ]]></code>
/// </example>
/// <example>
/// Error propagation with string processing:
/// <code><![CDATA[
/// Result<string> ValidateNotEmpty(string input) =>
///     string.IsNullOrEmpty(input)
///         ? Result.Failure<string>(ResultError.New("Input cannot be empty"))
///         : Result.Success(input);
///
/// Result<string> ValidateLength(string input) =>
///     input.Length > 10
///         ? Result.Failure<string>(ResultError.New("Input too long"))
///         : Result.Success(input);
///
/// var result = Result.Success("Hello")
///     .Bind(ValidateNotEmpty)
///     .Bind(ValidateLength);
/// Console.WriteLine(result.IsSuccess); // Output: True
///
/// var failedResult = Result.Success("")
///     .Bind(ValidateNotEmpty)
///     .Bind(ValidateLength);
/// Console.WriteLine(failedResult.IsFailure); // Output: True
/// Console.WriteLine(failedResult.Error.Message); // Output: "Input cannot be empty"
/// ]]></code>
/// </example>
/// <example>
/// User registration validation example:
/// <code><![CDATA[
/// public class UserRegistration
/// {
///     public Result<string> ValidateUsername(string username) =>
///         string.IsNullOrWhiteSpace(username)
///             ? Result.Failure<string>(ResultError.New("Username cannot be empty"))
///             : username.Length < 3
///                 ? Result.Failure<string>(ResultError.New("Username too short"))
///                 : Result.Success(username);
///
///     public Result<string> ValidatePassword(string password) =>
///         string.IsNullOrWhiteSpace(password)
///             ? Result.Failure<string>(ResultError.New("Password cannot be empty"))
///             : password.Length < 8
///                 ? Result.Failure<string>(ResultError.New("Password too short"))
///                 : Result.Success(password);
///
///     public Result<(string username, string password)> ValidateRegistration(
///         string username, string password) =>
///         Result.Success(username)
///             .Bind(ValidateUsername)
///             .Bind(validUsername =>
///                 ValidatePassword(password)
///                     .Bind(validPassword =>
///                         Result.Success((validUsername, validPassword))));
/// }
///
/// // Usage:
/// var registration = new UserRegistration();
/// var result = registration.ValidateRegistration("jo", "weak");
/// Console.WriteLine(result.IsFailure); // Output: True
/// Console.WriteLine(result.Error.Message); // Output: "Username too short"
///
/// var success = registration.ValidateRegistration("john", "strongpass123");
/// Console.WriteLine(success.IsSuccess); // Output: True
/// ]]></code>
/// </example>
/// <example>
/// Configuration parsing example:
/// <code><![CDATA[
/// public class ConfigParser
/// {
///     public Result<int> ParsePort(string value) =>
///         int.TryParse(value, out int port)
///             ? port > 0 && port < 65536
///                 ? Result.Success(port)
///                 : Result.Failure<int>(ResultError.New("Port must be between 1 and 65535"))
///             : Result.Failure<int>(ResultError.New("Invalid port format"));
///
///     public Result<TimeSpan> ParseTimeout(string value) =>
///         int.TryParse(value, out int seconds)
///             ? seconds > 0
///                 ? Result.Success(TimeSpan.FromSeconds(seconds))
///                 : Result.Failure<TimeSpan>(ResultError.New("Timeout must be positive"))
///             : Result.Failure<TimeSpan>(ResultError.New("Invalid timeout format"));
///
///     public Result<(int port, TimeSpan timeout)> ParseConfig(
///         string portStr, string timeoutStr) =>
///         ParsePort(portStr)
///             .Bind(port =>
///                 ParseTimeout(timeoutStr)
///                     .Bind(timeout =>
///                         Result.Success((port, timeout))));
/// }
///
/// // Usage:
/// var parser = new ConfigParser();
/// var failed = parser.ParseConfig("invalid", "30");
/// Console.WriteLine(failed.IsFailure); // Output: True
/// Console.WriteLine(failed.Error.Message); // Output: "Invalid port format"
///
/// var success = parser.ParseConfig("8080", "30");
/// Console.WriteLine(success.IsSuccess); // Output: True
/// ]]></code>
/// </example>
/// </remarks>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsBind
{
    /// <summary>
    /// Binds the value of a successful result to a new result by applying the specified function.
    /// If the input result is a failure, the error is propagated to the output result.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the input result.</typeparam>
    /// <typeparam name="TOut">The type of the value in the output result.</typeparam>
    /// <param name="self">The result to bind.</param>
    /// <param name="func">The function to apply to the value if the result is successful.</param>
    /// <returns>
    /// <list type="bullet">
    /// <item><description>The result of applying <paramref name="func"/> to the value if the input result is successful</description></item>
    /// <item><description>A failure containing the original error if the input result is a failure</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// Basic email validation example:
    /// <code><![CDATA[
    /// Result<string> ValidateEmail(string email) =>
    ///     !email.Contains("@")
    ///         ? Result.Failure<string>(ResultError.New("Invalid email format"))
    ///         : Result.Success(email);
    ///
    /// Result<string> ValidateDomain(string email) =>
    ///     !email.EndsWith(".com")
    ///         ? Result.Failure<string>(ResultError.New("Only .com domains allowed"))
    ///         : Result.Success(email);
    ///
    /// // Chain multiple validations
    /// var result = Result.Success("user@example.com")
    ///     .Bind(ValidateEmail)
    ///     .Bind(ValidateDomain);
    /// Console.WriteLine(result.IsSuccess); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> self, Func<TIn, Result<TOut>> func)
        => self.Match(
            success: func,
            failure: error => Result.Failure<TOut>(error));

    /// <summary>
    /// Binds the value of a successful <see cref="Try{TIn}"/> to a new result by applying the specified function.
    /// If the Try operation fails, the exception is converted to a ResultError.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the input Try.</typeparam>
    /// <typeparam name="TOut">The type of the value in the output Result.</typeparam>
    /// <param name="self">The Try operation to bind.</param>
    /// <param name="func">The function to apply to the value if the Try operation succeeds.</param>
    /// <returns>
    /// <list type="bullet">
    /// <item><description>The result of applying <paramref name="func"/> to the value if the Try operation succeeds</description></item>
    /// <item><description>A failure containing the exception as a ResultError if the Try operation fails</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// Safe number parsing and validation:
    /// <code><![CDATA[
    /// // Define a Try operation for parsing
    /// Try<int> ParseNumber(string input) => () =>
    /// {
    ///     if (int.TryParse(input, out int number))
    ///         return Result.Success(number);
    ///     return Result.Failure<int>(ResultError.New($"Cannot parse '{input}' as number"));
    /// };
    ///
    /// // Define a validation function
    /// Result<int> ValidateRange(int number) =>
    ///     number >= 1 && number <= 100
    ///         ? Result.Success(number)
    ///         : Result.Failure<int>(ResultError.New("Number must be between 1 and 100"));
    ///
    /// // Usage:
    /// var result = ParseNumber("42").Bind(ValidateRange);
    /// Console.WriteLine(result.IsSuccess); // Output: True
    ///
    /// var failed = ParseNumber("not a number").Bind(ValidateRange);
    /// Console.WriteLine(failed.IsFailure); // Output: True
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Result<TOut>> func)
        => self.Try().Match(
            success: func,
            failure: error => Result.Failure<TOut>(error));

    /// <summary>
    /// Binds the value of a successful <see cref="Try{TIn}"/> to a new result by applying another Try operation.
    /// Both Try operations must succeed for the result to be successful.
    /// </summary>
    /// <typeparam name="TIn">The type of the value in the input Try.</typeparam>
    /// <typeparam name="TOut">The type of the value in the output Result.</typeparam>
    /// <param name="self">The first Try operation to bind.</param>
    /// <param name="func">The function returning a second Try operation to apply if the first succeeds.</param>
    /// <returns>
    /// <list type="bullet">
    /// <item><description>Successful if both Try operations succeed</description></item>
    /// <item><description>A failure containing the first encountered exception as a ResultError</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// Safe date parsing and validation:
    /// <code><![CDATA[
    /// public class DateProcessor
    /// {
    ///     // Parse date string safely
    ///     public Try<DateTime> ParseDate(string input) => () =>
    ///     {
    ///         if (DateTime.TryParse(input, out var date))
    ///             return Result.Success(date);
    ///         return Result.Failure<DateTime>(ResultError.New($"Invalid date: {input}"));
    ///     };
    ///
    ///     // Validate business rules
    ///     public Try<DateTime> ValidateBusinessDay(DateTime date) => () =>
    ///     {
    ///         if (date.DayOfWeek is DayOfWeek.Saturday || date.DayOfWeek is DayOfWeek.Sunday)
    ///             return Result.Failure<DateTime>(ResultError.New("Must be a business day"));
    ///         if (date < DateTime.Today)
    ///             return Result.Failure<DateTime>(ResultError.New("Must not be in the past"));
    ///         return Result.Success(date);
    ///     };
    ///
    ///     // Process date with validation
    ///     public Result<DateTime> ProcessDate(string input) =>
    ///         ParseDate(input).Bind(ValidateBusinessDay);
    /// }
    /// ]]></code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Try<TOut>> func)
        => self.Try().Match(
            success: input => func(input).Try(),
            failure: error => Result.Failure<TOut>(error));
}