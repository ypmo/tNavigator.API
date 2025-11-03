using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.Common;

public static class StreamReaderExtentions
{
    static readonly Encoding Encoder = new One2OneEncoding();
    internal static byte[] ReadAsBytes(this StreamReader stream, int size)
    {
        char[] buffer = new char[size];
        stream.Read(buffer, 0, size);
        var data = Encoder.GetBytes(buffer);
        return data;
    }

    public static T? Unpack_data<T>(this StreamReader stream)
    {
        var obj = StreamParser.Unpack_data(stream);

        if (obj == null) return default;  
        if (obj is T result)
        {
            return result;
        }    
        
        throw new InvalidCastException($"Не удалоль привести тип {obj?.GetType()} к {typeof(T)}");
    }
}
