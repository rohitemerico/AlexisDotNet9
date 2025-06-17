using System.Data;
using Alexis.Dashboard.Models;
using Alexis.Dashboard.Pages.MDMWin;
using Dashboard.Common.Business.Component;
using MDM.Win.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.Firmware;

public class WindowsModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private readonly WinDeviceController mdmController = new WinDeviceController();
    const string FilterType = nameof(AuditCategory.MDM_Windows);
    public string ExportFileName { get => $"{FilterType}_Firmware_{DateTime.Now.ToString("yyyyMMdd")}"; }

    public List<WindowsDeviceInfoViewModel> Devices { get; set; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_FirmwareKeyWindows);
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
        DataTable DataTable1 = mdmController.GetAllClientDevices();
        Devices = Common.DataHelper.ConvertDataTableToList<WindowsDeviceInfoViewModel>(DataTable1);
    }
}