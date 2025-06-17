using System.Data;
using Alexis.Dashboard.Models;
using Alexis.Dashboard.Pages.MDMWin;
using Dashboard.Common.Business.Component;
using MDM.Win.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.DeviceListing;

public class WindowsModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private readonly WinDeviceController mdmController = new WinDeviceController();
    public string ExportFileName { get => $"Windows_Devices_{DateTime.Now.ToString("yyyyMMdd")}"; }

    public List<WindowsDeviceInfoViewModel> Devices { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_DeviceListingWindows);
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
        DataTable data = mdmController.GetAllClientDevices();
        Devices = Common.DataHelper.ConvertDataTableToList<WindowsDeviceInfoViewModel>(data);
    }
}
