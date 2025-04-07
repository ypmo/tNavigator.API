from subprocess import Popen, PIPE, DEVNULL
import struct
import os
import json
import sys

logfile = "log.txt"
have_pandas = False
try:
    import pandas as pd

    have_pandas = True
except ImportError:
    have_pandas = False

have_datetime = False
try:
    import datetime

    have_datetime = True
except ImportError:
    have_datetime = False

have_numpy = False
try:
    import numpy as np

    have_numpy = True
except ImportError:
    have_numpy = False


class ProjectID:
    invalid = -1


class ProjectType:
    MD = "md"
    GD = "gd"
    ND = "nd"
    RP = "rpd"
    PVTD = "pvtd"
    WD = "wd"


class CaseType:
    MD = "model_designer"
    ND = "network_designer"
    MBA = "mba"


class Project:
    def __init__(
        self,
        process=None,
        project_id=ProjectID.invalid,
        parent_id=ProjectID.invalid,
        project_type=ProjectType.MD,
        save_on_close=False,
    ):
        self.project_id = project_id
        self.parent_id = parent_id
        self.type = project_type
        self.process = process
        self.save_on_close = save_on_close

    def __enter__(self):
        if self.project_id == ProjectID.invalid:
            raise RuntimeError('project must be valid to be used in "with" statement')
        return self

    def __exit__(self, exc_type, exc_value, tb):
        if self.parent_id == ProjectID.invalid:
            self.close_project()
        return False

    def close_project(self):
        """
        close tNavigator project

        Notes
        -----
        Subprojects cannot be closed. They are closed automatically when main project is closed
        Project will be saved before closing if it was specified while opening (see save_on_close flag).
        Project is closed automatically if context manager ('with' syntax) is used.

        Examples
        --------
        Project.close_project ()

        """
        if self.save_on_close:
            self.__save_project()
        command = f'close_project (id = "{self.project_id}")\n'
        process_message(self.process, command)

    def get_subproject_by_name(self, name, type=ProjectType.ND):
        """
        get class for sending commands to subproject of already opened tNavigator project

        Parameters
        ----------
        name: string
            name of subproject
        type: ProjectType
            type of subproject

        Returns
        -------
        out: tNavigator_python_API.Project
            class for sending commands to subproject

        Examples
        --------
        Project.get_subproject_by_name (name='nd_project1')

        """
        command = f'get_subproject_by_name (id = "{self.project_id}", name = "{name}", type = "{type}")\n'
        process_message(self.process, command)
        new_id = int(readline(self.process).strip())
        return Project(self.process, new_id, self.project_id, type)

    def get_list_of_subprojects(self, type=ProjectType.ND) -> list:
        """
        get list of subprojects of already opened tNavigator project.

        Parameters
        ----------
        type: ProjectType
            type of subprojects

        Returns
        -------
        out: list of strings
            names of subprojects of gived type

        Examples
        --------
        Project.get_list_of_subprojects (type=ProjectType.ND)

        """
        command = (
            f'get_list_of_subprojects (id = "{self.project_id}", type = "{type}")\n'
        )
        process_message(self.process, command)
        subprojects = []
        count_str = int(readline(self.process))
        for i in range(count_str):
            subprojects.append(readline(self.process).strip())
        return subprojects

    def __save_project(self):
        id_to_save = self.parent_id
        if id_to_save == ProjectID.invalid:
            id_to_save = self.project_id

        save_command = f'run_py_code (code = "save_project ()", id = "{id_to_save}")\n'
        process_message(self.process, save_command)
        unpack_string(self.process.stdout)

    def run_py_code(self, file=None, files=None, code=None, save=False):
        """
        run python code in given project. The code is run like custom code of workflow
        Code from 'file', 'files' will be executed first, and then from 'code' argument
        Parameters
        ----------
        code: string
            string with python code for workflow
        file: string
            string with path to python file code for workflow.
        files: list of strings
            list of strings with path to python files code for workflow
        save: bool
            whether to save the project after execution of the code.
        Returns
        -------
        out:
            the function returns an object passed to 'return' instruction inside the given code
        Examples
        --------
        1.
        choke_list_str = nd_proj.run_py_code (code=\"\"\"
        def collect_all_obj_to_list (type_):
          lst=[]
          for o in get_objects_by_type (type=type_):
            lst.append(o.name ())
            return lst
        def send_list_to_tnav (lst):
            return ' '.join(str(el) for el in lst)
        return send_list_to_tnav (collect_all_obj_to_list ('choke'))
        \"\"\")

        2.
        'dataframetest.py':
        import pandas as pd
        d = {'col1': [0, 1, 2, 3], 'col2': pd.Series([2, 3], index=[2, 3])}
        df =  pd.DataFrame(data=d, index=[0, 1, 2, 3])
        return df

        df=proj.run_py_code (file='dataframetest.py')

        3.
        choke_list_str = nd_proj.run_py_code (code="return ' '.join (str (o.name ()) for o in get_objects_by_type (type='choke'))")
        """

        code_from_file = ""
        if file != None:
            code_from_file = open(file, "r").read()
        if files != None:
            for script_with_code in files:
                code_from_file += open(script_with_code, "r").read()
                code_from_file += "\n"

        if code_from_file == "" and code == None:
            raise RuntimeError("run_py_code needs at least one argument")

        res_code = code_from_file

        if code != None:
            res_code += "\n" + code

        res_code = res_code.replace("\\", "\\\\")
        res_code = res_code.replace('"', '\\"')
        command = f'run_py_code (code = "{res_code}", id = "{self.project_id}"'
        command += ")\n"

        process_message(self.process, command)
        ret_value = unpack_data(self.process.stdout)

        if save:
            self.__save_project()
        return ret_value


