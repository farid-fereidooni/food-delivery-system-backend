namespace FileManager.Core.Helpers;

public static class StringHelpers
{
    public static string PascalToKebabCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        IEnumerable<char> ConvertChar(char c, int index)
        {
            if (char.IsUpper(c) && index != 0) yield return '-';
            yield return char.ToLower(c);
        }

        return string.Concat(str.SelectMany(ConvertChar));
    }
}
