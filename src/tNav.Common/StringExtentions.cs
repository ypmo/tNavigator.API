using System.Text;

namespace tNav.Common;

public static class StringExtentions
{
    public static string OneToOneToUTF8(this string str)
    {
        var bytes = new One2OneEncoding().GetBytes(str);
        var text = Encoding.UTF8.GetString(bytes);
        return text;
    }
}
