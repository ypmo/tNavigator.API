
string tNpath = "~/tNavigator/tNavigator-con";

if (!Path.Exists(tNpath))
{
    Console.WriteLine($"Console tNavigator not found at {Path.GetFullPath(tNpath)}!");
    Environment.Exit(1);
}

if (!Path.Exists("Init_Data"))
{
    Console.WriteLine($"Init_Data folder not found!");
    Environment.Exit(1);
}