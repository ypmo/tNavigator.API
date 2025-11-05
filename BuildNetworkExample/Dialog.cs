namespace BuildNetworkExample;

public class Dialog
{
    private tNavSettings settings;
    public Dialog(FileInfo? tNavPath)
    {
        settings = new tNavSettings
        {
            HomePath = AppContext.BaseDirectory,
            tNavPath = tNavPath?.FullName ?? ""
        };
    }

    void printActions()
    {
        Console.WriteLine($"Текщая директория: {settings.HomePath}");
        Console.WriteLine($"Путь к tNavigator-con: {settings.tNavPath}");
        Console.WriteLine("1 Пример API1.3_HowToBuildNetworkViaAPIServer");
        Console.WriteLine("2 Пример API1.3_HowToBuildNetworkViaAPIServer new API");

    }
    public void Run(string? action)
    {
        if (string.IsNullOrEmpty(action))
            printActions();

        action ??= Console.ReadLine();

        switch (action)
        {
            case "1":
                new LikePython1_3().Run(settings);
                break;
            case "2":
                new LikePython1_3_NewAPI().Run(settings);
                action = "0";
                break;
        }
    }
}

