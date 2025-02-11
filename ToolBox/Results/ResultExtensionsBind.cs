using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;

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
/// <code>
/// // Create a function that validates a positive number
/// Result&lt;int&gt; ValidatePositive(int x) =>
///     x > 0 ? Result.Success(x)
///           : Result.Failure&lt;int&gt;(ResultError.New("Number must be positive"));
///
/// // Create a function that validates if a number is even
/// Result&lt;int&gt; ValidateEven(int x) =>
///     x % 2 == 0 ? Result.Success(x)
///                : Result.Failure&lt;int&gt;(ResultError.New("Number must be even"));
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
/// </code>
/// </example>
/// <example>
/// Error propagation with string processing:
/// <code>
/// Result&lt;string&gt; ValidateNotEmpty(string input) =>
///     string.IsNullOrEmpty(input)
///         ? Result.Failure&lt;string&gt;(ResultError.New("Input cannot be empty"))
///         : Result.Success(input);
///
/// Result&lt;string&gt; ValidateLength(string input) =>
///     input.Length > 10
///         ? Result.Failure&lt;string&gt;(ResultError.New("Input too long"))
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
/// </code>
/// </example>
/// <example>
/// User registration validation example:
/// <code>
/// public class UserRegistration
/// {
///     public Result&lt;string&gt; ValidateUsername(string username) =>
///         string.IsNullOrWhiteSpace(username)
///             ? Result.Failure&lt;string&gt;(ResultError.New("Username cannot be empty"))
///             : username.Length &lt; 3
///                 ? Result.Failure&lt;string&gt;(ResultError.New("Username too short"))
///                 : Result.Success(username);
///
///     public Result&lt;string&gt; ValidatePassword(string password) =>
///         string.IsNullOrWhiteSpace(password)
///             ? Result.Failure&lt;string&gt;(ResultError.New("Password cannot be empty"))
///             : password.Length &lt; 8
///                 ? Result.Failure&lt;string&gt;(ResultError.New("Password too short"))
///                 : Result.Success(password);
///
///     public Result&lt;(string username, string password)&gt; ValidateRegistration(
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
/// </code>
/// </example>
/// <example>
/// Configuration parsing example:
/// <code>
/// public class ConfigParser
/// {
///     public Result&lt;int&gt; ParsePort(string value) =>
///         int.TryParse(value, out int port)
///             ? port > 0 && port &lt; 65536
///                 ? Result.Success(port)
///                 : Result.Failure&lt;int&gt;(ResultError.New("Port must be between 1 and 65535"))
///             : Result.Failure&lt;int&gt;(ResultError.New("Invalid port format"));
///
///     public Result&lt;TimeSpan&gt; ParseTimeout(string value) =>
///         int.TryParse(value, out int seconds)
///             ? seconds > 0
///                 ? Result.Success(TimeSpan.FromSeconds(seconds))
///                 : Result.Failure&lt;TimeSpan&gt;(ResultError.New("Timeout must be positive"))
///             : Result.Failure&lt;TimeSpan&gt;(ResultError.New("Invalid timeout format"));
///
///     public Result&lt;(int port, TimeSpan timeout)&gt; ParseConfig(
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
/// </code>
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
    /// A new result that will be:
    /// <list type="bullet">
    /// <item><description>The result of applying <paramref name="func"/> to the value if the input result is successful</description></item>
    /// <item><description>A failure containing the original error if the input result is a failure</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// Here's an example of validating user input:
    /// <code>
    /// Result&lt;string&gt; ValidateEmail(string email) =>
    ///     email.Contains("@")
    ///         ? Result.Success(email)
    ///         : Result.Failure&lt;string&gt;(ResultError.New("Invalid email format"));
    ///
    /// Result&lt;string&gt; ValidateDomain(string email) =>
    ///     email.EndsWith(".com")
    ///         ? Result.Success(email)
    ///         : Result.Failure&lt;string&gt;(ResultError.New("Only .com domains allowed"));
    ///
    /// // Chain multiple validations
    /// var result = Result.Success("user@example.com")
    ///     .Bind(ValidateEmail)
    ///     .Bind(ValidateDomain);
    /// Console.WriteLine(result.IsSuccess); // Output: True
    ///
    /// // Invalid input shows how errors propagate
    /// var invalid = Result.Success("invalid-email")
    ///     .Bind(ValidateEmail)
    ///     .Bind(ValidateDomain);
    /// Console.WriteLine(invalid.IsFailure); // Output: True
    /// Console.WriteLine(invalid.Error.Message); // Output: "Invalid email format"
    /// </code>
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
    /// A new Result that will be:
    /// <list type="bullet">
    /// <item><description>The result of applying <paramref name="func"/> to the value if the Try operation succeeds</description></item>
    /// <item><description>A failure containing the exception as a ResultError if the Try operation fails</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// Safe number parsing and validation:
    /// <code>
    /// // Define a Try operation for parsing
    /// Try&lt;int&gt; ParseNumber = () =>
    ///     Result.Success(int.Parse("123"));
    ///
    /// // Define a validation function
    /// Result&lt;int&gt; ValidateRange(int number) =>
    ///     (number &gt;= 1 && number &lt;= 100)
    ///         ? Result.Success(number)
    ///         : Result.Failure&lt;int&gt;(ResultError.New("Number must be between 1 and 100"));
    ///
    /// // Combine parsing and validation
    /// var result = ParseNumber.Bind(ValidateRange);
    /// Console.WriteLine(result.IsSuccess); // Output: True
    ///
    /// // Example with parsing failure
    /// Try&lt;int&gt; ParseInvalid = () =>
    ///     Result.Success(int.Parse("not a number"));
    /// var failed = ParseInvalid.Bind(ValidateRange);
    /// Console.WriteLine(failed.IsFailure); // Output: True
    /// // Error will contain FormatException details
    /// </code>
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
    /// A new Result that will be:
    /// <list type="bullet">
    /// <item><description>Successful if both Try operations succeed</description></item>
    /// <item><description>A failure containing the first encountered exception as a ResultError</description></item>
    /// </list>
    /// </returns>
    /// <example>
    /// Safe data parsing example:
    /// <code>
    /// public class DataProcessor
    /// {
    ///     // First Try operation: Parse date string
    ///     public Try&lt;DateTime&gt; ParseDate(string input) => () =>
    ///     {
    ///         if (DateTime.TryParse(input, out var date))
    ///             return Result.Success(date);
    ///         throw new FormatException($"Invalid date format: {input}");
    ///     };
    ///
    ///     // Second Try operation: Validate business rules
    ///     public Try&lt;DateTime&gt; ValidateBusinessDay(DateTime date) => () =>
    ///     {
    ///         if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
    ///             throw new ArgumentException("Date must be a business day");
    ///         if (date.Date &lt; DateTime.Today)
    ///             throw new ArgumentException("Date must not be in the past");
    ///         return Result.Success(date);
    ///     };
    ///
    ///     public Result&lt;DateTime&gt; ProcessDate(string input) =>
    ///         ParseDate(input).Bind(ValidateBusinessDay);
    /// }
    ///
    /// // Usage:
    /// var processor = new DataProcessor();
    ///
    /// // Invalid format
    /// var result1 = processor.ProcessDate("not a date");
    /// Console.WriteLine(result1.IsFailure); // Output: True
    /// Console.WriteLine(result1.Error.Message); // Output: "Invalid date format: not a date"
    ///
    /// // Weekend date
    /// var result2 = processor.ProcessDate("2024-01-13"); // A Saturday
    /// Console.WriteLine(result2.IsFailure); // Output: True
    /// Console.WriteLine(result2.Error.Message); // Output: "Date must be a business day"
    ///
    /// // Valid business day
    /// var result3 = processor.ProcessDate("2024-01-15"); // A Monday
    /// Console.WriteLine(result3.IsSuccess); // Output: True
    /// </code>
    /// </example>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOut> Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Try<TOut>> func)
        => self.Try().Match(
            success: input => func(input).Try(),
            failure: error => Result.Failure<TOut>(error));

}