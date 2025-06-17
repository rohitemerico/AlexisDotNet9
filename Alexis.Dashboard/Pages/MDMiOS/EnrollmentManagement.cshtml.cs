using System.Data;
using Alexis.Common;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Enroll;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Machine;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_MessageQueue;
using MDM.iOS.Entities;
using MDM.iOS.Entities.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Alexis.Dashboard.Pages.MDMiOS;

public class EnrollmentManagementModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public static MDM_EnrollmentBase My_FMDMEnrollmentBase = MDM_EnrollmentFactory.Create("");
    protected static UserManageBase My_FUserManageBase = UserManageFactory.Create("");
    public static MDM_MachineBase My_FMachineBase = MDM_MachineFactory.Create("");
    public List<EnrollmentViewModel> Enrollments { get; set; }
    public List<Branches> Branches { get; set; }
    public string? ClientIp { get; set; }
    private string ErrorText { get; set; }

    private bool accessAudit;

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
        checkAuthorization(ModuleLogAction.View_iOSMDMEnrollmentManagement);
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
        DataTable data = My_FMDMEnrollmentBase.GetData_Enrollment();
        Enrollments = Common.DataHelper.ConvertDataTableToList<EnrollmentViewModel>(data);
        if (Enrollments.Any())
        {
            foreach (var item in Enrollments)
            {
                item.BranchId = item.StrStatus == "PROFILE MISSING" ? null : item.BranchId;
            }
        }
        DataTable data1 = My_FUserManageBase.getBranchwithAssignRestriction(null);
        Branches = Common.DataHelper.ConvertDataTableToList<Branches>(data1);
    }

    public IActionResult OnPostApprove(string BranchId, string MDMId, string SerialNo)
    {
        try
        {
            accessAudit = false;
            En_Enrollment En_Enrollment = new En_Enrollment();
            Guid w_mdmID = Guid.Parse(MDMId);
            string Serial = SerialNo;

            DataTable dt = new DataTable();
            En_Enrollment.MdmStatus = 1;
            En_Enrollment.BranchID = Guid.Parse(BranchId);
            En_Enrollment.MDMID = w_mdmID;
            En_Enrollment.LastApprovedDateTime = DateTime.Now;
            En_Enrollment.LastApprovedUser = _UserId;
            En_Enrollment.MdmAllowEnrollStatus = 1;

            DeviceEn Machine = new DeviceEn();
            Machine.BranchId = Guid.Parse(BranchId);
            Machine.DeviceSerial = Serial.Trim();

            if (My_FMDMEnrollmentBase.Enrollment_UpdateStatus(En_Enrollment))
            {
                if (My_FMachineBase.UpdateMachineBranchIDOnly(Machine))
                {
                    string signed_mobileConfig = string.Empty;
                    // verify and check if the particular enrollment data exists in the database.
                    dt = My_FMDMEnrollmentBase.GetData_Enrollment(En_Enrollment);

                    // if there is exactly one matching record 
                    if (dt.Rows.Count == 1)
                    {
                        string UDID = dt.Rows[0]["UDID"].ToString();
                        dt.Clear();

                        // this will be the message queue. 
                        List<MQ_IpadCommandList> my_MQ_IpadCommandList_List = new List<MQ_IpadCommandList>();
                        MQ_IpadCommandList My_MQ_IpadCommadList = new MQ_IpadCommandList();

                        // the command that should be fired by the iPad is for enrollment purposes.
                        My_MQ_IpadCommadList.commandName = Enum_CommandName.ProfileEnrollment;
                        My_MQ_IpadCommadList.uDID = UDID;
                        My_MQ_IpadCommadList.retryCount = 0;
                        My_MQ_IpadCommadList.queueCount = 0;
                        string prio = ConfigHelper.MQFirstPrior;
                        My_MQ_IpadCommadList.Mprio = Convert.ToInt32(prio);

                        // after adding all the properties for the commandlist, we add them in a list. 
                        my_MQ_IpadCommandList_List.Add(My_MQ_IpadCommadList);


                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', JsonConvert.SerializeObject(My_MQ_IpadCommadList));

                        // if the message queue fails to push the messages to the other side, display error message on UI
                        if (!MessageQueue_Fire.PushtoMessage(my_MQ_IpadCommandList_List))
                        {
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to use command devicelock Messaage Queue");
                            ErrorText = "Failed to Enroll a Ipad!";
                            accessAudit = false;
                        }

                        // if pushing the message to message queue is a success, then proceed to following steps. 
                        else
                        {
                            ErrorText = "Successful Update Ipad MDM_Enrollment status!";
                            accessAudit = true;
                        }
                    }
                }
                else
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to select from tblEnrollment", w_mdmID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrorText = "Failed to Enroll a Ipad!";
            accessAudit = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);

        }
        AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Approve_iOSMDMEnrollment, _UserId, accessAudit, ClientIp);
        PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        return Redirect("EnrollmentManagement");
    }

    public IActionResult OnPostDecline(string BranchId, string MDMId, string SerialNo)
    {
        try
        {
            accessAudit = false;
            En_Enrollment En_Enrollment = new En_Enrollment();
            Guid w_mdmID = Guid.Parse(MDMId);
            En_Enrollment.MdmStatus = 0;
            En_Enrollment.MDMID = w_mdmID;
            En_Enrollment.LastRejectDateTime = DateTime.Now;
            En_Enrollment.LastRejectedUser = _UserId;
            En_Enrollment.MdmAllowEnrollStatus = 0;

            if (!My_FMDMEnrollmentBase.Enrollment_UpdateStatus(En_Enrollment))
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Failed to update tblEnrollment Status,set mdmstatus=0", w_mdmID);
                ErrorText = "Failed to Update the Ipad MDM_Enrollment Status!";
                accessAudit = false;
            }
            else
            {
                ErrorText = "Successful Update Ipad MDM_Enrollment status";
                accessAudit = true;
            }
        }
        catch (Exception ex)
        {
            ErrorText = "Failed to Update the Ipad MDM_Enrollment Status!";
            accessAudit = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
        AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ModuleLogAction.Decline_iOSMDMEnrollment, _UserId, accessAudit, ClientIp);
        PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        return Redirect("EnrollmentManagement");
    }
}

public class Branches
{
    public string Profile_Desc { get; set; }
    public Guid Branch_ID { get; set; }
    public string Branch_Desc { get; set; }
}