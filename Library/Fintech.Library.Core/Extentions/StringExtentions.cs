namespace Fintech.Library.Core.Extentions;

public static class StringExtentions
{
    public static int GetEnumValue<T>(this string e)
    {
        return (int)Enum.Parse(typeof(T), e);
    }
}
