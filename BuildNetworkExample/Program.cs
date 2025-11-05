using BuildNetworkExample;
using Microsoft.Data.Analysis;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;


System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

string? action = null;

Option<FileInfo> tNavPathOption = new("--tNavPath")
{
    Description = "Путь к приложению 'консоль тНавигатор'"
};

Option<string> actionOption = new("--action")
{
    Description = "автоматическое выполнение пункта меню"
};

Option<string> helpOption = new("--help")
{
    Description = "автоматическое выполнение пункта меню"
};

var rootCommand = new RootCommand("Example tNavigator Console tool");
rootCommand.Options.Add(tNavPathOption);
rootCommand.Options.Add(actionOption);
rootCommand.Options.Add(helpOption);

rootCommand.SetAction(parseResult =>
{
    tNavSettings settings = new();

    if (parseResult.GetValue(tNavPathOption) is FileInfo fileInfo)
        settings.tNavPath = fileInfo.FullName;

    string? action = parseResult.GetValue(actionOption);
    new Dialog(settings).Run(action);
    return 0;
});

ParseResult parseResult = rootCommand.Parse(args);
return parseResult.Invoke();