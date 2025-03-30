using System;

namespace tNavigator.API;

public enum CaseType
{
    MD,
    ND,
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


