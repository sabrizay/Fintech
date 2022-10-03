namespace Fintech.Library.Utilities.Extensions;

public static class StringExtensions
{
    public static bool IsInt(this string value)
    {
        return int.TryParse(value, out _);
    }
    public static bool IsDecimal(this string value)
    {
        return decimal.TryParse(value, out _);
    }
}
