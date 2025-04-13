using System;

namespace tNav.FakeConsole;

public class Logger
{
    string outlog = "all.txt";
    string outErrorlog = "error.txt";
    public void Clear()
    {
        if (File.Exists(outErrorlog))
            File.Delete(outErrorlog);

        if (File.Exists(outlog))
            File.Delete(outlog);
    }

    public void Info(params string[] content)
    {
        File.WriteAllLines(outlog, content);
    }
    public void Error(params string[] content)
    {
        File.WriteAllLines(outErrorlog, content);
    }
}
