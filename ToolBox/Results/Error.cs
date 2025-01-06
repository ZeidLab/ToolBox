using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZeidLab.ToolBox.Results;

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
    /// <param name="isEmpty"></param>
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
    [Obsolete("Use factory methods like Error.New() instead. Any instance of public constructor will be considered empty.")]
    public Error() => throw new InvalidOperationException("Use factory methods like Error.New() instead.");
    

    public static implicit operator Error(Exception exception) => New(exception);

    public static implicit operator Error(string message) => New(message);

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the provided error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error New(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
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
        ArgumentNullException.ThrowIfNull(exception);
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
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(code, nameof(code));
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
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

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
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        return new Error(exception.HResult, name, message, exception);
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
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(code, nameof(code));

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
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentNullException.ThrowIfNull(exception, nameof(exception));
        return new Error(exception.HResult, DefaultName, message, exception);
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