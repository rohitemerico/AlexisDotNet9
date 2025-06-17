using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.UserMaintenance;

public class UserManagementModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    protected MasterSettingController controller = new MasterSettingController();
    private readonly UserRolePageLoad pageLoad = new();

    [BindProperty]
    public string SearchText { get; set; }
    public IList<UserViewModel> Users { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_UserMaintenance);
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
        DataTable data = pageLoad.UserCreate_Load2CY();
        Users = Common.DataHelper.ConvertDataTableToList<UserViewModel>(data);
        foreach (var item in Users)
        {
            var isAMaker = Maker;
            var editedDataExists = userControl.IsEditingDataExist(Guid.Parse(item.aID));
            var itemPending = item.uStatus == 0;
            if (itemPending && !editedDataExists)
            {
                item.ErrorVisible = true;
                item.Visible = false;
            }
            if (!isAMaker || editedDataExists) { item.Visible = false; }
        }
    }

    public IActionResult OnPostResetPassword(string id)
    {
        if (Guid.Parse(id) == Guid.Empty)
        {
            return new JsonResult(new { message = "Invalid ID." });
        }
        // Retrieve master settings
        AMasterSettings entity = controller.GetMasterSettings();
        bool result = userControl.ResetPassword(Guid.Parse(id), entity.PWord);
        if (result)
        {
            return new JsonResult(new { message = "Password Reset Successfully." });
        }
        else
        {
            return new JsonResult(new { message = "Error occurred while resetting Password." });
        }
    }

    public IActionResult OnPostEdit(string id)
    {
        if (Guid.Parse(id) == Guid.Empty)
        {
            return new JsonResult(new { message = "Invalid ID." });
        }
        HttpContext.Session.SetString("EditUserID", id);
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnGetAdd()
    {
        HttpContext.Session.Remove("EditUserID");
        return new JsonResult(new { message = "Success" });
    }
}