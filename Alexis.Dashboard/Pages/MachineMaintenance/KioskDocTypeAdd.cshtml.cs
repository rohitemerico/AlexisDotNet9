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

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class KioskDocTypeAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private DocumentSettingController documentControl = new DocumentSettingController();
    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }

    public string ErrorText { get; set; }
    public bool ActiveVisible { get; set; } = false;
    public string? ClientIp { get; set; }

    [BindProperty]
    public string? DID { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Document Type cannot be empty.")]
    public string DName { get; set; }

    [BindProperty]
    public string Component { get; set; }

    [BindProperty]
    public string? Remarks { get; set; }

    [BindProperty]
    public bool Status { get; set; }

    public List<SelectListItem> Components { get; set; }

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
        checkAuthorization(ModuleLogAction.Create_DocType_Template);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("EditDocTypeID") != null) //edit
            {
                DID = session.GetString("EditDocTypeID").ToString();
                ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                PageTitleText = "Machine Document Type > Edit";
                EditCommand(DID);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Document Type > Add New";
            }
            BindDropDownList();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            var EditDocTypeID = session.GetString("EditDocTypeID");
            MDocument entity = new MDocument();
            entity.DID = (EditDocTypeID == null) ? Guid.NewGuid() : Guid.Parse(EditDocTypeID.ToString());
            entity.DName = DName;
            entity.ComponentID = Component;
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

            if (EditDocTypeID == null)
            {
                Create_Process(entity);
            }
            else
            {
                Update_Process(entity);
            }
            BindDropDownList();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void Create_Process(MDocument entity)
    {
        if (!documentControl.isDocumentTypeExist(entity.DName))
        {
            bool accessAudit = documentControl.CreateDocType(entity) && documentControl.CreateData(entity, ClientIp);
            var ErrorText = accessAudit ? "Successfully Created. Please Wait for Approval." : "Failed to Create.";
            PopUp("Alert!", ErrorText);
            InsertAudit(entity.DName + ": " + ErrorText, ModuleLogAction.Create_Card_Maintenance, accessAudit);
        }
        else
        {
            ErrorText = "Document Template already exist.";
            ModelState.AddModelError("DName", ErrorText);
        }
        ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
        PageTitleText = "Machine Document Type > Add New";
    }

    private void Update_Process(MDocument entity)
    {
        string validate = documentControl.IsValidatedToInactive(entity.DID);
        if (!(string.IsNullOrEmpty(validate) || Status))
        {
            ErrorText = validate;
            ModelState.AddModelError("DName", ErrorText);
            ActiveVisible = true;
        }
        else
        {
            bool accessAudit = documentControl.EditData(entity, ClientIp);
            ErrorText = accessAudit ? "Successfully Edited. Please Wait for Approval." : "Failed to Update.";
            PopUp("Alert!", ErrorText);
            InsertAudit(entity.DName + ": " + ErrorText, ModuleLogAction.Update_DocType_Template, accessAudit);
            HttpContext.Session.Remove("EditDocTypeID");
        }
        ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
        PageTitleText = "Machine Document Type > Edit";
    }

    private void BindDropDownList()
    {
        Components = [];
        Components.Add(new SelectListItem { Text = "A4 Scanner", Value = "A4 Scanner" });
        Components.Add(new SelectListItem { Text = "Card Reader", Value = "Card Reader" });
        Components.Add(new SelectListItem { Text = "Card Scanner", Value = "Card Scanner" });
        Components.Add(new SelectListItem { Text = "Passport Scanner", Value = "Passport Scanner" });
    }

    private void EditCommand(string DID)
    {
        try
        {
            DataTable dt = documentControl.getDocTypeById(Guid.Parse(DID));
            DName = dt.Rows[0]["dName"].ToString();
            Component = dt.Rows[0]["cComponentID"].ToString();
            Remarks = dt.Rows[0]["dRemarks"].ToString();
            Status = Convert.ToInt32(dt.Rows[0]["dStatus"]) == 1 ? true : false;
            ActiveVisible = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("EditDocTypeID");
        return new JsonResult(new { message = "Success" });
    }

    private void InsertAudit(string desc, ModuleLogAction action, bool tof)
    {
        try
        {
            AuditLog.CreateAuditLog(desc, AuditCategory.Kiosk_Maintenance, action, _UserId, tof, ClientIp);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }
}
