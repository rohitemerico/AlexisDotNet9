using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages;

public class IndexModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public string? ClientIp { get; set; }

    public string iOS_TotalAllStatus { get; set; }
    public string iOS_TotalActive { get; set; }
    public string Andriod_TotalAllStatus { get; set; }
    public string Andriod_TotalActive { get; set; }
    public string Windows_TotalAllStatus { get; set; }
    public string Windows_TotalActive { get; set; }


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

        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
}
