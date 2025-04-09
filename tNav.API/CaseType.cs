using System;

namespace tNav.API;

public enum CaseType
{
/// <summary>
/// model_designer
/// </summary>
    MD,
    /// <summary>
    /// network_designer
    /// </summary>
    ND,
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
            CaseType.MD => "model_designer",
            CaseType.ND => "network_designer",
            CaseType.MBA => "mba",
            _ => throw new NotImplementedException()
        };
    }
}


