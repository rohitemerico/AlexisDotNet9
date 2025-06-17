using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.UserMaintenance;

public class BranchManagementModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    protected static UserManageBase My_FUserManageBase = UserManageFactory.Create(string.Empty);
    public required List<BranchViewModel> Branches { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_BranchMaintenance);
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
        DataTable data = My_FUserManageBase.getBranches(new List<Params>());
        Branches = Common.DataHelper.ConvertDataTableToList<BranchViewModel>(data);
        foreach (var item in Branches)
        {
            var editedDataExists = userControl.IsEditingDataExist(item.BID);
            if (!Maker || editedDataExists || item.Status == "Pending") { item.Visible = false; }
        }
    }

    public IActionResult OnPostEdit(string id)
    {
        if (Guid.Parse(id) == Guid.Empty)
        {
            return new JsonResult(new { message = "Invalid ID." });
        }
        HttpContext.Session.SetString("EditBranchID", id);
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnGetAdd()
    {
        HttpContext.Session.Remove("EditBranchID");
        return new JsonResult(new { message = "Success" });
    }
}