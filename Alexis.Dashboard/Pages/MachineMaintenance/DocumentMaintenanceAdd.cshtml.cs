using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Telerik.SvgIcons;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class DocumentMaintenanceAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    protected DocumentSettingController documentControl = new DocumentSettingController();

    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }

    public string ErrorText { get; set; }
    public bool ActiveVisible { get; set; } = false;
    public string? ClientIp { get; set; }

    [BindProperty]
    public string? DID { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Document Template cannot be empty.")]
    [RegularExpression(@"^[ A-Za-z0-9_@.,-]*$", ErrorMessage = "Please enter valid character only.")]
    public string DName { get; set; }

    [BindProperty]
    public string? Remarks { get; set; }

    [BindProperty]
    public bool Status { get; set; }

    [BindProperty]
    public List<DocTypeModel> Documents { get; set; } = new List<DocTypeModel>();

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
        checkAuthorization(ModuleLogAction.Create_Document_Template);
    }

    public IActionResult OnGet()
    {
        try
        {
            BindData();
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("EditDocumentID") != null) //edit
            {
                DID = session.GetString("EditDocumentID").ToString();
                ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                PageTitleText = "Machine Document > Edit";
                EditCommand(DID);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Document > Add New";
            }


        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void BindData()
    {
        DataTable data = documentControl.getActDocTypeList();
        Documents = Common.DataHelper.ConvertDataTableToList<DocTypeModel>(data);
    }

    public IActionResult OnPost()
    {
        try
        {
            List<string> docComponent = new List<string>();
            docComponent.Add("");
            foreach (var item in Documents)
            {
                docComponent.Add(item.cComponentID);
                docComponent.Add(item.dName);
                docComponent.Add(item.SWALLOW.ToString());
                docComponent.Add(item.PRINT.ToString());
            }
            var session = httpContextAccessor.HttpContext.Session;
            var EditDocID = session.GetString("EditDocumentID");
            MDocument entity = new MDocument();
            entity.DID = (EditDocID == null) ? Guid.NewGuid() : Guid.Parse(EditDocID.ToString());
            entity.DName = DName;
            entity.DocComponent = docComponent;
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

            if (EditDocID == null)
            {
                Create_Process(entity);
            }
            else
            {
                Update_Process(entity);
            }
            BindData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
    private void EditCommand(string ID)
    {
        try
        {
            DataTable dt = documentControl.getDocSettingById(Guid.Parse(DID));
            List<DocTypeViewModel> DocList = Common.DataHelper.ConvertDataTableToList<DocTypeViewModel>(dt);
            foreach (var item in Documents)
            {
                var element = DocList.Where(a => a.DocTypeId == item.dID).FirstOrDefault();
                item.SWALLOW = element != null && element.SWALLOW == 1;
                item.PRINT = element != null && element.PRINT == 1;
            }
            DName = dt.Rows[0]["dDesc"].ToString();
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

    private void Create_Process(MDocument entity)
    {
        if (!documentControl.isDocumentTypeExist(entity.DName))
        {
            bool accessAudit = documentControl.CreateDocument(entity) && documentControl.CreateData(entity, ClientIp);
            var ErrorText = accessAudit ? "Successfully Created. Please Wait for Approval." : "Failed to Create.";
            PopUp("Alert!", ErrorText);
            InsertAudit(entity.DName + ": " + ErrorText, ModuleLogAction.Create_Document_Template, accessAudit);
        }
        else
        {
            ErrorText = "Document Template already exist.";
            ModelState.AddModelError("DName", ErrorText);
        }
        ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
        PageTitleText = "Machine Document > Add New";
    }

    private void Update_Process(MDocument entity)
    {
        string validate = documentControl.IsValidatedToInactive(entity.DID);
        if (!(string.IsNullOrEmpty(validate) || Status))
        {
            ErrorText = validate;
            ModelState.AddModelError("DName", ErrorText);
        }
        else
        {
            bool accessAudit = documentControl.EditData(entity, ClientIp);
            ErrorText = accessAudit ? "Successfully Edited. Please Wait for Approval." : "Failed to Update.";
            PopUp("Alert!", ErrorText);
            InsertAudit(entity.DName + ": " + ErrorText, ModuleLogAction.Update_Document_Template, accessAudit);
            HttpContext.Session.Remove("EditDocumentID");
        }
        ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
        PageTitleText = "Machine Document > Edit";
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
        HttpContext.Session.Remove("EditDocumentID");
        return new JsonResult(new { message = "Success" });
    }
}


