using System.Data;
using Alexis.Common;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Machine;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_MessageQueue;
using MDM.iOS.Business.BusinessLogics.MDM_APNS;
using MDM.iOS.Entities;
using MDM.iOS.Entities.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMiOS;

public class DeviceManagementModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    protected static MonitoringBase My_FMonitoringBase = MonitoringFactory.Create("");
    protected static UserManageBase My_FUserManageBase = UserManageFactory.Create("");
    protected static MDM_MachineBase MyMachineBase = MDM_MachineFactory.Create("");

    public string? ClientIp { get; set; }
    public string ErrorText { get; set; }

    public List<iOSDeviceViewModel> Devices { get; set; }

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
        checkAuthorization(ModuleLogAction.View_iOSMDMDeviceManagement);
    }

    public IActionResult OnGet()
    {
        try
        {
            BindGridData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void BindGridData()
    {
        DataTable data = My_FMonitoringBase.GetDeviceDetails_All_Active(new List<Params>());
        Devices = Common.DataHelper.ConvertDataTableToList<iOSDeviceViewModel>(data);
    }

    public IActionResult OnPostRemovePassCode(string MachineUDID, string MachineSerial)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();

            my_MQ_IpadCommandList.uDID = MachineUDID;
            my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
            my_MQ_IpadCommandList.commandName = Enum_CommandName.ClearPasscode;

            string prio = ConfigHelper.MQSecondPrior;
            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

            if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the ClearPasscode Command");
                accessAudit = false;
            }
            else
            {
                accessAudit = true;
            }
            ErrorText = accessAudit ? "Successful Send Clear Passcode Command!" : "Failed to Send Clear Passcode  Command!";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMClearPasscode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostRestart(string MachineUDID, string MachineSerial)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
            my_MQ_IpadCommandList.uDID = MachineUDID;
            my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
            my_MQ_IpadCommandList.commandName = Enum_CommandName.RestartDevice;
            string prio = ConfigHelper.MQSecondPrior;
            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

            if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the RestartDevice Command");
                accessAudit = false;
            }
            else
            {
                accessAudit = true;
            }
            ErrorText = accessAudit ? "Successful Send Restart Device Command!" : "Failed to Send Restart Device  Command!";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMRestartDevice, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostShutdown(string MachineUDID, string MachineSerial)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
            my_MQ_IpadCommandList.uDID = MachineUDID;
            my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
            my_MQ_IpadCommandList.commandName = Enum_CommandName.ShutDownDevice;
            string prio = ConfigHelper.MQSecondPrior;
            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

            if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the RestartDevice Command");
                accessAudit = false;
            }
            else
            {
                accessAudit = true;
            }
            ErrorText = accessAudit ? "Successful Send Shutdown Device Command!" : "Failed to Send Shutdown Device  Command!";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMShutdownDevice, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostRefreshOSUpdate(string MachineUDID, string MachineSerial)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
            my_MQ_IpadCommandList.uDID = MachineUDID;
            my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
            my_MQ_IpadCommandList.commandName = Enum_CommandName.AvailableOSUpdates;
            string prio = ConfigHelper.MQSecondPrior;
            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

            if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the RestartDevice Command");
                accessAudit = false;
            }
            else
            {
                accessAudit = true;
            }
            ErrorText = accessAudit ? "Successful Send List OS Scan Command!" : "Failed to Send List OS Scan Command!";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMGetOSUpdate, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostRefreshFirmwareInfo(string MachineUDID, string MachineSerial)
    {
        try
        {
            bool accessAudit;
            DeviceEn machineEn = new DeviceEn();
            machineEn.DeviceSerial = MachineSerial;
            string devicetoken = My_FMonitoringBase.GetFirmwareAppDeviceToken(machineEn);

            if (devicetoken.Length > 0)
            {
                var pushNotification = new PushNotification();
                pushNotification.SendiOSAppPushNotification("sandbox", devicetoken, "PBB_APNS_Cert.p12", "123456", "silent");
                ErrorText = "Successfuly sent command to retrieve firmware information.";
                accessAudit = false;
            }
            else
            {
                ErrorText = "There is no device token available for this machine. Please install and activate the Common SDK App.";
                accessAudit = true;
            }
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostFactoryReset(string MachineUDID, string MachineSerial)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
            my_MQ_IpadCommandList.uDID = MachineUDID;
            my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
            my_MQ_IpadCommandList.commandName = Enum_CommandName.EraseDevice;
            string prio = ConfigHelper.MQSecondPrior;
            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

            if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the EraseDevice Command");

                accessAudit = false;
            }
            else
            {
                accessAudit = true;
            }
            ErrorText = accessAudit ? "Successful Send Erase Device Command!" : "Failed to Send Erase Device Command!";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMFactoryReset, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostEnableLostMode(string MachineUDID, string MachineSerial)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
            my_MQ_IpadCommandList.uDID = MachineUDID;
            string[] arg = new string[2];
            arg = MachineSerial.Split(';');
            string machineSerial = arg[0];
            my_MQ_IpadCommandList.payloadIdentifier_AddOn = machineSerial;
            my_MQ_IpadCommandList.commandName = Enum_CommandName.EnableLostMode;
            string prio = ConfigHelper.MQSecondPrior;
            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

            if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the EnableLostMode Command");
                accessAudit = false;
            }
            else
            {
                // call the method for query lost device location to ipad simultaneously
                GetLostDeviceLocation(MachineUDID, MachineSerial);
                // update lost mode bit data in database 
                My_FMonitoringBase.UpdateLostMode(machineSerial, true);
                accessAudit = true;
            }
            ErrorText = accessAudit ? "Successful Send Enable Lost Mode Command!" : "Failed to Send Enable Lost Mode Command!";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMEnableLostMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostDisableLostMode(string MachineUDID, string MachineSerial)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
            my_MQ_IpadCommandList.uDID = MachineUDID;
            my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
            my_MQ_IpadCommandList.commandName = Enum_CommandName.DisableLostMode;
            string prio = ConfigHelper.MQSecondPrior;
            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

            if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the DisableLostMode Command");
                accessAudit = false;
            }
            else
            {
                // update lost mode bit data in database 
                My_FMonitoringBase.UpdateLostMode(MachineSerial, false);
                accessAudit = true;
            }
            ErrorText = accessAudit ? "Successful Send Disable Lost Mode Command!" : "Failed to Send Disable Lost Mode Command!";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMDisableLostMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostPlayLostModeSound(string MachineUDID, string MachineSerial)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
            my_MQ_IpadCommandList.uDID = MachineUDID;
            my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
            my_MQ_IpadCommandList.commandName = Enum_CommandName.PlayLostModeSound;
            string prio = ConfigHelper.MQSecondPrior;
            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

            if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send Play Lost Mode Sound Command");
                accessAudit = false;
            }
            else
            {
                accessAudit = true;
            }
            ErrorText = accessAudit ? "Successful Send Play Lost Mode Sound Command!" : "Failed to Send Play Lost Mode Sound Command!";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMPlayLostModeSound, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostDetails(string MachineUDID, string MachineSerial)
    {
        DeviceInfo info = new DeviceInfo();
        string certs, apps, locations = "";
        try
        {
            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineserial", "LIKE", MachineSerial));

            DataTable details_DT = My_FMonitoringBase.GetDeviceDetailForDisplay(myParams);
            info = ConvertDataTableToDeviceInfo(details_DT);
            DeviceCertificateEn machineCert = new DeviceCertificateEn();
            machineCert.UDID = MachineUDID;
            InstalledAppEn app = new InstalledAppEn();
            app.UDID = MachineUDID;

            DataTable certs_DT = My_FMonitoringBase.GetDeviceDetailCerts(machineCert);
            DataTable apps_DT = My_FMonitoringBase.GetDeviceDetailInstalledApps(app);
            info.Certificates = Common.DataHelper.ConvertDataTableToList<CertificateInfo>(certs_DT);
            info.Applications = Common.DataHelper.ConvertDataTableToList<ApplicationsInfo>(apps_DT);

            List<Params> lParams = new List<Params>();
            string datefrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateto = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            lParams.Add(new Params("@machineserial", "NVARCHAR", MachineSerial));
            lParams.Add(new Params("@dateto", "NVARCHAR", dateto + " 00:00:00"));
            lParams.Add(new Params("@datefrom", "NVARCHAR", datefrom + " 23:59:59"));
            DataTable LocationHistory_DT = My_FMonitoringBase.GetLocationHistory(lParams);
            info.Locations = Common.DataHelper.ConvertDataTableToList<LocationsInfo>(LocationHistory_DT);
        }
        catch (Exception ex)
        {
            //return new JsonResult(new { message = "Error" });
        }
        return Partial("_DetailsPartial", info);
        //return new JsonResult(new { message = "Success", detailsData = details, certsData= certs, appsData= apps, locationDta = locations });
    }

    public IActionResult OnPostCheckUpdates(string MachineUDID, string MachineSerial)
    {
        List<UpdatesInfo> info = new List<UpdatesInfo>();
        string certs, apps, locations = "";
        try
        {
            OSUpdateEn update = new OSUpdateEn();
            update.UDID = MachineUDID;
            DataTable updates_DT = My_FMonitoringBase.GetDeviceDetailOSUpdates(update);
            info = Common.DataHelper.ConvertDataTableToList<UpdatesInfo>(updates_DT);
        }
        catch (Exception ex)
        {
            //return new JsonResult(new { message = "Error" });
        }
        return Partial("_UpdatesPartial", info);
        //return new JsonResult(new { message = "Success", detailsData = details, certsData= certs, appsData= apps, locationDta = locations });
    }

    public IActionResult OnPostScheduleOSUpdate(string UDID, string ProductKey, string ProductVersion)
    {
        try
        {
            bool accessAudit;

            OSUpdateEn update = new OSUpdateEn();
            update.UDID = UDID;
            update.ProductKey = ProductKey;
            update.ProductVersion = ProductVersion;
            update.UpdateInstalled = false;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
            my_MQ_IpadCommandList.uDID = update.UDID;

            // to change later. 
            my_MQ_IpadCommandList.commandName = Enum_CommandName.ScheduleOSUpdate;
            string prio = ConfigHelper.MQSecondPrior;
            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

            // we will save this configuration and state in a separate database table first for pending updates. 
            My_FMonitoringBase.UpdateOSInstallStatusbyMachine(update);

            if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Schedule OS Update Command");
                accessAudit = false;
            }
            else
            {
                accessAudit = true;
            }
            ErrorText = accessAudit ? "Successful Send Schedule OS Update Command!" : "Failed to Send Schedule OS Update Command!";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMGetOSUpdate, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }
    private DeviceInfo ConvertDataTableToDeviceInfo(DataTable table)
    {
        if (table == null || table.Rows.Count == 0)
            return null;

        DataRow row = table.Rows[0]; // Take only the first row

        var deviceInfo = new DeviceInfo
        {
            MachineName = row["MachineName"]?.ToString(),
            MachineImei = row["MachineImei"]?.ToString(),
            MachineSerial = row["MachineSerial"]?.ToString(),
            BuildVersion = row["BuildVersion"]?.ToString(),
            OSVersion = row["OSVersion"]?.ToString(),
            MachineStatus = row.Field<int>("MachineStatus"),
            AvailableDevice_Capacity = row["AvailableDevice_Capacity"]?.ToString(),
            DeviceCapacity = row["DeviceCapacity"]?.ToString(),
            PasscodePresent = row.Field<bool?>("PasscodePresent"),
            PasscodeCompliant = row.Field<bool?>("PasscodeCompliant"),
            PasscodeCompliantWithProfiles = row.Field<bool?>("PasscodeCompliantWithProfiles"),
            WifiMAC = row["WifiMAC"]?.ToString(),
            BluetoothMAC = row["BluetoothMAC"]?.ToString(),
            IsRoaming = row.Field<bool>("IsRoaming"),
            IsSupervised = row.Field<bool>("isSupervised"),
            FirmwareVersion = row["FirmwareVersion"]?.ToString(),
            FirmwareBatteryStatus = row["FirmwareBatteryStatus"]?.ToString(),
            FirmwareBatteryLevel = row["FirmwareBatteryLevel"]?.ToString(),
            FirmwareSerial = row["FirmwareSerial"]?.ToString(),
            Latitude = row["Latitude"]?.ToString(),
            Longitude = row["Longitude"]?.ToString()
        };

        return deviceInfo;
    }

    private void GetLostDeviceLocation(string UDID, string serialNo)
    {
        List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
        MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();

        my_MQ_IpadCommandList.uDID = UDID;
        my_MQ_IpadCommandList.payloadIdentifier_AddOn = serialNo;

        my_MQ_IpadCommandList.commandName = Enum_CommandName.DeviceLocation;
        string prio = ConfigHelper.MQSecondPrior;
        my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
        my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

        if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send Get Lost Device Location Command");

        }
        else
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Successfully Sent Get Lost Device Location Command");

        }
    }
}

