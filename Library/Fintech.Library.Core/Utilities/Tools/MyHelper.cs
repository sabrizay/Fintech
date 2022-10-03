using System.Text;

namespace Fintech.Library.Core.Utilities.Tools;

public static class MyHelper
{
    public static string ReplaceTurkishCharacters(string str)
    {
        StringBuilder stringBuilder = new(str.ToLower());
        stringBuilder.Replace('ü', 'u');
        stringBuilder.Replace('Ü', 'u');
        stringBuilder.Replace('İ', 'i');
        stringBuilder.Replace('I', 'i');
        stringBuilder.Replace('ı', 'i');
        stringBuilder.Replace('Ç', 'c');
        stringBuilder.Replace('ç', 'c');
        stringBuilder.Replace('ç', 'c');
        stringBuilder.Replace('|', '-');

        return stringBuilder.ToString();
    }
}
