using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class GroupMaintenanceModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public Guid ModuleId { get; set; }
    public List<GroupMaintenanceViewModel> GroupMaintenanceList { get; set; }

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
        checkAuthorization(ModuleLogAction.View_Group_Template);
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
        DataTable data = GroupMaintenanceController.getAssignedGroups();
        GroupMaintenanceList = Common.DataHelper.ConvertDataTableToList<GroupMaintenanceViewModel>(data);
        foreach (var item in GroupMaintenanceList)
        {
            if (!Maker || userControl.IsEditingDataExist(Guid.Parse(item.kId)) || (item.Status == "Pending"))
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
        HttpContext.Session.SetString("EditGroupID", id);
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnGetAdd()
    {
        HttpContext.Session.Remove("EditGroupID");
        return new JsonResult(new { message = "Success" });
    }
}
