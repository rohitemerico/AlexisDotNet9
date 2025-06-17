using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Telerik.SvgIcons;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class CardMaintenanceAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private CardMaintenanceControl cardControl = new CardMaintenanceControl();

    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }
    public bool ActiveVisible { get; set; } = false;
    public string? ClientIp { get; set; }

    [BindProperty]
    public string? CardId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please enter Card description.")]
    public string CardDesc { get; set; }

    [Required(ErrorMessage = "Please choose from the card type.")]
    [BindProperty]
    public string CardTypeId { get; set; }

    [Required(ErrorMessage = "Please choose from the contact type.")]
    [BindProperty]
    public string ContactTypeId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please enter Card Bin.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Please enter numbers only.")]
    public string Bin { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please enter Mask Settings.")]
    [RegularExpression(@"^X+$", ErrorMessage = "Please use capital 'X' only.")]
    public string Mask { get; set; }

    [BindProperty]
    public string? Remarks { get; set; }
    [BindProperty]
    public bool chkStatus { get; set; }

    public required List<SelectListItem> CardTypeList { get; set; }
    public required List<SelectListItem> ContactTypeList { get; set; }


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
        checkAuthorization(ModuleLogAction.Create_Card_Maintenance);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("EditCardID") != null) //edit
            {
                CardId = session.GetString("EditCardID").ToString();
                EditCommand(CardId);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Card > Add New";
            }
            BindDropDownList();
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

    private void EditCommand(string CardId)
    {
        try
        {
            ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
            PageTitleText = "Machine Card > Edit";
            ActiveVisible = true;
            DataTable dt = cardControl.getCardInfoByID(Guid.Parse(CardId));
            if (dt.Rows.Count != 0)
            {
                CardDesc = dt.Rows[0][1].ToString();
                ContactTypeId = Convert.ToInt32(dt.Rows[0][2]).ToString();
                CardTypeId = dt.Rows[0]["cType"].ToString();
                Bin = dt.Rows[0]["cBin"].ToString();
                Mask = dt.Rows[0]["cMask"].ToString();
                Remarks = dt.Rows[0]["cRemarks"].ToString();
                chkStatus = Convert.ToInt32(dt.Rows[0]["cStatus"]) == 1 ? true : false;
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

    public void BindDropDownList()
    {

        CardTypeList = [new SelectListItem { Text = "Please Select One.", Value = "" }];
        CardTypeList.Add(new SelectListItem { Text = "Debit", Value = "Debit" });
        CardTypeList.Add(new SelectListItem { Text = "Credit", Value = "Credit" });

        ContactTypeList = [new SelectListItem { Text = "Please Select One.", Value = "" }];
        ContactTypeList.Add(new SelectListItem { Text = "Dual Mode", Value = "0" });
        ContactTypeList.Add(new SelectListItem { Text = "Contact Only", Value = "1" });
    }

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("EditCardID");
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPost()
    {
        try
        {
            getUserDetails();
            if (accessLogin)
            {
                return RedirectToPage("/Login");
            }
            checkAuthorization(ModuleLogAction.Create_Card_Maintenance);
            var session = httpContextAccessor.HttpContext.Session;
            if (ModelState.IsValid)
            {
                var EditCardID = session.GetString("EditCardID");
                if (cardControl.IsCardNameExist(CardDesc) && EditCardID == null)
                {
                    ModelState.AddModelError("RoleName", "The name is already exist.");
                }
                else if (cardControl.isCardAvailable(Bin) && EditCardID == null)
                {
                    ModelState.AddModelError("Bin", "The Card Bin is already exist.");
                }
                else
                {
                    MCard entity = new MCard();
                    entity.CID = (EditCardID == null) ? Guid.NewGuid() : Guid.Parse(EditCardID.ToString());
                    entity.CDesc = CardDesc;
                    entity.CType = CardTypeId;
                    entity.cContactless = Convert.ToBoolean(Convert.ToInt32(ContactTypeId));
                    entity.CBin = Bin;
                    entity.CMask = Mask;

                    entity.Remarks = Remarks;
                    entity.Status = chkStatus ? 1 : 2;
                    entity.CreatedBy = _UserId;
                    entity.ApprovedBy = _UserId;
                    entity.DeclineBy = _UserId;
                    entity.UpdatedBy = _UserId;
                    entity.CreatedDate = DateTime.Now;
                    entity.ApprovedDate = DateTime.Now;
                    entity.DeclineDate = DateTime.Now;
                    entity.UpdatedDate = DateTime.Now;
                    if (EditCardID == null)
                    {
                        bool accessLog = cardControl.insertNewCard(entity) && cardControl.CreateData(entity, ClientIp);

                        var ErrorText = accessLog ? "Successfully Created. Please Wait for Approval." : "Failed to Create.";
                        PopUp("Alert!", ErrorText);
                        InsertAudit(entity.CDesc + ": " + ErrorText, ModuleLogAction.Create_Card_Maintenance, accessLog);
                    }
                    else
                    {
                        var ErrorText = "";
                        string validate = cardControl.IsValidatedToInactive(entity.CID);
                        if (!(string.IsNullOrEmpty(validate) || chkStatus))
                        {
                            ErrorText = validate;
                            ModelState.AddModelError("CardDesc", ErrorText);
                        }
                        bool accessAudit = cardControl.EditData(entity, ClientIp);
                        ErrorText = accessAudit ? "Successfully Edited. Please Wait for Approval." : "Failed to Update.";
                        PopUp("Alert!", ErrorText);
                        InsertAudit(entity.CDesc + ": " + ErrorText, ModuleLogAction.Update_Card_Maintenance, accessAudit);
                        HttpContext.Session.Remove("EditCardID");
                    }
                }
            }
            BindDropDownList();
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

    private void InsertAudit(string desc, ModuleLogAction action, bool tof)
    {
        try
        {
            AuditLog.CreateAuditLog(desc, AuditCategory.Agent_Maintenance, action, _UserId, tof, ClientIp);
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
