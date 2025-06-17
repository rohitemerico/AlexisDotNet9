using System.Data;
using Alexis.Common;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMAndroid;

public class PushApplicationModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public string ActivateTab { get => "show active"; }
    public MDMTabGroup CurrentTab { get; set; }
    public required List<AndroidMDMDevicesViewModel> Devices { get; set; }
    public required List<DeviceGroupViewModel> Groups { get; set; }
    public List<ApplicationViewModel> Applications { get; set; }
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
        checkAuthorization(ModuleLogAction.Push_AndroidMDMPushApp);
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
        DataTable data3 = AndroidMDMController.AppGetAll();
        Applications = Common.DataHelper.ConvertDataTableToList<ApplicationViewModel>(data3);
    }
    public IActionResult OnPostPushApplicationGroup(string GroupId, string ApplicationId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_PushApp(ApplicationId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());

            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Unable to push application '{appName}' to the device(s).";
                accessAudit = false;
            }
            else
            {
                try
                {
                    DataTable devices = AndroidMDMController.GetAllDevicesByGroupID(GroupId);
                    ErrorText = $"Push status: application '{appName}' was pushed to the device group.";
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
                    ErrorText = $"Unable to push application '{appName}' to the device(s).";
                    accessAudit = false;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMPushApp, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device_Group;
        }
        catch
        {
            ErrorText = $"Unable to push application '{appName}' to the device(s).";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostRemoveApplicationGroup(string GroupId, string ApplicationId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_RemoveApp(ApplicationId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());

            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Unable to remove application '{appName}' from the device(s).";
                accessAudit = false;
            }
            else
            {
                try
                {
                    DataTable devices = AndroidMDMController.GetAllDevicesByGroupID(GroupId);
                    ErrorText = $"Push status: application '{appName}' was removed from the device group.";
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
                    ErrorText = $"Unable to remove application '{appName}' from the device(s).";
                    accessAudit = false;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMRemoveApp, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device_Group;
        }
        catch
        {
            ErrorText = $"Unable to remove application '{appName}' from the device(s).";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostEnableKioskModeApplicationGroup(string GroupId, string ApplicationId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_KioskMode(ApplicationId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());

            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Failed to enable Kiosk Mode on device(s).";
                accessAudit = false;
            }
            else
            {
                try
                {
                    DataTable devices = AndroidMDMController.GetAllDevicesByGroupID(GroupId);
                    ErrorText = $"Kiosk Mode enabled on the device group.";
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
                    ErrorText = $"Failed to enable Kiosk Mode on device(s).";
                    accessAudit = false;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMEnableKioskMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device_Group;
        }
        catch
        {
            ErrorText = $"Failed to enable Kiosk Mode on device(s).";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostRemoveKioskModeApplicationGroup(string GroupId, string ApplicationId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_RemoveKioskMode(ApplicationId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());

            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Failed to remove Kiosk Mode on the device(s).";
                accessAudit = false;
            }
            else
            {
                try
                {
                    DataTable devices = AndroidMDMController.GetAllDevicesByGroupID(GroupId);
                    ErrorText = $"Kiosk Mode removed from the device group.";
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
                    ErrorText = $"Failed to remove Kiosk Mode on the device(s).";
                    accessAudit = false;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMRemoveKioskMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device_Group;
        }
        catch
        {
            ErrorText = $"Failed to remove Kiosk Mode on the device(s).";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostEnableGeoFenceModeGroup(string GroupId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_GeoFenceMode(GroupId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());

            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Failed to enable Kiosk Mode on device(s).";
                accessAudit = false;
            }
            else
            {
                try
                {
                    DataTable devices = AndroidMDMController.GetAllDevicesByGroupID(GroupId);
                    ErrorText = $"Kiosk Mode enabled on the device group.";
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
                    ErrorText = $"Failed to enable Kiosk Mode on device(s).";
                    accessAudit = false;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMEnableGeoFenceMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device_Group;
        }
        catch
        {
            ErrorText = $"Failed to enable Kiosk Mode on device(s).";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostRemoveGeoFenceModeGroup(string GroupId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_RemoveGeoFenceMode(GroupId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());

            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Failed to remove Kiosk Mode on the device(s).";
                accessAudit = false;
            }
            else
            {
                try
                {
                    DataTable devices = AndroidMDMController.GetAllDevicesByGroupID(GroupId);
                    ErrorText = $"Kiosk Mode removed from the device group.";
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
                    ErrorText = $"Failed to remove Kiosk Mode on the device(s).";
                    accessAudit = false;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMRemoveGeoFenceMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device_Group;
        }
        catch
        {
            ErrorText = $"Failed to remove Kiosk Mode on the device(s).";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostPushApplicationDevice(string DeviceId, string ApplicationId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_PushApp(ApplicationId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());
            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Unable to push application '{appName}' to the device.";
                accessAudit = false;
            }
            else
            {
                string idNotDevice = AndroidMDMController.InsertPushNotificationDevice(idNotifCommand, DeviceId, _UserId.ToString());
                if (string.IsNullOrEmpty(idNotDevice))
                {
                    ErrorText = $"Unable to push application '{appName}' to the device.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = $"Push status: application '{appName}' was pushed to the device.";
                    accessAudit = true;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMPushApp, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device;
        }
        catch
        {
            ErrorText = $"Unable to push application '{appName}' to the device.";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostRemoveApplicationDevice(string DeviceId, string ApplicationId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_RemoveApp(ApplicationId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());
            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Unable to remove application '{appName}' from the device.";
                accessAudit = false;
            }
            else
            {
                string idNotDevice = AndroidMDMController.InsertPushNotificationDevice(idNotifCommand, DeviceId, _UserId.ToString());
                if (string.IsNullOrEmpty(idNotDevice))
                {
                    ErrorText = $"Unable to remove application '{appName}' from the device.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = $"Remove status: application '{appName}' was removed the device.";
                    accessAudit = true;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMRemoveApp, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device;
        }
        catch
        {
            ErrorText = $"Unable to remove application '{appName}' from the device.";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostEnableKioskModeApplicationDevice(string DeviceId, string ApplicationId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_KioskMode(ApplicationId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());
            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Failed to enable Kiosk Mode on device.";
                accessAudit = false;
            }
            else
            {
                string idNotDevice = AndroidMDMController.InsertPushNotificationDevice(idNotifCommand, DeviceId, _UserId.ToString());
                if (string.IsNullOrEmpty(idNotDevice))
                {
                    ErrorText = $"Failed to enable Kiosk Mode on device.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = $"Kiosk Mode enabled on the device.";
                    accessAudit = true;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMEnableKioskMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device;
        }
        catch
        {
            ErrorText = $"Failed to enable Kiosk Mode on device.";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostRemoveKioskModeApplicationDevice(string DeviceId, string ApplicationId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_RemoveKioskMode(ApplicationId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());
            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Failed to remove Kiosk Mode on device.";
                accessAudit = false;
            }
            else
            {
                string idNotDevice = AndroidMDMController.InsertPushNotificationDevice(idNotifCommand, DeviceId, _UserId.ToString());
                if (string.IsNullOrEmpty(idNotDevice))
                {
                    ErrorText = $"Failed to remove Kiosk Mode on device.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = $"Kiosk Mode removed on the device.";
                    accessAudit = true;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMRemoveKioskMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device;
        }
        catch
        {
            ErrorText = $"Failed to remove Kiosk Mode on device.";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostEnableGeoFenceModeDevice(string DeviceId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_GeoFenceMode(DeviceId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());
            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Failed to enable GeoFence Mode on device.";
                accessAudit = false;
            }
            else
            {
                string idNotDevice = AndroidMDMController.InsertPushNotificationDevice(idNotifCommand, DeviceId, _UserId.ToString());
                if (string.IsNullOrEmpty(idNotDevice))
                {
                    ErrorText = $"Failed to enable GeoFence Mode on device.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = $"GeoFence Mode enabled on the device.";
                    accessAudit = true;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMEnableGeoFenceMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device;
        }
        catch
        {
            ErrorText = $"Failed to enable GeoFence Mode on device.";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    public IActionResult OnPostRemoveGeoFenceModeDevice(string DeviceId)
    {
        string appName = "";
        bool accessAudit;
        try
        {
            string cmd = CommandCreateCommand_RemoveGeoFenceMode(DeviceId);
            appName = Path.GetFileNameWithoutExtension(cmd);
            var idNotifCommand = AndroidMDMController.InsertPushNotificationCommand(cmd, _UserId.ToString());
            if (string.IsNullOrEmpty(idNotifCommand))
            {
                ErrorText = $"Failed to remove GeoFence Mode on device.";
                accessAudit = false;
            }
            else
            {
                string idNotDevice = AndroidMDMController.InsertPushNotificationDevice(idNotifCommand, DeviceId, _UserId.ToString());
                if (string.IsNullOrEmpty(idNotDevice))
                {
                    ErrorText = $"Failed to remove GeoFence Mode on device.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = $"GeoFence Mode removed on the device.";
                    accessAudit = true;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Push_AndroidMDMRemoveGeoFenceMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMTabGroup.By_Device;
        }
        catch
        {
            ErrorText = $"Failed to remove GeoFence Mode on device.";
            accessAudit = false;
            return new JsonResult(new { message = "Error" });
        }

        return new JsonResult(new { message = "Success" });
    }
    private static string CommandCreateCommand_PushApp(string id)
    {
        DataTable dtProfiles = AndroidMDMController.GetAppByID(id);
        string FPath = dtProfiles.Rows[0]["FPath"].ToString();
        FPath = FPath[0] == '/' ? FPath.Substring(1) : FPath;
        return "Download APK " + Path.Combine(ConfigHelper.PortalURL, FPath);
    }
    private static string CommandCreateCommand_RemoveApp(string id)
    {
        DataTable dtProfiles = AndroidMDMController.GetAppByID(id);
        string PACKAGE_NAME = dtProfiles.Rows[0]["PACKAGE_NAME"].ToString();
        return "Uninstall APK " + PACKAGE_NAME;
    }
    private static string CommandCreateCommand_KioskMode(string id)
    {
        DataTable dtProfiles = AndroidMDMController.GetAppByID(id);
        string FPath = dtProfiles.Rows[0]["FPath"].ToString();
        string PACKAGE_NAME = dtProfiles.Rows[0]["PACKAGE_NAME"].ToString();
        return "EnableKioskMode APK " + PACKAGE_NAME;
    }
    private static string CommandCreateCommand_RemoveKioskMode(string id)
    {
        DataTable dtProfiles = AndroidMDMController.GetAppByID(id);
        string FPath = dtProfiles.Rows[0]["FPath"].ToString();
        string PACKAGE_NAME = dtProfiles.Rows[0]["PACKAGE_NAME"].ToString();
        return "DisableKioskMode APK " + PACKAGE_NAME;
    }
    private static string CommandCreateCommand_GeoFenceMode(string id)
    {
        DataTable dtProfiles = AndroidMDMController.GetDeviceById(id);
        string latitute = dtProfiles.Rows[0]["Restriction_LATITUDE"].ToString();
        string longitute = dtProfiles.Rows[0]["Restriction_LONGITUDE"].ToString();
        string radius = dtProfiles.Rows[0]["Restriction_radius"].ToString();
        return string.Format("EnableGeoFence {0},{1},{2}", latitute, longitute, radius);
    }
    private static string CommandCreateCommand_RemoveGeoFenceMode(string id)
    {
        DataTable dtProfiles = AndroidMDMController.GetDeviceById(id);
        string latitute = dtProfiles.Rows[0]["Restriction_LATITUDE"].ToString();
        string longitute = dtProfiles.Rows[0]["Restriction_LONGITUDE"].ToString();
        string radius = dtProfiles.Rows[0]["Restriction_radius"].ToString();
        return string.Format("DisableGeoFence {0},{1},{2}", latitute, longitute, radius);
    }
    public IActionResult OnPostChangeGrid(string name)
    {
        TempData["CurrentTab"] = EnumHelper.ParseEnum<MDMTabGroup>(name);
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostConfigRestrictions(string DeviceId)
    {
        HttpContext.Session.SetString("DeviceMACID", DeviceId);
        return new JsonResult(new { message = "Success" });
    }
}