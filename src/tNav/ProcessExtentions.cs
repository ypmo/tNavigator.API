using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tNav.Common;

namespace tNav;

internal static class ProcessExtentions
{
    public static void process_message(this Process process, string message)
    {
        process.StandardInput.Write(message);
        process.StandardInput.Flush();

        var err_res = process.StandardOutput.ReadLine()?.OneToOneToUTF8() ?? "";
        if (err_res != "OK")
        {
            Console.WriteLine($"tNav: {err_res}");
            _ = int.TryParse(process.StandardOutput.ReadLine()?.OneToOneToUTF8()??"", out int count_str);
            string msg = "";
            for (int i = 0; i < count_str; i++)
            {
                msg += process.StandardOutput.ReadLine()?.OneToOneToUTF8()??"";
            }
            throw new Exception($"tNav Сообщает об ошибке\n Отправлено:{message}\nОтвет:{msg}");

        }
    }
}
