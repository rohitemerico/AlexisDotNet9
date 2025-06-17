using System.Data;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogic.Reporting;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_App;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.DeviceListing;

public class iOSModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private static UserManageBase My_FUserManageBase = UserManageFactory.Create("");
    private static MDM_AppBase My_FMDM_AppBase = MDM_AppFactory.Create("");
    private static MDM_ProfileBase My_FMDM_ProfileBase = MDM_ProfileFactory.Create("");
    private static ReportingBase iOS_MDM_ReportingBase = ReportingFactory.Create("");

    public string ActivateTab { get => "show active"; }
    public string ExportFileName { get => $"iOS_{Enum.GetName(typeof(DeviceListingTabGroup), CurrentTab) ?? ""}_{DateTime.Now.ToString("yyyyMMdd")}"; }
    public DeviceListingTabGroup CurrentTab { get; set; }
    public List<iOSMDMDeviceInfoViewModel> Devices { get; set; }
    public List<iOSMDMDeviceGroupViewModel> DeviceGroups { get; set; }
    public List<iOSMDMApplicationInfoViewModel> ApplicationInfo { get; set; }
    public List<iOSMDMProfileInfoViewModel> ProfileInfo { get; set; }


    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_DeviceListingiOS);
    }

    public IActionResult OnGet()
    {
        try
        {
            if (TempData["CurrentTab"] != null)
                CurrentTab = (DeviceListingTabGroup)TempData["CurrentTab"];
            else CurrentTab = DeviceListingTabGroup.Devices;
            BindGridData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void BindGridData()
    {
        switch (CurrentTab)
        {
            case DeviceListingTabGroup.Devices:
                DataTable DataTable1 = iOS_MDM_ReportingBase.GetAllDevices_Report();
                DataTable1.Columns.Add("CertListing", typeof(string));
                for (int i = 0; i < DataTable1.Rows.Count; i++)
                {
                    var certListing = iOS_MDM_ReportingBase.GetDeviceCert_Report(DataTable1.Rows[i]["MachineUDID"].ToString());

                    string certListing_str = string.Join("<br/><br/>",
                                certListing.Rows.OfType<DataRow>().Select(x => string.Join("/", x.ItemArray)));

                    DataTable1.Rows[i]["CertListing"] = certListing_str;
                }
                Devices = Common.DataHelper.ConvertDataTableToList<iOSMDMDeviceInfoViewModel>(DataTable1);
                break;
            case DeviceListingTabGroup.Device_Groups:
                DataTable DataTable2 = My_FUserManageBase.getActiveBranches();
                DeviceGroups = Common.DataHelper.ConvertDataTableToList<iOSMDMDeviceGroupViewModel>(DataTable2);
                break;
            case DeviceListingTabGroup.Applications:
                DataTable DataTable3 = My_FMDM_AppBase.GetMDM_APP("");
                ApplicationInfo = Common.DataHelper.ConvertDataTableToList<iOSMDMApplicationInfoViewModel>(DataTable3);
                break;
            case DeviceListingTabGroup.Profiles:
                DataTable DataTable4 = My_FMDM_ProfileBase.GetAllProfile();
                ProfileInfo = Common.DataHelper.ConvertDataTableToList<iOSMDMProfileInfoViewModel>(DataTable4);
                break;
        }
    }

    public IActionResult OnPostChangeGrid(string name)
    {
        TempData["CurrentTab"] = EnumHelper.ParseEnum<DeviceListingTabGroup>(name);
        return new JsonResult(new { message = "Success" });
    }
}


