using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.VoiceVideoCall;

public class SummaryModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private ReportingVoiceVideoCallController _reportController = new();

    public string ExportFileName { get => $"Voice_Video_Call_Transactions_Summary_{DateTime.Now.ToString("yyyyMMdd")}"; }
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
    public string ErrorText { get; set; }
    public List<VoiceVideoCallTransactionViewModel> CallTransactions { get; set; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_VoiceVideoCallSummary);
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
    private void BindDataByDateRange(DateTime? minDate, DateTime? maxDate)
    {
        DataTable data = new DataTable();
        if (!minDate.HasValue || !maxDate.HasValue)
        {
            ErrorText = "Please enter date range.";
        }
        else if (maxDate < minDate)
        {
            ErrorText = "'From' date cannot be greater than 'To' date";
        }
        else
        {
            data = _reportController.VoiceVideoCallTransactionsSummary(StartDate, EndDate);
            CallTransactions = Common.DataHelper.ConvertDataTableToList<VoiceVideoCallTransactionViewModel>(data);
        }
    }
}