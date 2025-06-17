using System.Data;
using System.Reflection;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.AppInstallation;

public class AndroidDevicesModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public string ExportFileName { get => $"Android_Devices_{DateTime.Now:yyyyMMdd}"; }

    public List<AndroidMDMDeviceInfoViewModel> Devices { get; set; }
    public string? ErrorText { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_AppDevicesAndroid);
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
        DataTable DataTable1 = AndroidMDMController.GetAllDevices_Report();
        Devices = Common.DataHelper.ConvertDataTableToList<AndroidMDMDeviceInfoViewModel>(DataTable1);
    }
}