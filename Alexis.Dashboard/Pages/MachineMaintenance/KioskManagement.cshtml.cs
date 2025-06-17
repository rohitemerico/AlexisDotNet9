using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class KioskManagementModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    //private GlobalController globalController = new GlobalController();
    private AlertMaintenanceController alertController = new AlertMaintenanceController();
    protected CardMaintenanceControl cardController = new CardMaintenanceControl();
    protected DocumentSettingController documentController = new DocumentSettingController();
    protected HopperMaintenanceController hopperController = new HopperMaintenanceController();

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
    public string ClientIp { get; set; }
    public string ActivateTab { get => "show active"; }
    public MachineTabGroup CurrentTab { get; set; }
    public List<AlertViewModel> Alerts { get; set; }
    public List<ApplicationMaintenanceViewModel> Applications { get; set; }
    public List<BusinessMaintenanceViewModel> BusinessMaintenanceList { get; set; }
    public List<CardViewModel> Cards { get; set; }
    public List<KioskMaintenanceViewModel> KioskMaintenanceList { get; set; }
    public List<KioskDocTypeViewModel> DocTypes { get; set; }
    public List<KioskDocTypeViewModel> Docs { get; set; }
    public List<GroupMaintenanceViewModel> GroupMaintenanceList { get; set; }
    public List<HopperMaintenanceViewModel> HopperMaintenanceList { get; set; }

    public bool AlertVisible { get; set; }
    public bool AppVisible { get; set; }
    public bool BizOpVisible { get; set; }
    public bool CardVisible { get; set; }
    public bool KioskCreateVisible { get; set; }
    public bool DocTypeVisible { get; set; }
    public bool DocVisible { get; set; }
    public bool GroupVisible { get; set; }
    public bool HopperVisible { get; set; }


    private Dictionary<string, MachineTabGroup> tabMapping = new Dictionary<string, MachineTabGroup>(StringComparer.OrdinalIgnoreCase)
{
    { "Alert", MachineTabGroup.Alert },
    { "BizOp", MachineTabGroup.Operating_Hour },
    { "Card", MachineTabGroup.Card },
    { "Create", MachineTabGroup.Machine_Management },
    { "DocType", MachineTabGroup.Document_Type },
    { "Document", MachineTabGroup.Document },
    { "Group", MachineTabGroup.Group },
    { "Hopper", MachineTabGroup.Hopper },
    { "App", MachineTabGroup.Application }
};
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
        checkAuthorization(ModuleLogAction.View_KioskManagement);
    }

    public IActionResult OnGet()
    {
        try
        {
            if (TempData["CurrentTab"] != null)
                CurrentTab = (MachineTabGroup)TempData["CurrentTab"];
            else CurrentTab = MachineTabGroup.Alert;
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
        string currentUser = UserDetails.Rows[0]["uName"].ToString();
        Guid Moduleid = Guid.Empty;
        switch (CurrentTab)
        {
            case MachineTabGroup.Alert:
                DataTable data1 = CheckerMakerController.getAlertListCM(UserDetails);
                Alerts = Common.DataHelper.ConvertDataTableToList<AlertViewModel>(data1);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_Alert_Template);
                getModulePermissionByModuleID(Moduleid);
                if (View) AlertVisible = true;
                foreach (AlertViewModel item in Alerts)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.aID));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.aID));
                    if (Checker)
                    {
                        if ((item.AlertStatus == "Pending") && item.CreatedName != UserDetails.Rows[0]["uName"].ToString())
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
            case MachineTabGroup.Application:

                DataTable data2 = CheckerMakerController.getAppListCM(UserDetails);
                Applications = Common.DataHelper.ConvertDataTableToList<ApplicationMaintenanceViewModel>(data2);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_Application);
                getModulePermissionByModuleID(Moduleid);
                if (View) AppVisible = true;
                foreach (ApplicationMaintenanceViewModel item in Applications)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.SysID));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.SysID));
                    if (Checker)
                    {
                        if ((item.FStatus == "Pending"))
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
            case MachineTabGroup.Operating_Hour:
                DataTable data3 = CheckerMakerController.getBusinessOperatingAllCM(UserDetails);
                BusinessMaintenanceList = Common.DataHelper.ConvertDataTableToList<BusinessMaintenanceViewModel>(data3);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_BusinessHour_Template);
                getModulePermissionByModuleID(Moduleid);
                if (View) BizOpVisible = true;
                foreach (BusinessMaintenanceViewModel item in BusinessMaintenanceList)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.bId));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.bId));
                    if (Checker)
                    {
                        if ((item.bStatus == "Pending") && item.CreatedBy != UserDetails.Rows[0]["uName"].ToString())
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
            case MachineTabGroup.Card:
                DataTable data4 = CheckerMakerController.getCardListCM(UserDetails);
                Cards = Common.DataHelper.ConvertDataTableToList<CardViewModel>(data4);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_Card_Maintenance);
                getModulePermissionByModuleID(Moduleid);
                if (View) CardVisible = true;
                foreach (CardViewModel item in Cards)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.cID));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.cID));
                    if (Checker)
                    {
                        if ((item.txtStatus == "Pending") && item.CreatedName != UserDetails.Rows[0]["uName"].ToString())
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
            case MachineTabGroup.Machine_Management:
                DataTable data5 = CheckerMakerController.bindMachineCM(UserDetails);
                KioskMaintenanceList = Common.DataHelper.ConvertDataTableToList<KioskMaintenanceViewModel>(data5);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_Kiosk);
                getModulePermissionByModuleID(Moduleid);
                if (View) KioskCreateVisible = true;
                foreach (KioskMaintenanceViewModel item in KioskMaintenanceList)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.mid));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.mid));
                    if (Checker)
                    {
                        if ((item.mstatus == "Pending") && item.CreatedBy != UserDetails.Rows[0]["uName"].ToString())
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
            case MachineTabGroup.Document_Type:
                DataTable data6 = CheckerMakerController.getDocTypeAllCM(UserDetails);
                DocTypes = Common.DataHelper.ConvertDataTableToList<KioskDocTypeViewModel>(data6);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_DocType_Template);
                getModulePermissionByModuleID(Moduleid);
                if (View) DocTypeVisible = true;
                foreach (KioskDocTypeViewModel item in DocTypes)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.dID));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.dID));
                    if (Checker)
                    {
                        if ((item.txtStatus == "Pending") && item.CreatedBy != UserDetails.Rows[0]["uName"].ToString())
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
            case MachineTabGroup.Document:
                DataTable data7 = CheckerMakerController.getDocSettingAllCM(UserDetails).DefaultView.ToTable(true, "did", "dDesc", "dCreatedDate", "CreatedBy", "docStatus");
                Docs = Common.DataHelper.ConvertDataTableToList<KioskDocTypeViewModel>(data7);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_Document_Template);
                getModulePermissionByModuleID(Moduleid);
                if (View) DocVisible = true;
                foreach (KioskDocTypeViewModel item in Docs)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.dID));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.dID));
                    if (Checker)
                    {
                        if ((item.docStatus == "Pending") && item.CreatedBy != UserDetails.Rows[0]["uName"].ToString())
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
            case MachineTabGroup.Group:
                DataTable data8 = CheckerMakerController.getAssignedGroupsCM(UserDetails);
                GroupMaintenanceList = Common.DataHelper.ConvertDataTableToList<GroupMaintenanceViewModel>(data8);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_Group_Template);
                getModulePermissionByModuleID(Moduleid);
                if (View) GroupVisible = true;
                foreach (GroupMaintenanceViewModel item in GroupMaintenanceList)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.kId));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.kId));
                    if (Checker)
                    {
                        if ((item.Status == "Pending") && item.CreatedBy != UserDetails.Rows[0]["uName"].ToString())
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
            case MachineTabGroup.Hopper:
                DataTable data9 = CheckerMakerController.getHopperListCM(UserDetails);
                HopperMaintenanceList = Common.DataHelper.ConvertDataTableToList<HopperMaintenanceViewModel>(data9);
                Moduleid = Module.GetModuleId(ModuleLogAction.Approve_Hopper_Template);
                getModulePermissionByModuleID(Moduleid);
                if (View) HopperVisible = true;
                foreach (HopperMaintenanceViewModel item in HopperMaintenanceList)
                {
                    bool editing = new GlobalController().IsEditingDataExist(Guid.Parse(item.hID));
                    string editor = new GlobalController().GetEditorIfEditingDataExist(Guid.Parse(item.hID));
                    if (Checker)
                    {
                        if ((item.hStatus == "Pending") && item.CreatedBy != UserDetails.Rows[0]["uName"].ToString())
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
        }
    }

    public IActionResult OnGetView()
    {
        HttpContext.Session.SetString("lblcheckermaker", "Machine");
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostConfirmApprove()
    {
        string id = EntityIdToApprove;
        string tab = TargetTabToApprove;
        string remarks = "APPROVED";
        bool accessLog = false;
        try
        {
            if (Guid.TryParse(id, out Guid guidOutput))
            {
                switch (tab)
                {
                    case "Alert":
                        accessLog = alertController.approveAlert(guidOutput, _UserId) && alertController.ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;
                    case "BizOp":
                        accessLog = BusinessHourMaintenanceController.approveBizHour(guidOutput, _UserId) && new BusinessHourMaintenanceController().ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;
                    case "Card":
                        accessLog = cardController.approveCard(guidOutput, _UserId) && cardController.ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;
                    case "Create":
                        accessLog = KioskCreateMaintenanceController.approveMachine(guidOutput, _UserId) && new KioskCreateMaintenanceController().ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;
                    case "DocType":
                        accessLog = documentController.approveDocType(guidOutput, _UserId) && documentController.ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;
                    case "Document":
                        accessLog = documentController.approveDoc(guidOutput, _UserId) && documentController.ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;
                    case "Group":
                        accessLog = GroupMaintenanceController.approveGroup(guidOutput, _UserId) && new GroupMaintenanceController().ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;
                    case "Hopper":
                        accessLog = hopperController.approveHopper(guidOutput, _UserId) && hopperController.ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;
                    case "App":
                        accessLog = KioskVersionMaintenance.ApproveTblFirmware(guidOutput, _UserId) && hopperController.ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;

                    default:
                        break;
                }
                if (tab != "App")
                {
                    ApproveDeclineAudit(id, "Approve", tab, accessLog);
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
            var (title, message) = accessLog ? ("Success!", "You have approved the user checker maker successfully.") : ("Fail!", "Failed to approve the changes.");
            TempData["ModalTitle"] = title;
            TempData["ModalMessage"] = message;
            TempData["ShowModal"] = true;
            TempData["CurrentTab"] = tabMapping.TryGetValue(tab, out var group) ? group : MachineTabGroup.Alert;
        }
        BindGridData();
        return RedirectToPage();
    }

    public IActionResult OnPostConfirmReject()
    {
        bool accessLog = false;
        string id = EntityIdToReject;
        string tab = TargetTabToReject;
        string remark = RejectRemarks;
        CheckerAction checkerAction = CheckerAction.Reject;

        bool deleted = false;
        try
        {
            if (Guid.TryParse(id, out Guid guidOutput))
            {
                switch (tab)
                {
                    case "Alert":
                        deleted = alertController.deleteAlert(guidOutput, _UserId);
                        break;
                    case "App":
                        deleted = KioskVersionMaintenance.deleteTblFirmware(guidOutput, _UserId);
                        break;
                    case "BizOp":
                        deleted = BusinessHourMaintenanceController.deleteBizHour(guidOutput, _UserId);
                        break;
                    case "Card":
                        deleted = cardController.deleteCard(guidOutput, _UserId);
                        break;
                    case "Create":
                        deleted = KioskCreateMaintenanceController.deleteMachine(guidOutput, _UserId);
                        break;
                    case "DocType":
                        deleted = documentController.deleteDocType(guidOutput, _UserId);
                        break;
                    case "Document":
                        deleted = documentController.deleteDoc(guidOutput, _UserId);
                        break;
                    case "Group":
                        deleted = GroupMaintenanceController.deleteGroup(guidOutput, _UserId);
                        break;
                    case "Hopper":
                        deleted = hopperController.deleteHopper(guidOutput, _UserId);
                        break;
                    default:
                        break;
                }
                accessLog = (new GlobalController().RejectCreating(guidOutput, _UserId, remark, ClientIp)) && deleted;

                if (tab != "App")
                {
                    ApproveDeclineAudit(id, "Decline", tab, accessLog);
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
            var (title, message) = accessLog ? ("Success!", "You have rejected the user checker maker successfully.") : ("Fail!", "Failed to reject the user checker maker.");
            TempData["ModalTitle"] = title;
            TempData["ModalMessage"] = message;
            TempData["ShowModal"] = true;
            TempData["CurrentTab"] = tabMapping.TryGetValue(tab, out var group) ? group : MachineTabGroup.Alert;
            //Clear Textbox
            RejectRemarks = "";
        }
        BindGridData();
        return RedirectToPage();
    }

    public IActionResult OnPostConfirmDecline()
    {
        bool accessLog = false;
        string id = EntityIdToDecline;
        string tab = TargetTabToDecline;
        string remark = RejectRemarks;
        CheckerAction checkerAction = CheckerAction.Reject;

        try
        {
            if (Guid.TryParse(id, out Guid guidOutput))
            {
                accessLog = new GlobalController().DeclineEditing(guidOutput, _UserId, remark, ClientIp);
                ApproveDeclineAudit(id, "Decline", tab, accessLog);
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
            //Popup modal
            var (title, message) = accessLog ? ("Success!", "You have Declined the user checker maker successfully.") : ("Fail!", "Failed to Decline the user checker maker.");
            TempData["ModalTitle"] = title;
            TempData["ModalMessage"] = message;
            TempData["ShowModal"] = true;
            TempData["CurrentTab"] = tabMapping.TryGetValue(tab, out var group) ? group : MachineTabGroup.Alert;
            //Clear Textbox
            RejectRemarks = "";
        }
        BindGridData();
        return RedirectToPage();
    }

    private void ApproveDeclineAudit(string desc, string approveOrDecline, string kioskModule, bool tof)
    {
        try
        {
            ModuleLogAction action = new ModuleLogAction();
            string change = "";

            switch (approveOrDecline)
            {
                case "Approve":
                    switch (kioskModule)
                    {
                        case "Alert":
                            action = ModuleLogAction.Approve_Alert_Template;
                            break;
                        case "App":
                            action = ModuleLogAction.Approve_Application;
                            break;
                        case "BizOp":
                            action = ModuleLogAction.Approve_BusinessHour_Template;
                            break;
                        case "Card":
                            action = ModuleLogAction.Approve_Card_Maintenance;
                            break;
                        case "Create":
                            action = ModuleLogAction.Approve_Kiosk;
                            break;
                        case "DocType":
                            action = ModuleLogAction.Approve_DocType_Template;
                            break;
                        case "Document":
                            action = ModuleLogAction.Approve_Document_Template;
                            break;
                        case "Group":
                            action = ModuleLogAction.Approve_Group_Template;
                            break;
                        case "Hopper":
                            action = ModuleLogAction.Approve_Hopper_Template;
                            break;
                        default:
                            break;
                    }
                    change = $"{kioskModule} , {desc}'s record(s) changed! ";
                    break;
                case "Decline":
                    switch (kioskModule)
                    {
                        case "Alert":
                            action = ModuleLogAction.Decline_Alert_Template;
                            break;
                        case "App":
                            action = ModuleLogAction.Decline_Application;
                            break;
                        case "BizOp":
                            action = ModuleLogAction.Decline_BusinessHour_Template;
                            break;
                        case "Card":
                            action = ModuleLogAction.Decline_Card_Maintenance;
                            break;
                        case "Create":
                            action = ModuleLogAction.Decline_Kiosk;
                            break;
                        case "DocType":
                            action = ModuleLogAction.Decline_DocType_Template;
                            break;
                        case "Document":
                            action = ModuleLogAction.Decline_Document_Template;
                            break;
                        case "Group":
                            action = ModuleLogAction.Decline_Group_Template;
                            break;
                        case "Hopper":
                            action = ModuleLogAction.Decline_Hopper_Template;
                            break;
                        default:
                            break;
                    }
                    change = $"{kioskModule} , {desc}'s record(s) remain unchange! ";
                    break;
                default:
                    break;
            }
            AuditLog.CreateAuditLog(kioskModule + ": " + change, AuditCategory.Kiosk_Maintenance, action, _UserId, tof, ClientIp);
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

    public IActionResult OnPostChangeGrid(string name)
    {
        TempData["CurrentTab"] = EnumHelper.ParseEnum<MachineTabGroup>(name);
        return new JsonResult(new { message = "Success" });
    }
}
