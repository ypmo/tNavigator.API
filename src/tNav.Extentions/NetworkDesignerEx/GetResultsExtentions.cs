
using System.Security.Permissions;
using Microsoft.Data.Analysis;
using tNav.NetworkDesignerEx;

namespace tNav.ModelDesignerEx;

public static class GetResultsExtentions
{
    public static DataFrame? GetResults(this IProject project, string resultName, DateTime dateTime, NetworkModelObjectType modelObjectType, params string[] parametrs)
    {

        var objType = modelObjectType switch
        {
            NetworkModelObjectType.Well => "well",
            NetworkModelObjectType.Pipe => "pipe",
            _ => throw new NotImplementedException(),
        };
        var available_results = parametrs.Any() ?
            "[" + string.Join(", ", parametrs.Select(t => $"'{t}'")) + "]" :
            $"allObjects[0].get_available_extended_results()";
                    var code = $"""
from datetime import datetime
import pandas as pd
date = datetime({dateTime.tNavFormat()})
allObjects = get_objects_by_type(type='{objType}')
available_results = {available_results}
object_results = pd.DataFrame()
for p in allObjects:
    results = p.get_extended_result_values(result_name='{resultName}', type='{objType}_segment', parameter_names=available_results, date=date)
    results['{objType}_name'] = p.name()
    object_results = pd.concat([object_results, results], ignore_index=True)
    
object_results.columns = object_results.columns.str.replace(' ', '_')
return object_results
""";
//         var code = $"""
// from datetime import datetime
// import pandas as pd
// date = datetime({dateTime.tNavFormat()})
// allObjects = get_objects_by_type(type='{objType}')
// available_results = {available_results}
// object_results = pd.DataFrame()
// for p in allObjects:
//     print(p.name())
//     results = p.get_extended_result_values(result_name='{resultName}', type='{objType}_segment', parameter_names=available_results, date=date)
//     results['{objType}_name'] = p.name()
//     object_results = pd.concat([object_results, results], ignore_index=True)
// object_results.columns = object_results.columns.str.replace(' ', '_')
// return object_results
// """;
        var df_nd_results = project.RunPyCode<DataFrame>(code: code);
        return df_nd_results;
    }
}
