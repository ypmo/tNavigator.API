using System;

namespace tNav.API.ProgectExtentions;

public static class LicenseRequest
{
    public static void RequestLicenseFeatures(this IProject project, params LicenseFeature[] licenseFeatures)
    {
        Func<LicenseFeature, string> licFeateFunc = (f) => $"{{\"feature\" : \"{LicenseName(f)}\"}}";
        var command = $"""
request_license_features (requested_features=[
{string.Join(",", licenseFeatures.Select(t=>licFeateFunc(t)))}
])
""";
    }

    private static string LicenseName(LicenseFeature featuter) => (featuter) switch
    {
        LicenseFeature.ModelDesiner => "FEAT_MODEL_DESIGNER",
        LicenseFeature.NetworkDesigner => "FEAT_NETWORK_DESIGNER",
        LicenseFeature.WELL_DESIGNE => "FEAT_WELL_DESIGNER",
        LicenseFeature.PVT_DESIGNER => "FEAT_PVT_DESIGNER",
        _ => throw new NotImplementedException(),
    };

}
public enum LicenseFeature
{
    ModelDesiner,
    NetworkDesigner,
    WELL_DESIGNE,
    PVT_DESIGNER,
}