using System;

namespace tNavigator.API;

public class ConnectionOptions
{
    public string? minimum_required_version { get; set; }
    public string? license_wait_time_limit__secs { get; set; }
    public string? license_settings { get; set; }
    public string? license_server_url { get; set; }
    public string? license_type { get; set; }
    public string? api_server_url { get; set; }
    public string? login { get; set; }
    public string? plain_password { get; set; }
    public string? tNavigator_API_client_exe = "tNavigator-API-client";

}
