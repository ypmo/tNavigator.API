
using tNav.FakeConsole;


Logger log = new();
log.Clear();
log.Info(DateTime.Now.ToString());
string pathToLog = "../../../../BuildNetworkExample/out/log.txt";
var logText = File.ReadAllText(pathToLog);
var queries = LogParser.Parse(logText);


foreach (var query in queries)
{
    List<string> lines = [];

    log.Info("начинаем чтение строки");
    bool firstLine = true;
    bool endFinded = true;
    while (firstLine || !endFinded)
    {
        var line = Console.ReadLine();
        if (firstLine && line.EndsWith("= \""))
        {
            endFinded = false;
        }
        else if (!firstLine && line.EndsWith("\")"))
        {
            endFinded = true;
        }
        firstLine = false;
        log.Info($"Считали {line}");
        lines.Add(line);
    }
    log.Info("Закончили чтение");
    var input = string.Join('\n', lines);
    log.Info("***Получили***", input);
    if (!string.Equals(query.Query.Trim('\n'), input.Trim('\n')))
    {
        List<string> content = [];
        content.Add("***ожидалось***");
        content.Add(query.Query);
        content.Add("***получили***");
        content.Add(input);
        log.Error(content.ToArray());
        //throw new Exception("Not equals");
    }
    foreach (var responce in query.Responses)
    {
        ///Console.OpenStandardOutput().Write(responce.Data)
        log.Info("***ОТВЕТ***", responce.Data);
        Console.Write(responce.Data);

        //Console.WriteLine(responce);
        // sw.Flush();
    }
}


