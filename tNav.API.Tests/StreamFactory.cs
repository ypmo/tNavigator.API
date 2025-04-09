using System;

namespace tNav.API.Tests;

public static class StreamFactory
{
    public static Stream ASCII2ByteStream(string str)
    {
        var data = System.Convert.FromHexString(str);
        return new MemoryStream(data, writable: false);
    }
}
