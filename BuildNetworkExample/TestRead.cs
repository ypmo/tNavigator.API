using Microsoft.Data.Analysis;
using System;
using System.Text;
using tNav.Common;

namespace BuildNetworkExample;

public class TestRead
{

    public void ReadFrame()
    {
        if (false) return;

        var curdir = Directory.GetCurrentDirectory();
        if (!File.Exists("dataframe.txt"))
        {
            throw new FileNotFoundException();
        }
        var data = File.ReadAllBytes("dataframe.txt");

        List<byte> bytes = [];
        for (int i = 0; i < data.Length - 1; i += 2)
        {
            var s = Encoding.UTF8.GetString([data[i], data[i + 1]]);
            var b = Convert.FromHexString(s);
            bytes.AddRange(b);
        }
        var stream = new MemoryStream(bytes.ToArray());
        var stremReader = new StreamReader(stream);
        var obj = StreamParser.Unpack_data(stremReader);
        var frame = obj as DataFrame;
        var table = frame?.ToTable();
        var csv = Utils.DataTableToCSV(table);
        var curDir=Directory.GetCurrentDirectory();
        if (!Directory.Exists("Result_Tables"))
            Directory.CreateDirectory("Result_Tables");
        File.WriteAllText("Result_Tables/pipes_table_results.csv", csv);
        Console.WriteLine("Done");
        Environment.Exit(0);
    }
}
