using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.API;

public static class ConnectionFactory
{
    /// <summary>
    /// initialize connection with tNavigator
    /// </summary>
    /// <param name="path_to_exe">path to tNavigator-con.exe</param>
    /// <param name="connection_options"></param>
    /// <param name="minimum_required_version">tuple, optional 
    ///     minimum required version of tNavigator.It can be tuple of two or three elements.
    ///     If minimum required version is "23.3", then(23,3) should be passed.
    ///     Third element should be used if some certain update level is required.                
    ///     If v23.3-2724-g70d0cc8 or newer version is required, then(23,3,2724) should be passed</param>
    /// <param name="license_wait_time_limit__secs">License wait time limit, in seconds. License wait time is unlimited by default.</param>
    public static IConnection GetConnection(
       string? path_to_exe = null,
       ConnectionOptions? connection_options = null,
       (int major, int minor, int? update)? minimum_required_version = null,
       int? license_wait_time_limit__secs = null
       )
    {
        Log.Write($"{DateTime.Now}\n");
        connection_options ??= new();

        if (minimum_required_version != null) connection_options.MinimumRequiredVersion = minimum_required_version;
        if (license_wait_time_limit__secs != null) connection_options.LicenseWaitTimeLimitSecs = license_wait_time_limit__secs;
        path_to_exe ??= tNavigatorPath.tNavigator_API_client_exe();


        var cmd_args = init_cmd_args_from_connection_options(path_to_exe, connection_options);

        var startInfo = new ProcessStartInfo
        {
            FileName = path_to_exe,
            Arguments = string.Join(" ", cmd_args),
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
        };
        var process =Process.Start(startInfo);

        if (process==null)
        {
            throw new InvalidOperationException($"Не удалось запустить приложение  {path_to_exe}");
        }

        if (connection_options.MinimumRequiredVersion != null)
        {
            var command = $"minimum_required_version (major=\"{connection_options.MinimumRequiredVersion.Value.major}\", minor=\"{connection_options.MinimumRequiredVersion.Value.minor}\"";
            if (connection_options.MinimumRequiredVersion.Value.update != null)
                command += $", update=\"{connection_options.MinimumRequiredVersion.Value.update}\"";
            command += ")\n";
            Processes.process_message(process, command);
        }
        return new Connection(process);
    }

    static List<string> init_cmd_args_from_connection_options(string? path_to_exe, ConnectionOptions conn_opts)
    {
        List<string> cmd_args = [path_to_exe];
        if (path_to_exe == tNavigatorPath.tNavigator_API_client_exe())
        {
            if (conn_opts.ApiServerUrl != null)
            { cmd_args.AddRange(parse_dispatcher_ip_and_port(conn_opts.ApiServerUrl)); }
            else
            { throw new InvalidOperationException($"Could not launch {path_to_exe} without api_server_url. Please, specify at least api_server_url in ConnectionOptions"); }
        }
        else
        {
            cmd_args.Add(conn_opts.ApiServerUrl != null ? "--api-client" : "--api-server");
            if (conn_opts.ApiServerUrl != null)
            { cmd_args.AddRange(parse_dispatcher_ip_and_port(conn_opts.ApiServerUrl)); }
        }

        if (conn_opts.LicenseWaitTimeLimitSecs.HasValue)
            cmd_args.Add($"--license-wait-time-limit={conn_opts.LicenseWaitTimeLimitSecs}");

        if (conn_opts.LicenseSettings != null || conn_opts.LicenseServerUrl != null)
        {
            conn_opts.LicenseType = "network";
            cmd_args.Add($"--license-type={conn_opts.LicenseType}");

            if (conn_opts.LicenseSettings != null)
            { cmd_args.Add($"--license-settings={conn_opts.LicenseSettings}"); }
            else if (conn_opts.LicenseServerUrl != null)
            { cmd_args.Add($"--server-url={conn_opts.LicenseServerUrl}"); }
        }
        else if (conn_opts.LicenseType != null)
        {
            cmd_args.Add($"--license-type={conn_opts.LicenseType}");
        }
        return cmd_args;
    }

    static List<string> parse_dispatcher_ip_and_port(string? api_server_url)
    {
        var network_data = api_server_url?.Split(":") ?? [];
        if (network_data.Length != 2)
        {
            throw new InvalidOperationException("Wrong parameter: api_server_url. Use api_server_url=\"hostname:port\"");
        }
        var (host, port) = (network_data[0], network_data[1]);
        return [$"--dispatcher-ip={host}", $"--dispatcher-task-port={port}"];
    }
}
