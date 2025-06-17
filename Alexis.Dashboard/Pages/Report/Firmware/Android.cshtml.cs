using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.Firmware;

public class AndroidModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    const string FilterType = nameof(AuditCategory.MDM_Android);
    public string ExportFileName { get => $"{FilterType}_Firmware_{DateTime.Now.ToString("yyyyMMdd")}"; }

    public List<AndroidMDMDeviceInfoViewModel> Devices { get; set; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_FirmwareKeyAndroid);
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
        DataTable DataTable1 = AndroidMDMController.GetAllDevices_Report();
        Devices = Common.DataHelper.ConvertDataTableToList<AndroidMDMDeviceInfoViewModel>(DataTable1);
    }
}
