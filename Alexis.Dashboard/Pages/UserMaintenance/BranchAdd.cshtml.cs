using System.ComponentModel.DataAnnotations;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.UserMaintenance;

public class BranchAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private BranchController branchControl = new();
    protected static UserManageBase My_FUserManageBase = UserManageFactory.Create(string.Empty);
    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }
    public bool ActiveVisible { get; set; } = false;

    [BindProperty]
    [Required(ErrorMessage = "The Branch Name cannot be empty.")]
    [RegularExpression(@"^[ A-Za-z0-9_@.,-]*$", ErrorMessage = "Please enter valid character only.")]
    public string BranchName { get; set; }
    [BindProperty]
    public bool Monday { get; set; }
    [BindProperty]
    public bool Tuesday { get; set; }
    [BindProperty]
    public bool Wednesday { get; set; }
    [BindProperty]
    public bool Thursday { get; set; }
    [BindProperty]
    public bool Friday { get; set; }
    [BindProperty]
    public bool Saturday { get; set; }
    [BindProperty]
    public bool Sunday { get; set; }
    [BindProperty]
    public bool Status { get; set; }
    [BindProperty]
    public string Start { get; set; } = "08:00"; // Default time
    [BindProperty]
    public string End { get; set; } = "17:00"; // Default time
    [BindProperty]
    public string? BranchId { get; set; }
    [BindProperty]
    public string? Remarks { get; set; }
    public string? LabelInfo { get; set; }
    public string? ClientIp { get; set; }
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
        checkAuthorization(ModuleLogAction.Create_BranchMaintenance);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("EditBranchID") != null) //edit
            {
                BranchId = session.GetString("EditBranchID").ToString();
                EditCommand(BranchId);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Branch Management > Add New";
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
            if (ModelState.IsValid)
            {
                var BID = session.GetString("EditBranchID");
                Branch entity = new Branch();
                entity.Bid = BID == null ? Guid.NewGuid() : Guid.Parse(BID.ToString());
                entity.TemplateName = BranchName;
                entity.Monday = Monday;
                entity.Tuesday = Tuesday;
                entity.Wednesday = Wednesday;
                entity.Thursday = Thursday;
                entity.Friday = Friday;
                entity.Saturday = Saturday;
                entity.Sunday = Sunday;
                entity.Starttime = TimeSpan.Parse(End);
                entity.Endtime = TimeSpan.Parse(Start);
                entity.Remarks = Remarks;
                entity.Status = Status ? 1 : 2;
                entity.CreatedBy = _UserId;
                entity.UpdatedBy = _UserId;
                entity.CreatedDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;


                if (BID == null)
                {
                    Create_Process(entity);
                }
                else
                {
                    Update_Process(entity);
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
        return Page();
    }

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("EditBranchID");
        return new JsonResult(new { message = "Success" });
    }

    private void Create_Process(Branch entity)
    {
        try
        {
            if (BranchController.isBranchAvailable(entity.TemplateName))
            {
                bool accessAudit = false;
                var pendingItem = entity;
                pendingItem.Status = 0;

                if (new GlobalController().CreateData(entity, ClientIp))
                {
                    if (BranchController.CreateBranch(pendingItem, ClientIp))
                    {
                        ErrorText = "Successfully Created. Please Wait for Approval.";
                        accessAudit = true;
                    }
                    else
                    {
                        ErrorText = "Failed to update " + entity.TemplateName;
                    }
                }
                else
                {
                    ErrorText = "Failed to create " + entity.TemplateName;
                }
                AuditLog.CreateAuditLog(entity.TemplateName + ": " + ErrorText, AuditCategory.System_User_Maintenance, ModuleLogAction.Create_BranchMaintenance, _UserId, accessAudit, ClientIp);
                PopUp("Alert!", ErrorText);
            }
            else
            {
                ErrorText = "Branch Name already exist.";
                ModelState.AddModelError("BranchName", ErrorText);
            }
            ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
            PageTitleText = "Branch Management > Add New";
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    private void Update_Process(Branch entity)
    {
        try
        {
            bool accessAudit = userControl.EditData(entity, ClientIp);
            ErrorText = accessAudit ? "Successfully Edited. Please Wait for Approval." : "Failed to Update.";
            AuditLog.CreateAuditLog(entity.TemplateName + ": " + ErrorText, AuditCategory.System_User_Maintenance, ModuleLogAction.Update_BranchMaintenance, _UserId, accessAudit, ClientIp);
            PopUp("Alert!", ErrorText);
            HttpContext.Session.Remove("EditBranchID");
            ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
            PageTitleText = "Branch Management > Edit";
            ActiveVisible = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    private void EditCommand(string BranchId)
    {
        try
        {
            ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
            PageTitleText = "Branch Management > Edit";
            //get branch details 
            Branch branchToEdit = new Branch();
            branchToEdit = BranchController.getBusinessOperating(Guid.Parse(BranchId));

            //update description
            BranchName = branchToEdit.TemplateName;

            //update sliders 
            Monday = branchToEdit.Monday;
            Tuesday = branchToEdit.Tuesday;
            Wednesday = branchToEdit.Wednesday;
            Thursday = branchToEdit.Thursday;
            Friday = branchToEdit.Friday;
            Saturday = branchToEdit.Saturday;
            Sunday = branchToEdit.Sunday;
            //update operating hours
            string[] tmpstartTime = branchToEdit.Starttime.ToString().Split(':');
            string[] tmpendTime = branchToEdit.Endtime.ToString().Split(':');

            Start = tmpstartTime[0].ToString() + ":" + tmpstartTime[1].ToString();
            End = tmpendTime[0].ToString() + ":" + tmpendTime[1].ToString();
            Remarks = branchToEdit.Remarks;
            Status = branchToEdit.Status == 1 ? true : false;
            if (branchToEdit.Status == 0) //pending
            {
                LabelInfo = "Note: Cannot edit branch. Status is already pending.";
            }
            ActiveVisible = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }
}