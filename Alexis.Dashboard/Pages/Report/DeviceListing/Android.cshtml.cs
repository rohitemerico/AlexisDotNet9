using System.Data;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.DeviceListing;

public class AndroidModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    public string ActivateTab { get => "show active"; }
    public string ExportFileName { get => $"Android_{Enum.GetName(typeof(DeviceListingTabGroup), CurrentTab) ?? ""}_{DateTime.Now.ToString("yyyyMMdd")}"; }

    public DeviceListingTabGroup CurrentTab { get; set; }
    public List<AndroidMDMDeviceInfoViewModel> Devices { get; set; }
    public List<AndroidMDMDeviceGroupViewModel> DeviceGroups { get; set; }
    public List<AndroidMDMApplicationInfoViewModel> ApplicationInfo { get; set; }
    public List<AndroidMDMProfileInfoViewModel> ProfileInfo { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_DeviceListingAndroid);
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
                DataTable DataTable1 = AndroidMDMController.GetAllDevices_Report();
                Devices = Common.DataHelper.ConvertDataTableToList<AndroidMDMDeviceInfoViewModel>(DataTable1);
                break;
            case DeviceListingTabGroup.Device_Groups:
                DataTable DataTable2 = AndroidMDMController.GetAllDeviceGroups_Report();
                DeviceGroups = Common.DataHelper.ConvertDataTableToList<AndroidMDMDeviceGroupViewModel>(DataTable2);
                break;
            case DeviceListingTabGroup.Applications:
                DataTable DataTable3 = AndroidMDMController.GetAllApp_Report();
                ApplicationInfo = Common.DataHelper.ConvertDataTableToList<AndroidMDMApplicationInfoViewModel>(DataTable3);
                break;
            case DeviceListingTabGroup.Profiles:
                DataTable DataTable4 = AndroidMDMController.GetAllProfiles_Report();
                ProfileInfo = Common.DataHelper.ConvertDataTableToList<AndroidMDMProfileInfoViewModel>(DataTable4);
                break;
        }
    }

    public IActionResult OnPostChangeGrid(string name)
    {
        TempData["CurrentTab"] = EnumHelper.ParseEnum<DeviceListingTabGroup>(name);
        return new JsonResult(new { message = "Success" });
    }
}