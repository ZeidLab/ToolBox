using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Extension methods for <see cref="ResultError"/> to provide additional functionality
/// and fluent API operations for error handling and manipulation.
/// </summary>
/// <remarks>
/// This class provides extension methods that enhance <see cref="ResultError"/> functionality by adding
/// methods for error code manipulation, exception handling, and error type checking. The extensions
/// support a fluent API style and maintain the immutable nature of <see cref="ResultError"/>.
///
/// Each method creates a new instance of <see cref="ResultError"/> rather than modifying the existing one,
/// ensuring thread safety and immutability.
/// </remarks>
/// <example>
/// Here's an example demonstrating common usage patterns:
/// <code><![CDATA[
/// // Create and customize an error
/// var error = ResultError.New("Invalid input data")
///     .WithCode((int)ResultErrorCode.Validation)
///     .WithMessage("Username must be between 3 and 20 characters");
///
/// // Create a Result with the error
/// Result<string> result = error;
/// Console.WriteLine(result.IsFailure); // true
///
/// // Working with exceptions
/// if (error.TryGetException(out var exception))
/// {
///     // Log or handle the exception
///     Console.WriteLine(exception.Message);
/// }
/// ]]></code>
/// </example>

public static class ResultErrorExtensions
{

    /// <summary>
    /// Tries to get the exception associated with this error.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <returns>
    /// A <see cref="Maybe{T}"/> that contains the exception associated with this
    /// error, or <see cref="Maybe{T}.IsNone"/> if there is no exception.
    /// </returns>
    /// <example>
    /// <code><![CDATA[
    /// var ex = new Exception("Test");
    /// var error = ResultError.New(ex);
    /// var maybeException = error.TryGetException();
    /// if (maybeException.IsSome)
    /// {
    ///     Console.WriteLine(maybeException.Value.Message); // Outputs: Test
    /// }
    /// ]]></code>
    /// </example>
    public static Maybe<Exception> TryGetException(this ResultError self)
    {
	    return self.Exception is { } exception ? exception.ToSome() : Maybe.None<Exception>();
    }


    /// <summary>
    /// Creates a new <see cref="ResultError"/> instance with the specified error code.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="code">The new error code. It must be greater than 0.</param>
    /// <returns>A new <see cref="ResultError"/> instance with the updated code.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="code"/> is negative or zero.</exception>
    /// <example>
    /// <code><![CDATA[
    /// // Create an error with a custom error code
    /// var error = ResultError.New("Resource access denied")
    ///     .WithCode(403);
    ///
    /// // Use the error in a Result
    /// Result<string> result = error;
    /// if (result.IsFailure)
    /// {
    ///     Console.WriteLine($"Error [{result.Error.Code}]: {result.Error.Message}");
    ///     // Output: Error [403]: Resource access denied
    /// }
    /// ]]></code>
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
    /// <code><![CDATA[
    /// // Create an error with a standard error code
    /// var error = ResultError.New("Invalid user data")
    ///     .WithCode((int)ResultErrorCode.Validation)
    ///     .WithName("ValidationError");
    ///
    /// // Use in validation logic
    /// if (error.Code == (int)ResultErrorCode.Validation)
    /// {
    ///     Console.WriteLine("Handling validation error");
    /// }
    /// ]]></code>
    /// </example>
    [Pure]
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ResultError WithCode(this ResultError self, ResultErrorCode code)
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
    /// <code><![CDATA[
    /// // Create an error and update its message
    /// var error = ResultError.New("Initial error")
    ///     .WithCode((int)ResultErrorCode.Validation)
    ///     .WithMessage("Email format is invalid");
    ///
    /// // Use in Result pattern
    /// Result<string> result = error;
    /// if (result.IsFailure)
    /// {
    ///     Console.WriteLine(result.Error.Message);
    ///     // Output: "Email format is invalid"
    /// }
    /// ]]></code>
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
    /// <code><![CDATA[
    /// // Create and customize an error with a specific name
    /// var error = ResultError.New("Connection timed out")
    ///     .WithName("DatabaseError")
    ///     .WithCode((int)ResultErrorCode.Internal);
    ///
    /// // Use in error handling
    /// if (error.Name == "DatabaseError")
    /// {
    ///     Console.WriteLine($"Database error occurred: {error.Message}");
    /// }
    /// ]]></code>
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
    /// <param name="exception">The new exception to associate with the error. It must be non-null.</param>
    /// <returns>A new <see cref="ResultError"/> instance with the updated exception.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is null.</exception>
    /// <example>
    /// <code><![CDATA[
    /// // Create an error with an associated exception
    /// var error = ResultError.New("Operation failed")
    ///     .WithException(new InvalidOperationException("Invalid state"))
    ///     .WithCode((int)ResultErrorCode.Internal);
    ///
    /// // Extract and handle the exception if present
    /// if (error.TryGetException(out var ex))
    /// {
    ///     Console.WriteLine($"Root cause: {ex.Message}");
    ///     // Handle the specific exception type
    /// }
    /// ]]></code>
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
	/// <code><![CDATA[
	/// var error = ResultError.New("Error message")
	///     .WithCode(ResultErrorCode.Validation);
	/// if (error.IsErrorCode(ResultErrorCode.Validation))
	/// {
	///     // Handle validation error
	/// }
	/// ]]></code>
	/// </example>
	[Pure]
	[ExcludeFromCodeCoverage]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool IsErrorCode(this ResultError self, ResultErrorCode code)
	{
		return self.Code == (int)code;
	}

	/// <summary>
	/// Determines if the error represents a validation error.
	/// </summary>
	/// <param name="self">The error instance.</param>
	/// <returns>True if the error is a validation error; otherwise, false.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var error = ResultError.New("Error message")
	///     .WithCode(ResultErrorCode.Validation);
	/// if (error.IsValidationError())
	/// {
	///     // Handle validation error
	/// }
	/// ]]></code>
	/// </example>
	[Pure]
	[ExcludeFromCodeCoverage]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool IsValidationError(this ResultError self)
	{
		return self.IsErrorCode(ResultErrorCode.Validation);
	}

	/// <summary>
	/// Determines if the error represents a not found error.
	/// </summary>
	/// <param name="self">The error instance.</param>
	/// <returns>True if the error is a not found error; otherwise, false.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var error = ResultError.New("Resource not found")
	///     .WithCode(ResultErrorCode.NotFound);
	/// if (error.IsNotFoundError())
	/// {
	///     // Handle not found error
	/// }
	/// ]]></code>
	/// </example>
	[Pure]
	[ExcludeFromCodeCoverage]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool IsNotFoundError(this ResultError self)
	{
		return self.IsErrorCode(ResultErrorCode.NotFound);
	}

	/// <summary>
	/// Determines if the error represents an internal error.
	/// </summary>
	/// <param name="self">The error instance.</param>
	/// <returns>True if the error is an internal error; otherwise, false.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var error = ResultError.New("Internal server error")
	///     .WithCode(ResultErrorCode.Internal);
	/// if (error.IsInternalError())
	/// {
	///     // Handle internal error
	/// }
	/// ]]></code>
	/// </example>
	[Pure]
	[ExcludeFromCodeCoverage]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool IsInternalError(this ResultError self)
	{
		return self.IsErrorCode(ResultErrorCode.Internal);
	}
}