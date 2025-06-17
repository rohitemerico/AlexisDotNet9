using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMiOS;

public class ApplicationManagementModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    public static MDM_AppBase My_FMDM_AppBase = MDM_AppFactory.Create("");

    public List<iOSMDMApplicationViewModel> Applications { get; set; }

    public string? ClientIp { get; set; }


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
        checkAuthorization(ModuleLogAction.View_iOSMDMApplicationManagement);
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
        DataTable data = My_FMDM_AppBase.GetMDM_APP("");
        Applications = Common.DataHelper.ConvertDataTableToList<iOSMDMApplicationViewModel>(data);
    }
}
