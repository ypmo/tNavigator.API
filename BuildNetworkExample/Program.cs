using BuildNetworkExample;
using Microsoft.Data.Analysis;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;


System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
tNavSettings settings = new();
string? variant = null;

Option<FileInfo> tNavPathOption = new("--tNavPath")
{
    Description = "Путь к приложению 'консоль тНавигатор'"
};

Option<string> actionOption = new("--action")
{
    Description = "автоматическое выполнение пункта меню"
};

var rootCommand = new RootCommand("Example tNavigator Console tool");
rootCommand.Options.Add(tNavPathOption);
rootCommand.Options.Add(actionOption);
ParseResult parseResult = rootCommand.Parse(args);

if (parseResult.Errors.Count != 0)
{
    foreach (ParseError parseError in parseResult.Errors)
        Console.Error.WriteLine(parseError.Message);
    Environment.Exit(1);
}

if (parseResult.GetValue(tNavPathOption) is FileInfo fileInfo)
    settings.tNavPath = fileInfo.FullName;

if (parseResult.GetValue(actionOption) is string action && !string.IsNullOrEmpty(action))
    variant = action;


Console.WriteLine($"Текщая директория: {settings.HomePath}");
Console.WriteLine($"Путь к tNavigator-con: {settings.tNavPath}");


Console.WriteLine("0 Выход");
Console.WriteLine("1 Изменить текущую директорию");
Console.WriteLine("2 Изменить путь к tNavigator-con");
Console.WriteLine("3 Пример API1.3_HowToBuildNetworkViaAPIServer");
Console.WriteLine("4 Пример API1.3_HowToBuildNetworkViaAPIServer new API");
Console.WriteLine("5 Тест на чтение DataFrame");


variant ??= Console.ReadLine();
while (variant != "0")
{
    switch (variant)
    {
        case "1":
            Console.WriteLine("Новый путь:");
            settings.HomePath = Console.ReadLine() ?? "";
            if (Directory.Exists(settings.HomePath))
                settings.HomePath = new DirectoryInfo(settings.HomePath).FullName;
            else
                settings.HomePath = "";
            Console.WriteLine($"Принято:{settings.HomePath}");
            variant = Console.ReadLine();
            break;
        case "2":
            Console.WriteLine("Путь к tNavigator-con:");
            settings.tNavPath = Console.ReadLine() ?? "";
            if (File.Exists(settings.tNavPath))
                settings.tNavPath = new FileInfo(settings.tNavPath).FullName;
            else
                settings.tNavPath = "";
            Console.WriteLine($"Принято:{settings.tNavPath}");
            variant = Console.ReadLine();
            break;
        case "3":
            new LikePython1_3().Run(settings);
            variant = "0";
            break;
        case "4":
            new LikePython1_3_NewAPI().Run(settings);
            variant = "0";
            break;
        case "5":
            new TestRead().ReadFrame(settings);
            variant = "0";
            break;
    }

}
Console.WriteLine("Нажмите любую клавишу для выхода");
Console.ReadKey();