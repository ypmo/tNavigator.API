using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.Common;

public static class StreamReaderExtentionsv
{
    public static byte[] ReadAsBytes(this StreamReader stream, int size)
    {

        byte[] buffer = new byte[size]; 
        Stream baseStream = stream.BaseStream;
        var bytesRead = baseStream.Read(buffer, 0, buffer.Length);
        return buffer;
    }
}
