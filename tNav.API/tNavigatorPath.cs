using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.API;

public static class tNavigatorPath
{
    public static string tNavigator_API_client_exe()
    {
        return Path.Combine(
            Directory.GetCurrentDirectory(), 
            Environment.OSVersion.Platform==PlatformID.Unix? "tNavigator-API-client": "tNavigator-API-client.exe");

    }
}
