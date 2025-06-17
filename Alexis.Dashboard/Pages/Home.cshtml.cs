using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.Android.Business.BusinessLogics.MDM;
using MDM.Win.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alexis.Dashboard.Pages;

public class HomeModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    private static MonitoringBase My_FMonitoringBase = MonitoringFactory.Create("");
    private readonly WinDeviceController mdmController = new WinDeviceController();
    public string? ClientIp { get; set; }
    public string iOS_TotalAllStatus { get; set; }
    public string iOS_TotalActive { get; set; }
    public string iOS_TotalInactive { get; set; }
    public string Andriod_TotalAllStatus { get; set; }
    public string Andriod_TotalActive { get; set; }
    public string Andriod_TotalInactive { get; set; }
    public string Windows_TotalAllStatus { get; set; }
    public string Windows_TotalActive { get; set; }
    public string Windows_TotalInactive { get; set; }
    public string HardwareError { get; set; }
    public string CardReader { get; set; }
    public string FingerPrint { get; set; }
    [BindProperty]
    public string? DashboardSelector { get; set; }
    public List<SelectListItem> DashboardOptions { get; set; }
    public List<DeviceMapMarker> Markers { get; set; }
    public List<string> MachineNames { get; set; }

    private static string TOOLTIP_TEMPLATE = @"<div class=""headerCol theme_gradient""><div class=""flag flag-{0}""><div id=""logo_alexis_v_series""></div></div></div><div class=""bodyCol""><div class=""machineName""><span class=""property"">Device Name:</span><span class=""object"">{2}</span></div><div class=""machineAddr""><span class=""property"">GeoLocation:</span><span class=""object"">{3},{4}</span></div></div><div id=""hiddenGeneralData-{1}"" class=""d-none"">{2}|{3}|{4}|{5}|{6}</div>";

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var forwardedHeader = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        ClientIp = forwardedHeader ?? HttpContext.Connection.RemoteIpAddress?.ToString();
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
    }


    public IActionResult OnGet()
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }


    private void BindData()
    {
        DashboardOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "~/Home", Text = "Main Dashboard" },
            new SelectListItem { Value = "~/Home_MOB", Text = "MOB Dashboard" },
            new SelectListItem { Value = "~/Home_MSF", Text = "MSF Dashboard" }
        };
        DashboardSelector = "~/Home";
        DataTable dt_online = My_FMonitoringBase.GetipadOnlineOfflineCount("ONLINE");
        DataTable dt_offline = My_FMonitoringBase.GetipadOnlineOfflineCount("OFFLINE");
        iOS_TotalActive = dt_online?.Rows[0]["count_"]?.ToString() ?? "0";
        iOS_TotalInactive = dt_offline?.Rows[0]["count_"]?.ToString() ?? "0";
        iOS_TotalAllStatus = (int.Parse(iOS_TotalActive) + int.Parse(iOS_TotalInactive)).ToString();

        DataTable dt_online2 = AndroidMDMController.GetAndroidOnOffCount(true);
        DataTable dt_offline2 = AndroidMDMController.GetAndroidOnOffCount(false);

        Andriod_TotalActive = dt_online2?.Rows[0]["count_"]?.ToString() ?? "0";
        Andriod_TotalInactive = dt_offline2?.Rows[0]["count_"]?.ToString() ?? "0";
        Andriod_TotalAllStatus = (int.Parse(Andriod_TotalActive) + int.Parse(Andriod_TotalInactive)).ToString();

        DataTable dt_online3 = WinDeviceController.GetWindowsOnOffCount(true);
        DataTable dt_offline3 = WinDeviceController.GetWindowsOnOffCount(false);

        Windows_TotalActive = dt_online3?.Rows[0]["count_"]?.ToString() ?? "0";
        Windows_TotalInactive = dt_offline3?.Rows[0]["count_"]?.ToString() ?? "0";
        Windows_TotalAllStatus = (int.Parse(Windows_TotalActive) + int.Parse(Windows_TotalInactive)).ToString();

        HardwareError = "0";
        CardReader = "0";
        FingerPrint = "0";

        Markers = GetAllDevices();
        MachineNames = Markers.Select(a => a.MachineName).ToList();
    }


    private List<DeviceMapMarker> GetAllDevices()
    {
        List<DeviceMapMarker> markers = new();
        DeviceMapMarker marker = new DeviceMapMarker();
        DataTable dt = My_FMonitoringBase.GetDeviceDetailForDisplay(new List<Params>());
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            marker = new DeviceMapMarker();
            marker.MapMarkerID = Marker.GenerateID().ToString();
            marker.Latitude_std = double.TryParse(dt.Rows[i]["Latitude"]?.ToString(), out var lat) ? lat : 0;
            marker.Longitude_std = double.TryParse(dt.Rows[i]["Longitude"]?.ToString(), out var lng) ? lng : 0;
            marker.MachineName = dt.Rows[i]["MachineName"]?.ToString();
            marker.MachineStatus = dt.Rows[i]["MachineStatus"]?.ToString();
            marker.MachineImei = dt.Rows[i]["MachineImei"]?.ToString();
            marker.MachineSerial = dt.Rows[i]["MachineSerial"]?.ToString();
            marker.BuildVersion = dt.Rows[i]["BuildVersion"]?.ToString();
            marker.OSVersion = dt.Rows[i]["OSVersion"]?.ToString();
            marker.AvailableSpace = dt.Rows[i]["AvailableDevice_Capacity"]?.ToString() + " / " + dt.Rows[i]["DeviceCapacity"]?.ToString();
            marker.DeviceType = nameof(OSType.iOS);
            marker.CustomShape = "ios-custom-shape";

            var TOOLTIP_TEMPLATE_iOS = TOOLTIP_TEMPLATE + @"<div id=""iOS-Data-{1}"" class=""d-none"">{7}|{8}|{9}|{10}|{11}</div>";
            marker.Content = String.Format(TOOLTIP_TEMPLATE_iOS, "uniqueFlag", marker.MapMarkerID, marker.MachineName, marker.Latitude_std, marker.Longitude_std, marker.MachineStatus, marker.DeviceType, marker.MachineImei, marker.MachineSerial, marker.BuildVersion, marker.OSVersion, marker.AvailableSpace);
            markers.Add(marker);
        }
        dt = AndroidMDMController.GetAllDevices();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            marker = new DeviceMapMarker();
            marker.MapMarkerID = Marker.GenerateID().ToString();
            marker.Latitude_std = double.TryParse(dt.Rows[i]["Latitude"]?.ToString(), out var lat) ? lat : 0;
            marker.Longitude_std = double.TryParse(dt.Rows[i]["Longitude"]?.ToString(), out var lng) ? lng : 0;
            marker.MachineName = dt.Rows[i]["DeviceName"]?.ToString();
            marker.MachineStatus = dt.Rows[i]["DeviceStatus"]?.ToString() == "True" ? "1" : "0";
            marker.DeviceType = nameof(OSType.Android);
            marker.CustomShape = "android-custom-shape";
            marker.Content = String.Format(TOOLTIP_TEMPLATE, "uniqueFlag", marker.MapMarkerID, marker.MachineName, marker.Latitude_std, marker.Longitude_std, marker.MachineStatus, marker.DeviceType);
            markers.Add(marker);
        }

        DataTable data = mdmController.GetAllClientDevices();
        for (int i = 0; i < data.Rows.Count; i++)
        {
            marker = new DeviceMapMarker();
            marker.MapMarkerID = Marker.GenerateID().ToString();
            marker.Latitude_std = double.TryParse(data.Rows[i]["Latitude"]?.ToString(), out var lat) ? lat : 0;
            marker.Longitude_std = double.TryParse(data.Rows[i]["Longitude"]?.ToString(), out var lng) ? lng : 0;
            marker.MachineName = data.Rows[i]["Name"]?.ToString();
            marker.MachineStatus = data.Rows[i]["IsOnline"]?.ToString() == "True" ? "1" : "0";
            marker.DeviceType = nameof(OSType.Windows);
            marker.CustomShape = "windows-custom-shape";
            marker.Content = String.Format(TOOLTIP_TEMPLATE, "uniqueFlag", marker.MapMarkerID, marker.MachineName, marker.Latitude_std, marker.Longitude_std, marker.MachineStatus, marker.DeviceType);
            markers.Add(marker);
        }

        return markers;
    }

}


