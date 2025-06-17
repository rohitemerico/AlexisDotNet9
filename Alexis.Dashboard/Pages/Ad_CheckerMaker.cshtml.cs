using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages;

public class Ad_CheckerMakerModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private AdvertisementController advController = new AdvertisementController();

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
    public AdvTabGroup CurrentTab { get; set; }
    public List<AdViewModel> Ads { get; set; }
    public string? ClientIp { get; set; }
    public bool AdvertisementVisible { get; set; }

    private Dictionary<string, AdvTabGroup> tabMapping = new Dictionary<string, AdvTabGroup>(StringComparer.OrdinalIgnoreCase)
    {
        { "Advertisement", AdvTabGroup.Advertisement }
    };

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
        checkAuthorization(ModuleLogAction.View_AdvertisementCheckerMaker);
    }

    public IActionResult OnGet()
    {
        try
        {
            if (TempData["CurrentTab"] != null)
                CurrentTab = (AdvTabGroup)TempData["CurrentTab"];
            else CurrentTab = AdvTabGroup.Advertisement;
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

    private void BindGridData()
    {
        string currentUser = UserDetails.Rows[0]["uName"].ToString();
        DataTable data = CheckerMakerController.getAllAdvertisementCM(UserDetails);
        Ads = Common.DataHelper.ConvertDataTableToList<AdViewModel>(data);
        Guid Moduleid = Guid.Empty;

        Moduleid = Module.GetModuleId(ModuleLogAction.Approve_AdvertisementManagement);
        getModulePermissionByModuleID(Moduleid);
        if (View) AdvertisementVisible = true;
        foreach (AdViewModel item in Ads)
        {
            bool editing = new GlobalController().IsEditingDataExist(item.aID);
            string editor = new GlobalController().GetEditorIfEditingDataExist(item.aID);
            if (Checker)
            {
                if (item.AdvertStatus == "Pending" && item.CreatedBy != UserDetails.Rows[0]["uName"].ToString())
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
                        accessLog = advController.approveAdvert(guidOutput, _UserId) && advController.ApproveCreating(guidOutput, _UserId, remarks, ClientIp);
                        break;
                    default:
                        break;
                }
                ApproveDeclineAudit(id, "Approve", tab, accessLog);
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
            TempData["CurrentTab"] = tabMapping.TryGetValue(tab, out var group) ? group : AdvTabGroup.Advertisement;
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

        bool deleted = false;
        try
        {
            if (Guid.TryParse(id, out Guid guidOutput))
            {
                switch (tab)
                {
                    case "Advertisement":
                        deleted = advController.deleteAdvert(guidOutput, _UserId);
                        break;
                    default:
                        break;
                }
                accessLog = (new GlobalController().RejectCreating(guidOutput, _UserId, remark, ClientIp)) && deleted;
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
            var (title, message) = accessLog ? ("Success!", "You have rejected the user checker maker successfully.") : ("Fail!", "Failed to reject the user checker maker.");
            TempData["ModalTitle"] = title;
            TempData["ModalMessage"] = message;
            TempData["ShowModal"] = true;
            TempData["CurrentTab"] = tabMapping.TryGetValue(tab, out var group) ? group : AdvTabGroup.Advertisement;
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
            TempData["CurrentTab"] = tabMapping.TryGetValue(tab, out var group) ? group : AdvTabGroup.Advertisement;
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
}
