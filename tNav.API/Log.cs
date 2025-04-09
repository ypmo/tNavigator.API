using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.API;

internal static class Log
{
    static string logfile = "log.txt";

    public static void Write(string message)
    {
        File.AppendAllText(logfile, message);
    }
}
