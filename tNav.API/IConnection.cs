using System;

namespace tNav.API;

public interface IConnection : IDisposable
{
    /// <summary>
    /// Create tNavigator project
    /// <example>
    ///  API Server running on its own machine:
    /// <code>
    /// Connection.create_project (path='C:\\Projects\\Example.snp', case_type = CaseType.MD, project_type = ProjectType.MD)
    /// </code>
    /// API Server running on another machine:
    /// </example>
    /// <code>
    /// Connection.create_project (path='\\Projects\\Example.snp', case_type = CaseType.MD, project_type = ProjectType.MD)
    /// </code>
    /// </summary>
    /// <param name="path">path to tNavigator project file
    /// If the API server is running on another machine, then it has its own directory with models.
    /// You will need to specify the relative path and folder where the project_folder/project.snp project will be created
    /// If the API server is running on its own machine, then you need to specify the absolute path to the project
    /// </param>
    /// <param name="case_type">type of case that is being installed for the project</param>
    /// <param name="project_type">type of project. Currently only Model Designer projects are supported for opening</param>
    /// <returns>tNav.API.IProject
    /// class for sending commands to opened tNavigator project
    /// </returns>
    IProject CreateProject(string path, CaseType case_type, ProjectType project_type);

    /// <summary>
    /// get list of projects which api-server contains in api-server-models-dir
    /// <example>
    /// <code>
    /// Connection.get_list_of_projects ()
    /// </code>
    /// </example>
    /// </summary>    /// 
    /// <returns> relative paths to projects that are in the api-server-models-dir folder</returns>
    List<string> GetListOfProjects();

    /// <summary>
    /// open tNavigator project
    /// <code>
    /// Connection.open_project (path='C:\\Projects\\Example.snp')
    /// </code>
    /// </summary>
    /// <param name="path">path to tNavigator project file</param>
    /// <param name="project_type"> type of project. Currently only Model Designer projects are supported for opening</param>
    /// <param name="save_on_close"> if changes in project will be saved before closing</param>
    /// <returns> tNav.API.IProject
    /// class for sending commands to opened tNavigator project
    /// </returns>
    IProject OpenProject(string path, ProjectType project_type = ProjectType.MD, bool save_on_close = false);

    /// <summary>
    /// get version of tNavigator API server
    /// <code>
    /// print ('tNavigator version is ' + Connection.get_version_string ())
    /// </code>
    /// </summary>
    /// <returns> string with tNavigator API server version</returns>
    string GetVersionString();
}
