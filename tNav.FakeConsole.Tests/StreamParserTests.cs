using Microsoft.Data.Analysis;
using System.Data;
using System.Globalization;
using tNav.Common;

namespace tNav.FakeConsole.Tests;

public class StreamParserTests
{
    [Fact]
    public void ParseDataFrame()
    {
        var log = File.ReadAllText("../../../../BuildNetworkExample/out/log.txt");
        var testsData = new LogParser().Parse(log);
        var lastTest = testsData[70];
        var response = lastTest.Responses.Last().Data ?? "";
        using var stream = GenerateStreamFromString(response);
        using var reader = new StreamReader(stream);
        var parsed = StreamParser.Unpack_data(reader);
        var table = parsed as DataFrame;
        Assert.NotNull(table);
        using var testOut = File.OpenWrite("testout.csv");
        DataFrame.SaveCsv(dataFrame: table, csvStream: testOut, separator: ',', encoding: System.Text.Encoding.UTF8, cultureInfo: CultureInfo.InvariantCulture); ;
        table.
        testOut.Close();
    }

    static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}