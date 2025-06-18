using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Profile;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMiOS;

public class CheckerMakerModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    public static UserManageBase My_UserBase = UserManageFactory.Create("");
    public static MDM_ProfileBase My_FMDM_ProfileBase = MDM_ProfileFactory.Create("");


    public string ClientIp { get; set; }

    [BindProperty]
    public string aProfileId { get; set; }
    [BindProperty]
    public string aCProfileId { get; set; }
    [BindProperty]
    public string aStatus { get; set; }
    [BindProperty]
    public string ProfileId { get; set; }
    [BindProperty]
    public string CProfileId { get; set; }
    [BindProperty]
    public string Status { get; set; }
    [BindProperty]
    public string RejectRemarks { get; set; }

    public List<iOSMDMProfileViewModel> MDMProfiles { get; set; }
    private static readonly int[] sourceArray = new[] { 0, 1, 2, 3 };

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var forwardedHeader = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        ClientIp = forwardedHeader ?? (HttpContext.Connection.RemoteIpAddress?.ToString());
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_iOSMDMCheckerMaker);
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
        MDM_Profile_General gen = new MDM_Profile_General();
        DataTable general = My_FMDM_ProfileBase.GetAllProfile();

        MDMProfiles = Common.DataHelper.ConvertDataTableToList<iOSMDMProfileViewModel>(general)
            .Where(x => sourceArray.Contains(x.pStatus))
            .ToList();

        foreach (var item in MDMProfiles)
        {
            switch (item.Status?.ToUpperInvariant())
            {
                case "ACTIVE":
                    item.AllowApprove = false;
                    item.AllowReject = true;
                    break;

                case "INACTIVE":
                    item.AllowApprove = true;
                    item.AllowReject = false;
                    break;
                case "PENDING":
                    item.AllowApprove = true;
                    item.AllowReject = true;
                    break;
                case "EDITED":
                    item.AllowApprove = true;
                    item.AllowReject = true;
                    break;

                default:
                    item.AllowApprove = false;
                    item.AllowReject = false;
                    break;
            }
        }
    }

    public IActionResult OnPostConfirmApprove()
    {
        bool passExecution = false;

        try
        {
            string profileId = aProfileId;
            string cProfileId = aCProfileId;
            string profileStatus = aStatus;
            passExecution = ProfileDeclineOrApprove(profileId, cProfileId, profileStatus, "APPROVE");
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        string errortext = passExecution ? "You have approved the checker maker successfully." : "Failed to approve the checker maker.";
        PopUp(passExecution ? "Success!" : "Fail!", errortext);

        BindGridData();
        return RedirectToPage();
    }

    public IActionResult OnPostConfirmReject()
    {
        bool passExecution = false;


        try
        {
            //check data validity 
            string profileId = ProfileId;
            string cProfileId = CProfileId;
            string profileStatus = Status;
            string remark = RejectRemarks;
            passExecution = ProfileDeclineOrApprove(profileId, cProfileId, profileStatus, "DECLINE");
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        string errortext = passExecution ? "You have rejected the checker maker successfully." : "Failed to reject the checker maker.";
        PopUp(passExecution ? "Success!" : "Fail!", errortext);
        BindGridData();
        return RedirectToPage();
    }

    private bool ProfileDeclineOrApprove(string profileId, string cProfileId, string profileStatus, string action)
    {
        bool actionPass = false;

        try
        {
            Guid PID = Guid.Parse(profileId); //unique id
            Guid CPID = Guid.Parse(cProfileId); //id pointing to the latest or active profile (note, old versions kept in db) 

            string status = profileStatus;

            switch (action.ToUpper())
            {
                case "APPROVE":
                    MDM_Profile_General general_approve = new MDM_Profile_General();
                    if (status.ToUpper() == "INACTIVE")
                    {
                        general_approve.pStatus = 1;
                        general_approve.Profile_ID = PID;
                        general_approve.LastUpdateDate = DateTime.Now;
                        general_approve.LastUpdateBy = _UserId;
                        general_approve.CProfileId = general_approve.Profile_ID;

                        actionPass = My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_approve, "CHECKERMAKER");
                        auditTemplate(actionPass, "APPROVE", "PROFILE");
                    }
                    else if (status.ToUpper() == "EDITED")
                    {
                        general_approve.pStatus = 1;
                        general_approve.Profile_ID = PID;
                        general_approve.LastUpdateDate = DateTime.Now;
                        general_approve.LastUpdateBy = _UserId;
                        general_approve.CProfileId = general_approve.Profile_ID;


                        MDM_Profile_General general_approve_old = new MDM_Profile_General();
                        general_approve_old.CProfileId = general_approve.Profile_ID;
                        general_approve_old.Profile_ID = CPID;
                        general_approve_old.pStatus = 4;
                        general_approve_old.LastUpdateDate = DateTime.Now;
                        general_approve_old.LastUpdateBy = _UserId;
                        My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_approve_old, "CHECKERMAKER");

                        MDM_Profile_General_BranchID general_BranchID = new MDM_Profile_General_BranchID();
                        general_BranchID.Profile_ID = general_approve.Profile_ID;
                        general_BranchID.cProfile_ID = general_approve.Profile_ID;

                        My_FMDM_ProfileBase.UpdateProfileGeneralBranchByID(general_BranchID);

                        MDM_Profile_General_BranchID general_BranchID_old = new MDM_Profile_General_BranchID();
                        general_BranchID_old.Profile_ID = CPID;

                        general_BranchID_old.cProfile_ID = general_approve.Profile_ID;

                        My_FMDM_ProfileBase.UpdateProfileGeneralBranchByID(general_BranchID_old);

                        actionPass = My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_approve, "CHECKERMAKER");
                        auditTemplate(actionPass, "APPROVE", "EDIT");
                    }
                    else if (status.ToUpper() == "PENDING")
                    {
                        general_approve.pStatus = 1;
                        general_approve.Profile_ID = PID;
                        general_approve.CProfileId = general_approve.Profile_ID;

                        actionPass = My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_approve, "CHECKERMAKER");
                        auditTemplate(actionPass, "APPROVE", "PROFILE");
                    }
                    break;

                case "DECLINE":
                    MDM_Profile_General general_decline = new MDM_Profile_General();
                    if (status.ToUpper() == "ACTIVE")
                    {
                        general_decline.pStatus = 2;
                        general_decline.Profile_ID = PID;
                        general_decline.LastUpdateDate = DateTime.Now;
                        general_decline.LastUpdateBy = _UserId;

                        actionPass = My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_decline, "CheckerMaker");
                        auditTemplate(actionPass, "DECLINE", "PROFILE");
                    }
                    else if (status.ToUpper() == "EDITED")
                    {
                        general_decline.pStatus = 4;
                        general_decline.Profile_ID = PID;
                        general_decline.LastUpdateDate = DateTime.Now;
                        general_decline.LastUpdateBy = _UserId;
                        general_decline.CProfileId = CPID;
                        //My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_decline, "CHECKERMAKER");

                        MDM_Profile_General general_decline_old = new MDM_Profile_General();
                        general_decline_old.Profile_ID = CPID;
                        general_decline_old.CProfileId = general_decline_old.Profile_ID;
                        general_decline_old.pStatus = 1;
                        general_decline_old.LastUpdateDate = DateTime.Now;
                        general_decline_old.LastUpdateBy = _UserId;
                        My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_decline_old, "CHECKERMAKER");

                        actionPass = My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_decline, "CheckerMaker");
                        auditTemplate(actionPass, "DECLINE", "EDIT");
                    }
                    else if (status.ToUpper() == "PENDING")
                    {
                        general_decline.pStatus = 2;
                        general_decline.Profile_ID = PID;

                        actionPass = My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_decline, "CheckerMaker");
                        auditTemplate(actionPass, "DECLINE", "PROFILE");
                    }
                    break;
            }
        }

        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                       System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                       System.Reflection.MethodBase.GetCurrentMethod().Name,
                       ex);


        }

        return actionPass;
    }

    public void auditTemplate(bool isSuccess, string CreateUpdate, string auditType)
    {
        try
        {
            string message = "";
            if (auditType == "PROFILE")
            {
                message = isSuccess == true ? "Profile has been successfully " + CreateUpdate.ToLower() + "d" : "Failed to " + CreateUpdate.ToLower() + " a profile";
            }

            else if (auditType == "EDIT")
            {
                if (CreateUpdate.ToUpper() == "APPROVE")
                {
                    message = isSuccess == true ? "Edited profile has been approved" : "Failed to approve an edited profile";
                }
                else if (CreateUpdate.ToUpper() == "DECLINE")
                {
                    message = isSuccess == true ? "Edited profile has been declined" : "Failed to declined an edited profile";

                }
            }

            AuditLog.CreateAuditLog(message, AuditCategory.MDM_iOS, CreateUpdate == "APPROVE" ? ModuleLogAction.Approve_iOSMDMProfile : ModuleLogAction.Decline_iOSMDMProfile, _UserId, isSuccess, ClientIp);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
}