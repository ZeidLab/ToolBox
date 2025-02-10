using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZeidLab.ToolBox.Common;
namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Represents an error in a structured and immutable way, encapsulating details such as an error code,
/// a human-readable name, a descriptive message, and an optional exception. This type is designed to
/// be used in railway-oriented programming (ROP) paradigms, where operations are modeled as a series
/// of steps that either succeed or fail. The <see cref="ResultError"/> type is typically used as part of a
/// <see cref="Result{T}"/> type to represent the failure state of an operation.
///
/// The <see cref="ResultError"/> type provides factory methods for creating instances with validation,
/// ensuring that all errors are constructed with valid data. It also supports implicit conversions
/// from <see cref="string"/> and <see cref="Exception"/> to simplify error creation in common scenarios.
///
/// Key Features:
/// - Immutable and thread-safe: Once created, an <see cref="ResultError"/> instance cannot be modified.
/// - Value semantics: As a <see><cref>record struct</cref></see> , it is lightweight and copied by value.
/// - Serialization support: Marked with <see cref="SerializableAttribute"/> for use in logging,
///   transmission, or persistence scenarios.
/// - Deconstruction: Allows easy extraction of error details using tuple deconstruction.
/// - Default values: Provides default error codes and names for common use cases.
///
/// Example Usage:
/// <code>
/// // Create an error with a custom message
/// ResultError error1 = Error.New("An unexpected error occurred.");
///
/// // Create an error from an exception
/// ResultError error2 = new InvalidOperationException("Invalid operation");
///
/// // Create an error with a custom code, name, and message
/// ResultError error3 = Error.New(404, "NotFound", "The requested resource was not found.");
///
/// // Deconstruct an error into its components
/// var (code, name, message, exception) = error3;
/// </code>
///
/// This type is a fundamental building block for robust and predictable error handling in functional-style
/// C# applications.
/// </summary>

/// <summary>
/// Represents standard error codes for <see cref="ResultError"/>
/// </summary>
public enum ResultErrorCode
{
    /// <summary>
    /// No error
    /// </summary>
    None = 0,

    /// <summary>
    /// Generic error
    /// </summary>
    Generic = 1,

    /// <summary>
    /// Validation error
    /// </summary>
    Validation = 400,

    /// <summary>
    /// Not found error
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// Internal error
    /// </summary>
    Internal = 500
}

