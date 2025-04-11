using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.API;

internal static class ProcessExtentions
{
    public static void process_message(this Process process, string message)
    {
        process.StandardInput.Write(message);
        process.StandardInput.Flush();

        var err_res = process.StandardOutput.ReadLine();
        if (err_res != "OK")
        {
            int.TryParse(process.StandardOutput.ReadLine(), out int count_str);
            string msg = "";
            for (int i = 0; i < count_str; i++)
            {
                msg += process.StandardOutput.ReadLine();
            }
        }
    }
}
