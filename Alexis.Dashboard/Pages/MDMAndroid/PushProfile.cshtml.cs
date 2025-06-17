using System.Data;
using Alexis.Common;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMAndroid;

public class PushProfileModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    public string ActivateTab { get => "show active"; }
    public MDMTabGroup CurrentTab { get; set; }
    public required List<AndroidMDMDevicesViewModel> Devices { get; set; }
    public required List<DeviceGroupViewModel> Groups { get; set; }
    public List<ProfileViewModel> Profiles { get; set; }
    public string? ClientIp { get; set; }

    public bool DeviceVisible { get; set; } = true;
    public bool GroupVisible { get; set; } = true;
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
        checkAuthorization(ModuleLogAction.Push_AndroidMDMPushProfile);
    }

    public IActionResult OnGet()
    {
        try
        {
            if (TempData["CurrentTab"] != null)
                CurrentTab = (MDMTabGroup)TempData["CurrentTab"];
            else CurrentTab = MDMTabGroup.By_Device;
            BindGridData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    public void BindGridData()
    {
        switch (CurrentTab)
        {
            case MDMTabGroup.By_Device:
                DataTable data1 = AndroidMDMController.GetAllDevices();
                Devices = Common.DataHelper.ConvertDataTableToList<AndroidMDMDevicesViewModel>(data1);
                break;
            case MDMTabGroup.By_Device_Group:
                DataTable data2 = AndroidMDMController.getAllMDMDeviceGroup();
                Groups = Common.DataHelper.ConvertDataTableToList<DeviceGroupViewModel>(data2);
                break;
        }
        DataTable data3 = AndroidMDMController.getAllProfiles();
        Profiles = Common.DataHelper.ConvertDataTableToList<ProfileViewModel>(data3);
    }

    public IActionResult OnPostPushProfileGroup(string GroupId, string ProfileId)
    {
        string profileName = "";
        bool accessAudit;
        try
        {
            string cmd = commandCreateCommand(ProfileId);
            profileName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());
            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Unable to push profile '{profileName}' to the device(s).";
                accessAudit = false;
            }
            else
            {
                try
                {
                    DataTable devices = AndroidMDMController.GetAllDevicesByGroupID(GroupId);
                    ErrorText = $"Push status: profile '{profileName}' was pushed to the device group.";
                    for (int i = 0; i < devices.Rows.Count; i++)
                    {
                        string idNotDevice = AndroidMDMController.InsertPushNotificationDevice(idNotifCommand, devices.Rows[i]["deviceMACAdd"].ToString(), _UserId.ToString());
                        if (string.IsNullOrEmpty(idNotDevice))
                        {
                            throw new Exception("One of the devices will not receive notification.");
                        }
                    }
                    accessAudit = true;
                }
                catch
                {
                    ErrorText = $"Unable to push profile '{profileName}' to the device(s).";
                    accessAudit = false;
                }
            }
        }
        catch
        {
            ErrorText = $"Unable to push profile '{profileName}' to the device(s).";
            accessAudit = false;
        }
        AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMPushProfile, _UserId, accessAudit, ClientIp);
        PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        TempData["CurrentTab"] = MDMTabGroup.By_Device_Group;
        return Redirect("PushProfile");
    }

    public IActionResult OnPostPushProfileDevice(string DeviceId, string ProfileId)
    {
        string profileName = "";
        bool accessAudit;
        try
        {
            string cmd = commandCreateCommand(ProfileId);
            profileName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());

            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Unable to push profile '{profileName}' to the device(s).";
                accessAudit = false;
            }
            else
            {
                string idNotDevice = AndroidMDMController.InsertPushNotificationDevice(idNotifCommand, DeviceId, _UserId.ToString());
                if (string.IsNullOrEmpty(idNotDevice))
                {
                    ErrorText = $"Unable to push profile '{profileName}' to the device(s).";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = $"Push status: profile '{profileName}' was pushed to the device.";
                    accessAudit = true;
                }
            }
        }
        catch
        {
            ErrorText = $"Unable to push profile '{profileName}' to the device(s).";
            accessAudit = false;
        }
        AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMPushProfile, _UserId, accessAudit, ClientIp);
        PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        TempData["CurrentTab"] = MDMTabGroup.By_Device;
        return Redirect("PushProfile");
    }

    private string commandCreateCommand(string id)
    {
        DataTable dtProfiles = AndroidMDMController.getProfileByID(id);
        string FPath = dtProfiles.Rows[0]["FPath"].ToString();
        FPath = FPath[0] == '/' ? FPath.Substring(1) : FPath;
        return "Download Profile " + ConfigHelper.PortalURL + FPath;
    }
    public IActionResult OnPostChangeGrid(string name)
    {
        TempData["CurrentTab"] = EnumHelper.ParseEnum<MDMTabGroup>(name);
        return new JsonResult(new { message = "Success" });
    }
}
