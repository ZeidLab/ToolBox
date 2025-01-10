using System.Runtime.CompilerServices;
using ZeidLab.ToolBox.Common;
using ZeidLab.ToolBox.Options;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// an extension class for <see cref="Error"/>
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Tries to get the exception associated with this error.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="exception">The exception associated with this error, or <c>null</c> if there is no exception.</param>
    /// <returns>
    /// <see langword="true"/> if there is an exception associated with this error;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetException(this Error self, out Exception? exception)
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
    /// error, or <see cref="Maybe{T}.None"/> if there is no exception.
    /// </returns>
    public static Maybe<Exception> TryGetException(this Error self)
    {
        return self.Exception is {} exception ? exception.ToSome(): Maybe<Exception>.None();
        
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified error code.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="code">The new error code. It must be greater than 0.</param>
    /// <returns>A new <see cref="Error"/> instance with the updated code.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="code"/> is negative.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error WithCode(this Error self, int code)
    {
        Guards.ThrowIfNegativeOrZero(code, nameof(code));
        return new Error(code, self.Name, self.Message, self.Exception);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified error message.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="message">The new error message.
    /// It must be non-empty and non-null.
    /// </param>
    /// <returns>A new <see cref="Error"/> instance with the updated message.</returns>
    /// <exception cref="ArgumentException"><paramref name="message"/> is null or empty.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error WithMessage(this Error self, string message)
    {
        Guards.ThrowIfNullOrWhiteSpace(message, nameof(message));
        return new Error(self.Code, self.Name, message, self.Exception);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified error name.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="name">The new error name.
    /// It is a meaningful name instead of numbers and more readable.
    /// It must be non-empty and non-null.
    /// </param>
    /// <returns>A new <see cref="Error"/> instance with the updated name.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or empty.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error WithName(this Error self, string name)
    {
        Guards.ThrowIfNullOrWhiteSpace(name, nameof(name));
        return new Error(self.Code, name, self.Message, self.Exception);
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified exception.
    /// </summary>
    /// <param name="self">The original error instance.</param>
    /// <param name="exception">The exception to associate with the new error instance.
    /// It must be non-null.
    /// </param>
    /// <returns>A new <see cref="Error"/> instance with the updated exception.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Error WithException(this Error self, Exception exception)
    {
        Guards.ThrowIfNull(exception, nameof(exception));
        return new Error(self.Code, self.Name, self.Message, exception);
    }
}