class ConnectionOptions:
    """
    Parameters
    ----------
    minimum_required_version: tuple, optional
        minimum required version of tNavigator. It can be tuple of two or three elements.
        If minimum required version is "23.3", then (23,3) should be passed.
        Third element should be used if some certain update level is required.
        If v23.3-2724-g70d0cc8 or newer version is required, then (23,3,2724) should be passed
    license_wait_time_limit__secs: integer
        License wait time limit, in seconds. License wait time is unlimited by default.
    license_settings: string
        path to the file with license server connection
            settings:
            [tNavigator_license_settings]
            url=
            login=
            pass=
    license_server_url: string
        url of the license server
    license_type: string
        license type  (possible values="network", "usb")
    For license_settings and license_server_url, license_type is specified with the "network" parameter.
    If license_settings and license_server_url are specified, license_settings will be selected.
    api_server_url: string
        address of the server to which the api client will connect
        Address and port recording format is hostname:port. For example, 127.0.0.1:5555
    """

    minimum_required_version = None
    license_wait_time_limit__secs = None
    license_settings = None
    license_server_url = None
    license_type = None
    api_server_url = None
    login = None
    plain_password = None


tNavigator_API_client_exe = os.path.join(
    os.path.dirname(os.path.abspath(__file__)),
    "tNavigator-API-client" if os.name == "posix" else "tNavigator-API-client.exe",
)


