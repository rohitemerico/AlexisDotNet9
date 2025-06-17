using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alexis.Dashboard.Pages;

public class Home_MSFModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private MSFController _msf = new MSFController();
    public string? ClientIp { get; set; }
    public string Json_AllSummary { get; set; }
    public string Json_TransType { get; set; }

    [BindProperty]
    public string? DashboardSelector { get; set; }
    public List<SelectListItem> DashboardOptions { get; set; }

    [BindProperty]
    public string? DatePeriod { get; set; }
    public List<SelectListItem> DatePeriodOptions { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var forwardedHeader = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        ClientIp = forwardedHeader ?? HttpContext.Connection.RemoteIpAddress?.ToString();
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
    }

    public IActionResult OnGet()
    {
        try
        {
            BindData();
            DatePeriod = "1";
            GetData();
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
            BindData();
            GetData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void BindData()
    {
        DashboardOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "~/Home", Text = "Main Dashboard" },
            new SelectListItem { Value = "~/Home_MOB", Text = "MOB Dashboard" },
            new SelectListItem { Value = "~/Home_MSF", Text = "MSF Dashboard" }
        };
        DashboardSelector = "~/Home_MSF";
        DatePeriodOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Today" },
            new SelectListItem { Value = "7", Text = "Last 7 Days" },
            new SelectListItem { Value = "30", Text = "Last 30 Days" },
            new SelectListItem { Value = "60", Text = "Last 60 Days" },
            new SelectListItem { Value = "90", Text = "Last 90 Days" }
        };
    }

    private void GetData()
    {
        Json_AllSummary = _msf.GetAllTransSummaryByDays(int.Parse(DatePeriod));
        Json_TransType = _msf.GetTransTypeSummaryByDays(int.Parse(DatePeriod));
    }
}
