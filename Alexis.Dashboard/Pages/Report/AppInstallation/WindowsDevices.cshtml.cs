using System.Data;
using System.Reflection;
using Alexis.Dashboard.Models;
using Alexis.Dashboard.Pages.MDMWin;
using Dashboard.Common.Business.Component;
using MDM.Win.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.AppInstallation;

public class WindowsDevicesModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public string ExportFileName { get => $"Windows_Devices_{DateTime.Now:yyyyMMdd}"; }
    private readonly WinDeviceController mdmController = new WinDeviceController();
    public List<WindowsDeviceInfoViewModel> Devices { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_AppDevicesiOS);
    }

    public IActionResult OnGet()
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod()?.DeclaringType?.Name, MethodBase.GetCurrentMethod()?.Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }


    private void BindData()
    {
        DataTable data = mdmController.GetAllClientDevices();
        Devices = Common.DataHelper.ConvertDataTableToList<WindowsDeviceInfoViewModel>(data);
    }
}