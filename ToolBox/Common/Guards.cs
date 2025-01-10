namespace ZeidLab.ToolBox.Common;

internal static class Guards
{
    public static void ThrowIfNullOrWhiteSpace(string value , string paramName) 
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(paramName);
        }
    } 
    public static void ThrowIfNegativeOrZero(int value , string paramName) 
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }
    } 
    
    public static void ThrowIfNull<T>(T value , string paramName) 
    {
        if (value.IsNull())
        {
            throw new ArgumentNullException(paramName);
        }
    }
}