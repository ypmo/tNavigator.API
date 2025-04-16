
using System.Text;
using tNav.FakeConsole;


Logger log = new();
log.Clear();
log.Info(DateTime.Now.ToString());
string pathToLog = "../../../../BuildNetworkExample/out/log.txt";
var logText = File.ReadAllText(pathToLog);
var queries = LogParser.Parse(logText);


foreach (var query in queries)
{
    ReadInput(query.Query);



    PushOut(query.Responses);

}

void ReadInput(string? original)
{
    log.Info("начинаем чтение строки");
    byte[] buffer = new byte[4096];

    StringBuilder sb = new();
    bool hasValue = true;

    while (hasValue)
    {
        int readed = Console.Read();
        hasValue = readed >= 0;
        if (hasValue)
        {
            log. InfoSimbol(((char)readed).ToString());
            sb.Append((char)readed);
        }
    }
    var input = sb.ToString();
    log.Info("Закончили чтение");
    log.Info("***Получили***", input);
    if (!string.Equals(original?.Trim('\n'), input.Trim('\n')))
    {
        List<string> content = [];
        content.Add("***ожидалось***");
        content.Add(original ?? "");
        content.Add("***получили***");
        content.Add(input);
        log.Error(content.ToArray());
    }
}

void PushOut(IEnumerable<string> input)
{
    foreach (var data in input)
    {
        log.Info("***ОТВЕТ***", data);
        Console.Write(data);
    }
}