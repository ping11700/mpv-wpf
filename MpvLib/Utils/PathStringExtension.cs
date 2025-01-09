namespace MpvLib.Utils;

public static class PathStringExtension
{
    public static string Ext(this string filepath) => filepath.Ext(false);

    public static string Ext(this string filepath, bool includeDot)
    {
        if (string.IsNullOrEmpty(filepath))
            return "";

        char[] chars = filepath.ToCharArray();

        for (int x = filepath.Length - 1; x >= 0; x--)
        {
            if (chars[x] == Path.DirectorySeparatorChar)
                return "";

            if (chars[x] == '.')
                return filepath.Substring(x + (includeDot ? 0 : 1)).ToLowerInvariant();
        }

        return "";
    }


}