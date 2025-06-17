using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report;

public class AuditTrail_AndroidModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public string ExportFileName { get => $"AuditTrail_Android_{DateTime.Now.ToString("yyyyMMdd")}"; }

    const string FilterType = nameof(AuditCategory.MDM_Android);
    public Guid ModuleId { get; set; }

    [BindProperty]
    public int FilterPassBtn { get; set; } = 2;
    public string SearchText { get; set; }

    private DateTime minDate;
    [BindProperty]
    public DateTime MinDate
    {
        get => minDate;
        set
        {
            minDate = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
        }
    }

    private DateTime maxDate;
    [BindProperty]
    public DateTime MaxDate
    {
        get => maxDate;
        set
        {
            maxDate = value.Date.AddDays(1).AddSeconds(-1);
        }
    }

    public List<AuditRecordViewModel> AuditRecords { get; set; }

    public string ErrorText { get; private set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_AuditTrailAndroid);
    }

    public IActionResult OnGet()
    {
        try
        {
            MinDate = DateTime.Now;
            MaxDate = DateTime.Now;
            DataTable data = BindDataByDateRange(MinDate, MaxDate);
            AuditRecords = Common.DataHelper.ConvertDataTableToList<AuditRecordViewModel>(data);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        try
        {
            var filter = FilterPassBtn;
            DataTable data = BindDataByDateRange(MinDate, MaxDate);
            AuditRecords = Common.DataHelper.ConvertDataTableToList<AuditRecordViewModel>(data);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    public IActionResult OnPostClear()
    {
        MinDate = DateTime.Now;
        MaxDate = DateTime.Now;
        return RedirectToPage();
    }

    private DataTable BindDataByDateRange(DateTime? minDate, DateTime? maxDate)
    {
        DataTable ret = new DataTable();

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
            ret = AuditLogController.getBindGrid(minDate.Value, maxDate.Value, Guid.Empty, Guid.Empty);

            if (ret.Rows.Count != 0 || ret != null)
            {
                var filtered = ret.Rows.Cast<DataRow>().Where(r => r.Field<String>("description").ToString().ToLower().Contains(FilterType.ToLower())).ToList();
                if (filtered.Count() != 0)
                    ret = filtered.CopyToDataTable();
                else
                    ret = ret.Clone(); //empty rows

                if (FilterPassBtn != 2)
                {
                    var filtered1 = ret.Rows.Cast<DataRow>().Where(r => r.Field<Decimal>("status") == FilterPassBtn).ToList();
                    if (filtered1.Count() != 0)
                        ret = filtered1.CopyToDataTable();
                    else
                        ret = ret.Clone(); //empty rows
                }
            }
        }
        return ret;
    }
}
