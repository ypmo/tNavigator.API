namespace BuildNetworkExample;

public class tNavSettings
{
    public string tNavPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "tNavigator/24.4/tNavigator-con");
    public string HomePath { get; set; } = Path.Combine(AppContext.BaseDirectory);
}
