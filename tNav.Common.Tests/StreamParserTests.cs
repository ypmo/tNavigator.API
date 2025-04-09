using System.Data;

namespace tNav.Common.Tests;

public class StreamParserTests
{
    [Theory]
    [InlineData("04000000000000004e6f6e65", null)]
    public void ParseString(string indata, string? outdata)
    {
        var stream = StreamFactory.ASCII2ByteStream(indata);
        var parsed = StreamParser.unpack_data(stream);
        // Assert.Equal(parsed, outdata);
        // Assert.Equal(parsed, outdata);
    }   
}