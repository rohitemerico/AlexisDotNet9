using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogic.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.Hardware;

public class iOSModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private static ReportingBase iOS_MDM_ReportingBase = ReportingFactory.Create("");
    public string ExportFileName { get => $"iOS_Hardware_{DateTime.Now.ToString("yyyyMMdd")}"; }
    public List<iOSDevicesByModelViewModel> DevicesByModel { get; set; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_HardwareiOS);
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
        var devices = iOS_MDM_ReportingBase.GetAllDevices_Report();
        var devicesByModel = devices.AsEnumerable()
            .GroupBy(row => row.Field<string>("MODELNAME"))
            .Select(group => new iOSDevicesByModelViewModel
            {
                ModelName = group.Key,
                DeviceCount = group.Count(),
                ActiveDevices = group.Count(d => d.Field<int>("MachineStatus") == 1),
                InactiveDevices = group.Count(d => d.Field<int>("MachineStatus") != 1),
            })
            .OrderByDescending(x => x.DeviceCount)
            .ToList();
        DevicesByModel = devicesByModel;
    }
}
