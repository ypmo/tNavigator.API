using System;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;

namespace tNav.WellDesigner;

public static class AdjustBasicDataExtantion
{
    public static void AdjustBasicData(this IProject project, )
    {

    }
}
public class AdjustBasicDataOptions
{
    public required string Name { get; set; }
    public string group_name{ get; set; } = "";
    public string Object { get; set; } = "well";
    public required well_type well_type { get; set; }
    public string current_vfp { get; set; } = "";
    public string preferred_phase { get; set; } = "1*";
    public string reference_depth_mode { get; set; } = "auto";
    public int user_tvd { get; set; } = 0;
    public string inflow_equation { get; set; } = "STD";
    public string instructions { get; set; } = "SHUT";

              "preferred_phase = \"1*\", reference_depth_mode = \"auto\", user_tvd = 0, " +
              "inflow_equation = \"STD\", instructions = \"SHUT\", density_type = \"SEG\", " +
              "drainage_radius = 0, crossflow_ability = True, use_fluid_esp_heating = False, " +
              "max_deviation_angle = 5, use_segment_model = False, flow_model = False, " +
              "suppress_annular_segments = False, use_segment_params = False, " +
              "min_segment_length = 0, max_segment_length = 1000, use_thermal_parameters = False, " +
              "thickness = 0, thermal_conductivity = 0, link_segment_nodes = False, " +
              "well_head_x = 0, well_head_y = 0, well_head_z = 0, sc_pressure = 0, " +
              "sc_temperature = 0, use_concentric_tubings = False, " +
              "use_segment_graph = False, use_bottomhole_depth_unification = False
}

public enum well_type
{
    producer
}
internal static class  well_typeExtention
{
    public static string Name(this well_type well_Type) => (well_Type) switch
    {
        well_type.producer => "producer",
        _ => throw new NotImplementedException(),
    };
}
"well_designer_adjust_basic_data (name=\"Well\", " +
              "group_name = \"\", object = \"well\", well_type = \"producer\", current_vfp = \"\", " +
              "preferred_phase = \"1*\", reference_depth_mode = \"auto\", user_tvd = 0, " +
              "inflow_equation = \"STD\", instructions = \"SHUT\", density_type = \"SEG\", " +
              "drainage_radius = 0, crossflow_ability = True, use_fluid_esp_heating = False, " +
              "max_deviation_angle = 5, use_segment_model = False, flow_model = False, " +
              "suppress_annular_segments = False, use_segment_params = False, " +
              "min_segment_length = 0, max_segment_length = 1000, use_thermal_parameters = False, " +
              "thickness = 0, thermal_conductivity = 0, link_segment_nodes = False, " +
              "well_head_x = 0, well_head_y = 0, well_head_z = 0, sc_pressure = 0, " +
              "sc_temperature = 0, use_concentric_tubings = False, " +
              "use_segment_graph = False, use_bottomhole_depth_unification = False)"