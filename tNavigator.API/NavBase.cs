using System;
using System.Diagnostics;

namespace tNavigator.API;

public class NavBase
{
    protected void process_message(Process process, string message)
    {
        process.StandardInput.Write(message);
        process.StandardInput.Flush();

        var err_res = readline(process);
        if (err_res != "OK")
        {
            int.TryParse(readline(process), out int count_str);
            string msg = "";
            for (int i = 0; i < count_str; i++)
            {
                msg += readline(process);
            }
        }
    }

    protected string? readline(Process process)
    {
        return process.StandardOutput.ReadLine();
    }
}
