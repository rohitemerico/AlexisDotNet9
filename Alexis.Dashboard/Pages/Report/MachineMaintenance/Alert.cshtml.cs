using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.MachineMaintenance;

public class AlertModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private AlertMaintenanceController alertController = new();
    public string ExportFileName { get => $"MachineMaintenance_Alerts_{DateTime.Now:yyyyMMdd}"; }

    public List<AlertViewModel> Alerts { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_MachineMaintenanceAlert);
    }

    public IActionResult OnGet()
    {
        try
        {
            LoadData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    public IActionResult OnPost()
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

    public IActionResult OnPostClear()
    {
        return RedirectToPage();
    }

    private void LoadData()
    {
        DataTable data = alertController.getAlertList();
        Alerts = Common.DataHelper.ConvertDataTableToList<AlertViewModel>(data);
    }
}