class Connection:
    def __init__(
        self,
        path_to_exe=None,
        connection_options=ConnectionOptions(),
        minimum_required_version=None,
        license_wait_time_limit__secs=None,
    ):
        """
        initialize connection with tNavigator

        Parameters
        ----------
        path_to_exe: string
            path to tNavigator-con.exe
        minimum_required_version: tuple, optional
            minimum required version of tNavigator. It can be tuple of two or three elements.
            If minimum required version is "23.3", then (23,3) should be passed.
            Third element should be used if some certain update level is required.
            If v23.3-2724-g70d0cc8 or newer version is required, then (23,3,2724) should be passed
        license_wait_time_limit__secs: integer
            License wait time limit, in seconds. License wait time is unlimited by default.

        Returns
        -------
        out : tNavigator_python_API.Connection
            class for sending commands via connection with tNavigator API server

        Examples
        --------
        Connection (path_to_exe='C:\\Program Files\\tNavigator-23.3\\tNavigator.exe', minimum_required_version=(23,3))

        """
        open(logfile, "w").write(f"{datetime.datetime.now()}\n")
        if minimum_required_version:
            connection_options.minimum_required_version = minimum_required_version
        if license_wait_time_limit__secs:
            connection_options.license_wait_time_limit__secs = (
                license_wait_time_limit__secs
            )
        if path_to_exe == None:
            path_to_exe = tNavigator_API_client_exe

        if connection_options.login and connection_options.plain_password:
            auth_info = {
                "login": str(connection_options.login),
                "plain_password": str(connection_options.plain_password),
            }
            api_client_config_json_object = json.dumps(auth_info, indent=2)
            api_server_config_dir = os.getenv("HOME")
            if sys.platform.startswith("linux"):
                api_server_config_dir += "/.config/tNavigator/api_server/"
            else:
                api_server_config_dir = os.getenv("APPDATA") + "/tNavigator/api_server/"
            if not os.path.exists(api_server_config_dir):
                os.mkdir(api_server_config_dir)
            api_client_config_json_filepath = (
                api_server_config_dir + "api_client_config.json"
            )
            api_client_config_json = open(api_client_config_json_filepath, "w")
            api_client_config_json.write(api_client_config_json_object)
            api_client_config_json.close()

        self.__init_cmd_args_from_connection_options(path_to_exe, connection_options)
        process = Popen(self.cmd_args, stdin=PIPE, stdout=PIPE, stderr=DEVNULL)
        self.process = process
        if connection_options.minimum_required_version:
            command = f'minimum_required_version (major="{connection_options.minimum_required_version[0]}", minor="{connection_options.minimum_required_version[1]}"'
            if len(connection_options.minimum_required_version) > 2:
                command += (
                    f', update="{connection_options.minimum_required_version[2]}"'
                )
            command += ")\n"
            process_message(self.process, command)

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_value, tb):
        if self.process.poll() == None:
            self.stop()
        return False

    def create_project(
        self, path: str, case_type: CaseType, project_type: ProjectType
    ) -> Project:
        """
        create tNavigator project

        Parameters
        ----------
        path : string
            path to tNavigator project file.
            If the API server is running on another machine, then it has its own directory with models.
            You will need to specify the relative path and folder where the project_folder/project.snp project will be created
            If the API server is running on its own machine, then you need to specify the absolute path to the project
        case_type: CaseType
            type of case that is being installed for the project
        project_type: ProjectType
            type of project. Currently only Model Designer projects are supported for opening

        Returns
        -------
        out : tNavigator_python_API.Project
            class for sending commands to opened tNavigator project

        Examples
        --------
        API Server running on its own machine:
          Connection.create_project (path='C:\\Projects\\Example.snp', case_type = CaseType.MD, project_type = ProjectType.MD)
        API Server running on another machine:
          Connection.create_project (path='\\Projects\\Example.snp', case_type = CaseType.MD, project_type = ProjectType.MD)
        """
        escaped_path = path.replace("\\", "\\\\")
        command = f'create_project (path = "{escaped_path}", case = "{case_type}", type = "{project_type}")\n'
        process_message(self.process, command)
        project_id = int(readline(self.process))
        parent_id = ProjectID.invalid
        return Project(
            self.process, project_id, parent_id, project_type, save_on_close=True
        )

    def get_list_of_projects(self):
        """
        get list of projects which api-server contains in api-server-models-dir

        Parameters
        ----------
        empty

        Returns
        -------
        out : list of strings
            relative paths to projects that are in the api-server-models-dir folder

        Examples
        --------
        Connection.get_list_of_projects ()

        """
        command = f"get_list_of_projects ()"
        process_message(self.process, command)
        projects = []
        count_str = int(readline(self.process))
        for i in range(count_str):
            projects.append(readline(self.process).strip())
        return projects

    def open_project(
        self, path, project_type=ProjectType.MD, save_on_close=False
    ) -> Project:
        """
        open tNavigator project

        Parameters
        ----------
        path : string
            path to tNavigator project file
        project_type: ProjectType
            type of project. Currently only Model Designer projects are supported for opening
        save_on_close: bool
            if changes in project will be saved before closing

        Returns
        -------
        out : tNavigator_python_API.Project
            class for sending commands to opened tNavigator project

        Examples
        --------
        Connection.open_project (path='C:\\Projects\\Example.snp')

        """
        escaped_path = path.replace("\\", "\\\\")
        command = f'open_project (path = "{escaped_path}", type="{project_type}")\n'
        process_message(self.process, command)
        project_id = int(readline(self.process))
        parent_id = ProjectID.invalid
        return Project(self.process, project_id, parent_id, project_type, save_on_close)

    def get_version_string(self):
        """
        get version of tNavigator API server

        Returns
        -------
        out: string
            string with tNavigator API server version

        Examples
        --------
        print ('tNavigator version is ' + Connection.get_version_string ())
        """
        process_message(self.process, "get_version_string ()\n")
        return readline(self.process).rstrip()

    def stop(self):
        process_message(self.process, "stop ()\n")

    def __parse_dispatcher_ip_and_port(self, api_server_url: str):
        network_data = api_server_url.split(":")
        if len(network_data) != 2:
            raise RuntimeError(
                'Wrong parameter: api_server_url. Use api_server_url="hostname:port"'
            )
        (host, port) = (network_data[0], network_data[1])
        self.cmd_args.append(f"--dispatcher-ip={host}")
        self.cmd_args.append(f"--dispatcher-task-port={port}")

    def __init_cmd_args_from_connection_options(
        self, path_to_exe: str, conn_opts: ConnectionOptions
    ):
        self.cmd_args = [path_to_exe]
        if path_to_exe == tNavigator_API_client_exe:
            if conn_opts.api_server_url:
                self.__parse_dispatcher_ip_and_port(conn_opts.api_server_url)
            else:
                msg = f"Could not launch {path_to_exe} without api_server_url. Please, specify at least api_server_url in ConnectionOptions"
                raise RuntimeError(msg)
        else:
            self.cmd_args.append(
                "--api-client" if conn_opts.api_server_url else "--api-server"
            )
            if conn_opts.api_server_url:
                self.__parse_dispatcher_ip_and_port(conn_opts.api_server_url)

        if conn_opts.license_wait_time_limit__secs:
            self.cmd_args.append(
                f"--license-wait-time-limit={conn_opts.license_wait_time_limit__secs}"
            )

        if conn_opts.license_settings or conn_opts.license_server_url:
            conn_opts.license_type = "network"
            self.cmd_args.append(f"--license-type={conn_opts.license_type}")
            if conn_opts.license_settings:
                self.cmd_args.append(f"--license-settings={conn_opts.license_settings}")
            elif conn_opts.license_server_url:
                self.cmd_args.append(f"--server-url={conn_opts.license_server_url}")
        elif conn_opts.license_type:
            self.cmd_args.append(f"--license-type={conn_opts.license_type}")


