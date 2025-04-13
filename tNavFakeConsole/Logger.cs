using System;

namespace tNav.FakeConsole;

public class Logger
{
    string outlog = "all.txt";
    string outErrorlog = "error.txt";
    public void Clear()
    {
        if (File.Exists(outErrorlog))
            File.WriteAllText(outErrorlog,"");

        if (File.Exists(outlog))
            File.WriteAllText(outlog,"");
    }

    public void Info(params string[] content)
    {
        File.AppendAllLines(outlog, content);
    }
    public void Error(params string[] content)
    {
        File.AppendAllLines(outErrorlog, content);
    }
}
