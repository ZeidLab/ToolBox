using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Represents an error in a structured and immutable way, encapsulating details such as an error code,
/// a human-readable name, a descriptive message, and an optional exception. This type is designed to
/// be used in railway-oriented programming (ROP) paradigms, where operations are modeled as a series
/// of steps that either succeed or fail. The <see cref="Error"/> type is typically used as part of a
/// <see cref="Result{T}"/> type to represent the failure state of an operation.
/// 
/// The <see cref="Error"/> type provides factory methods for creating instances with validation,
/// ensuring that all errors are constructed with valid data. It also supports implicit conversions
/// from <see cref="string"/> and <see cref="Exception"/> to simplify error creation in common scenarios.
/// 
/// Key Features:
/// - Immutable and thread-safe: Once created, an <see cref="Error"/> instance cannot be modified.
/// - Value semantics: As a <see><cref>record struct</cref></see> , it is lightweight and copied by value.
/// - Serialization support: Marked with <see cref="SerializableAttribute"/> for use in logging,
///   transmission, or persistence scenarios.
/// - Deconstruction: Allows easy extraction of error details using tuple deconstruction.
/// - Default values: Provides default error codes and names for common use cases.
/// 
/// Example Usage:
/// <code>
/// // Create an error with a custom message
/// Error error1 = Error.New("An unexpected error occurred.");
/// 
/// // Create an error from an exception
/// Error error2 = new InvalidOperationException("Invalid operation");
/// 
/// // Create an error with a custom code, name, and message
/// Error error3 = Error.New(404, "NotFound", "The requested resource was not found.");
/// 
/// // Deconstruct an error into its components
/// var (code, name, message, exception) = error3;
/// </code>
/// 
/// This type is a fundamental building block for robust and predictable error handling in functional-style
/// C# applications.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly record struct Error
{
    internal const string DefaultName = "DefaultErrorName";
    internal const int DefaultCode = 1;


    /// <summary>Error code</summary>
    public readonly int Code;

    /// <summary>Error name. It is a meaningful name instead of numbers and more readable.</summary>
    public readonly string Name;

    /// <summary>Error message</summary>
    public readonly string Message;

    /// <summary>system exception</summary>
    public readonly Exception? Exception;


    /// <summary>
    /// Creates an instance of the <see cref="Error"/> struct.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="name">The error name. It is a meaningful name instead of numbers and more readable.</param>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The exception associated with this error, or <see langword="null"/> if there is no exception.</param>
    internal Error(int code, string name, string message, Exception? exception = null)
    {
        Code = code;
        Name = name;
        Message = message;
        Exception = exception;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> struct.
    /// </summary>
    /// <remarks>
    /// This constructor is not intended to be used directly. Instead, use the static factory methods
    /// like <see cref="New(string)"/> or <see cref="New(System.Exception)"/>.
    /// </remarks>
    [Obsolete("Use factory methods like Error.New() instead. Any instance of public constructor will be considered empty.",true)]
    public Error() => throw new InvalidOperationException("Use factory methods like Error.New() instead.");
    

    /// <summary>
    /// implicitly converts an exception to an error
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public static implicit operator Error(Exception exception) => New(exception);

    /// <summary>
    /// implicitly converts an error message to an error
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static implicit operator Error(string message) => New(message);

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the provided error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error New(string message)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        return new Error(DefaultCode, DefaultName, message);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the provided exception.
    /// </summary>
    /// <param name="exception">The exception to create a new <see cref="Error"/> instance with.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error New(Exception exception)
    {
        Guards.ThrowIfNull(exception,nameof(exception));
        return new Error(exception.HResult, DefaultName, exception.Message, exception);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the provided code and error message.
    /// </summary>
    /// <param name="code">The error code. It should be greater than 0.</param>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error New(int code, string message)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNegativeOrZero(code, nameof(code));
        return new Error(code, DefaultName, message);
    }


    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the provided error name and error message.
    /// </summary>
    /// <param name="name">The error name. It is a meaningful name instead of numbers and more readable. It should not be <see langword="null"/> or empty.</param>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error New(string name, string message)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNullOrWhiteSpace(name, nameof(name));

        return new Error(DefaultCode, name, message);
    }


    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the provided error name, error message, and exception.
    /// </summary>
    /// <param name="name">The error name. It is a meaningful name instead of numbers and more readable. It should not be <see langword="null"/> or empty.</param>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <param name="exception">The exception associated with this error. It should not be <see langword="null"/>.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error New(string name, string message, Exception exception)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNullOrWhiteSpace(name, nameof(name));

        return new Error(DefaultCode, name, message, exception);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the provided code, error message, and exception.
    /// </summary>
    /// <param name="code">The code of the error. It should be greater than 0.</param>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <param name="exception">The exception associated with this error. It should not be <see langword="null"/>.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error New(int code, string message, Exception exception)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNegativeOrZero(code, nameof(code));

        return new Error(code, DefaultName, message, exception);
    }


    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the provided code, error message, and exception.
    /// The code of the error is set to the HResult of the exception.
    /// </summary>
    /// <param name="message">The error message. It should not be <see langword="null"/> or empty.</param>
    /// <param name="exception">The exception associated with this error. It should not be <see langword="null"/>.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error New(string message, Exception exception)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        Guards.ThrowIfNull(exception, nameof(exception));
        return new Error(DefaultCode, DefaultName, message, exception);
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