_encoding = "utf-8"


def readline(process):
    message = process.stdout.readline().decode(_encoding)
    output_log(message)
    return message


def process_message(process, message):
    input_log(message)
    process.stdin.write(message.encode(_encoding))
    process.stdin.flush()
    err_res = readline(process)
    if err_res != "OK\n":
        count_str = int(readline(process))
        msg = ""
        for i in range(count_str):
            msg += readline(process)
        raise RuntimeError(msg.rstrip())


def input_log(message):
    if not message.endswith("\n"):
        message = message + "\n"
    open(logfile, "a").write(f"***INPUT***\n{message}***END***\n")


def output_log(message):
    open(logfile, "a").write(message)


class size_const:
    size_t = 8
    integer = 4
    double = 8


def unpack_data(stream, read_type=""):
    if read_type == "":
        read_type = unpack_string(stream)
    if read_type == "None":
        ret_value = None
    elif read_type == "Int":
        ret_value = unpack_int(stream)
    elif read_type == "bool":
        ret_value = bool(unpack_int(stream))
    elif read_type == "Float":
        ret_value = unpack_double(stream)
    elif read_type == "String":
        ret_value = unpack_string(stream)
    elif read_type == "DataFrame":
        ret_value = unpack_dataframe(stream)
    elif read_type == "datetime":
        ret_value = unpack_datetime(stream)
    elif read_type == "numpy.ndarray":
        ret_value = unpack_numpy_array(stream)
    elif read_type == "List":
        ret_value = unpack_list_and_len(stream)
    elif read_type == "Dict":
        ret_value = unpack_dict(stream)
    elif read_type == "Tuple":
        ret_value = unpack_tuple(stream)
    elif read_type == "Error":
        raise RuntimeError(unpack_string(stream))
    else:
        raise RuntimeError(f"Unsupported type {read_type}")
    return ret_value


