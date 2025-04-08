using System;
using System.Diagnostics;
namespace tNavigator.API;
public class Connection : NavBase, IConnection
{
    List<string> cmd_args = [];
    string tNavigator_API_client_exe = "";
    readonly Process process;
    public Connection(string? path_to_exe, ConnectionOptions connection_options)
    {
        connection_options ??= new();
        if (path_to_exe == null)
        { path_to_exe = tNavigator_API_client_exe; }

        init_cmd_args_from_connection_options(path_to_exe, connection_options);

        process = new Process();
        process.StartInfo.FileName = path_to_exe;
        process.StartInfo.Arguments = string.Join(" ", cmd_args);
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        //process.StartInfo.RedirectStandardError = true;
        //process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { Console.WriteLine(e.Data); });

        var started = process.Start();
        if (!string.IsNullOrEmpty(connection_options.minimum_required_version))
        {
            var command = $"minimum_required_version (major=\"{connection_options.minimum_required_version[0]}\", minor=\"{connection_options.minimum_required_version[1]}\"";
            if (connection_options.minimum_required_version?.Length > 2)
                command += $", update=\"{connection_options.minimum_required_version[2]}\"";
            command += ")\n";
            process_message(process, command);
        }
    }
    public Project create_project(string path, CaseType case_type, ProjectType project_type)
    {

        var command = $"create_project (path = \"{path}\", case = \"{case_type.tNavValue()}\", type = \"{project_type.tNavValue()}\")\n";
        process_message(process, command);
        int.TryParse(readline(process), out int project_id);
        var parent_id = ProjectID.invalid;
        return new Project(
            process, project_id, parent_id, project_type, save_on_close: true);

    }

    public List<string> get_list_of_projects()
    {
        throw new NotImplementedException();
    }

    public Project open_project(string path, ProjectType project_type = ProjectType.MD, bool save_on_close = false)
    {
        var command = $"open_project (path = \"{path}\", type=\"{project_type}\")\n";
        process_message(process, command);
        int.TryParse(readline(process), out int project_id);
        var parent_id = ProjectID.invalid;
        return new Project(process, project_id, parent_id, project_type, save_on_close);
    }
    public string get_version_string()
    {
        throw new NotImplementedException();
    }

    public void stop()
    {
        throw new NotImplementedException();
    }

    void init_cmd_args_from_connection_options(string? path_to_exe, ConnectionOptions conn_opts)
    {
        cmd_args = [];
        if (path_to_exe == tNavigator_API_client_exe)
        {
            if (!string.IsNullOrEmpty(conn_opts.api_server_url))
            {
                parse_dispatcher_ip_and_port(conn_opts.api_server_url);
            }
            else
            {
                throw new InvalidOperationException($"Could not launch {path_to_exe} without api_server_url. Please, specify at least api_server_url in ConnectionOptions");
            }
        }
        else
        {


            if (!string.IsNullOrEmpty(conn_opts.api_server_url))
            {
                cmd_args.Add("--api-client");
            }
            else
            {
                cmd_args.Add("--api-server");
            }
            if (!string.IsNullOrEmpty(conn_opts.api_server_url))
            {
                parse_dispatcher_ip_and_port(conn_opts.api_server_url);
            }
        }
        if (conn_opts.license_wait_time_limit__secs.HasValue)
            cmd_args.Add(
                $"--license-wait-time-limit={conn_opts.license_wait_time_limit__secs}"
            );

        if (!string.IsNullOrEmpty(conn_opts.license_settings) || !string.IsNullOrEmpty(conn_opts.license_server_url))
        {
            conn_opts.license_type = "network";
            cmd_args.Add($"--license-type={conn_opts.license_type}");
            if (!string.IsNullOrEmpty(conn_opts.license_settings))
            {
                cmd_args.Add($"--license-settings={conn_opts.license_settings}");
            }
            else if (!string.IsNullOrEmpty(conn_opts.license_server_url))
            {
                cmd_args.Add($"--server-url={conn_opts.license_server_url}");
            }
        }
        else if (conn_opts.license_type != null)
        {
            cmd_args.Add($"--license-type={conn_opts.license_type}");
        }
    }

    void parse_dispatcher_ip_and_port(string? api_server_url)
    {
        var network_data = api_server_url?.Split(":") ?? [];
        if (network_data.Length != 2)
        {
            throw new InvalidOperationException("Wrong parameter: api_server_url. Use api_server_url=\"hostname:port\"");
        }
        var (host, port) = (network_data[0], network_data[1]);
        cmd_args.Add($"--dispatcher-ip={host}");
        cmd_args.Add($"--dispatcher-task-port={port}");
    }



}
