using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.AppInstallation;

public class WindowsSummaryModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private readonly ReportController reportController = new();
    public string ExportFileName { get => $"Windows_App_Installation_Summary_{DateTime.Now:yyyyMMdd}"; }

    private DateTime startDate;
    [BindProperty]
    public DateTime StartDate
    {
        get => startDate;
        set
        {
            startDate = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
        }
    }
    private DateTime endDate;
    [BindProperty]
    public DateTime EndDate
    {
        get => endDate;
        set
        {
            endDate = value.Date.AddDays(1).AddSeconds(-1);
        }
    }
    public string? ErrorText { get; set; }
    public List<WindowsAppInstallationSummaryViewModel> AppInstallSummary { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_AppInstallationWindows);
    }
    public IActionResult OnGet()
    {
        try
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            BindDataByDateRange(StartDate, EndDate);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
    public IActionResult OnPost()
    {
        try
        {
            BindDataByDateRange(StartDate, EndDate);
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
        StartDate = DateTime.Now;
        EndDate = DateTime.Now;
        return RedirectToPage();
    }
    private void BindDataByDateRange(DateTime? startDate, DateTime? endDate)
    {
        DataTable data = new DataTable();
        if (!startDate.HasValue || !endDate.HasValue)
        {
            ErrorText = "Please enter date range.";
        }
        else if (endDate < startDate)
        {
            ErrorText = "'From' date cannot be greater than 'To' date";
        }
        else
        {
            data = reportController.GetAppInstallationSummaryWindows(StartDate, EndDate);
            AppInstallSummary = Common.DataHelper.ConvertDataTableToList<WindowsAppInstallationSummaryViewModel>(data);
        }
    }
}
