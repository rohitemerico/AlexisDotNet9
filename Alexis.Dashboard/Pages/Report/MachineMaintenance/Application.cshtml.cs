using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.MachineMaintenance;

public class ApplicationModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public List<ApplicationMaintenanceViewModel> Applications { get; set; }
    public string ExportFileName { get => $"MachineMaintenance_Application_{DateTime.Now.ToString("yyyyMMdd")}"; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_MachineMaintenanceApplication);
    }

    public IActionResult OnGet()
    {
        try
        {
            LoadData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void LoadData()
    {
        DataTable data = KioskVersionMaintenance.bindTbl().DefaultView.ToTable(true, "SysID", "FPATH", "COUNTDL", "VER", "FILESIZE", "TYPE", "CREATEDDATETIME", "pilot", "fStatus");
        Applications = Common.DataHelper.ConvertDataTableToList<ApplicationMaintenanceViewModel>(data);
    }
}
