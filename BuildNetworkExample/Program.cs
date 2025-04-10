#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
using Microsoft.Data.Analysis;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using tNav.API;
using tNav.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;
using tnav = tNav.API;
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.

Console.Write("Путь к консоли tNavigator (tNavigator-con)");
string tNpath = Console.ReadLine() ?? "";

if (!Path.Exists(tNpath))
{
    Console.WriteLine($"Console tNavigator not found at {Path.GetFullPath(tNpath)}!");
    Environment.Exit(1);
}

if (!Path.Exists("Init_Data"))
{
    Console.WriteLine($"Init_Data folder not found!");
    Environment.Exit(1);
}

List<string> xls_list = ["Init_Data/WD_data.xlsx", "Init_Data/ND_data.xlsx"];

var df_data = new Dictionary<string, DataFrame>();
foreach (var xls in xls_list)
{
    df_data.update(pd.read_excel(xls, engine: "openpyxl", sheet_name: null, skiprows: 1, keep_default_na: false));
}
df_data["VFP Correlation Plotting Points"] = pd.read_excel("Init_Data/WD_data.xlsx", engine: "openpyxl", sheet_name: "VFP Correlation Plotting Points", skiprows: 1);

List<string> quoted_names = ["name", "perforation_status", "poro_system", "status",
               "type", "type_out", "name_out", "type_in", "name_in", "object",
               "choke_control_type", "critical_corr", "subcritical_corr",
               "corr_type", "rate_type", "data_type", "compressor_route"];

List<string> datetime_names = ["time_step", "event_date"];

List<DateTime> timestamps = [];
foreach (var df in df_data.values())
{
    foreach (var i in datetime_names)
    {
        if (df.columns.Contains(i))
        {
            timestamps.AddRange(df[i].Distinct());
        }
        timestamps = timestamps.OrderBy(t => t).ToList();
    }
}

string fmt = "";
if (Environment.OSVersion.Platform == PlatformID.Unix)
    fmt = "-";
else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
    fmt = "#";
else
{
    Console.WriteLine($"Unknown operating system: {Environment.OSVersion.Platform}");
    Environment.Exit(1);
}

string time_format = $"year=%Y,month=%{fmt}m, day=%{fmt}d";

string obj(sheet)
{
    List<string> line = [];
    for (int i = 0; i < range(sheet.shape[0]); i++)
    {
        List<string> token = [];
        foreach (var col in sheet.columns)
        {
            if (quoted_names.Contains(col))
                token.Add($" \"{col}\" : \"{sheet.iloc[i][col]}\" ");
            else if (datetime_names.Contains(col))
                token.Add($" \"{col}\" : datetime ({sheet.iloc[i][col].strftime(time_format)}) ");
            else
                token.Add($" \"{col}\" : {sheet.iloc[i][col]} ");
        }
        line.Add("{" + "," + string.Join(null, token) + "}");
    }
    var obj = " , " + string.Join(line);
    return obj;
}

Console.WriteLine("Running script");
Console.Write("Creating and opening snp project...");

var conn = ConnectionFactory.GetConnection(path_to_exe: tNpath, license_wait_time_limit__secs: 30);

var snp_new = conn.CreateProject(path: "SNP/API_BuildND.snp", case_type: tnav.CaseType.MD, project_type: tnav.ProjectType.MD);
snp_new.CloseProject();
var MD_proj = conn.OpenProject(path: "SNP/API_BuildND.snp", save_on_close: true);
Console.WriteLine("Done");

Console.Write("Requesting licenses...");
MD_proj.RunPyCode(code: "request_license_features (requested_features=[" +
    "{\"feature\" : \"FEAT_MODEL_DESIGNER\"}, " +
    "{\"feature\" : \"FEAT_NETWORK_DESIGNER\"}, " +
    "{\"feature\" : \"FEAT_WELL_DESIGNER\"}, " +
    "{\"feature\" : \"FEAT_PVT_DESIGNER\"}])");
Console.WriteLine("Done");

Console.Write("Importing BO variant...");
MD_proj.RunPyCode(code: "pvt_import_e1_format (file_name=\"../Init_Data/Blackoil.inc\", " +
    "region_count = 1, " +
    "units = \"METRIC\", " +
    "clear_tables = True)");
Console.WriteLine("Done");

