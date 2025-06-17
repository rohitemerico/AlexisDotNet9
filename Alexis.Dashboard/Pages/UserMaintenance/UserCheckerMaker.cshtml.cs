using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Module = Dashboard.Common.Business.Component.Module;

namespace Alexis.Dashboard.Pages.UserMaintenance;

public class UserCheckerMakerModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private readonly GlobalController globalController = new GlobalController();
    private readonly UserRoleController urControl = new UserRoleController();

    [BindProperty]
    public string EntityIdToApprove { get; set; }
    [BindProperty]
    public string TargetTabToApprove { get; set; }
    [BindProperty]
    public string RejectRemarks { get; set; }
    [BindProperty]
    public string EntityIdToReject { get; set; }
    [BindProperty]
    public string TargetTabToReject { get; set; }
    [BindProperty]
    public string EntityIdToDecline { get; set; }
    [BindProperty]
    public string TargetTabToDecline { get; set; }

    public string ActivateTab { get => "show active"; }
    public TabGroup CurrentTab { get; set; }
    public required List<RoleModel> Roles { get; set; }
    public required List<BranchViewModel> Branches { get; set; }
    public required List<UserViewModel> Users { get; set; }
    public string? ClientIp { get; set; }

    public bool RoleVisible { get; set; }
    public bool BranchVisible { get; set; }
    public bool UserVisible { get; set; }

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
        checkAuthorization(ModuleLogAction.View_UserCheckerMaker);
    }

    public IActionResult OnGet()
    {
        try
        {
            if (TempData["CurrentTab"] != null)
                CurrentTab = (TabGroup)TempData["CurrentTab"];
            else CurrentTab = TabGroup.Role;
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

    public IActionResult OnPostConfirmApprove()
    {
        bool passExecution = false;
        string id = EntityIdToApprove;
        string tab = TargetTabToApprove;
        string remark = "";
        var checkerAction = CheckerAction.Approve;
        try
        {
            if (Guid.TryParse(id, out Guid guidOutput))
            {
                var dt = globalController.GetMakerActionOnPending(guidOutput);
                if (dt.Rows.Count == 1)
                {
                    string makerAction = dt.Rows[0]["ACTIONNAME"].ToString();
                    string newValues = dt.Rows[0]["NEWVALUES"].ToString();
                    var isSuccessful = UpdateDbByAction(makerAction, checkerAction, guidOutput, remark, tab, newValues);
                    passExecution = isSuccessful;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            var (success, message) = passExecution ? (true, "You have approved the user checker maker successfully.") : (false, "Failed to approve the user checker maker.");
            TempData["ModalTitle"] = success ? "Success!" : "Fail!";
            TempData["ModalMessage"] = message;
            ApproveDeclineAudit(id, checkerAction, tab, success);
            TempData["ShowModal"] = true;
            CurrentTab = (TabGroup)Enum.Parse(typeof(TabGroup), tab);
            TempData["CurrentTab"] = CurrentTab;
        }
        BindGridData();
        return RedirectToPage();
    }

    public IActionResult OnPostConfirmReject()
    {
        bool passExecution = false;
        //Initialize
        string id = EntityIdToReject;
        string tab = TargetTabToReject;
        string remark = RejectRemarks;
        CheckerAction checkerAction = CheckerAction.Reject;

        try
        {
            //check data validity 
            Guid guidOutput = new Guid();
            bool isValidId = Guid.TryParse(id, out guidOutput);
            if (isValidId)
            {
                var dt = globalController.GetMakerActionOnPending(guidOutput);
                if (dt.Rows.Count == 1) //smtg went wrong
                {
                    string makerAction = dt.Rows[0]["ACTIONNAME"].ToString();
                    var isSuccessful = UpdateDbByAction(makerAction, checkerAction, guidOutput, remark, tab, null);
                    if (isSuccessful) passExecution = true;
                }
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
        finally
        {
            var (title, message, success) = passExecution ? ("Success!", "You have rejected the user checker maker successfully.", true) : ("Fail!", "Failed to reject the user checker maker.", false);
            TempData["ModalTitle"] = title;
            TempData["ModalMessage"] = message;
            ApproveDeclineAudit(id, checkerAction, tab, success);
            TempData["ShowModal"] = true;
            RejectRemarks = "";//Clear Textbox
            CurrentTab = (TabGroup)Enum.Parse(typeof(TabGroup), tab);
            TempData["CurrentTab"] = CurrentTab;
        }
        BindGridData();
        return RedirectToPage();
    }

    public IActionResult OnPostConfirmDecline()
    {
        bool passExecution = false;
        //Initialize
        string id = EntityIdToDecline;
        string tab = TargetTabToDecline;
        string remark = RejectRemarks;
        CheckerAction checkerAction = CheckerAction.Reject;

        try
        {
            //check data validity 
            Guid guidOutput = new Guid();
            bool isValidId = Guid.TryParse(id, out guidOutput);

            if (isValidId)
            {
                var dt = globalController.GetMakerActionOnPending(guidOutput);
                if (dt.Rows.Count == 1)
                {
                    string makerAction = dt.Rows[0]["ACTIONNAME"].ToString();
                    var isSuccessful = UpdateDbByAction(makerAction, checkerAction, guidOutput, remark, tab, null);
                    if (isSuccessful) passExecution = true;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            var (title, message, auditSuccess) = passExecution ? ("Success!", "You have declined the user checker maker successfully.", true) : ("Fail!", "Failed to decline the user checker maker.", false);
            TempData["ModalTitle"] = title;
            TempData["ModalMessage"] = message;
            ApproveDeclineAudit(id, checkerAction, tab, auditSuccess);
            TempData["ShowModal"] = true;
            RejectRemarks = "";
            CurrentTab = (TabGroup)Enum.Parse(typeof(TabGroup), tab);
            TempData["CurrentTab"] = CurrentTab;
        }
        BindGridData();
        return RedirectToPage();
    }

    protected void ApproveDeclineAudit(string targetEntity, CheckerAction checkerAction, string group, bool actionSuccess)
    {
        try
        {
            var thisGroup = (TabGroup)Enum.Parse(typeof(TabGroup), group);

            var moduleLogAction = new ModuleLogAction();

            switch (thisGroup)
            {
                case TabGroup.Role:
                    if (checkerAction == CheckerAction.Approve)
                        moduleLogAction = ModuleLogAction.Approve_RoleMaintenance;

                    if (checkerAction == CheckerAction.Reject)
                        moduleLogAction = ModuleLogAction.Decline_RoleMaintenance;
                    break;
                case TabGroup.User:
                    if (checkerAction == CheckerAction.Approve)
                        moduleLogAction = ModuleLogAction.Approve_UserMaintenance;

                    if (checkerAction == CheckerAction.Reject)
                        moduleLogAction = ModuleLogAction.Decline_UserMaintenance;
                    break;
                case TabGroup.Branch:
                    if (checkerAction == CheckerAction.Approve)
                        moduleLogAction = ModuleLogAction.Approve_BranchMaintenance;

                    if (checkerAction == CheckerAction.Reject)
                        moduleLogAction = ModuleLogAction.Decline_BranchMaintenance;
                    break;
                default:
                    break;
            }

            string note = "";
            if (checkerAction == CheckerAction.Approve)
                note = $"{group} , {targetEntity} _Record changed!";
            else
                note = $"{group} , {targetEntity} _Record remains unchanged!";

            AuditLog.CreateAuditLog(note, AuditCategory.System_User_Maintenance, moduleLogAction, _UserId, actionSuccess, ClientIp);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    protected bool UpdateDbByAction(string makerAction, CheckerAction checkerAction, Guid CheckMakeId, string remarks, string group, string? newValuesToUpdate)
    {
        var forwardedHeader = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        ClientIp = forwardedHeader ?? HttpContext.Connection.RemoteIpAddress?.ToString();
        bool passExecution = false;
        bool pass1 = false;
        bool pass2 = false;

        switch (makerAction)
        {
            case nameof(MakerAction.Create):
                if (checkerAction == CheckerAction.Approve)  //approve create
                {
                    switch (group) //update data status only
                    {
                        case nameof(TabGroup.Role):
                            pass1 = urControl.ApproveRoleById(CheckMakeId, _UserId);
                            break;
                        case nameof(TabGroup.Branch):
                            pass1 = BranchController.approveBizHour(CheckMakeId, _UserId);
                            break;
                        case nameof(TabGroup.User):
                            pass1 = urControl.ApproveUserById(CheckMakeId, _UserId);
                            break;
                    }

                    if (pass1)
                        pass2 = globalController.ApproveCreating(CheckMakeId, _UserId, remarks, ClientIp); //update tblEditing

                    if (pass1 && pass2) passExecution = true;
                }
                if (checkerAction == CheckerAction.Reject) //reject create
                {
                    switch (group) //delete data
                    {
                        case nameof(TabGroup.Role):
                            pass1 = urControl.DeleteRoleById(CheckMakeId);
                            break;
                        case nameof(TabGroup.Branch):
                            pass1 = BranchController.deleteBizHour(CheckMakeId);
                            break;
                        case nameof(TabGroup.User):
                            pass1 = urControl.DeleteUserById(CheckMakeId);
                            break;
                    }
                    if (pass1)
                        pass2 = globalController.RejectCreating(CheckMakeId, _UserId, remarks, ClientIp); //update tblEditing

                    if (pass1 && pass2) passExecution = true;
                }
                break;
            case nameof(MakerAction.Update):
                if (checkerAction == CheckerAction.Approve) //approve update
                {
                    switch (group) //update data
                    {
                        case nameof(TabGroup.Role):
                            pass1 = urControl.ApproveRoleById(CheckMakeId, _UserId);
                            break;
                        case nameof(TabGroup.Branch):
                            pass1 = BranchController.approveBizHour(CheckMakeId, _UserId);
                            break;
                        case nameof(TabGroup.User):
                            pass1 = urControl.ApproveUserById(CheckMakeId, _UserId);
                            break;
                    }
                    if (pass1)
                        pass2 = globalController.ApproveEditing(CheckMakeId, _UserId, remarks, ClientIp); //update tblEditing

                    if (pass1 && pass2) passExecution = true;
                }
                if (checkerAction == CheckerAction.Reject) //reject update
                {
                    passExecution = globalController.DeclineEditing(CheckMakeId, _UserId, remarks, ClientIp); //update tblEditing
                }
                break;
        }

        return passExecution;
    }

    public void BindGridData()
    {
        string currentUser = UserDetails.Rows[0]["uName"].ToString();
        Guid Moduleid = Guid.Empty;
        switch (CurrentTab)
        {
            case TabGroup.Role:
                DataTable data1 = CheckerMakerController.GetRolesCM(UserDetails);
                Roles = Common.DataHelper.ConvertDataTableToList<RoleModel>(data1);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_RoleMaintenance);
                getModulePermissionByModuleID(Moduleid);
                if (View) RoleVisible = true;
                foreach (RoleModel item in Roles)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.rID));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.rID));
                    if (Checker)
                    {
                        if (item.Status == "Pending" && item.CREATEDBY != UserDetails.Rows[0]["uName"].ToString())
                        {
                            item.AllowView = true;
                            item.AllowApprove = true;
                            item.AllowReject = true;
                        }
                        else if (editing && (editor != UserDetails.Rows[0]["uName"].ToString()))
                        {
                            item.AllowView = true;
                            item.AllowDecline = true;
                        }
                    }
                }
                break;
            case TabGroup.Branch:
                DataTable data2 = CheckerMakerController.GetBranchCM(UserDetails);
                Branches = Common.DataHelper.ConvertDataTableToList<BranchViewModel>(data2);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_BranchMaintenance);
                getModulePermissionByModuleID(Moduleid);
                if (View) BranchVisible = true;
                foreach (BranchViewModel item in Branches)
                {
                    item.Sunday = item.bSunday == false ? 0 : 1;
                    item.Monday = item.bMonday == false ? 0 : 1;
                    item.Tuesday = item.bTuesday == false ? 0 : 1;
                    item.Wednesday = item.bWednesday == false ? 0 : 1;
                    item.Thursday = item.bThursday == false ? 0 : 1;
                    item.Friday = item.bFriday == false ? 0 : 1;
                    item.Saturday = item.bSaturday == false ? 0 : 1;
                    bool editing = new GlobalController().IsEditingDataExist(item.BID);
                    string editor = new GlobalController().GetEditorIfEditingDataExist(item.BID);

                    if (Checker)
                    {
                        if (item.Status == "Pending" && item.CREATEDBY != UserDetails.Rows[0]["uName"].ToString())
                        {
                            item.AllowApprove = true;
                            item.AllowReject = true;
                            item.AllowView = true;
                        }
                        else if (editing && (editor != UserDetails.Rows[0]["uName"].ToString()))
                        {
                            item.AllowView = true;
                            item.AllowDecline = true;
                        }
                    }
                }
                break;
            case TabGroup.User:
                DataTable data3 = CheckerMakerController.GetUserCM(UserDetails);
                Users = Common.DataHelper.ConvertDataTableToList<UserViewModel>(data3);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_UserMaintenance);
                getModulePermissionByModuleID(Moduleid);
                if (View) UserVisible = true;
                foreach (UserViewModel item in Users)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.aID));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.aID));
                    if (Checker)
                    {
                        if (item.Status == "Pending" && item.CreatedByName != UserDetails.Rows[0]["uName"].ToString())
                        {
                            item.AllowApprove = true;
                            item.AllowReject = true;
                            item.AllowView = true;
                        }
                        else if (editing && (editor != UserDetails.Rows[0]["uName"].ToString()))
                        {
                            item.AllowView = true;
                            item.AllowDecline = true;
                        }
                    }
                }
                break;
        }
    }

    public IActionResult OnGetView()
    {
        HttpContext.Session.SetString("lblcheckermaker", "User");
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostChangeGrid(string name)
    {
        TempData["CurrentTab"] = EnumHelper.ParseEnum<TabGroup>(name);
        return new JsonResult(new { message = "Success" });
    }
}