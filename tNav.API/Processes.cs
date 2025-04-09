using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.API;

internal static class Processes
{
    public static void process_message(Process process, string message)
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

    public static string? readline(Process process)
    {
        return process.StandardOutput.ReadLine();
    }
}