/// <summary>
/// Represents an error in a structured and immutable way.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly record struct ResultError
{
    /// <summary>
    /// The default error value.
    /// </summary>
	public static readonly  ResultError Default ;
    internal const string DefaultName = "UnspecifiedError";
    internal const int DefaultCode = (int)ResultErrorCode.Generic;

    private readonly int code;
    private readonly string name;
    private readonly string message;
    private readonly Exception? exception;

    /// <summary>Error code</summary>
    public int Code => code;

    /// <summary>Error name. It is a meaningful name instead of numbers and more readable.</summary>
    public string Name => name;

    /// <summary>Error message</summary>
    public string Message => message;

    /// <summary>System exception</summary>
    public Exception? Exception => exception;

    /// <summary>
    /// Creates an instance of the <see cref="ResultError"/> struct.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="name">The error name. It is a meaningful name instead of numbers and more readable.</param>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The exception associated with this error, or <see langword="null"/> if there is no exception.</param>
    internal ResultError(int code, string name, string message, Exception? exception = null)
    {
        this.code = code;
        this.name = name;
        this.message = message;
        this.exception = exception;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultError"/> struct.
    /// </summary>
    /// <remarks>
    /// This constructor is not intended to be used directly. Instead, use the static factory methods
    /// like <see cref="New(string)"/> or <see cref="New(System.Exception)"/>.
    /// </remarks>
#pragma warning disable S1133 // Do not forget to remove this deprecated code someday
    [Obsolete(
        "Use factory methods like ResultError.New() instead. Any instance of public constructor will be considered empty.",
        true)]
    public ResultError() => throw new InvalidOperationException("Use factory methods like ResultError.New() instead.");
#pragma warning restore S1133 //Do not forget to remove this deprecated code someday

    /// <summary>
    /// Provides implicit conversion from an <see cref="Exception"/> to a <see cref="ResultError"/>.
    /// </summary>
    /// <param name="exception">
    /// The exception to convert. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="ResultError"/> instance with:
    /// <list type="bullet">
    ///   <item><description>Code: The exception's <see cref="Exception.HResult"/></description></item>
    ///   <item><description>Name: <see cref="DefaultName"/></description></item>
    ///   <item><description>Message: The exception's message</description></item>
    ///   <item><description>Exception: The original exception</description></item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="exception"/> is <see langword="null"/>.
    /// </exception>
    public static implicit operator ResultError(Exception exception) => New(exception);

    /// <summary>
    /// Implicitly converts a string message to a ResultError.
    /// </summary>
    /// <param name="message">The error message</param>
    /// <remarks>
    /// The resulting error will use the default error code and name.
    /// </remarks>
    /// <returns>A new ResultError instance</returns>
    public static implicit operator ResultError(string message) => New(message);

    /// <summary>
    /// Returns a string representation of the error for debugging.
    /// </summary>
    /// <returns>A string containing the error's code, name, and message.</returns>
    public override string ToString() => $"[{Code}] {Name}: {Message}";

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the provided error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new <see cref="ResultError"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError New(string message)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        return new ResultError(DefaultCode, DefaultName, message);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the provided exception.
    /// </summary>
    /// <param name="exception">The exception to create a new <see cref="ResultError"/> instance with.</param>
    /// <returns>A new <see cref="ResultError"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError New(Exception exception)
    {
        Guards.ThrowIfNull(exception, nameof(exception));
        return new ResultError(exception.HResult, DefaultName, exception.Message, exception);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the provided code and error message.
    /// </summary>
    /// <param name="code">The error code. It should be greater than 0.</param>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <returns>A new <see cref="ResultError"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError New(int code, string message)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNegativeOrZero(code, nameof(code));
        return new ResultError(code, DefaultName, message);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the provided error name and error message.
    /// </summary>
    /// <param name="name">The error name. It is a meaningful name instead of numbers and more readable. It should not be <see langword="null"/> or empty.</param>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <returns>A new <see cref="ResultError"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError New(string name, string message)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNullOrWhiteSpace(name, nameof(name));
        return new ResultError(DefaultCode, name, message);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the provided error name, error message, and exception.
    /// </summary>
    /// <param name="name">The error name. It is a meaningful name instead of numbers and more readable. It should not be <see langword="null"/> or empty.</param>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <param name="exception">The exception associated with this error. It should not be <see langword="null"/>.</param>
    /// <returns>A new <see cref="ResultError"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError New(string name, string message, Exception exception)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Guards.ThrowIfNull(exception, nameof(exception));
        return new ResultError(exception.HResult, name, message, exception);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the provided code, error message, and exception.
    /// </summary>
    /// <param name="code">The code of the error. It should be greater than 0.</param>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <param name="exception">The exception associated with this error. It should not be <see langword="null"/>.</param>
    /// <returns>A new <see cref="ResultError"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError New(int code, string message, Exception exception)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNegativeOrZero(code, nameof(code));
        Guards.ThrowIfNull(exception, nameof(exception));
        return new ResultError(code, DefaultName, message, exception);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the provided error message and exception.
    /// </summary>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <param name="exception">The exception associated with this error. It should not be <see langword="null"/>.</param>
    /// <returns>A new <see cref="ResultError"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError New(string message, Exception exception)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNull(exception, nameof(exception));
        return new ResultError(exception.HResult, DefaultName, message, exception);
    }

    /// <summary>
    /// Deconstructs the error into its code, name, message, and unhandled exception.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="name">The error name.</param>
    /// <param name="message">The error message.</param>
    /// <param name="unhandledException">The unhandled exception that caused this error, or none if none.</param>
    public void Deconstruct(out int code, out string name, out string message, out Exception? unhandledException)
    {
        code = Code;
        name = Name;
        message = Message;
        unhandledException = Exception;
    }
}
