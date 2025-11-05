using BuildNetworkExample;
using Microsoft.Data.Analysis;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

tNavSettings settings = new tNavSettings();


Console.WriteLine($"Текщая директория: {settings.HomePath}");

if (File.Exists(settings.tNavPath))
    settings.tNavPath = new FileInfo(settings.tNavPath).FullName;
else
    settings.tNavPath = "";
Console.WriteLine($"Путь к tNavigator-con: {settings.tNavPath}");


Console.WriteLine("0 Выход");
Console.WriteLine("1 Изменить текущую директорию");
Console.WriteLine("2 Изменить путь к tNavigator-con");
Console.WriteLine("3 Пример API1.3_HowToBuildNetworkViaAPIServer");
Console.WriteLine("4 Пример API1.3_HowToBuildNetworkViaAPIServer new API");
Console.WriteLine("5 Тест на чтение DataFrame");


var variant = Console.ReadLine();
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
            break;
        case "2":
            Console.WriteLine("Путь к tNavigator-con:");
            settings.tNavPath = Console.ReadLine() ?? "";
            if (File.Exists(settings.tNavPath))
                settings.tNavPath = new FileInfo(settings.tNavPath).FullName;
            else
                settings.tNavPath = "";
            Console.WriteLine($"Принято:{settings.tNavPath}");
            break;
        case "3":
            new LikePython1_3().Run(settings);
            break;
        case "4":
            new LikePython1_3_NewAPI().Run(settings);
            break;
        case "5":
            new TestRead().ReadFrame(settings);
            break;
    }
    variant = Console.ReadLine();
}
Console.WriteLine("Нажмите любую клавишу для выхода");
Console.ReadKey();