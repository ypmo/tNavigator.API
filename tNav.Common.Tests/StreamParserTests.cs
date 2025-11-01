using System.Data;
using System.Text;

namespace tNav.Common.Tests;

public class StreamParserTests
{
    static Stream GenerateStreamFromString(string s)
    {
        var bytes = Convert.FromHexString(s);

        var stream = new MemoryStream(bytes);
        stream.Position = 0;
        return stream;
    }

    [Theory]
    [InlineData("04000000000000004e6f6e65", "None")]
 [InlineData("0900000000000000446174614672616d65", "DataFrame")]

    public void ParseString(string indata, object? outdata)
    {

        using var stream = GenerateStreamFromString(indata);
        using var reader = new StreamReader(stream);
        var parsed = StreamParser.UnpackString(reader);
        Assert.Equal(parsed, outdata);
        // Assert.Equal(parsed, outdata);
    }
    [Theory]
    [InlineData("04000000000000004e6f6e65", "String", "None")]
    [InlineData("04000000", "Int", 4)]
    [InlineData("a00f0000", "Int", 4000)]
    public void ParseValuet(string indata, string format, object? outdata)
    {

        using var stream = GenerateStreamFromString(indata);
        using var reader = new StreamReader(stream, encoding:Encoding.UTF8, false);
        var parsed = StreamParser.Unpack_data(reader, format);
        Assert.Equal(parsed, outdata);
        // Assert.Equal(parsed, outdata);
    }





}