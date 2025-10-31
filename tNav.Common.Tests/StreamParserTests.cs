using System.Data;

namespace tNav.Common.Tests;

public class StreamParserTests
{
    static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    [Theory]
    [InlineData("04000000000000004e6f6e65", "" , "None")]
    [InlineData("04000000","Int", 4)]
    public void ParseString(string indata, string format,  object? outdata)
    {

        using var stream = GenerateStreamFromString(indata);
        using var reader = new StreamReader(stream);
        var parsed = StreamParser.Unpack_data(reader, format);
        Assert.Equal(parsed, outdata);
        // Assert.Equal(parsed, outdata);
    }

  



}