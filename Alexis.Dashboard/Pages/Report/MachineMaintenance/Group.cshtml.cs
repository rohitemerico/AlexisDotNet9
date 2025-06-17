using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.MachineMaintenance;

public class GroupModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public List<GroupMaintenanceViewModel> GroupMaintenanceList { get; set; }

    public string ExportFileName { get => $"MachineMaintenance_Group_{DateTime.Now.ToString("yyyyMMdd")}"; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_MachineMaintenanceGroup);
    }

    public IActionResult OnGet()
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

    private void LoadData()
    {
        DataTable data = GroupMaintenanceController.getAssignedGroups();
        GroupMaintenanceList = Common.DataHelper.ConvertDataTableToList<GroupMaintenanceViewModel>(data);

    }
}
