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
    [InlineData("04000000000000004e6f6e65", "String", "None")]
    [InlineData("0900000000000000446174614672616d65", "String", "DataFrame")]
    [InlineData("04000000", "Int", 4)]
    [InlineData("a00f0000", "Int", 4000)]
    public void ParseValue(string indata, string format, object? outdata)
    {

        using var stream = GenerateStreamFromString(indata);
        using var reader = new StreamReader(stream, encoding: Encoding.UTF8, false);
        var parsed = StreamParser.Unpack_data(reader, format);
        Assert.Equal(parsed, outdata);
    }

    [Theory]
    [InlineData("04000000000000004e6f6e65", null)]
    [InlineData("0300000000000000496e7404000000", 4)]
    [InlineData("0300000000000000496e74a00f0000", 4000)]
    public void Unpack_data(string indata, object? outdata)
    {

        using var stream = GenerateStreamFromString(indata);
        using var reader = new StreamReader(stream, encoding: Encoding.UTF8, false);
        var parsed = StreamParser.Unpack_data(reader);
        Assert.Equal(parsed, outdata);;
    }







}