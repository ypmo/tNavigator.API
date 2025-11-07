using Microsoft.Data.Analysis;
using System;
using System.Text;
using tNav.Common;

namespace BuildNetworkExample;

public class TestRead
{
    public void ReadFrame(FileInfo fileName)
    {
        string dataPath = fileName.FullName;
        if (!File.Exists(dataPath))
        {
            throw new FileNotFoundException();
        }
        var data = File.ReadAllBytes(dataPath);

        List<byte> bytes = [];
        for (int i = 0; i < data.Length - 1; i += 2)
        {
            var s = Encoding.UTF8.GetString([data[i], data[i + 1]]);
            var b = Convert.FromHexString(s);
            bytes.AddRange(b);
        }
        var stream = new MemoryStream(bytes.ToArray());
        var stremReader = new StreamReader(stream, new One2OneEncoding() );
        var obj = StreamParser.Unpack_data(stremReader);
        var frame = obj as DataFrame;
        var table = frame?.ToTable();
        var csv = Utils.DataTableToCSV(table);

        var resultDir = Path.Combine(AppContext.BaseDirectory, "Result_Tables");
        if (!Directory.Exists(resultDir))
            Directory.CreateDirectory(resultDir);
        File.WriteAllText(Path.Combine(resultDir, "pipes_table_results.csv"), csv);
        Console.WriteLine("Done");
    }
}