def unpack_string(stream):
    data=stream.read(size_const.size_t)
    size = struct.unpack("Q", data)[0]
    output_log(data.hex())
    data = stream.read(size) 
    output_log(f'<{data.decode(_encoding)}>{data.hex()}')
    #output_log(data.hex())
    return data.decode(_encoding)


def unpack_int(stream):
    data = stream.read(size_const.integer)
    value = struct.unpack("i", data)[0]
    output_log(data.hex())
    return value


def unpack_double(stream):
    data =stream.read(size_const.double)
    value= struct.unpack("d", data)[0]
    output_log(data.hex())
    return value


def unpack_tuple(stream):
    length = unpack_int(stream)
    lst = []
    for i in range(length):
        lst.append(unpack_data(stream))
    return tuple(lst)


def unpack_list_and_len(stream):
    length = unpack_int(stream)
    return unpack_list(stream, length)


def unpack_dict(stream):
    length = unpack_int(stream)
    dic = {}
    for i in range(length):
        key = unpack_data(stream)
        value = unpack_data(stream)
        dic.update({key: value})
    return dic


def unpack_list(stream, length):
    lst = []
    if length == 0:
        return lst
    read_type = unpack_string(stream)
    for i in range(length):
        lst.append(unpack_data(stream, read_type))
    return lst


def unpack_dataframe(stream):
    if not have_pandas:
        raise RuntimeError("dataframe received but pandas module was not found")
    dt = pd.DataFrame()
    col_count = unpack_int(stream)
    row_count = unpack_int(stream)
    for i in range(col_count):
        column_name = unpack_string(stream)
        dt.insert(i, column_name, unpack_list(stream, row_count))
    dt.index = unpack_list(stream, row_count)
    return dt


def unpack_datetime(stream):
    if not have_datetime:
        raise RuntimeError(
            "datetime.datetime received but datetime module was not found"
        )
    year = unpack_int(stream)
    month = unpack_int(stream)
    day = unpack_int(stream)
    hour = unpack_int(stream)
    minute = unpack_int(stream)
    second = unpack_int(stream)
    microsecond = unpack_int(stream)
    return datetime.datetime(year, month, day, hour, minute, second, microsecond)


def unpack_numpy_array(stream):
    if not have_numpy:
        raise RuntimeError("numpy.ndarray received but datetime module was not found")
    shape = unpack_list_and_len(stream)
    lst = unpack_list(stream, np.prod(shape))
    return np.array(lst).reshape(shape)