Console.Write("Creating Network Designer (ND) and Well Designer(WD) subprojects...");
MD_proj.RunPyCode(code: "project_manager_create_project (projects_table=[ " +
    "{ \"project_type\" : \"vfp_project\", \"project_name\" : \"Well_Project\"}, " +
    "{ \"project_type\" : \"nd_project\", \"project_name\" : \"standalone_network\"}])");
Console.WriteLine("Done");

Console.Write("Running WD calculations...");
var WD_proj = MD_proj.GetSubProjectByName(type: ProjectType.WD, name: "Well_Project");
WD_proj.RunPyCode(code: "well_designer_adjust_basic_data (name=\"Well\", " +
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
      "use_segment_graph = False, use_bottomhole_depth_unification = False)");

WD_proj.RunPyCode(code: "wd_trajectories_import (imported_object=\"well\", " +
      "format = \"Well Path / Deviation Text\", file_names = [\".. / .. / .. / .. / .. / .. / Init_Data / Well.dev\"], " +
      "input_data_type = \"wid_md_x_y_z\", las_header_1 = \"\", las_header_2 = \"\", las_header_3 = \"\", " +
      "las_header_4 = \"\", method = \"tangent\", units_system_xy = \"METRIC\", units_system_z = \"METRIC\", " +
      "use_oem_encoding = False, add_md_zero_point = False, invert_z = True, use_keywords = True, " +
      "txt_table_format = TableFormat(separator = \"all spaces\", comment = \"#\", " +
      "skip_lines = 1, columns = [\"md\", \"x\", \"y\", \"z\"]), gwtd_table_format = TableFormat(separator = \"all spaces\", " +
      "comment = \"#\", skip_lines = 0, columns = [\"md\", \"x\", \"y\", \"z\"]), " +
      "vert_well_table_format = TableFormat(separator = \"all spaces\", comment = \"\", skip_lines = 1, " +
      "columns = [\"well\", \"x\", \"y\", \"kb\", \"last_point_md\", \"last_point_tvdss\", \"well_code\"]), " +
      "well_name = None, wellbore_name = None, dst_branch_num = 0)");

WD_proj.RunPyCode(code: $"well_designer_object_casing (branch_num=0, objects_table=[{obj(df_data["Casing"])}])");
WD_proj.RunPyCode(code: $"well_designer_object_tubing (branch_num=0, objects_table=[{obj(df_data["Tubing"])}])");
WD_proj.RunPyCode(code: $"well_designer_object_perforation (branch_num=0, objects_table=[{obj(df_data["Perforation"])}])");
WD_proj.RunPyCode(code: $"well_designer_object_packer (branch_num=0, objects_table=[{obj(df_data["Packer"])}])");
WD_proj.RunPyCode(code: $"well_designer_object_bottom_hole (branch_num=0, objects_table=[{obj(df_data["Bottomhole"])}])");
WD_proj.RunPyCode(code: $"well_designer_object_pressure_gauge (branch_num=0, objects_table=[{obj(df_data["Pressure Gauge"])}])");

WD_proj.RunPyCode(code: """
vfp_table_create_select_pvt (table=[
    {"vfp_table" : "VFP1", "pvt_project" : "PVT Data", "variant_type" : "blackoil",
    "variant_name" : "Blackoil.inc 1", "compos_name" : ""}])

vfp_table_adjust_correlation_parameters (table=[ 
    {"vfp_table" : "VFP1", "vertical_deviated_swap_angle" : 5, "horizontal_deviated_swap_angle" : 5,
    "single_phase_corr" : "moody", "liq_vap_flow" : 0.001, "use_tubing_correlations" : True,
    "vertical_corr" : "Hagedorn-Brown", "deviated_corr" : "Beggs-Brill", "horizontal_corr" : "Beggs-Brill",
    "frict_tubing" : 1, "hydro_tubing" : 1, "use_annulus_correlations" : False,
    "use_same_as_tubing_correlations" : False, "vertical_annulus_corr" : "Hagedorn-Brown",
    "deviated_annulus_corr" : "Beggs-Brill", "horizontal_annulus_corr" : "Beggs-Brill","frict_annulus" : 1,
    "hydro_annulus" : 1, "use_acceleration_component" : False}])
""");

