using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.Common;

public static class StreamReaderExtentions
{
    public static byte[] ReadAsBytes(this StreamReader stream, int size)
    {
        char[] buffer = new char[size * 2];
        stream.Read(buffer, 0, size * 2);
        var data = System.Convert.FromHexString(buffer);
        return data;
    }
}
