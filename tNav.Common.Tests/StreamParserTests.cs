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
    [InlineData("04000000000000004e6f6e65", null)]
    public void ParseString(string indata, string? outdata)
    {

        using var stream = GenerateStreamFromString(indata);
        using var reader = new StreamReader(stream);
        var parsed = StreamParser.Unpack_data(reader);
        // Assert.Equal(parsed, outdata);
        // Assert.Equal(parsed, outdata);
    }



}