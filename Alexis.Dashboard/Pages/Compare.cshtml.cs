using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using User = Dashboard.Entities.ADCB.Dashboard.User;

namespace Alexis.Dashboard.Pages;

public class CompareModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private string remarks = "";
    private string newString = "";
    private Guid editedBy = Guid.Empty;
    private readonly GlobalController controller = new GlobalController();
    public string PageTitleText { get; set; }
    public string? OldText { get; set; }
    public string? NewText { get; set; }
    [BindProperty]
    public string? NewString { get; set; }
    [BindProperty]
    public string? CommandName { get; set; }
    [BindProperty]
    public string? CommandArgument { get; set; }
    public string? EditedBy { get; set; }
    public string? MenuText { get; set; }
    [BindProperty]
    public string? Remarks { get; set; }
    [BindProperty]
    public string? OriginalId { get; set; }
    public bool btnApproveVisible { get; set; } = true;
    public bool btnDeclineVisible { get; set; } = true;
    public bool btnRejectCreateVisible { get; set; } = true;
    public bool btnApproveCreateVisible { get; set; } = true;
    public string? ClientIp { get; set; }

    public enum CheckerAction
    {
        None,
        DeclineUpdate,
        ApproveUpdate,
        RejectCreate,
        ApproveCreate
    }

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
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            string title1 = "";
            switch (session.GetString("lblcheckermaker"))
            {
                case "User":
                    title1 = "User Checker Maker";
                    break;
                case "Content":
                    title1 = "Content Checker Maker";
                    break;
                case "Machine":
                    title1 = "Machine Checker Maker";
                    break;
                default:
                    title1 = "Checker Maker";
                    break;
            }
            PageTitleText = $"{title1} > View";
            OriginalId = Request.Query["id"].ToString();
            BindData(OriginalId);
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

    public void OnPostBackClick()
    {
        AssignPageRedirect(CommandName, false, CheckerAction.None);
    }

    public IActionResult OnPostDeclineClick()
    {
        bool isDecline = false;
        try
        {
            if (string.IsNullOrWhiteSpace(Remarks))
            {
                PopUp("Alert!", "Remarks cannot be empty.", 0);
                return RedirectToPage("Compare", new { id = OriginalId });
            }
            isDecline = controller.DeclineEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }

        if (isDecline)
        {
            AssignPageRedirect(CommandName, true, CheckerAction.DeclineUpdate);
        }
        else
        {
            PopUp("Alert!", "Failed to decline.", 0);
        }
        var session = httpContextAccessor.HttpContext.Session;
        string title1 = "";
        switch (session.GetString("lblcheckermaker"))
        {
            case "User":
                title1 = "User Checker Maker";
                break;
            case "Content":
                title1 = "Content Checker Maker";
                break;
            case "Machine":
                title1 = "Machine Checker Maker";
                break;
            default:
                title1 = "Checker Maker";
                break;
        }
        PageTitleText = $"{title1} > View";
        return RedirectToPage("Compare", new { id = OriginalId });
    }

    public IActionResult OnPostApproveEditedClick()
    {
        bool isApprove = false;
        try
        {
            newString = NewString;
            if (newString.Contains("#REMARKS#"))
                remarks = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "#REMARKS#")[1].Trim();
            editedBy = Guid.Parse(CommandArgument);
            switch (CommandName)
            {
                //User
                case "User_Roles":
                    isApprove = ApproveRole();
                    break;
                case "User_Login":
                    isApprove = ApproveUser();
                    break;
                case "User_Branch":
                    isApprove = ApproveBranch();
                    break;
                case "Machine_Alert":
                    isApprove = ApproveAlert();
                    break;
                case "Machine_Group":
                    isApprove = ApproveGroup();
                    break;
                case "Hopper_Card":
                    isApprove = ApproveCard();
                    break;
                case "DocType_Setup":
                    isApprove = ApproveDocType();
                    break;
                case "Machine_Document":
                    isApprove = ApproveDocumentSetting();
                    break;
                case "Machine_Hopper":
                    isApprove = ApproveHopper();
                    break;
                case "Machine_BusinessHour":
                    isApprove = ApproveBizHour();
                    break;
                case "Machine":
                    isApprove = ApproveKiosk();
                    break;
                case "Wrap_Serv_Main":
                    isApprove = ApproveWrapServ();
                    break;
                //Agent
                case "tblMasterSetting":
                    isApprove = ApproveMasterSetting();
                    break;
                //Kiosk
                case "Machine_Advertisement":
                    isApprove = ApproveAdvertisement();
                    break;
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
        var session = httpContextAccessor.HttpContext.Session;
        string title1 = "";
        switch (session.GetString("lblcheckermaker"))
        {
            case "User":
                title1 = "User Checker Maker";
                break;
            case "Content":
                title1 = "Content Checker Maker";
                break;
            case "Machine":
                title1 = "Machine Checker Maker";
                break;
            default:
                title1 = "Checker Maker";
                break;
        }
        PageTitleText = $"{title1} > View";
        if (isApprove)
        {
            AssignPageRedirect(CommandName, true, CheckerAction.ApproveUpdate);
        }
        else
        {
            PopUp("Alert!", "Failed to Approve.", 0);
        }
        return RedirectToPage("Compare", new { id = OriginalId });
    }

    public IActionResult OnPostRejectCreatedClick()
    {
        bool isReject = false;
        try
        {
            newString = NewString;
            if (string.IsNullOrWhiteSpace(Remarks))
            {
                PopUp("Alert!", "Remarks cannot be empty.", 0);
                return RedirectToPage("Compare", new { id = OriginalId });
            }
            isReject = controller.RejectCreating(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp);

            editedBy = Guid.Parse(CommandArgument.ToString());
            switch (CommandName)
            {
                //User
                case "User_Roles":
                    string role_name = "";
                    string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
                    for (int i = 0; i < tmpnew.Length; i++)
                    {
                        string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                        switch (tmprow[0].ToString().Trim())
                        {
                            case "RDESC":
                                role_name = tmprow[1].ToString().Trim();
                                break;
                        }
                    }
                    isReject = new UserRoleController().DeleteRoleUser("Roles", role_name, Guid.Parse(UserDetails.Rows[0][0].ToString()));
                    break;
                case "User_Login":
                    string user_name = "";
                    string[] tmpnew1 = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
                    for (int i = 0; i < tmpnew1.Length; i++)
                    {
                        string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew1[i].ToString(), ": ");
                        switch (tmprow[0].ToString().Trim())
                        {
                            case "UNAME":
                                user_name = tmprow[1].ToString().Trim();
                                break;
                        }
                    }
                    isReject = new UserRoleController().DeleteRoleUser("User", user_name, Guid.Parse(UserDetails.Rows[0][0].ToString()));
                    break;
                case "User_Branch":
                    isReject = BranchController.deleteBizHour(Guid.Parse(OriginalId));
                    break;
                case "MACHINE_ADVERTISEMENT":
                    isReject = new AdvertisementController().deleteAdvert(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Alert":
                    isReject = new AlertMaintenanceController().deleteAlert(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Group":
                    isReject = GroupMaintenanceController.deleteGroup(Guid.Parse(OriginalId), _UserId);
                    break;
                case "tblFirmware":
                    isReject = KioskVersionMaintenance.deleteTblFirmware(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Advertisement":
                    isReject = new AdvertisementController().deleteAdvert(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Hopper_Card":
                    isReject = new CardMaintenanceControl().deleteCard(Guid.Parse(OriginalId), _UserId);
                    break;
                case "DocType_Setup":
                    isReject = new DocumentSettingController().deleteDocType(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Document":
                    isReject = new DocumentSettingController().deleteDoc(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Hopper":
                    isReject = new HopperMaintenanceController().deleteHopper(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_BusinessHour":
                    isReject = BusinessHourMaintenanceController.deleteBizHour(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine":
                    isReject = KioskCreateMaintenanceController.deleteMachine(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Wrap_Serv_Main":
                    WrapServType ws_name = WrapServType.Wrap;
                    string[] tmpnew2 = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
                    bool isDetail = false;
                    for (int i = 0; i < tmpnew2.Length; i++)
                    {
                        string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew2[i].ToString(), ": ");
                        switch (tmprow[0].ToString().Trim())
                        {
                            case "WSTYPE":
                                if (tmprow[1].Trim() == WrapServType.Serv.ToString())
                                    ws_name = WrapServType.Serv;
                                else
                                    ws_name = WrapServType.Wrap;
                                break;
                            case "WSID_DETAIL":
                                isDetail = true;
                                break;
                        }
                    }

                    isReject = isDetail ? true : new WrapServController().deleteWrapServ(ws_name, Guid.Parse(OriginalId), _UserId);
                    break;
            }

            newString = NewString;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }

        if (isReject)
        {
            AssignPageRedirect(CommandName, true, CheckerAction.RejectCreate);
        }
        else
        {
            PopUp("Alert!", "Failed to decline.", 0);
        }
        var session = httpContextAccessor.HttpContext.Session;
        string title1 = "";
        switch (session.GetString("lblcheckermaker"))
        {
            case "User":
                title1 = "User Checker Maker";
                break;
            case "Content":
                title1 = "Content Checker Maker";
                break;
            case "Machine":
                title1 = "Machine Checker Maker";
                break;
            default:
                title1 = "Checker Maker";
                break;
        }
        PageTitleText = $"{title1} > View";
        return RedirectToPage("Compare", new { id = OriginalId });
    }

    public IActionResult OnPostApproveCreatedClick()
    {

        bool isApprove = false;
        try
        {
            newString = NewString;

            editedBy = Guid.Parse(CommandArgument.ToString());
            switch (CommandName)
            {
                //User
                case "User_Roles":
                    string role_name = "";
                    string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
                    for (int i = 0; i < tmpnew.Length; i++)
                    {
                        string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                        switch (tmprow[0].ToString().Trim())
                        {
                            case "RDESC":
                                role_name = tmprow[1].ToString().Trim();
                                break;
                        }
                    }
                    isApprove = new UserRoleController().ApproveDeclineRoleUser("Roles", role_name, Guid.Parse(UserDetails.Rows[0][0].ToString()), "Approve");
                    break;
                case "User_Login":
                    string user_name = "";
                    string[] tmpnew1 = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
                    for (int i = 0; i < tmpnew1.Length; i++)
                    {
                        string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew1[i].ToString(), ": ");
                        switch (tmprow[0].ToString().Trim())
                        {
                            case "UNAME":
                                user_name = tmprow[1].ToString().Trim();
                                break;
                        }
                    }
                    isApprove = new UserRoleController().ApproveDeclineRoleUser("User", user_name, Guid.Parse(UserDetails.Rows[0][0].ToString()), "Approve");
                    break;
                case "User_Branch":
                    isApprove = BranchController.approveBizHour(Guid.Parse(OriginalId), _UserId);
                    break;
                case "MACHINE_ADVERTISEMENT":
                    isApprove = new AdvertisementController().approveAdvert(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Alert":
                    isApprove = new AlertMaintenanceController().approveAlert(Guid.Parse(OriginalId), _UserId);
                    break;
                case "tblFirmware":
                    isApprove = KioskVersionMaintenance.ApproveTblFirmware(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Group":
                    isApprove = GroupMaintenanceController.approveGroup(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Advertisement":
                    isApprove = new AdvertisementController().approveAdvert(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Hopper_Card":
                    isApprove = new CardMaintenanceControl().approveCard(Guid.Parse(OriginalId), _UserId);
                    break;
                case "DocType_Setup":
                    isApprove = new DocumentSettingController().approveDocType(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Document":
                    isApprove = new DocumentSettingController().approveDoc(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_Hopper":
                    isApprove = new HopperMaintenanceController().approveHopper(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine_BusinessHour":
                    isApprove = BusinessHourMaintenanceController.approveBizHour(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Machine":
                    isApprove = KioskCreateMaintenanceController.approveMachine(Guid.Parse(OriginalId), _UserId);
                    break;
                case "Wrap_Serv_Main":
                    WrapServType ws_name = WrapServType.Wrap;
                    string[] tmpnew2 = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
                    AWrapServ entity = new AWrapServ();
                    AWrapServ.WrapServDetails detail = new AWrapServ.WrapServDetails();
                    bool isDetail = false;
                    for (int i = 0; i < tmpnew2.Length; i++)
                    {
                        string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew2[i].ToString(), ": ");
                        switch (tmprow[0].ToString().Trim())
                        {
                            case "WSTYPE":
                                if (tmprow[1].Trim() == WrapServType.Serv.ToString())
                                    ws_name = WrapServType.Serv;
                                else
                                    ws_name = WrapServType.Wrap;
                                break;
                            case "WSID_DETAIL":
                                detail.Ref_wsID = Guid.Parse(OriginalId);
                                detail.WSID_Detail = Guid.Parse(tmprow[1].Trim());
                                isDetail = true;
                                break;
                            case "WSDETAIL":
                                detail.WSDetailName = tmprow[1].Trim();
                                break;
                        }
                        if (tmprow[0].Trim() == "#CREATE#" && isDetail)
                        {
                            entity.DetailList.Add(detail);
                            detail = new AWrapServ.WrapServDetails();
                        }
                    }
                    isApprove = new WrapServController().approveWrapServ(ws_name, Guid.Parse(OriginalId), _UserId);

                    if (isDetail)
                    {
                        isApprove = new WrapServController().insertWrapServDetail(entity);
                    }
                    break;
            }
            isApprove = controller.ApproveCreating(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }

        if (isApprove)
        {
            AssignPageRedirect(CommandName, true, CheckerAction.ApproveCreate);
        }
        else
        {
            PopUp("Alert!", "Failed to Approve.", 0);
        }
        var session = httpContextAccessor.HttpContext.Session;
        string title1 = "";
        switch (session.GetString("lblcheckermaker"))
        {
            case "User":
                title1 = "User Checker Maker";
                break;
            case "Content":
                title1 = "Content Checker Maker";
                break;
            case "Machine":
                title1 = "Machine Checker Maker";
                break;
            default:
                title1 = "Checker Maker";
                break;
        }
        PageTitleText = $"{title1} > View";
        return RedirectToPage("Compare", new { id = OriginalId });
    }

    private void BindData(string id)
    {
        try
        {
            DataTable dt = controller.GetEdititngData(Guid.Parse(id));
            NewString = dt.Rows[0]["newValues"].ToString();

            string[] oldnew = new CheckerMakerController().CompareAndReturnString(dt.Rows[0]["oldValues"].ToString(), dt.Rows[0]["newValues"].ToString());

            string oldStr = oldnew[0].Replace("<br />", "\r\n");
            string newStr = oldnew[1].Replace("<br />", "\r\n");

            oldStr = oldStr.Replace("STATUS: 0", "STATUS: PENDING").Replace("STATUS: 1", "STATUS: ACTIVE").Replace("STATUS: 2", "STATUS: INACTIVE").Replace("HSTATID: 0", "STATUS: PENDING").Replace("HSTATID: 1", "STATUS: ACTIVE").Replace("HSTATID: 2", "STATUS: INACTIVE");
            newStr = newStr.Replace("STATUS: 0", "STATUS: PENDING").Replace("STATUS: 1", "STATUS: ACTIVE").Replace("STATUS: 2", "STATUS: INACTIVE").Replace("HSTATID: 0", "STATUS: PENDING").Replace("HSTATID: 1", "STATUS: ACTIVE").Replace("HSTATID: 2", "STATUS: INACTIVE");
            oldStr = oldStr.Replace("SWALLOW: 0", "SWALLOW: FALSE").Replace("SWALLOW: 1", "SWALLOW: TRUE").Replace("PRINT: 0", "PRINT: FALSE").Replace("PRINT: 1", "PRINT: TRUE");
            newStr = newStr.Replace("SWALLOW: 0", "SWALLOW: FALSE").Replace("SWALLOW: 1", "SWALLOW: TRUE").Replace("PRINT: 0", "PRINT: FALSE").Replace("PRINT: 1", "PRINT: TRUE");
            OldText = oldStr;// oldnew[0].Replace("<br />", "\r\n");
            NewText = newStr;// oldnew[1].Replace("<br />", "\r\n");

            CommandName = dt.Rows[0]["tblName"].ToString();
            CommandArgument = dt.Rows[0]["EditedBy"].ToString();

            EditedBy = dt.Rows[0]["EditedName"].ToString();
            MenuText = new UserRoleController().getModuleNameByID(Guid.Parse(dt.Rows[0]["MID"].ToString()));

            if (OldText != "")
            {
                if (EditedBy == UserDetails.Rows[0]["uName"].ToString())
                {
                    btnApproveVisible = false;
                    btnDeclineVisible = false;
                }
                btnRejectCreateVisible = false;
                btnApproveCreateVisible = false;
            }
            else
            {
                btnApproveVisible = false;
                btnDeclineVisible = false;

                if (EditedBy == UserDetails.Rows[0]["uName"].ToString())
                {
                    btnRejectCreateVisible = false;
                    btnApproveCreateVisible = false;
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
    }

    protected void AssignPageRedirect(string module, bool triggerPopUpFirst, CheckerAction action)
    {
        var session = httpContextAccessor.HttpContext.Session;
        HttpContext.Session.Remove("RedirectTo");

        switch (module.ToLower())
        {
            /*Checker maker - user maintenance*/
            case "user_roles":
                TempData["CurrentTab"] = TabGroup.Role;
                HttpContext.Session.SetString("RedirectTo", "UserMaintenance/UserCheckerMaker");
                break;
            case "user_login":
                TempData["CurrentTab"] = TabGroup.User;
                HttpContext.Session.SetString("RedirectTo", "UserMaintenance/UserCheckerMaker");
                break;
            case "user_branch":
                TempData["CurrentTab"] = TabGroup.Branch;
                HttpContext.Session.SetString("RedirectTo", "UserMaintenance/UserCheckerMaker");
                break;
            case "machine_alert":
                TempData["CurrentTab"] = MachineTabGroup.Alert;
                HttpContext.Session.SetString("RedirectTo", "MachineMaintenance/KioskManagement");
                break;
            case "tblfirmware":
                TempData["CurrentTab"] = MachineTabGroup.Application;
                HttpContext.Session.SetString("RedirectTo", "MachineMaintenance/KioskManagement");
                break;
            case "machine_businesshour":
                TempData["CurrentTab"] = MachineTabGroup.Operating_Hour;
                HttpContext.Session.SetString("RedirectTo", "MachineMaintenance/KioskManagement");
                break;
            case "hopper_card":
                TempData["CurrentTab"] = MachineTabGroup.Card;
                HttpContext.Session.SetString("RedirectTo", "MachineMaintenance/KioskManagement");
                break;
            case "machine":
                TempData["CurrentTab"] = MachineTabGroup.Machine_Management;
                HttpContext.Session.SetString("RedirectTo", "MachineMaintenance/KioskManagement");
                break;
            case "doctype_setup":
                TempData["CurrentTab"] = MachineTabGroup.Document_Type;
                HttpContext.Session.SetString("RedirectTo", "MachineMaintenance/KioskManagement");
                break;
            case "machine_document":
                TempData["CurrentTab"] = MachineTabGroup.Document;
                HttpContext.Session.SetString("RedirectTo", "MachineMaintenance/KioskManagement");
                break;
            case "machine_group":
                TempData["CurrentTab"] = MachineTabGroup.Group;
                HttpContext.Session.SetString("RedirectTo", "MachineMaintenance/KioskManagement");
                break;
            case "machine_hopper":
                TempData["CurrentTab"] = MachineTabGroup.Hopper;
                HttpContext.Session.SetString("RedirectTo", "MachineMaintenance/KioskManagement");
                break;
            /*Checker maker - advertisement*/
            case "machine_advertisement":
                TempData["CurrentTab"] = AdvTabGroup.Advertisement;
                HttpContext.Session.SetString("RedirectTo", "Ad_CheckerMaker");
                break;
            /*Checker maker - master settings*/
            case "tblmastersetting":
                HttpContext.Session.SetString("RedirectTo", "SettingCheckerMaker");
                break;
            default:
                HttpContext.Session.SetString("RedirectTo", "Home");
                break;
        }

        if (triggerPopUpFirst)
        {
            switch (action)
            {
                case CheckerAction.ApproveCreate:
                    PopUp("Success!", "You have approved the selected checker maker successfully.", 1);
                    break;
                case CheckerAction.ApproveUpdate:
                    PopUp("Success!", "You have approved the selected checker maker successfully.", 1);
                    break;
                case CheckerAction.RejectCreate:
                    PopUp("Success!", "You have rejected the selected checker maker successfully.", 1);
                    break;
                case CheckerAction.DeclineUpdate:
                    PopUp("Success!", "You have declined the selected checker maker successfully.", 1);
                    break;
                default:
                    break;
            }
        }
        else
        {

            var page = session.GetString("RedirectTo");
            Response.Redirect(page.ToString());
        }

    }

    private void PopUp(string caption, string message, int status)
    {
        HttpContext.Session.SetString("popUpStatusPushProfile", status.ToString());
        TempData["ModalStatus"] = status;
        TempData["ModalTitle"] = caption;
        TempData["ModalMessage"] = message;
        TempData["ShowModal"] = true;
    }

    #region ApproveCategory 
    protected bool ApproveRole()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            UserRoleController urController = new UserRoleController();
            Role entity = new Role();
            Role.RolePermission permission = new Role.RolePermission();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "RDESC":
                        entity.RoleID = Guid.Parse(OriginalId);
                        entity.RoleDesc = tmprow[1].ToString().Trim();
                        break;
                    case "RREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "RSTATUS":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                    case "MID":
                        permission.RoleRefID = Guid.Parse(OriginalId);
                        permission.MenuID = Guid.Parse(tmprow[1].ToString().Trim());
                        break;
                    case "MVIEW":
                        permission.MView = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "MMAKER":
                        permission.MMake = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "MCHECKER":
                        permission.MCheck = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                }
                if (tmprow[0].ToString() == "#UPDATE#")
                {
                    entity.PermissionList.Add(permission);
                    permission = new Role.RolePermission();
                }
            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (urController.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (urController.UpdateRole(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveUser()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            UserRoleController urController = new UserRoleController();
            User entity = new User();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "UNAME":
                        entity.UserID = Guid.Parse(OriginalId);
                        entity.UserName = tmprow[1].ToString().Trim();
                        break;
                    case "UFULLNAME":
                        entity.UserFullName = tmprow[1].ToString().Trim();
                        break;
                    case "UCMID":
                        entity.UserCMID = tmprow[1].ToString().Trim();
                        break;
                    case "RID":
                        entity.RoleID = Guid.Parse(tmprow[1].Trim());
                        break;
                    case "AGENTFLAG":
                        entity.AgentFlag = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "MCARDREPLENISHMENT":
                        entity.CardReplenishment = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "MCHEQUEREPLENISHMENT":
                        entity.ChequeReplenishment = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "MCONSUMABLE":
                        entity.ConsumableCollection = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "MSECURITY":
                        entity.Security = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "MTROUBLESHOOT":
                        entity.Troubleshoot = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "LOCALUSER":
                        entity.LocalUser = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "LPASSWORD":
                        entity.Password = tmprow[1]?.ToString().Trim();
                        break;
                    case "LEMAIL":
                        entity.Email = tmprow[1]?.ToString().Trim();
                        break;
                    case "UREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "USTATUS":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (urController.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (urController.UpdateUser(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveBranch()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            BranchController controller = new BranchController();
            Branch entity = new Branch();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "BDESC":
                        entity.Bid = Guid.Parse(OriginalId);
                        entity.TemplateName = tmprow[1].Trim();
                        break;
                    case "BMONDAY":
                        entity.Monday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BTUESDAY":
                        entity.Tuesday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BWEDNESDAY":
                        entity.Wednesday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BTHURSDAY":
                        entity.Thursday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BFRIDAY":
                        entity.Friday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BSATURDAY":
                        entity.Saturday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BSUNDAY":
                        entity.Sunday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BOPENTIME":
                        entity.Starttime = TimeSpan.Parse(tmprow[1].Trim());
                        break;
                    case "BCLOSETIME":
                        entity.Endtime = TimeSpan.Parse(tmprow[1].Trim());
                        break;
                    case "BREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "BSTATID":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (BranchController.UpdateBranch(entity, ClientIp))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveMasterSetting()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            MasterSettingController controller = new MasterSettingController();
            AMasterSettings entity = new AMasterSettings();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    /*case "MONTHCHEQUEMAX":
                        entity.MonthChequeMax = (String.IsNullOrEmpty(tmprow[1].Trim())) ? 0 : Convert.ToInt32(tmprow[1].Trim());
                        break;
                    case "VDNS":
                        entity.VDNs = tmprow[1].Trim();
                        break;
                    case "ENGLISH":
                        entity.English = tmprow[1].Trim();
                        break;
                    case "ARABIC":
                        entity.Arabic = tmprow[1].Trim();
                        break;*/
                    case "PWORD":
                        entity.PWord = tmprow[1].Trim();
                        break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (controller.SetMasterSettings(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveAdvertisement()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            AdvertisementController controller = new AdvertisementController();
            MAdvertisement entity = new MAdvertisement();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "ANAME":
                        entity.AID = Guid.Parse(OriginalId);
                        entity.AName = tmprow[1].Trim();
                        break;
                    case "ADESC":
                        entity.ADesc = tmprow[1].ToString().Trim();
                        break;
                    case "ARELATIVEPATHURL":
                        entity.ADirectory = tmprow[1].ToString().Trim();
                        //entity.AbsolutePath = Path.Combine(FileManager.GetLocalStoragePath(StorageType.Advertisement), Path.GetFileName(entity.RelativePathURL));
                        break;
                    case "ADURATION":
                        entity.ADuration = Convert.ToInt32(tmprow[1].Trim());
                        break;
                    case "AREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "ASTATUS":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                }
            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;

            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (controller.updateAdvert(entity))
                    if (entity.AName != "#NOT CHANGE#")
                    {
                        if (controller.updateAdvertPackage(entity))
                            ret = true;
                    }
                    else
                        ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveAlert()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            AlertMaintenanceController controller = new AlertMaintenanceController();
            MAlert entity = new MAlert();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "ADESC":
                        entity.AID = Guid.Parse(OriginalId);
                        entity.ADesc = tmprow[1].ToString().Trim();
                        break;
                    case "AMINCARDBAL":
                        entity.AMinCardBal = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "AMINCHEQUEBAL":
                        entity.AMinChequeBal = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "AMINPAPERBAL":
                        entity.AMinPaperBal = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "AMINREJCARDBAL":
                        entity.AMinRejCardBal = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "AMINRIBFRONTBAL":
                        entity.ARibFrontBal = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "AMINRIBREARBAL":
                        entity.ARibRearBal = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "AMINRIBTIPBAL":
                        entity.ARibTipBal = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    //case "AMINCHEQUEPRINTBAL":
                    //    entity.AChequePrintBal = Convert.ToInt32(tmprow[1].ToString().Trim());
                    //    break;
                    case "AMINCHEQUEPRINTCATRIDGE":
                        entity.AChequePrintCatridge = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "AMINCATRIDGEBAL":
                        entity.ACatridgeBal = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "ACARDEMAIL":
                        entity.ACardEmail = tmprow[1].ToString().Trim();
                        break;
                    case "ACARDSMS":
                        entity.ACardSMS = tmprow[1].ToString().Trim();
                        break;
                    case "ACARDTINTERVAL":
                        entity.ACardTimeInterval = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "ACHEQUEEMAIL":
                        entity.AChequeEmail = tmprow[1].ToString().Trim();
                        break;
                    case "ACHEQUESMS":
                        entity.AChequeSMS = tmprow[1].ToString().Trim();
                        break;
                    case "ACHEQUETINTERVAL":
                        entity.AChequeTimeInterval = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "AMAINTENANCEEMAIL":
                        entity.AMaintenanceEmail = tmprow[1].ToString().Trim();
                        break;
                    case "AMAINTENANCESMS":
                        entity.AMaintenanceSMS = tmprow[1].ToString().Trim();
                        break;
                    case "AMAINTENANCETINTERVAL":
                        entity.AMaintenanceTimeInterval = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "ASECURITYEMAIL":
                        entity.ASecurityEmail = tmprow[1].ToString().Trim();
                        break;
                    case "ASECURITYSMS":
                        entity.ASecuritySMS = tmprow[1].ToString().Trim();
                        break;
                    case "ASECURITYTINTERVAL":
                        entity.ASecurityTimeInterval = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "ATROUBLESHOOTEMAIL":
                        entity.ATroubleShootEmail = tmprow[1].ToString().Trim();
                        break;
                    case "ATROUBLESHOOTSMS":
                        entity.ATroubleShootSMS = tmprow[1].ToString().Trim();
                        break;
                    case "ATROUBLESHOOTTINTERVAL":
                        entity.ATroubleShootTimeInterval = Convert.ToInt32(tmprow[1].ToString().Trim());
                        break;
                    case "AREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "ASTATUS":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (controller.updateAlert(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveGroup()
    {
        bool ret = false;
        try
        {
            List<string> listAdv = new List<string>();
            string[] tmpAdv = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "#ADVERTISEMENT#");
            string[] tmpAdv1 = System.Text.RegularExpressions.Regex.Split(tmpAdv[1].Trim(), "\r\n");

            for (int i = 0; i < tmpAdv1.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpAdv1[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "ADVERTID":
                        listAdv.Add(tmprow[1].ToString().Trim());
                        break;
                }
            }
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            MGroup entity = new MGroup();
            MAdvertisement entityAdv = new MAdvertisement();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "DESCRIPTION":
                        entity.KID = Guid.Parse(OriginalId);
                        entity.KDesc = tmprow[1].ToString().Trim();
                        break;
                    case "KSCREENBACKGROUND":
                        entityAdv.AID = Guid.Parse(tmprow[1].ToString().Trim());
                        entityAdv.ADesc = "For Background Image!";
                        entityAdv.ADuration = 0;
                        entityAdv.AIsBackgroundIMG = true;
                        break;
                    case "BACKGROUNDIMAGE":
                        entityAdv.AName = tmprow[1].ToString().Trim();
                        break;
                    case "ADIRECTORY":
                        entityAdv.ADirectory = tmprow[1].ToString().Trim();
                        entity.KScreenBackground = entityAdv;
                        break;
                    case "ALERTID":
                        entity.KAlertID = Guid.Parse(tmprow[1].ToString().Trim());
                        break;
                    case "BID":
                        entity.KBusinessHourID = Guid.Parse(tmprow[1].ToString().Trim());
                        break;
                    case "DID":
                        entity.KDocumentID = Guid.Parse(tmprow[1].ToString().Trim());
                        break;
                    case "HID":
                        entity.KHopperID = Guid.Parse(tmprow[1].ToString().Trim());
                        break;
                    case "KREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "KSTATUS":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                    case "ADVERT":
                        entity.AdvIds = listAdv;
                        break;
                }
            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            entity.KScreenBackground.UpdatedBy = editedBy;
            entity.KScreenBackground.UpdatedDate = DateTime.Now;
            if (new GlobalController().ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (GroupMaintenanceController.updateGroup(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveCard()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            CardMaintenanceControl controller = new CardMaintenanceControl();
            MCard entity = new MCard();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "CDESC":
                        entity.CID = Guid.Parse(OriginalId);
                        entity.CDesc = tmprow[1].Trim();
                        break;
                    case "CCONTACTLESS":
                        entity.cContactless = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "CTYPE":
                        entity.CType = tmprow[1].Trim();
                        break;
                    case "CBIN":
                        entity.CBin = tmprow[1].Trim();
                        break;
                    case "CMASK":
                        entity.CMask = tmprow[1].Trim();
                        break;
                    case "CREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "CSTATUS":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (controller.updateCard(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveDocType()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            DocumentSettingController controller = new DocumentSettingController();
            MDocument entity = new MDocument();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "DNAME":
                        entity.DID = Guid.Parse(OriginalId);
                        entity.DName = tmprow[1].Trim();
                        break;
                    case "CCOMPONENTID":
                        entity.ComponentID = tmprow[1].Trim();
                        break;
                    case "DREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "DSTATUS":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (controller.UpdateDocType(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveDocumentSetting()
    {
        bool ret = false;
        try
        {
            List<string> docComponent = new List<string>();
            docComponent.Add("");

            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            DocumentSettingController controller = new DocumentSettingController();
            MDocument entity = new MDocument();
            entity.DocComponent.Add("");

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "DDESC":
                        entity.DID = Guid.Parse(OriginalId);
                        entity.DName = tmprow[1].Trim();
                        break;
                    case "DOCTYPEID":
                    case "SWALLOW":
                    case "PRINT":
                        entity.DocComponent.Add(tmprow[1].Trim());
                        break;
                    case "DREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "DSTATUS":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (controller.UpdateDocument(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveHopper()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            HopperMaintenanceController controller = new HopperMaintenanceController();
            MHopper entity = new MHopper();
            string[,] hopper = new string[8, 4];

            for (int i = 0; i < tmpnew.Length;)
            {
                for (int j = 0; j < 4; j++)
                {
                    string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                    switch (tmprow[0].ToString().Trim())
                    {
                        case "HDESC":
                            entity.HID = Guid.Parse(OriginalId);
                            entity.HName = tmprow[1].Trim();
                            break;
                        case "H1TEMP":
                        case "H1RANGE":
                        case "H1MASK":
                        case "H2TEMP":
                        case "H2RANGE":
                        case "H2MASK":
                        case "H3TEMP":
                        case "H3RANGE":
                        case "H3MASK":
                        case "H4TEMP":
                        case "H4RANGE":
                        case "H4MASK":
                        case "H5TEMP":
                        case "H5RANGE":
                        case "H5MASK":
                        case "H6TEMP":
                        case "H6RANGE":
                        case "H6MASK":
                        case "H7TEMP":
                        case "H7RANGE":
                        case "H7MASK":
                        case "H8TEMP":
                        case "H8RANGE":
                        case "H8MASK":
                            string number = tmprow[0].Replace("H", "").Replace("TEMP", "").Replace("RANGE", "").Replace("MASK", "").Trim();
                            hopper[Convert.ToInt32(number) - 1, j - 1] = tmprow[1].Trim();
                            break;
                        case "HREMARKS":
                            entity.Remarks = remarks;
                            break;
                        case "HSTATID":
                            entity.Status = Convert.ToInt32(tmprow[1].Trim());
                            break;
                    }

                    if (tmprow[0].ToString() == "#UPDATE#")
                    {
                        entity.HopperArray = hopper;
                    }
                    if (i++ == 0 || i == tmpnew.Length)
                        break;
                }
            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (controller.updateHopper(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveBizHour()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            BusinessHourMaintenanceController controller = new BusinessHourMaintenanceController();
            MBizHour entity = new MBizHour();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "BDESC":
                        entity.Bid = Guid.Parse(OriginalId);
                        entity.TemplateName = tmprow[1].Trim();
                        break;
                    case "BMONDAY":
                        entity.Monday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BTUESDAY":
                        entity.Tuesday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BWEDNESDAY":
                        entity.Wednesday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BTHURSDAY":
                        entity.Thursday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BFRIDAY":
                        entity.Friday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BSATURDAY":
                        entity.Saturday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BSUNDAY":
                        entity.Sunday = Convert.ToBoolean(Convert.ToInt32(tmprow[1].Trim()));
                        break;
                    case "BSTARTTIME":
                        entity.Starttime = TimeSpan.Parse(tmprow[1].Trim());
                        break;
                    case "BENDTIME":
                        entity.Endtime = TimeSpan.Parse(tmprow[1].Trim());
                        break;
                    case "BREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "BSTATID":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (BusinessHourMaintenanceController.UpdateBusinessHour(entity, ClientIp))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveKiosk()
    {
        bool ret = false;
        try
        {
            string[] tmpAdress = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "#ADDRESS#");
            string address = tmpAdress[1].Trim();


            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            KioskCreateMaintenanceController controller = new KioskCreateMaintenanceController();
            MKiosk entity = new MKiosk();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "DESCRIPTION":
                        entity.MachineID = Guid.Parse(OriginalId);
                        entity.MachineDescription = tmprow[1].Trim();
                        break;
                    case "SERIAL":
                        entity.MachineSerial = tmprow[1].Trim();
                        break;
                    case "MKIOSKID":
                        entity.MachineKioskID = tmprow[1].Trim();
                        break;
                    case "MSTATIONID":
                        entity.MachineStationID = tmprow[1].Trim();
                        break;
                    case "ADDRESS":
                        entity.MachineAddress = address;//tmprow[1].Trim();
                        break;
                    case "LATITUDE":
                        entity.MachineLatitude = tmprow[1].Trim();
                        break;
                    case "LONGITUDE":
                        entity.MachineLongtitude = tmprow[1].Trim();
                        break;
                    case "MGROUPID":
                        entity.MachineGroupID = Guid.Parse(tmprow[1].Trim());
                        break;
                    case "MREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "MSTATID":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                    case "IPADDRESS":
                        entity.MacIP = tmprow[1].Trim();
                        break;
                    case "PORTNUMBER":
                        entity.MacPort = tmprow[1].Trim();
                        break;
                        //case "MPILOT":
                        //    entity.MacPilot = Convert.ToBoolean(tmprow[1].Trim());
                        //    break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
                if (KioskCreateMaintenanceController.updateMachine(entity))
                    ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    protected bool ApproveWrapServ()
    {
        bool ret = false;
        try
        {
            string[] tmpnew = System.Text.RegularExpressions.Regex.Split(newString.Trim(), "\r\n");
            WrapServController controller = new WrapServController();
            AWrapServ entity = new AWrapServ();

            for (int i = 0; i < tmpnew.Length; i++)
            {
                string[] tmprow = System.Text.RegularExpressions.Regex.Split(tmpnew[i].ToString(), ": ");
                switch (tmprow[0].ToString().Trim())
                {
                    case "WSMAIN":
                        entity.WSID = Guid.Parse(OriginalId);
                        entity.WSMainName = tmprow[1].Trim();
                        break;
                    case "WSTYPE":
                        if (tmprow[1].Trim() == WrapServType.Serv.ToString())
                            entity.WSType = WrapServType.Serv;
                        else
                            entity.WSType = WrapServType.Wrap;
                        break;
                    case "WSID_DETAIL":
                        entity.Detail.Ref_wsID = Guid.Parse(OriginalId);
                        entity.Detail.WSID_Detail = Guid.Parse(tmprow[1].Trim());
                        break;
                    case "WSDETAIL":
                        entity.Detail.WSDetailName = tmprow[1].Trim();
                        break;
                    case "WSREMARKS":
                        entity.Remarks = remarks;
                        break;
                    case "WSSTATUS":
                        entity.Status = Convert.ToInt32(tmprow[1].Trim());
                        break;
                }

            }

            entity.UpdatedBy = editedBy;
            entity.UpdatedDate = DateTime.Now;
            if (controller.ApproveEditing(Guid.Parse(OriginalId), _UserId, Remarks, ClientIp))
            {
                if (entity.Detail.WSID_Detail == Guid.Empty)
                {
                    if (controller.updateWrapServ(entity))
                        ret = true;
                }
                else
                {
                    if (controller.updateWrapServDetail(entity))
                        ret = true;
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
        return ret;
    }

    #endregion

    public IActionResult OnGetModalCloseClick()
    {
        var page = HttpContext.Session.GetString("RedirectTo");
        if (!string.IsNullOrEmpty(page))
        {
            return new JsonResult(new { message = "Success", page = page });
        }
        return new JsonResult(new { message = "", page = "" });
    }

}
