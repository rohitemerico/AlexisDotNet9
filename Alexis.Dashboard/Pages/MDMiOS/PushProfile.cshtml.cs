using System.Data;
using Alexis.Common;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Enroll;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Machine;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_MessageQueue;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Profile;
using MDM.iOS.Entities.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMiOS;

public class PushProfileModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    protected static MDM_ProfileBase My_FMDM_ProfileBase = MDM_ProfileFactory.Create("");
    protected static MonitoringBase My_FMonitoringBase = MonitoringFactory.Create("");
    protected static MDM_EnrollmentBase My_FMDMEnrollmentBase = MDM_EnrollmentFactory.Create("");
    protected static MDM_MachineBase My_FMDM_MachineBase = MDM_MachineFactory.Create("");
    public string ActivateTab { get => "show active"; }
    public MDMiOSTabGroup CurrentTab { get; set; }
    public required List<iOSMDMDevicesViewModel> Devices { get; set; }
    public required List<iOSMDMBranchViewModel> Branches { get; set; }
    public List<iOSMDMProfileViewModel> Profiles { get; set; }
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
        checkAuthorization(ModuleLogAction.View_iOSMDMPushProfile);
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
        switch (CurrentTab)
        {
            case MDMiOSTabGroup.By_Device:
                DataTable data1 = My_FMonitoringBase.GetDeviceDetails_All_Active(new List<Params>());
                Devices = Common.DataHelper.ConvertDataTableToList<iOSMDMDevicesViewModel>(data1);
                break;
            case MDMiOSTabGroup.By_Branch:
                DataTable data2 = My_FMonitoringBase.GetBranchesWithProfiles(new List<Params>());
                Branches = Common.DataHelper.ConvertDataTableToList<iOSMDMBranchViewModel>(data2);
                break;
        }
        DataTable data3 = My_FMDM_ProfileBase.GetProfiles();
        Profiles = Common.DataHelper.ConvertDataTableToList<iOSMDMProfileViewModel>(data3);
    }

    public IActionResult OnPostPushProfileBranch(string BranchId, string ProfileId)
    {
        bool accessAudit;
        try
        {
            if (string.IsNullOrEmpty(ProfileId))
            {
                ErrorText = "No profile selected";
                accessAudit = false;
            }
            else
            {
                List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();

                DataTable security = My_FMDMEnrollmentBase.GetData_EnrollmentDataByProfileID(Guid.Parse(ProfileId));

                // get the available machines 
                DataTable Machines = My_FMDM_MachineBase.getMachine_byBranch(Guid.Parse(BranchId));

                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Machine Count Number " + Machines.Rows.Count.ToString());


                for (int m = 0; m < Machines.Rows.Count; m++)
                {
                    MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                    my_MQ_IpadCommandList.uDID = Machines.Rows[m]["MachineUDID"].ToString();
                    my_MQ_IpadCommandList.payloadIdentifier_AddOn = Machines.Rows[m]["MachineSerial"].ToString();
                    my_MQ_IpadCommandList.ipadProfile_ID = Guid.Parse(ProfileId);
                    my_MQ_IpadCommandList.commandName = Enum_CommandName.InstallProfile_Restriction;
                    my_MQ_IpadCommandList.securityType = security.Rows[0]["Security"].ToString();
                    string prio = ConfigHelper.MQSecondPrior;
                    my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
                    my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);
                }

                if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the remove Profile APNS Command");
                    ErrorText = $"Failed to push profile '{ProfileId}' to {BranchId}.";
                    accessAudit = false;
                }

                else
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Successfully sent Profile APNS Command: " + my_MQ_IpadCommandList_List);
                    ErrorText = $"Successfully push profile '{ProfileId}' to {BranchId}.";
                    accessAudit = true;
                }
            }

        }
        catch (Exception ex)
        {
            ErrorText = $"Failed to push profile '{ProfileId}' to {BranchId}.";
            accessAudit = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
        AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Push_iOSMDMPushProfile, _UserId, accessAudit, ClientIp);
        PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        TempData["CurrentTab"] = MDMiOSTabGroup.By_Branch;
        return Redirect("PushProfile");
    }

    public IActionResult OnPostPushProfileDevice(string MachineSerial, string ProfileId)
    {
        bool accessAudit;
        try
        {
            if (string.IsNullOrEmpty(ProfileId))
            {
                ErrorText = "No profile selected";
                accessAudit = false;
            }
            else
            {
                List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
                // we need to get the index of the branch that corresponds to the index where we select the
                // drop down list for the profiles.  
                // get the enrollment data based on the profile ID
                DataTable security = My_FMDMEnrollmentBase.GetData_EnrollmentDataByProfileID(Guid.Parse(ProfileId));
                // we need to get the IPad's UDID and other specific information using the specific SQL query 
                List<Params> myParams = new List<Params>();
                myParams.Add(new Params("@machineserial", "LIKE", MachineSerial));
                DataTable dt_ret = My_FMonitoringBase.GetDeviceDetails_All_Active(myParams);



                MQ_IpadCommandList my_MQ_IpadCommandList = new MQ_IpadCommandList();
                my_MQ_IpadCommandList.uDID = dt_ret.Rows[0]["MachineUDID"].ToString();
                my_MQ_IpadCommandList.payloadIdentifier_AddOn = MachineSerial;
                my_MQ_IpadCommandList.ipadProfile_ID = Guid.Parse(ProfileId);
                my_MQ_IpadCommandList.commandName = Enum_CommandName.InstallProfile_Restriction;
                my_MQ_IpadCommandList.securityType = security.Rows[0]["Security"].ToString();
                string prio = ConfigHelper.MQSecondPrior;
                my_MQ_IpadCommandList.Mprio = Convert.ToInt32(prio);
                my_MQ_IpadCommandList_List.Add(my_MQ_IpadCommandList);


                if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to Send the Install Application Command");
                    ErrorText = $"Failed to push profile '{ProfileId}' to {MachineSerial}.";
                    accessAudit = false;
                }

                else
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Push Profile by Machine Successful");
                    ErrorText = $"Successfully push profile '{ProfileId}' to {MachineSerial}.";
                    accessAudit = true;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorText = $"Failed to push profile '{ProfileId}' to {MachineSerial}.";
            accessAudit = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
        AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Push_iOSMDMPushProfile, _UserId, accessAudit, ClientIp);
        PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        TempData["CurrentTab"] = MDMiOSTabGroup.By_Device;
        return Redirect("PushProfile");
    }

    public IActionResult OnPostChangeGrid(string name)
    {
        TempData["CurrentTab"] = EnumHelper.ParseEnum<MDMiOSTabGroup>(name);
        return new JsonResult(new { message = "Success" });
    }
}