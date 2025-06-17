using System.ComponentModel.DataAnnotations;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages;

public class MasterSettingsModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    protected MasterSettingController controller = new();
    public string? ClientIp { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string Password { get; set; }
    public string LabelInfo { get; set; }
    public string ErrorText { get; set; }
    public bool LabelInfoVisible { get; set; } = false;

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
        checkAuthorization(ModuleLogAction.Create_MasterSetting);
    }

    public IActionResult OnGet()
    {
        try
        {
            // check if master setting is already edited and pending(cannot have duplicates)
            var pending = controller.GetAllPendingEditingData(Module.GetModuleId(ModuleLogAction.View_MasterSetting));
            if (pending.Rows.Count > 1) //something went wrong here
            {
                string errorMsg = "Please contact maintenance team for support on this.";
                LabelInfo = errorMsg;
                LabelInfoVisible = true;
                ErrorText = "Failed! " + errorMsg;
                PopUp("Alert!", ErrorText);

                AuditLog.CreateAuditLog("Master Setting: " + ErrorText, AuditCategory.System_Settings, ModuleLogAction.Update_MasterSetting, _UserId, false, ClientIp);
            }
            else if (pending.Rows.Count == 1) //already edited and pending
            {
                LabelInfo = "Already updated. Status is pending for approval/rejection.";
                LabelInfoVisible = true;
            }
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
            if (ModelState.IsValid)
            {
                AMasterSettings entity = new()
                {
                    PWord = Password,
                    UpdatedBy = _UserId,
                    UpdatedDate = DateTime.Now
                };
                if (controller.EditData(entity, ClientIp))
                {
                    ErrorText = "Successfully Edited. Please Wait for Approval.";
                    PopUp("Alert!", ErrorText);
                    AuditLog.CreateAuditLog("Master Setting: " + ErrorText, AuditCategory.System_Settings, ModuleLogAction.Update_MasterSetting, _UserId, true, ClientIp);
                }
                else
                {
                    ErrorText = "Unsuccessful Update.";
                    PopUp("Alert!", ErrorText);
                    AuditLog.CreateAuditLog("Master Setting: " + ErrorText, AuditCategory.System_Settings, ModuleLogAction.Update_MasterSetting, _UserId, false, ClientIp);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
}
