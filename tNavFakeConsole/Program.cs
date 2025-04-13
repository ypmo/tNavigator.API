
using tNav.FakeConsole;
string outlog = "error.txt";
string pathToLog = "../../../../BuildNetworkExample/out/log.txt";
var logText = File.ReadAllText(pathToLog);
var queries = LogParser.Parse(logText);
if(File.Exists(outlog))
{
File.Delete(outlog);
}

foreach (var query in queries)
{
    using var sr = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    using var sw = new StreamWriter(Console.OpenStandardOutput(), Console.OutputEncoding);
    var input = sr.ReadToEnd();
    if (!string.Equals(query.Query, input))
    {
        List<string> content = [];
        content.Add("***ожидалось***");
        content.Add(query.Query);
        content.Add("***получили***");
        content.Add(input);
        await File.AppendAllLinesAsync(outlog, content);
        throw new Exception("Not equals");

    }
    foreach (var responce in query.Responses)
    {
        sw.Write(responce);
        sw.Flush();
    }
}


while (true)
{

}
