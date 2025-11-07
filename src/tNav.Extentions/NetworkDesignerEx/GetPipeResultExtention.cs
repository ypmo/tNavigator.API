using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tNav.ModelDesignerEx;

namespace tNav.NetworkDesignerEx;

public static class GetPipeResultExtention
{
    static readonly string[] parametrs = ["pressure", "temperature"];
    public static IEnumerable<PipeResult> GetPipeResults(this IProject project, DateTime dateTime, string resultName)
    {
        List<PipeResult> result = [];
        var frame = project.GetResults(resultName: resultName, dateTime: dateTime, modelObjectType: NetworkModelObjectType.Pipe, parametrs: parametrs);
        if (frame != null)
        {
            foreach (var row in frame.Rows)
            {
                result.Add(new PipeResult
                {
                    Id = (int)row["index"],
                    Name = (string?)row["pipe_name"],
                    Pressure = (double)row["pressure"],
                    Temperature = (double)row["temperature"],
                });
            }
        }
        return result;
    }
}

public class PipeResult
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    public double Pressure { get; set; }
    public double Temperature { get; set; }
}