public class DeviceMapMarker
{
    public string MapMarkerID { get; set; }
    public string MachineName { get; set; }
    public string MachineStatus { get; set; }
    public string MachineImei { get; set; }
    public string MachineSerial { get; set; }
    public string BuildVersion { get; set; }
    public string OSVersion { get; set; }
    public string AvailableSpace { get; set; }
    public string DeviceType { get; set; }
    public double Latitude_std { get; set; }
    public double Longitude_std { get; set; }
    public string CustomShape { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}

public class Marker
{
    public string marker_style;
    public Marker(OSType os)
    {
        switch (os)
        {
            case OSType.iOS:
                marker_style = "ios-custom-shape";
                break;
            case OSType.Android:
                marker_style = "android-custom-shape";
                break;
            case OSType.Windows:
                marker_style = "windows-custom-shape";
                break;
            default:
                marker_style = "my-custom-shape";
                break;
        }
    }

    public string CustomShape { get => marker_style; }
    //target css class [.k-marker.k-marker-my-custom-shape]
    //Details at https://docs.telerik.com/devtools/aspnet-ajax/controls/map/appearance-and-styling/customizing-markers-in-radmap"

    public static int idCounter;
    public static int GenerateID() => ++idCounter; //auto increment marker ID 
}

public enum OSType
{
    iOS,
    Windows,
    Android,
    Default
}