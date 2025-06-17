using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class ApplicationMaintenanceModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public Guid ModuleId { get; set; }

    public List<ApplicationMaintenanceViewModel> Applications { get; set; }
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
        checkAuthorization(ModuleLogAction.View_Application);
    }

    public IActionResult OnGet()
    {
        try
        {
            BindGridData();
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

    private void BindGridData()
    {
        DataTable data = KioskVersionMaintenance.bindTbl().DefaultView.ToTable(true, "SysID", "FPATH", "COUNTDL", "VER", "FILESIZE", "TYPE", "CREATEDDATETIME", "pilot", "fStatus");
        Applications = Common.DataHelper.ConvertDataTableToList<ApplicationMaintenanceViewModel>(data);
    }
}