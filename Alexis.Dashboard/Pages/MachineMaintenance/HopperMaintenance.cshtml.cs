using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class HopperMaintenanceModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private HopperMaintenanceController hopperControl = new HopperMaintenanceController();
    public List<HopperMaintenanceViewModel> HopperMaintenanceList { get; set; }
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
        checkAuthorization(ModuleLogAction.View_Hopper_Template);
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
        DataTable data = hopperControl.getHopperList();
        HopperMaintenanceList = Common.DataHelper.ConvertDataTableToList<HopperMaintenanceViewModel>(data);
        foreach (var item in HopperMaintenanceList)
        {
            if (!Maker || userControl.IsEditingDataExist(Guid.Parse(item.hID)) || (item.hStatus == "Pending"))
            {
                item.Visible = false;
            }
        }
    }

    public IActionResult OnPostEdit(string id)
    {
        if (Guid.Parse(id) == Guid.Empty)
        {
            return new JsonResult(new { message = "Invalid ID." });
        }
        HttpContext.Session.SetString("EditHopperID", id);
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnGetAdd()
    {
        HttpContext.Session.Remove("EditHopperID");
        return new JsonResult(new { message = "Success" });
    }
}
