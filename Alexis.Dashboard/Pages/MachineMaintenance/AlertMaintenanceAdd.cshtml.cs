using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class AlertMaintenanceAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private AlertMaintenanceController alertControl = new AlertMaintenanceController();
    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }
    public bool ActiveVisible { get; set; } = false;
    public string? ClientIp { get; set; }
    [BindProperty]
    public string? aID { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string Name { get; set; }
    [BindProperty]
    public string? Remarks { get; set; }
    [BindProperty]
    public bool Status { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MinCardBal { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MinChequeBal { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MinPaperBal { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MinRejCardBal { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MINRIBFRONTBAL { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MINRIBREARBAL { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MINRIBTIPBAL { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MINCHEQUEPRINTCATRIDGE { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MINCATRIDGEBAL { get; set; }


    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string CardEmail { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string CardSms { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string CardTInterval { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string ChequeEmail { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string ChequeSms { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string ChequeTInterval { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string MaintenanceEmail { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string MaintenanceSms { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string MaintenanceTInterval { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string SecurityEmail { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string SecuritySms { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string SecurityTInterval { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string TroubleshootEmail { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    public string TroubleshootSms { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string TroubleshootTInterval { get; set; }
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
        checkAuthorization(ModuleLogAction.Create_Alert_Template);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("EditAlertID") != null) //edit
            {
                aID = session.GetString("EditAlertID").ToString();
                EditCommand(aID);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Alert > Add New";
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
        return Page();
    }


    public IActionResult OnPost()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            var AID = session.GetString("EditAlertID");
            if (ModelState.IsValid)
            {

                if (alertControl.isAlertAvailable(Name) && AID is null)
                {

                    ErrorText = "The name is already exist.";
                    ModelState.AddModelError("Name", ErrorText);
                }
                else
                {
                    MAlert entity = new MAlert();
                    entity.AID = (AID == null) ? Guid.NewGuid() : Guid.Parse(AID.ToString());
                    entity.ADesc = Name;
                    entity.AMinCardBal = Convert.ToInt32(MinCardBal);
                    entity.AMinChequeBal = Convert.ToInt32(MinChequeBal);
                    entity.AMinPaperBal = Convert.ToInt32(MinPaperBal);
                    entity.AMinRejCardBal = Convert.ToInt32(MinRejCardBal);
                    entity.ARibFrontBal = Convert.ToInt32(MINRIBFRONTBAL);
                    entity.ARibRearBal = Convert.ToInt32(MINRIBREARBAL);
                    entity.ARibTipBal = Convert.ToInt32(MINRIBTIPBAL);
                    entity.AChequePrintCatridge = Convert.ToInt32(MINCHEQUEPRINTCATRIDGE);
                    entity.ACatridgeBal = Convert.ToInt32(MINCATRIDGEBAL);

                    entity.ACardEmail = CardEmail;
                    entity.ACardSMS = CardSms;
                    entity.ACardTimeInterval = Convert.ToInt32(CardTInterval);
                    entity.AChequeEmail = ChequeEmail;
                    entity.AChequeSMS = ChequeSms;
                    entity.AChequeTimeInterval = Convert.ToInt32(ChequeTInterval);
                    entity.AMaintenanceEmail = MaintenanceEmail;
                    entity.AMaintenanceSMS = MaintenanceSms;
                    entity.AMaintenanceTimeInterval = Convert.ToInt32(MaintenanceTInterval);
                    entity.ASecurityEmail = SecurityEmail;
                    entity.ASecuritySMS = SecuritySms;
                    entity.ASecurityTimeInterval = Convert.ToInt32(SecurityTInterval);
                    entity.ATroubleShootEmail = TroubleshootEmail;
                    entity.ATroubleShootSMS = TroubleshootSms;
                    entity.ATroubleShootTimeInterval = Convert.ToInt32(TroubleshootTInterval);

                    entity.Remarks = Remarks;
                    entity.Status = Status ? 1 : 2;
                    entity.CreatedBy = _UserId;
                    entity.ApprovedBy = _UserId;
                    entity.DeclineBy = _UserId;
                    entity.UpdatedBy = _UserId;
                    entity.CreatedDate = DateTime.Now;
                    entity.ApprovedDate = DateTime.Now;
                    entity.DeclineDate = DateTime.Now;
                    entity.UpdatedDate = DateTime.Now;
                    if (AID == null)
                    {
                        bool accessAudit = alertControl.createAlert(entity) && alertControl.CreateData(entity, ClientIp);
                        ErrorText = accessAudit ? "Successfully Created. Please Wait for Approval." : "Failed to Create Alert.";
                        PopUp("Alert!", ErrorText);
                        InsertAudit(Name + ": " + ErrorText, ModuleLogAction.Create_Alert_Template, accessAudit);
                    }
                    else
                    {
                        string validate = alertControl.IsValidatedToInactive(entity.AID);
                        if (!(string.IsNullOrEmpty(validate) || Status))
                        {
                            ErrorText = validate;
                            ModelState.AddModelError("Name", ErrorText);
                        }
                        else
                        {
                            bool accessAudit = alertControl.EditData(entity, ClientIp);
                            ErrorText = accessAudit ? "Successfully Edited. Please Wait for Approval." : "Failed to Update.";
                            PopUp("Alert!", ErrorText);
                            InsertAudit(Name + ": " + ErrorText, ModuleLogAction.Update_Alert_Template, accessAudit);
                        }
                    }
                }
            }
            if (AID == null)
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Alert > Add New";
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                PageTitleText = "Machine Alert > Edit";
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void EditCommand(string Id)
    {
        try
        {
            ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
            PageTitleText = "Machine Alert > Edit";
            ActiveVisible = true;
            DataTable alertDt = alertControl.getAlertByID(Guid.Parse(aID.ToString()));
            Name = alertDt.Rows[0]["aDESC"].ToString();
            MinCardBal = alertDt.Rows[0]["aMINCARDBAL"].ToString();
            MinChequeBal = alertDt.Rows[0]["aMINCHEQUEBAL"].ToString();
            MinPaperBal = alertDt.Rows[0]["aMINPAPERBAL"].ToString();
            MinRejCardBal = alertDt.Rows[0]["aMINREJCARDBAL"].ToString();
            MINRIBFRONTBAL = alertDt.Rows[0]["aMINRIBFRONTBAL"].ToString();
            MINRIBREARBAL = alertDt.Rows[0]["aMINRIBREARBAL"].ToString();
            MINRIBTIPBAL = alertDt.Rows[0]["aMINRIBTIPBAL"].ToString();
            //txtChequePrintBal.Text = alertDt.Rows[0]["aMINCHEQUEPRINTBAL"].ToString();
            MINCHEQUEPRINTCATRIDGE = alertDt.Rows[0]["aMINCHEQUEPRINTCATRIDGE"].ToString();
            MINCATRIDGEBAL = alertDt.Rows[0]["aMINCATRIDGEBAL"].ToString();

            CardEmail = alertDt.Rows[0]["aCARDEMAIL"].ToString();
            CardSms = alertDt.Rows[0]["aCARDSMS"].ToString();
            CardTInterval = alertDt.Rows[0]["aCARDTINTERVAL"].ToString();
            ChequeEmail = alertDt.Rows[0]["aCHEQUEEMAIL"].ToString();
            ChequeSms = alertDt.Rows[0]["aCHEQUESMS"].ToString();
            ChequeTInterval = alertDt.Rows[0]["aCHEQUETINTERVAL"].ToString();
            MaintenanceEmail = alertDt.Rows[0]["aMAINTENANCEEMAIL"].ToString();
            MaintenanceSms = alertDt.Rows[0]["aMAINTENANCESMS"].ToString();
            MaintenanceTInterval = alertDt.Rows[0]["aMAINTENANCETINTERVAL"].ToString();
            SecurityEmail = alertDt.Rows[0]["aSECURITYEMAIL"].ToString();
            SecuritySms = alertDt.Rows[0]["aSECURITYSMS"].ToString();
            SecurityTInterval = alertDt.Rows[0]["aSECURITYTINTERVAL"].ToString();
            TroubleshootEmail = alertDt.Rows[0]["aTROUBLESHOOTEMAIL"].ToString();
            TroubleshootSms = alertDt.Rows[0]["aTROUBLESHOOTSMS"].ToString();
            TroubleshootTInterval = alertDt.Rows[0]["aTROUBLESHOOTTINTERVAL"].ToString();

            Remarks = alertDt.Rows[0]["aRemarks"].ToString();
            Status = Convert.ToInt32(alertDt.Rows[0]["aStatus"]) == 1 ? true : false;

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

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("EditAlertID");
        return new JsonResult(new { message = "Success" });
    }

}