var src = df_data["VFP Correlation Plotting Points"];
List<string> vfp_points = src.columns.Select(col => "[" + string.Join(", ", src.loc[src[col].notnull()][col].ToList()) + "]");
WD_proj.RunPyCode(code: $"""
vfp_adjust_correlation_plotting_points (table_name="VFP1", 
  thp ={vfp_points[0]}, 
  flo_type = "OIL", 
  flo ={vfp_points[1]}, 
  wfr_type = "WCT", 
  wfr ={vfp_points[2]}, 
  gfr_type = "GOR", 
  gfr ={vfp_points[3]}, 
  alq_type = "GRAT", 
  alq ={vfp_points[4]})
""");

WD_proj.RunPyCode(code: "wd_create_ipr_curve (ipr=\"IPR1\", ignore_if_exists=True)");

WD_proj.RunPyCode(code: $"""
wd_adjust_ipr_well_test_data (ipr="IPR1", \
  use_date = False, \
  date = datetime({pd.to_datetime(timestamps[0]).strftime(time_format)}), \
  change_ipr_base = True, ipr_base = "gas", change_model = True, use_well_test_data = True, \
  well_test_data_type = "multipoint", \
  well_test_data = [{obj(df_data["IPR Well Test Data"])}])
""");
Console.WriteLine("Done");

Console.Write("Running ND project calculations...");
var ND_proj = MD_proj.GetSubProjectByName(type: ProjectType.ND, name: "standalone_network");

ND_proj.RunPyCode(code: """
nd_settings_solver_parameters (temperature_options_widget=True, 
  use_temperature_equation = True, use_heat_balance_equation = True, 
  use_iterative_method = True, initial_approximation_options_widget = True, 
  use_initial_approximation = False, use_directed_graph_for_initial_rate_approximation = True, 
  use_constraints_for_initial_rate_approximation = False, 
  wells_uterations_chop_settings_widget = True, wells_chop_coefficient = 0.5, 
  use_limit_chop_well_solution = False, chop_well_solution_max_count = 50, 
  newton_iterations_widget = True, use_newton_step_mult = False, 
  newt_step_mult_start_iteration = 2, verification_widget = True, 
  enable_verification = True, linear_solver_settings_widget = True, 
  solver_type = "iterative", newton_iter = 50, solver_max_it = 400, 
  tolerance_widget = True, newton_rhs_tol = 0.001, newton_diff_tol = 1e-8, 
  double_newton_relaxation = 1, use_separate_tol_for_object_pressure_rhs = False, 
  object_pressure_equations_tol = 0.001, differentiation_widget = True, 
  enable_automatic_differentiation = False)
""");

foreach (var t in timestamps)
{
    ND_proj.RunPyCode(code: $"""
nd_timestep_add ( \
  first_date = datetime({pd.to_datetime(t).strftime(time_format)}), \
  step_length = "Single Step", custom_step_length = 1, custom_step_type = "Second")
""");
}

var nd_obj = obj(df_data["Objects List"][["type", "'name"]]);

ND_proj.RunPyCode(code: $"nd_object_create (objects=[{nd_obj}])");
ND_proj.RunPyCode(code: $"nd_objects_adjust_3d_coordinates (adjust_on_scheme=False, coordinates_table=[{obj(df_data["Objects List"])}])");
ND_proj.RunPyCode(code: $"nd_object_create_link (skip_incompatible_object_linking=False, objects=[{obj(df_data["Create Link"])}])");
ND_proj.RunPyCode(code: $"nd_object_create_pipe (skip_incompatible_object_linking=False, objects=[{obj(df_data["Create Pipe"])}])");
ND_proj.RunPyCode(code: $"d_set_coordinates_from_map ()");
ND_proj.RunPyCode(code: $"nd_objects_adjust_choke (create_objects=True, events_table=[{obj(df_data["Chokes"])}])");
ND_proj.RunPyCode(code: $"nd_objects_adjust_pipe (events_table=[{obj(df_data["Pipes"])}])");

src = df_data["Adjust Pipe Geometry"];
for (int n = 0; n < range(src.shape[0]); i++)
    ND_proj.RunPyCode(code: $"""
nd_object_adjust_pipe_geometry_simple ( 
  event_date = datetime({src.iloc[n, 0].strftime(time_format)}), 
  object= find_nd_object(name = "{src.iloc[n, 1]}", 
  type = "{src.iloc[n, 2]}"), 
  length={src.iloc[n, 3]}, 
  height_diff ={src.iloc[n, 4]})
""");

