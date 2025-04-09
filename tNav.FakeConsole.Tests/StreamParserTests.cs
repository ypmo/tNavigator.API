using System.Data;
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
      using   var stream = GenerateStreamFromString(response);
        using var reader = new StreamReader(stream);
        var parsed = StreamParser.unpack_data(reader);
        var table = parsed as DataTable;
        Assert.NotNull(table);
        using var testOut = File.CreateText("testout.csv");
        bool firstStep = true;
        for (int i = 0; i < table.Columns.Count; i++)
        {
            string splitter = firstStep ? "" : ",";
            var colname = table.Columns[i].ColumnName?.Replace(", ", "_");
            testOut.Write(splitter + colname);
            firstStep = false;
        }
        testOut.Write('\n');
        foreach (DataRow row in table.Rows)
        {
            firstStep = true;
            for (var i = 0; i < table.Columns.Count; i++)
            {
                string splitter = firstStep ? "" : ",";
                var colvalue = row[i]?.ToString()?.Replace(", ", "_");
                testOut.Write($"{splitter}{colvalue}");
                firstStep = false;
            }
            testOut.Write('\n');
        }
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