using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.UserMaintenance;

public class RoleAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    protected new UserRoleController userControl = new();
    private readonly UserRolePageLoad pageLoad = new();
    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }
    public bool ActiveVisible { get; set; } = false;
    public string? ClientIp { get; set; }
    public string ErrorText { get; set; }

    [BindProperty]
    public List<ModuleViewModel> Modules { get; set; } = new List<ModuleViewModel>();

    [BindProperty]
    [Required(ErrorMessage = "The Role Name cannot be empty.")]
    public string RoleName { get; set; }

    [BindProperty]
    public string? Remarks { get; set; }
    [BindProperty]
    public string? RoleId { get; set; }

    [BindProperty]
    public bool ChkStatus { get; set; }

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
        checkAuthorization(ModuleLogAction.Create_RoleMaintenance);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            BindData();
            if (session.GetString("EditRoleID") != null) //edit
            {
                RoleId = session.GetString("EditRoleID").ToString();
                EditCommand(RoleId);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Role Management > Add New";
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
        DataTable data = pageLoad.GetListOfModule();
        List<ModuleDataViewModel> moduleData = Common.DataHelper.ConvertDataTableToList<ModuleDataViewModel>(data);
        foreach (var item in moduleData)
        {
            Modules.Add(new ModuleViewModel { mID = item.mID, parent = item.parent, child = item.child });
        }
    }

    protected void EditCommand(string roleId)
    {
        try
        {
            DataTable dt = userControl.GetModulePermissionByRoleId(Guid.Parse(roleId.ToString()));
            RoleId = roleId;
            List<ModuleDataViewModel> permissions = Common.DataHelper.ConvertDataTableToList<ModuleDataViewModel>(dt);

            foreach (var item in Modules)
            {
                var itemData = permissions.Where(a => a.mID == item.mID).FirstOrDefault();
                if (itemData != null)
                {
                    item.mView = itemData.mView != 0;
                    item.mChecker = itemData.mChecker != 0;
                    item.mMaker = itemData.mMaker != 0;
                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                bool breakable = false;
                if (dr["rDesc"].ToString() != "" && dr["rDesc"].ToString() != null)
                {
                    RoleName = dr["rDesc"].ToString();
                    breakable = true;
                }
                if (dr["rRemarks"].ToString() != "" && dr["rRemarks"].ToString() != null)
                {
                    Remarks = dr["rRemarks"].ToString();
                    breakable = true;
                }
                if (dr["rStatus"].ToString() != "" && dr["rStatus"].ToString() != null)
                {
                    ChkStatus = Convert.ToInt32(dr["rStatus"]) == 1 ? true : false;

                    breakable = true;
                }
                if (breakable)
                    break;
            }
            ActiveVisible = true;
            PageTitleText = "Role Management > Edit";
            ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("EditRoleID");
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPost()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (ModelState.IsValid)
            {
                var RID = session.GetString("EditRoleID");


                Role entity = new Role();
                entity.RoleID = RID == null ? Guid.NewGuid() : Guid.Parse(RID.ToString());
                entity.RoleDesc = RoleName;
                entity.Items = Modules;
                entity.Remarks = Remarks;
                entity.Status = ChkStatus ? 1 : 2;
                entity.CreatedBy = _UserId;
                entity.ApprovedBy = _UserId;
                entity.DeclineBy = _UserId;
                entity.UpdatedBy = _UserId;
                entity.CreatedDate = DateTime.Now;
                entity.ApprovedDate = DateTime.Now;
                entity.DeclineDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;

                if (RID == null)
                {
                    Create_Process(entity);
                }
                else
                {
                    Update_Process(entity);
                }

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

    private void Create_Process(Role entity)
    {
        try
        {
            if (!userControl.isRoleAvailable(RoleName))
            {
                bool accessAudit = userControl.InsertRole(entity, Modules) && userControl.CreateData(entity, ClientIp);
                ErrorText = accessAudit ? "Successfully Created. Please Wait for Approval." : "Failed to Create Role.";
                AuditLog.CreateAuditLog(entity.RoleDesc + ": " + ErrorText, AuditCategory.System_User_Maintenance, ModuleLogAction.Create_RoleMaintenance, _UserId, accessAudit, ClientIp);
                PopUp("Alert!", ErrorText);
            }
            else
            {
                ErrorText = "The Name already exist.";
                ModelState.AddModelError("RoleName", ErrorText);
            }
            ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
            PageTitleText = "Role Management > Add New";
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    private void Update_Process(Role entity)
    {
        try
        {
            string validate = userControl.IsValidatedToInactive(entity.RoleID);
            if (!(string.IsNullOrEmpty(validate) || ChkStatus))
            {
                ErrorText = validate;
                ModelState.AddModelError("RoleName", ErrorText);
            }
            else
            {
                bool accessAudit = userControl.EditData(entity, ClientIp);
                ErrorText = accessAudit ? "Successfully Edited. Please Wait for Approval." : "Failed to Update.";
                PopUp("Alert!", ErrorText);
                AuditLog.CreateAuditLog(entity.RoleDesc + ": " + ErrorText, AuditCategory.System_User_Maintenance, ModuleLogAction.Update_RoleMaintenance, _UserId, accessAudit, ClientIp);
                HttpContext.Session.Remove("EditRoleID");
            }
            ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
            PageTitleText = "Role Management > Edit";
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }
}