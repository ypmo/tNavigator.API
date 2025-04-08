namespace tNavigator.API.Tests;

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

    [Fact]
    public void ParseDataFrame()
    {
        var log = File.ReadAllText("log.txt");
        var testsData = new LogParser().Parse(log);
        var lastTest = testsData[70];
        var response = lastTest.Responses.Last().Data ?? "";
        var stream = StreamFactory.ASCII2ByteStream(response);
        var parsed = StreamParser.unpack_data(stream);
        Assert.NotNull(parsed);
    }
}