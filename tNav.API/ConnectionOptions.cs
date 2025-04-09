using System;

namespace tNav.API;

public class ConnectionOptions
{
    public string? MinimumRequiredVersion { get; set; }
    public int? LicenseWaitTimeLimitSecs { get; set; }
    public string? LicenseSettings { get; set; }
    public string? LicenseServerUrl { get; set; }
    public string? LicenseType { get; set; }
    public string? ApiServerUrl { get; set; }
    public string? Login { get; set; }
    public string? PlainPassword { get; set; }

}