public class DeviceInfo
{
    public string MachineName { get; set; }
    public string MachineImei { get; set; }
    public string MachineSerial { get; set; }
    public string BuildVersion { get; set; }
    public string OSVersion { get; set; }
    public int MachineStatus { get; set; }
    public string AvailableDevice_Capacity { get; set; }
    public string DeviceCapacity { get; set; }
    public bool? PasscodePresent { get; set; }
    public bool? PasscodeCompliant { get; set; }
    public bool? PasscodeCompliantWithProfiles { get; set; }
    public string WifiMAC { get; set; }
    public string BluetoothMAC { get; set; }
    public bool IsRoaming { get; set; }
    public bool IsSupervised { get; set; }
    public string FirmwareVersion { get; set; }
    public string FirmwareBatteryStatus { get; set; }
    public string FirmwareBatteryLevel { get; set; }
    public string FirmwareSerial { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public List<CertificateInfo> Certificates { get; set; }
    public List<ApplicationsInfo> Applications { get; set; }
    public List<LocationsInfo> Locations { get; set; }
}

public class CertificateInfo
{
    public string UDID { get; set; }
    public string CertName { get; set; }
    public bool IsIdentityCert { get; set; }
}

public class ApplicationsInfo
{
    public string AppName { get; set; }
    public string Version { get; set; }
    public string Identifier { get; set; }
}

public class LocationsInfo
{
    public string MachineName { get; set; }
    public string MachineSerial { get; set; }
    public string Location { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class UpdatesInfo
{
    public string HumanReadableName { get; set; }
    public string ProductKey { get; set; }
    public string ProductVersion { get; set; }
    public bool RestartRequired { get; set; }
    public string UDID { get; set; }
}