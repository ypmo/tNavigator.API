using System;

namespace tNav;

public enum CaseType
{
    /// <summary>
    /// model_designer
    /// </summary>
    ModelDesigner,
    /// <summary>
    /// network_designer
    /// </summary>
    NetworkDesigner,
    /// <summary>
    /// mba
    /// </summary>
    MBA,

}
public static class CaseTypeEx
{
    public static string tNavValue(this CaseType type)
    {
        return (type) switch
        {
            CaseType.ModelDesigner => "model_designer",
            CaseType.NetworkDesigner => "network_designer",
            CaseType.MBA => "mba",
            _ => throw new NotImplementedException()
        };
    }
}


