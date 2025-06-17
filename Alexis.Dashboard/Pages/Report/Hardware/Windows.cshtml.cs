using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Win.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.Hardware;

public class WindowsModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private readonly WinDeviceController mdmController = new WinDeviceController();

    public List<WindowsDevicesByModelViewModel> DevicesByModel { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_HardwareWindows);
    }

    public IActionResult OnGet()
    {
        try
        {
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
        var devices = mdmController.GetAllClientDevices();
        var devicesByModel = devices.AsEnumerable()
            .GroupBy(row => row.Field<string>("OEM"))
            .Select(group => new WindowsDevicesByModelViewModel
            {
                OEM = group.Key,
                DeviceCount = group.Count(),
                ActiveDevices = group.Count(d => d.Field<bool>("IsOnline") == true),
                InactiveDevices = group.Count(d => d.Field<bool>("IsOnline") != true),
            })
            .OrderByDescending(x => x.DeviceCount)
            .ToList();
        DevicesByModel = devicesByModel;
    }
}
