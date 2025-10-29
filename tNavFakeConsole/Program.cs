
using System.Text;
using tNav.FakeConsole;


Logger log = new();
log.Clear();
log.Info(DateTime.Now.ToString());
string pathToLog = "../../../../BuildNetworkExample/out/log.txt";
var logText = File.ReadAllText(pathToLog);
var queries = LogParser.Parse(logText);

var listener = new StreamListener(Console.OpenStandardInput());

foreach (var query in queries)
{
    ReadInput(query.Query);
    PushOut(query.Responses);
}

void ReadInput(string? original)
{
    log.Info("начинаем чтение строки mark_02");
    var input = ReadStandardInput();
    log.Info("Закончили чтение");
    log.Info("***Получили***", $"{input} {string.Join("", Encoding.ASCII.GetBytes(input))}");
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

string? ReadStandardInput()
{
    bool dataResived = false;
    string result = "";
    while (!dataResived || string.IsNullOrEmpty(result))
    {
        if (listener.Age > 100)
        {
            result = listener.GetString();
            dataResived = true;
        }
        else
        {
            System.Threading.Thread.Sleep(100);
        }
    }
    return result;
}

void PushOut(IEnumerable<string> input)
{
    foreach (var data in input)
    {
        log.Info("***ОТВЕТ***", data);
        Console.Write(data);
    }
}