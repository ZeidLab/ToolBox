namespace ZeidLab.ToolBox.Common;

internal static class Guards
{
    /// <summary>
    /// Throws <see cref="ArgumentNullException"/> if the given string is null, empty or whitespace.
    /// </summary>
    /// <param name="value">The string to be checked.</param>
    /// <param name="paramName">The name of the parameter which value is being checked.</param>
    public static void ThrowIfNullOrWhiteSpace(string value , string paramName) 
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(paramName);
        }
    } 
    /// <summary>
    /// Throws <see cref="ArgumentOutOfRangeException"/> if the given integer is negative or zero.
    /// </summary>
    /// <param name="value">The integer to be checked.</param>
    /// <param name="paramName">The name of the parameter which value is being checked.</param>
    public static void ThrowIfNegativeOrZero(int value , string paramName) 
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }
    } 
    
    /// <summary>
    /// Throws <see cref="ArgumentNullException"/> if the given value is null.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="paramName">The name of the parameter which value is being checked.</param>
    public static void ThrowIfNull<T>(T value , string paramName) 
    {
        if (value.IsNull())
        {
            throw new ArgumentNullException(paramName);
        }
    }
}