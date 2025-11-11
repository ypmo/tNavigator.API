using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tNav.ModelDesignerEx;

namespace tNav.NetworkDesignerEx;

public static class GetWellResultExtentions
{
    static readonly string[] parametrs = ["length", "pressure", "temperature"];
    public static IEnumerable<WellResult> GetWellResults(this IProject project, DateTime dateTime, string resultName)
    {
        List<WellResult> result = [];
        var frame = project.GetResults(resultName: resultName, dateTime: dateTime, modelObjectType: NetworkModelObjectType.Well, parametrs: parametrs);
        if (frame != null)
        { 
            foreach (var row in frame.Rows)
            {
                result.Add(new WellResult
                {
                    Index = (int)row["index"],
                    Name = (string?)row["well_name"],
                    Length=(double)row["length"],
                    Pressure= (double)row["pressure"],
                    Temperature=(double)row["temperature"],
                });
            }
        }
        return result;
    }
}

public class WellResult
{
    public int Index { get; set; }
    public string? Name { get; set; }
    public double Length{ get; set; }
    public double Pressure { get; set; }
    public double Temperature { get; set; }
}