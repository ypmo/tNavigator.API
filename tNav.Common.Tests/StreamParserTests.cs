using System.Data;
using System.Text;

namespace tNav.Common.Tests;

public class StreamParserTests
{
    static One2OneEncoding Encoding = new One2OneEncoding();
    static StreamReader GenerateStreamFromString(string s)
    {
        var bytes = Convert.FromHexString(s);
        var stream = new MemoryStream(bytes)
        {
            Position = 0
        };
        var reader = new StreamReader(stream, encoding: Encoding, false);
        return reader;
    }


    [Theory]
    [MemberData(nameof(TestData))]
    public void Unpack_data(string indata, string format, object? outdata)
    {
        using var stream = GenerateStreamFromString(indata);
        var parsed = StreamParser.Unpack_data(stream, format);
        Assert.Equal(parsed, outdata); ;
    }

    public static IEnumerable<object?[]> TestData() =>
    [
        ["04000000000000004e6f6e65", "String", "None"],
        ["0900000000000000446174614672616d65", "String", "DataFrame"],
        ["04000000", "Int", 4],
        ["a00f0000", "Int", 4000],
        ["04000000000000004e6f6e65", "", null],
        ["0300000000000000496e7404000000", "", 4],
        ["0300000000000000496e74a00f0000", "", 4000]
    ];

    [Fact]
    public void Unpack_dataGeneric()
    {
        Assert.Equal(4, UnpackGeneric<int>("0300000000000000496e7404000000"));
        Assert.Null(UnpackGeneric<int?>("04000000000000004e6f6e65"));
    }

    [Fact]
    public void Unpack_StringToIntThrowsException() =>
       Assert.Throws<InvalidCastException>(() => UnpackGeneric<string>("0300000000000000496e7404000000"));

    [Fact]
    public void Unpack_IntToThrowsException() =>
        Assert.Throws<InvalidCastException>(() => UnpackGeneric<int>("0900000000000000446174614672616d65"));


    T? UnpackGeneric<T>(string indata)
    {
        using var stream = GenerateStreamFromString(indata);
        var parsed = stream.Unpack_data<T>();
        return parsed;
    }
}