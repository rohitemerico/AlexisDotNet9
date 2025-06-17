using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.Hardware;

public class AndroidModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public List<AndroidDevicesByModelViewModel> DevicesByModel { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_HardwareAndroid);
    }

    public IActionResult OnGet()
    {
        try
        {
            BindGridData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void BindGridData()
    {
        var devices = AndroidMDMController.GetAllDevices_Report();
        var devicesByModel = devices.AsEnumerable()
            .GroupBy(row => row.Field<string>("MODELNAME"))
            .Select(group => new AndroidDevicesByModelViewModel
            {
                ModelName = group.Key,
                DeviceCount = group.Count(),
                ActiveDevices = group.Count(d => d.Field<bool>("DEVICESTATUS") == true),
                InactiveDevices = group.Count(d => d.Field<bool>("DEVICESTATUS") != true),
            })
            .OrderByDescending(x => x.DeviceCount)
            .ToList();
        DevicesByModel = devicesByModel;
    }
}