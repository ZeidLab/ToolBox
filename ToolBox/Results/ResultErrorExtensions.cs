using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Extension methods for <see cref="ResultError"/> to provide additional functionality
/// and fluent API operations.
/// </summary>
/// <remarks>
/// This class provides a set of extension methods that enhance the functionality of <see cref="ResultError"/>
/// by adding methods for error code manipulation, exception handling, and error type checking.
///
/// <example>
/// Basic usage:
/// <code>
/// var error = new ResultError(1, "ValidationError", "Invalid input")
///     .WithCode(ResultErrorCode.Validation)
///     .WithMessage("Email format is invalid");
///
/// if (error.IsValidationError())
/// {
///     // Handle validation error
/// }
/// </code>
/// </example>
/// </remarks>
public static class ResultErrorExtensions
{
    /// <summary>
    /// Tries to get the exception associated with this error.
    /// </summary>
    /// <param name="self">The error instance.</param>
    /// <param name="exception">The exception associated with this error, or <c>null</c> if there is no exception.</param>
    /// <returns>
    /// <see langword="true"/> if there is an exception associated with this error;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// var error = new ResultError(1, "Error", "Failed", new Exception("Test"));
    /// if (error.TryGetException(out var ex))
    /// {
    ///     Console.WriteLine(ex.Message); // Outputs: Test
    /// }
    /// </code>
    /// </example>
    public static bool TryGetException(this ResultError self, out Exception? exception)
    {
        exception = self.Exception;
        return exception is not null;
    }

    /// <summary>
    /// Tries to get the exception associated with this error.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <returns>
    /// A <see cref="Maybe{T}"/> that contains the exception associated with this
    /// error, or <see cref="Maybe.None{T}"/> if there is no exception.
    /// </returns>
    /// <example>
    /// <code>
    /// var error = new ResultError(1, "Error", "Failed", new Exception("Test"));
    /// var maybeException = error.TryGetException();
    /// if (maybeException.IsSome)
    /// {
    ///     Console.WriteLine(maybeException.Value.Message); // Outputs: Test
    /// }
    /// </code>
    /// </example>
    public static Maybe<Exception> TryGetException(this ResultError self)
    {
        return self.Exception is {} exception ? exception.ToSome() : Maybe.None<Exception>();
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the specified error code.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="code">The new error code. It must be greater than 0.</param>
    /// <returns>A new <see cref="ResultError"/> instance with the updated code.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="code"/> is negative or zero.</exception>
    /// <example>
    /// <code>
    /// var error = new ResultError(1, "Error", "Message")
    ///     .WithCode(42);
    /// Console.WriteLine(error.Code); // Outputs: 42
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError WithCode(this ResultError self, int code)
    {
        Guards.ThrowIfNegativeOrZero(code, nameof(code));
        return new ResultError(code, self.Name, self.Message, self.Exception);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the specified error code.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="code">The new error code from the <see cref="ResultErrorCode"/> enum.</param>
    /// <returns>A new <see cref="ResultError"/> instance with the updated code.</returns>
    /// <example>
    /// <code>
    /// var error = new ResultError(1, "Error", "Message")
    ///     .WithCode(ResultErrorCode.Validation);
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError WithCode(this ResultError self, ResultErrorCode code)
    {
        return new ResultError((int)code, self.Name, self.Message, self.Exception);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the specified error message.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="message">The new error message. It must be non-empty and non-null.</param>
    /// <returns>A new <see cref="ResultError"/> instance with the updated message.</returns>
    /// <exception cref="ArgumentException"><paramref name="message"/> is null or empty.</exception>
    /// <example>
    /// <code>
    /// var error = new ResultError(1, "Error", "Old message")
    ///     .WithMessage("New error message");
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError WithMessage(this ResultError self, string message)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        return new ResultError(self.Code, self.Name, message, self.Exception);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the specified error name.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="name">The new error name. It must be non-empty and non-null.</param>
    /// <returns>A new <see cref="ResultError"/> instance with the updated name.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or empty.</exception>
    /// <example>
    /// <code>
    /// var error = new ResultError(1, "OldName", "Message")
    ///     .WithName("ValidationError");
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError WithName(this ResultError self, string name)
    {
        Guards.ThrowIfNullOrWhiteSpace(name, nameof(name));
        return new ResultError(self.Code, name, self.Message, self.Exception);
    }

    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the specified exception.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="exception">The exception to associate with the new error instance. It must be non-null.</param>
    /// <returns>A new <see cref="ResultError"/> instance with the updated exception.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is null.</exception>
    /// <example>
    /// <code>
    /// var error = new ResultError(1, "Error", "Message")
    ///     .WithException(new InvalidOperationException("Operation failed"));
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultError WithException(this ResultError self, Exception exception)
    {
        Guards.ThrowIfNull(exception, nameof(exception));
        return new ResultError(self.Code, self.Name, self.Message, exception);
    }

    /// <summary>
    /// Determines if the error is of a specific error code.
    /// </summary>
    /// <param name="self">The error instance.</param>
    /// <param name="code">The error code to check against.</param>
    /// <returns>True if the error's code matches the specified code; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// var error = new ResultError((int)ResultErrorCode.Validation, "Error", "Message");
    /// if (error.IsErrorCode(ResultErrorCode.Validation))
    /// {
    ///     // Handle validation error
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsErrorCode(this ResultError self, ResultErrorCode code)
    {
        return self.Code == (int)code;
    }

    /// <summary>
    /// Determines if the error represents a validation error.
    /// </summary>
    /// <param name="self">The error instance.</param>
    /// <returns>True if the error is a validation error; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// var error = new ResultError((int)ResultErrorCode.Validation, "Error", "Message");
    /// if (error.IsValidationError())
    /// {
    ///     // Handle validation error
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidationError(this ResultError self)
    {
        return self.IsErrorCode(ResultErrorCode.Validation);
    }

    /// <summary>
    /// Determines if the error represents a not found error.
    /// </summary>
    /// <param name="self">The error instance.</param>
    /// <returns>True if the error is a not found error; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// var error = new ResultError((int)ResultErrorCode.NotFound, "Error", "Message");
    /// if (error.IsNotFoundError())
    /// {
    ///     // Handle not found error
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotFoundError(this ResultError self)
    {
        return self.IsErrorCode(ResultErrorCode.NotFound);
    }

    /// <summary>
    /// Determines if the error represents an internal error.
    /// </summary>
    /// <param name="self">The error instance.</param>
    /// <returns>True if the error is an internal error; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// var error = new ResultError((int)ResultErrorCode.Internal, "Error", "Message");
    /// if (error.IsInternalError())
    /// {
    ///     // Handle internal error
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInternalError(this ResultError self)
    {
        return self.IsErrorCode(ResultErrorCode.Internal);
    }
}