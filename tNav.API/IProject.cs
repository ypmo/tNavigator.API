using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

namespace tNav.API;

public interface IProject : IDisposable
{

    /// <summary>
    /// close tNavigator project
    /// <code>
    /// Project.close_project ()
    /// </code>
    /// </summary>
    /// <remarks>    
    /// Subprojects cannot be closed. They are closed automatically when main project is closed
    /// Project will be saved before closing if it was specified while opening(see save_on_close flag).
    /// Project is closed automatically if context manager('with' syntax) is used.
    /// </remarks>
    void CloseProject();

    /// <summary>
    /// get class for sending commands to subproject of already opened tNavigator project
    /// <code>
    /// Project.get_subproject_by_name (name='nd_project1')
    /// </code>
    /// </summary>
    /// <param name="name">name of subproject</param>
    /// <param name="type">type of subproject</param>
    /// <returns>tNav.API.IProject
    /// class for sending commands to subproject
    /// </returns>
    IProject GetSubProjectByName(string name, ProjectType type = ProjectType.ND);

    /// <summary>
    /// get list of subprojects of already opened tNavigator project.
    /// <code>
    /// Project.get_list_of_subprojects (type=ProjectType.ND)
    /// </code>
    /// </summary>
    /// <param name="type">type of subprojects</param>
    /// <returns>names of subprojects of gived type</returns>
    List<string> GetListOfSubProjects(ProjectType type = ProjectType.ND);

    /// <summary>
    /// run python code in given project. The code is run like custom code of workflow
    /// Code from 'file', 'files' will be executed first, and then from 'code' argument
    /// </summary>
    /// <example>
    /// <code>
    /// 1.
    /// choke_list_str = nd_proj.run_py_code(code=\"\"\" 
    /// def collect_all_obj_to_list(type_):
    ///     lst=[]
    ///     for o in get_objects_by_type(type= type_) :
    ///     lst.append(o.name ())
    ///     return lst
    /// def send_list_to_tnav(lst) :
    ///     return ' '.join(str(el) for el in lst)
    /// return send_list_to_tnav(collect_all_obj_to_list ('choke'))
    ///    \"\"\")
    /// 2.
    /// 'dataframetest.py':
    /// import pandas as pd
    ///  d = {'col1': [0, 1, 2, 3], 'col2': pd.Series([2, 3], index=[2, 3])}
    ///  df =  pd.DataFrame(data=d, index=[0, 1, 2, 3])
    ///  return df
    ///
    ///  df=proj.run_py_code (file='dataframetest.py')        
    ///  
    /// 3.
    /// choke_list_str = nd_proj.run_py_code(code="return ' '.join (str (o.name ()) for o in get_objects_by_type (type='choke'))")
    /// </code>
    /// </example>
    /// <param name="file">string with path to python file code for workflow.</param>
    /// <param name="files">list of strings with path to python files code for workflow</param>
    /// <param name="code">string with python code for workflow</param>
    /// <param name="save">whether to save the project after execution of the code.</param>
    /// <returns>he function returns an object passed to 'return' instruction inside the given code</returns>
    object RunPyCode(string? file = null, string[]? files = null, string? code = null, bool save = false);
}
