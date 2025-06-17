using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMAndroid;

public class DeviceGroupAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public string PageTitleText { get; set; }

    public string? ClientIp { get; set; }
    [BindProperty]
    public string? GID { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string GroupName { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string GroupDesc { get; set; }
    public string ErrorText { get; set; }

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
        checkAuthorization(ModuleLogAction.Create_AndroidMDMDeviceGroup);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("DeviceGroupID") != null)
            {
                GID = session.GetString("DeviceGroupID").ToString();
                EditCommand(GID);
            }
            PageTitleText = GID == null ? "Device Group > Add New" : "Device Group > Edit";
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
            var session = httpContextAccessor.HttpContext.Session;
            var GID = session.GetString("DeviceGroupID");
            if (ModelState.IsValid)
            {

                if (GID == null)
                {
                    bool accessAudit = AndroidMDMController.AddDeviceGroup(GroupName, GroupDesc, _UserId.ToString());
                    ErrorText = accessAudit ? "Successfully added new device group!" : "Fail to add new device group!";
                    PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
                    AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Create_AndroidMDMDeviceGroup, _UserId, accessAudit, ClientIp);

                }
                else
                {
                    bool accessAudit = AndroidMDMController.EditDeviceGroup(GID, GroupName, GroupDesc, _UserId.ToString());
                    ErrorText = accessAudit ? "Successfully updated device group information!" : "Fail to update device group information!";
                    PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
                    AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Update_AndroidMDMDeviceGroup, _UserId, accessAudit, ClientIp);

                }
            }
            PageTitleText = GID == null ? "Device Group > Add New" : "Device Group > Edit";
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void EditCommand(string Id)
    {
        try
        {
            PageTitleText = "Device Group > Edit";
            var groupRow = AndroidMDMController.getAllMDMDeviceGroup().AsEnumerable().SingleOrDefault(gg => (string)gg["GID"] == Id);
            if (groupRow != null)
            {
                GroupName = groupRow["GROUPNAME"] as string;
                GroupDesc = groupRow["GROUPDESC"] as string;
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("DeviceGroupID");
        return new JsonResult(new { message = "Success" });
    }

}