ND_proj.RunPyCode(code: $"nd_objects_adjust_source (create_objects=True, events_table=[{obj(df_data["Source"])}])");
ND_proj.RunPyCode(code: $"nd_objects_adjust_sink (create_objects=True, events_table=[{obj(df_data["Sinks"])}])");
ND_proj.RunPyCode(code: $"nd_objects_adjust_well (create_objects=True, events_table=[{obj(df_data["Wells"])}])");

src = df_data["Adjust Rates"];
for (int n = 0; n < range(src.shape[0]); i++)
    ND_proj.RunPyCode(code: $"""
nd_object_adjust_surface_volume_rate (object=find_nd_object (name="{src.iloc[n, 0]}", 
      type = "{src.iloc[n, 1]}"), 
      hydrocarbon_param = "{src.iloc[n, 2]}", 
      hydrocarbon_value ={src.iloc[n, 3]}, 
      water_param = "{src.iloc[n, 4]}", 
      water_value ={src.iloc[n, 5]}, 
      event_date = datetime({src.iloc[n, 6].strftime(time_format)}))
""");

ND_proj.RunPyCode(code: $"nd_objects_adjust_pump (create_objects=True, events_table=[{obj(df_data["Pump"])}])");
ND_proj.RunPyCode(code: $"nd_objects_adjust_compressor (create_objects=True, events_table=[{obj(df_data["Compressor"])}])");
ND_proj.RunPyCode(code: $"nd_objects_adjust_three_phase_separator (create_objects=True, events_table=[{obj(df_data["Separator"])}])");
ND_proj.RunPyCode(code: $"""
nd_object_change_three_phase_separation_objects (object=find_nd_object (name="3-phase Separator", 
  type = "three-phase separator"), 
  gas_separation_obj = "Comp_line", 
  water_separation_obj = "Pump_line")
""");

ND_proj.RunPyCode(code: $"""
nd_select_pvt_for_nd (use_pvt_variant=True, 
  pvt_project = "PVT Data", 
  variant_type = "blackoil", 
  variant_name = "Blackoil.inc 1")
""");

src = df_data["Wells"];
for (int n = 0; n < range(src.shape[0]); i++)
{
    ND_proj.RunPyCode(code: $"""
 nd_object_select_vfp_table ( 
   object = find_nd_object(name = "{src.iloc[n, 0]}", type = "well"), 
   use_vfp = True, 
   well_project = "Well_Project", 
   vfp = "VFP1")
 """);
    ND_proj.RunPyCode(code: $"""
nd_object_select_ipr_table ( \
  object = find_nd_object(name = "{src.iloc[n, 0]}", type = "well"), \
  use_ipr = True, \
  well_project = "Well_Project", \
  ipr = "IPR1")'
""");
}

Console.WriteLine("Done");

Console.Write("Calculating surface network...");
ND_proj.RunPyCode(code: "run_network_model_calculations (result=\"Result\", replace_if_exists=True)");
Console.WriteLine("Done");

Console.Write("Creating dataframe with result...");
var df_nd_results = ND_proj.RunPyCode(code: $"""
from datetime import datetime
import pandas as pd
date = datetime({pd.to_datetime(timestamps[1]).strftime(time_format)})
pipes = get_objects_by_type(type='pipe')
available_results = pipes[0].get_available_extended_results()
pipeline_results = pd.DataFrame()
for p in pipes:
    print(p.name())
    results = p.get_extended_result_values(result_name='Result', type='pipe_segment', parameter_names=available_results, date=date)
    results['pipe_name'] = p.name()
    pipeline_results = pd.concat([pipeline_results, results], ignore_index=True)
pipeline_results.columns = pipeline_results.columns.str.replace(' ', '_')
return pipeline_results
""");
Console.WriteLine("Done");

Console.Write("Creating results folder...");
var new_folder = "Result_Tables";
if (!Directory.Exists(new_folder))
    Directory.CreateDirectory(new_folder);
else
    Console.WriteLine($"The folder with the `{new_folder}` name already exists!");
Console.WriteLine("Done");

Console.Write("Saving to file...");
var table = df_nd_results as DataTable;
var csv = Utils.DataTableToCSV(table);
File.WriteAllText("Result_Tables/pipes_table_results.csv", csv);
Console.WriteLine("Done");

Console.Write("Closing project...");
MD_proj.RunPyCode(code: "save_project ()");
MD_proj.CloseProject();
Console.WriteLine("Done");

Console.WriteLine("Surface network is successfully calculated. The script has been finished");