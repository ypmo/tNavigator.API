
using tNav.FakeConsole;


Logger log = new();
log.Clear();
log.Info(DateTime.Now.ToString());
string pathToLog = "../../../../BuildNetworkExample/out/log.txt";
var logText = File.ReadAllText(pathToLog);
var queries = LogParser.Parse(logText);


foreach (var query in queries)
{
    //using var sr = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    //using var sw = new StreamWriter(Console.OpenStandardOutput(), Console.OutputEncoding);
    var input = Console.ReadLine();
    if (!string.Equals(query.Query.Trim('\n'), input))
    {
        List<string> content = [];
        content.Add("***ожидалось***");
        content.Add(query.Query);
        content.Add("***получили***");
        content.Add(input);
        log.Error(content.ToArray());
        throw new Exception("Not equals");

    }
    foreach (var responce in query.Responses)
    {
        ///Console.OpenStandardOutput().Write(responce.Data)
        Console.Write(responce.Data);

        //Console.WriteLine(responce);
        // sw.Flush();
    }
}


