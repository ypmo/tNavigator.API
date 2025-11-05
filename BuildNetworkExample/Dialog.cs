namespace BuildNetworkExample;

public class Dialog
{
    private tNavSettings settings;
    public Dialog(tNavSettings tNavSettings)
    {
        settings = tNavSettings;
    }

    void printActions()
    {
        Console.WriteLine($"Текщая директория: {settings.HomePath}");
        Console.WriteLine($"Путь к tNavigator-con: {settings.tNavPath}");
        Console.WriteLine("0 Выход");
        Console.WriteLine("1 Изменить текущую директорию");
        Console.WriteLine("2 Изменить путь к tNavigator-con");
        Console.WriteLine("3 Пример API1.3_HowToBuildNetworkViaAPIServer");
        Console.WriteLine("4 Пример API1.3_HowToBuildNetworkViaAPIServer new API");
        Console.WriteLine("5 Тест на чтение DataFrame");

    }
    public void Run(string? action)
    {
        if (string.IsNullOrEmpty(action))
            printActions();

        action ??= Console.ReadLine();
        while (action != "0")
        {
            switch (action)
            {
                case "1":
                    Console.WriteLine("Новый путь:");
                    settings.HomePath = Console.ReadLine() ?? settings.HomePath;
                    break;
                case "2":
                    Console.WriteLine("Путь к tNavigator-con:");
                    settings.tNavPath = Console.ReadLine() ?? settings.tNavPath;
                    break;
                case "3":
                    new LikePython1_3().Run(settings);
                    continue;
                case "4":
                    new LikePython1_3_NewAPI().Run(settings);
                    action = "0";
                    continue;
                case "5":
                    new TestRead().ReadFrame(settings);
                    action = "0";
                    continue;
            }
            printActions();
            action = Console.ReadLine();
        }

    }
}

