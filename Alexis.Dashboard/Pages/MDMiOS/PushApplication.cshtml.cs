using System.Data;
using Alexis.Common;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_App;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Machine;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_MessageQueue;
using MDM.iOS.Entities.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alexis.Dashboard.Pages.MDMiOS;

public class PushApplicationModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private static MDM_AppBase My_FMDM_AppBase = MDM_AppFactory.Create("");
    private static UserManageBase My_FUserManageBase = UserManageFactory.Create("");
    private static MDM_MachineBase My_FMDM_MachineBase = MDM_MachineFactory.Create("");
    private static MonitoringBase My_FMonitoringBase = MonitoringFactory.Create("");
    public string? ClientIp { get; set; }
    public string ErrorText { get; set; }
    public string ActivateTab { get => "show active"; }
    public MDMiOSTabGroup CurrentTab { get; set; }

    public string BranchId { get; set; }

    public required List<iOSMDMDevicesViewModel> Devices { get; set; }
    public List<iOSMDMApplicationViewModel> Applications { get; set; }

    public List<SelectListItem> Branches { get; set; }

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
        checkAuthorization(ModuleLogAction.View_iOSMDMPushApplication);
    }

    public IActionResult OnGet()
    {
        try
        {
            if (TempData["CurrentTab"] != null)
                CurrentTab = (MDMiOSTabGroup)TempData["CurrentTab"];
            else CurrentTab = MDMiOSTabGroup.By_Device;
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
        DataTable data1 = My_FMonitoringBase.GetDeviceDetails_All_Active(new List<Params>());
        switch (CurrentTab)
        {

            case MDMiOSTabGroup.By_Device:

                Devices = Common.DataHelper.ConvertDataTableToList<iOSMDMDevicesViewModel>(data1);
                break;
            case MDMiOSTabGroup.By_Branch:
                DataTable data2 = My_FUserManageBase.getBranchwithAssignRestriction(@" and MGB.Branch_ID IN 
                (select tb.BranchId from tblMachine tb, MDM_Profile_General_BranchID mb 
                where tb.MachineStatus != 0 and mb.Branch_ID = tb.BranchId)");
                List<iOSMDMBranchViewModel> BranchList = Common.DataHelper.ConvertDataTableToList<iOSMDMBranchViewModel>(data2);
                Branches = [new SelectListItem { Text = "All", Value = "00000000-0000-0000-0000-000000000000" }];
                foreach (var item in BranchList)
                {
                    Branches.Add(new SelectListItem { Text = item.Branch_Desc, Value = item.Branch_ID.ToString() });
                }
                Devices = Common.DataHelper.ConvertDataTableToList<iOSMDMDevicesViewModel>(data1);
                break;
        }
        DataTable data3 = My_FMDM_AppBase.GetMDM_APP();
        Applications = Common.DataHelper.ConvertDataTableToList<iOSMDMApplicationViewModel>(data3);
    }

    public IActionResult OnPostPushApplicationByMachine(string MachineSerial, string ApplicationId, bool allowBackup)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            DataTable dt = My_FMDM_AppBase.GetMDM_APP_APPID(Guid.Parse(ApplicationId));
            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineserial", "LIKE", MachineSerial));
            DataTable dt_ret = My_FMonitoringBase.GetDeviceDetails_All_Active(myParams);
            if (bool.Parse(dt_ret.Rows[0]["LostModeEnabled"].ToString()))
            {
                ErrorText = "Please disable the device lost mode. Command execution failed.";
                accessAudit = false;
            }
            else
            {
                MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                my_MQ_IpadCommandList.uDID = dt_ret.Rows[0]["MachineUDID"].ToString();
                my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
                my_MQ_IpadCommandList.app_identifier = dt.Rows[0]["Identifier"].ToString();
                my_MQ_IpadCommandList.filePath = dt.Rows[0]["Path"].ToString();
                my_MQ_IpadCommandList.Mprio = Convert.ToInt32(ConfigHelper.MQSecondPrior);
                my_MQ_IpadCommandList.commandName = allowBackup ? Enum_CommandName.InstallApplication : Enum_CommandName.InstallApplicationNoBackup;
                my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);


                InstalledAppEn app = new InstalledAppEn();
                app.UDID = dt_ret.Rows[0]["MachineUDID"].ToString();
                DataTable dt_Machine = My_FMonitoringBase.GetDeviceApps(app);

                if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Install Application Command");
                    ErrorText = "Failed to Send Install Application command.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = "Successful Send Install Application command.";
                    accessAudit = true;
                }
                //En_MDMAppInstallationSummary summary = new En_MDMAppInstallationSummary()
                //{
                //    ID = Guid.NewGuid(),
                //    AppID = Guid.Parse(ApplicationId),
                //    MachineID = my_MQ_IpadCommandList.uDID,
                //    Status = Convert.ToInt32(accessAudit),
                //    InstallType = 1,
                //    CreatedBy = _UserId,
                //    CreatedDate = DateTime.Now,
                //};

                //My_FMDM_AppBase.AppInstallSummaryInsertIntoDb(summary);
                AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMInstallApps, _UserId, accessAudit, ClientIp);
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

    public IActionResult OnPostRemoveApplicationByMachine(string MachineSerial, string ApplicationId)
    {
        try
        {
            bool accessAudit;
            List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
            DataTable dt = My_FMDM_AppBase.GetMDM_APP_APPID(Guid.Parse(ApplicationId));
            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineserial", "LIKE", MachineSerial));
            DataTable dt_ret = My_FMonitoringBase.GetDeviceDetails_All_Active(myParams);
            if (bool.Parse(dt_ret.Rows[0]["LostModeEnabled"].ToString()))
            {
                ErrorText = "Please disable the device lost mode. Command execution failed.";
                accessAudit = false;
            }
            else
            {
                MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                my_MQ_IpadCommandList.uDID = dt_ret.Rows[0]["MachineUDID"].ToString();
                my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
                my_MQ_IpadCommandList.app_identifier = dt.Rows[0]["Identifier"].ToString();
                my_MQ_IpadCommandList.commandName = Enum_CommandName.RemoveApplication;
                my_MQ_IpadCommandList.filePath = dt.Rows[0]["Path"].ToString();
                my_MQ_IpadCommandList.Mprio = Convert.ToInt32(ConfigHelper.MQSecondPrior);
                my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

                InstalledAppEn app = new InstalledAppEn();
                app.UDID = dt_ret.Rows[0]["MachineUDID"].ToString(); ;
                DataTable dt_Machine = My_FMonitoringBase.GetDeviceApps(app);

                if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Install Application Command");
                    ErrorText = "Failed to Send Remove Application command.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = "Successful Send Remove Application command.";
                    accessAudit = true;
                }
                //En_MDMAppInstallationSummary summary = new En_MDMAppInstallationSummary()
                //{
                //    ID = Guid.NewGuid(),
                //    AppID = Guid.Parse(ApplicationId),
                //    MachineID = my_MQ_IpadCommandList.uDID,
                //    Status = Convert.ToInt32(accessAudit),
                //    InstallType = 2,
                //    CreatedBy = _UserId,
                //    CreatedDate = DateTime.Now,
                //};
                //My_FMDM_AppBase.AppInstallSummaryInsertIntoDb(summary);
                AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMRemoveApps, _UserId, accessAudit, ClientIp);
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

    public IActionResult OnPostSingleAppModeByMachine(string MachineUDID, string MachineSerial, string ApplicationId, bool allowBackup)
    {
        try
        {
            bool accessAudit;
            InstalledAppEn app = new InstalledAppEn();
            app.UDID = MachineUDID;

            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineserial", "LIKE", MachineSerial));             // retrieve the machine by specifying serial number
            DataTable dt_ret = My_FMonitoringBase.GetDeviceDetails_All_Active(myParams);  // retrieve the list of machines
            DataTable dt_appList = My_FMonitoringBase.GetDeviceApps(app);                 // retrieve the list of managed apps installed in machine 
            DataTable dt = My_FMDM_AppBase.GetMDM_APP_APPID(Guid.Parse(ApplicationId));

            if (!bool.Parse(dt_ret.Rows[0]["IsSupervised"].ToString()))
            {
                ErrorText = "Single App Mode is not enabled due to device not being supervised.";
                accessAudit = false;
            }
            else if (bool.Parse(dt_ret.Rows[0]["LostModeEnabled"].ToString()))
            {

                ErrorText = "Please disable the device lost mode. Command execution failed.";
                accessAudit = false;
            }
            else if (bool.Parse(dt_ret.Rows[0]["SingleAppModeEnabled"].ToString()))
            {
                ErrorText = "Single app mode is already enabled in this device.";
                accessAudit = false;
            }
            else
            {
                List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
                MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                my_MQ_IpadCommandList.uDID = dt_ret.Rows[0]["MachineUDID"].ToString();
                my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
                my_MQ_IpadCommandList.app_identifier = dt.Rows[0]["Identifier"].ToString();
                my_MQ_IpadCommandList.commandName = Enum_CommandName.SingleAppMode;
                my_MQ_IpadCommandList.filePath = dt.Rows[0]["Path"].ToString();
                my_MQ_IpadCommandList.Mprio = Convert.ToInt32(ConfigHelper.MQSecondPrior);
                my_MQ_IpadCommandList.commandName = allowBackup ? Enum_CommandName.InstallApplication : Enum_CommandName.InstallApplicationNoBackup;
                my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

                if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Install Application Command");
                    ErrorText = "Failed to Send Single app mode command.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = "Successful Send Single app mode command.";
                    accessAudit = true;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMSingleAppMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostRemoveSingleAppModeByMachine(string MachineUDID, string MachineSerial, string ApplicationId, bool allowBackup)
    {
        try
        {
            bool accessAudit;
            InstalledAppEn app = new InstalledAppEn();
            app.UDID = MachineUDID;

            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineserial", "LIKE", MachineSerial));             // retrieve the machine by specifying serial number
            DataTable dt_ret = My_FMonitoringBase.GetDeviceDetails_All_Active(myParams);  // retrieve the list of machines
            DataTable dt = My_FMDM_AppBase.GetMDM_APP_APPID(Guid.Parse(ApplicationId));

            if (bool.Parse(dt_ret.Rows[0]["LostModeEnabled"].ToString()))
            {

                ErrorText = "Please disable the device lost mode. Command execution failed.";
                accessAudit = false;
            }
            else if (!bool.Parse(dt_ret.Rows[0]["SingleAppModeEnabled"].ToString()))
            {
                ErrorText = "Single app mode is not enabled.";
                accessAudit = false;
            }
            else
            {
                List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
                MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                my_MQ_IpadCommandList.uDID = dt_ret.Rows[0]["MachineUDID"].ToString();
                my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
                my_MQ_IpadCommandList.app_identifier = dt.Rows[0]["Identifier"].ToString();
                my_MQ_IpadCommandList.commandName = Enum_CommandName.RemoveSingleAppMode;
                my_MQ_IpadCommandList.filePath = dt.Rows[0]["Path"].ToString();
                my_MQ_IpadCommandList.Mprio = Convert.ToInt32(ConfigHelper.MQSecondPrior);
                my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);

                if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Remove Single App Mode Command");
                    ErrorText = "Failed to Send Remove Single App Mode command.";
                    accessAudit = false;
                }
                else
                {
                    ErrorText = "Successful Send Remove Single App Mode command.";
                    accessAudit = true;
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMSingleAppMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostPushApplicationByBranch(string ApplicationId, string BranchId, bool allowBackup)
    {
        try
        {
            bool accessAudit = false;
            DataTable dt = My_FMDM_MachineBase.getMachine_byBranch(Guid.Parse(BranchId));
            if (dt.Rows.Count == 0)
            {
                ErrorText = "No Machines inside this branch!";
                accessAudit = false;
            }
            else
            {
                DataTable dtApp = My_FMDM_AppBase.GetMDM_APP_APPID(Guid.Parse(ApplicationId));
                List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
                bool lostModeDevicePresent = false;
                if (dt.Rows.Count > 0)
                {
                    for (int a = 0; a < dt.Rows.Count; a++)
                    {
                        MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                        my_MQ_IpadCommandList.uDID = dt.Rows[a]["MachineUDID"].ToString();
                        my_MQ_IpadCommandList.payloadIdentifier_AddOn = dt.Rows[a]["MachineSerial"].ToString();
                        my_MQ_IpadCommandList.app_identifier = dtApp.Rows[0]["Identifier"].ToString(); ;
                        my_MQ_IpadCommandList.commandName = Enum_CommandName.InstallApplication;
                        my_MQ_IpadCommandList.filePath = dtApp.Rows[0]["Path"].ToString();
                        my_MQ_IpadCommandList.Mprio = Convert.ToInt32(ConfigHelper.MQSecondPrior);
                        my_MQ_IpadCommandList.commandName = allowBackup ? Enum_CommandName.InstallApplication : Enum_CommandName.InstallApplicationNoBackup;
                        my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);
                        if (bool.Parse(dt.Rows[a]["LostModeEnabled"].ToString()))
                        {
                            lostModeDevicePresent = true;
                        }
                    }

                    if (lostModeDevicePresent)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "Alert", "alert('Certain device(s) are in lost mode. Command will not be sent to the device.');", true);
                    }
                    if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Install Application Command");
                        ErrorText = "Failed to Send the Install Application Command";
                        accessAudit = false;
                    }
                    else
                    {
                        ErrorText = "Successful Send Install Application command.";
                        accessAudit = true;
                    }
                    //foreach (var item in my_MQ_IpadCommandList_List)
                    //{
                    //    En_MDMAppInstallationSummary summary = new En_MDMAppInstallationSummary()
                    //    {
                    //        ID = Guid.NewGuid(),
                    //        AppID = Guid.Parse(ApplicationId),
                    //        MachineID = item.uDID,
                    //        Status = Convert.ToInt32(accessAudit),
                    //        InstallType = 1,
                    //        CreatedBy = _UserId,
                    //        CreatedDate = DateTime.Now,
                    //    };
                    //    My_FMDM_AppBase.AppInstallSummaryInsertIntoDb(summary);
                    //}
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMInstallApps, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMiOSTabGroup.By_Branch;
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostRemoveApplicationByBranch(string ApplicationId, string BranchId)
    {
        try
        {
            bool accessAudit = false;
            DataTable dt = My_FMDM_MachineBase.getMachine_byBranch(Guid.Parse(BranchId));
            if (dt.Rows.Count == 0)
            {
                ErrorText = "No Machines inside this branch!";
                accessAudit = false;
            }
            else
            {
                DataTable dtApp = My_FMDM_AppBase.GetMDM_APP_APPID(Guid.Parse(ApplicationId));
                List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
                bool lostModeDevicePresent = false;
                if (dt.Rows.Count > 0)
                {
                    for (int a = 0; a < dt.Rows.Count; a++)
                    {
                        MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                        my_MQ_IpadCommandList.uDID = dt.Rows[a]["MachineUDID"].ToString();
                        my_MQ_IpadCommandList.payloadIdentifier_AddOn = dt.Rows[a]["MachineSerial"].ToString();
                        my_MQ_IpadCommandList.app_identifier = dtApp.Rows[0]["Identifier"].ToString(); ;
                        my_MQ_IpadCommandList.commandName = Enum_CommandName.RemoveApplication;
                        my_MQ_IpadCommandList.filePath = dtApp.Rows[0]["Path"].ToString();
                        my_MQ_IpadCommandList.Mprio = Convert.ToInt32(ConfigHelper.MQSecondPrior);
                        my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);
                        if (bool.Parse(dt.Rows[a]["LostModeEnabled"].ToString()))
                        {
                            lostModeDevicePresent = true;
                        }
                    }

                    if (lostModeDevicePresent)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "Alert", "alert('Certain device(s) are in lost mode. Command will not be sent to the device.');", true);
                    }
                    if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Remove Application Command");
                        ErrorText = "Failed to Send the Remove Application Command";
                        accessAudit = false;
                    }
                    else
                    {
                        ErrorText = "Successful Send Remove Application command.";
                        accessAudit = true;
                    }
                    //foreach (var item in my_MQ_IpadCommandList_List)
                    //{
                    //    En_MDMAppInstallationSummary summary = new En_MDMAppInstallationSummary()
                    //    {
                    //        ID = Guid.NewGuid(),
                    //        AppID = Guid.Parse(ApplicationId),
                    //        MachineID = item.uDID,
                    //        Status = Convert.ToInt32(accessAudit),
                    //        InstallType = 2,
                    //        CreatedBy = _UserId,
                    //        CreatedDate = DateTime.Now,
                    //    };
                    //    My_FMDM_AppBase.AppInstallSummaryInsertIntoDb(summary);
                    //}
                }

            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMRemoveApps, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMiOSTabGroup.By_Branch;
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostSingleAppModeByBranch(string ApplicationId, string BranchId, bool allowBackup)
    {
        try
        {
            bool accessAudit = false;
            DataTable dt = My_FMDM_MachineBase.getMachine_byBranch(Guid.Parse(BranchId));
            if (dt.Rows.Count == 0)
            {
                ErrorText = "No Machines inside this branch!";
                accessAudit = false;
            }
            else
            {
                DataTable dtApp = My_FMDM_AppBase.GetMDM_APP_APPID(Guid.Parse(ApplicationId));
                List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
                bool lostModeDevicePresent = false;
                if (dt.Rows.Count > 0)
                {
                    for (int a = 0; a < dt.Rows.Count; a++)
                    {
                        MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                        my_MQ_IpadCommandList.uDID = dt.Rows[a]["MachineUDID"].ToString();
                        my_MQ_IpadCommandList.payloadIdentifier_AddOn = dt.Rows[a]["MachineSerial"].ToString();
                        my_MQ_IpadCommandList.app_identifier = dtApp.Rows[0]["Identifier"].ToString();
                        my_MQ_IpadCommandList.commandName = Enum_CommandName.InstallApplication;
                        my_MQ_IpadCommandList.filePath = dtApp.Rows[0]["Path"].ToString();
                        my_MQ_IpadCommandList.Mprio = Convert.ToInt32(ConfigHelper.MQSecondPrior);
                        my_MQ_IpadCommandList.commandName = allowBackup ? Enum_CommandName.InstallApplication : Enum_CommandName.InstallApplicationNoBackup;
                        my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);
                        if (bool.Parse(dt.Rows[a]["LostModeEnabled"].ToString()))
                        {
                            lostModeDevicePresent = true;
                        }
                    }

                    if (lostModeDevicePresent)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "Alert", "alert('Certain device(s) are in lost mode. Command will not be sent to the device.');", true);
                    }
                    if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Remove Application Command");
                        ErrorText = "Failed to Send the Remove Application Command";
                        accessAudit = false;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(15000);
                        my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
                        for (int a = 0; a < dt.Rows.Count; a++)
                        {

                            MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                            my_MQ_IpadCommandList.uDID = dt.Rows[a]["MACHINEUDID"].ToString();
                            my_MQ_IpadCommandList.payloadIdentifier_AddOn = dt.Rows[a]["MachineSerial"].ToString();
                            my_MQ_IpadCommandList.app_identifier = dtApp.Rows[0]["Identifier"].ToString();
                            my_MQ_IpadCommandList.commandName = Enum_CommandName.SingleAppMode;
                            my_MQ_IpadCommandList.Mprio = Convert.ToInt32(ConfigHelper.MQSecondPrior);
                            my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);
                            if (bool.Parse(dt.Rows[a]["LostModeEnabled"].ToString()))
                            {
                                lostModeDevicePresent = true;
                            }
                            else
                            {
                                My_FMonitoringBase.UpdateSingleAppMode(dt.Rows[a]["MachineSerial"].ToString(), true);
                            }
                        }

                        if (lostModeDevicePresent)
                        {
                            //.RegisterStartupScript(this, typeof(string), "Alert", "alert('Certain device(s) are in lost mode. Command will not be sent to the device.');", true);
                        }

                        if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                        {
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Single App Mode Command");

                            ErrorText = "Failed to Send the Single App Mode Command";
                            accessAudit = false;
                        }
                        else
                        {
                            ErrorText = "Successful Send Single App Mode Command";
                            accessAudit = true;
                        }
                    }
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMSingleAppMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMiOSTabGroup.By_Branch;
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostCancelSingleAppModeByBranch(string BranchId)
    {
        try
        {
            bool accessAudit = false;
            DataTable dt = My_FMDM_MachineBase.getMachine_byBranch(Guid.Parse(BranchId));
            if (dt.Rows.Count == 0)
            {
                ErrorText = "No Machines inside this branch!";
                accessAudit = false;
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
                    for (int a = 0; a < dt.Rows.Count; a++)
                    {
                        MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                        my_MQ_IpadCommandList.uDID = dt.Rows[a]["MachineUDID"].ToString();
                        my_MQ_IpadCommandList.payloadIdentifier_AddOn = dt.Rows[a]["MachineSerial"].ToString();
                        my_MQ_IpadCommandList.commandName = Enum_CommandName.RemoveSingleAppMode;
                        my_MQ_IpadCommandList.Mprio = Convert.ToInt32(ConfigHelper.MQSecondPrior);
                        my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);
                        My_FMonitoringBase.UpdateSingleAppMode(dt.Rows[a]["MachineSerial"].ToString(), false);
                    }
                    if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Remove Single App Mode Command");
                        ErrorText = "Failed to Send the Remove Single App Mode Command";
                        accessAudit = false;
                    }
                    else
                    {
                        ErrorText = "Successful Send Remove Single App Mode Command";
                        accessAudit = true;
                    }
                }
            }
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Send_iOSMDMSingleAppMode, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            TempData["CurrentTab"] = MDMiOSTabGroup.By_Branch;
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostChangeGrid(string name)
    {
        TempData["CurrentTab"] = EnumHelper.ParseEnum<MDMiOSTabGroup>(name);
        return new JsonResult(new { message = "Success" });
    }
}
