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

namespace Alexis.Dashboard.Pages.UserMaintenance
{
    public class UserAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
    {
        protected new UserRoleController userControl = new();
        private readonly UserRolePageLoad pageLoad = new();
        protected Guid ModuleId { get; set; }


        #region Properties

        [BindProperty]
        public string? UserId { get; set; }
        [Required(ErrorMessage = "User Name cannot be empty.")]
        [BindProperty]
        public string Uname { get; set; }

        [Required(ErrorMessage = "Please Select Role.")]
        [BindProperty]
        public string RoleId { get; set; }

        [BindProperty]
        public string? Remarks { get; set; }

        [BindProperty]
        //[Required(ErrorMessage = "Password cannot be empty.")]
        //[PageRemote(ErrorMessage = "The Password field is required.", AdditionalFields = "checkLocal,__RequestVerificationToken", HttpMethod = "POST", PageHandler = "RequiredOrNot")]
        public string? Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Full Name cannot be empty.")]
        public string Fname { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email cannot be empty.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [BindProperty]
        public bool Status { get; set; }

        public bool ActiveVisible { get; set; } = false;

        public List<SelectListItem> Roles { get; set; }
        public string ConfirmText { get; set; }
        public string PageTitleText { get; set; }

        [BindProperty]
        public bool checkLocal { get; set; } = false;

        public bool ConfirmVisible { get; set; } = true;
        public string ErrorText { get; set; }
        public string? ClientIp { get; set; }

        #endregion

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
            checkAuthorization(ModuleLogAction.Create_UserMaintenance);
        }

        public IActionResult OnGet()
        {
            try
            {
                UserRoleController userControl = new UserRoleController();
                userControl.disabledormant();
                var session = httpContextAccessor.HttpContext.Session;
                if (session.GetString("EditUserID") != null) //edit
                {
                    string aID = session.GetString("EditUserID").ToString();
                    EditCommand(aID);
                }
                else
                {
                    ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                    PageTitleText = "User Management > Add New";
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

        public void BindDropDownList()
        {
            DataTable data = pageLoad.UserRole_DDLLoad();
            List<RoleModel> roles = Common.DataHelper.ConvertDataTableToList<RoleModel>(data);
            Roles = [new SelectListItem { Text = "Please select a role.", Value = "" }];
            foreach (var role in roles)
            {
                Roles.Add(new SelectListItem { Text = role.RDESC, Value = role.rID });
            }
        }

        public IActionResult OnGetCancel()
        {
            HttpContext.Session.Remove("EditUserID");
            return new JsonResult(new { message = "Success" });
        }

        public IActionResult OnPost()
        {
            try
            {
                var session = httpContextAccessor.HttpContext.Session;
                var UID = session.GetString("EditUserID");
                if (ModelState.IsValid)
                {
                    User entity = new();
                    entity.UserID = UID == null ? Guid.NewGuid() : Guid.Parse(UID);
                    entity.UserName = Uname;
                    entity.UserFullName = Fname;
                    entity.Email = Email;
                    entity.RoleID = RoleId != "" ? Guid.Parse(RoleId) : Guid.NewGuid();
                    entity.Remarks = Remarks;
                    entity.Status = Uname == UserDetails.Rows[0]["uName"].ToString() ? 1 : Status ? 1 : 2;
                    entity.CreatedBy = _UserId;
                    entity.ApprovedBy = _UserId;
                    entity.DeclineBy = _UserId;
                    entity.UpdatedBy = _UserId;
                    entity.CreatedDate = DateTime.Now;
                    entity.UpdatedDate = DateTime.Now;
                    if (checkLocal)
                    {
                        entity.LocalUser = true;
                        if (!string.IsNullOrWhiteSpace(Password))
                        {
                            entity.Password = Password;
                        }
                        else
                        {
                            entity.Password = "";
                        }
                    }
                    if (UID == null)
                    {
                        Create_Process(entity);
                    }
                    else
                    {
                        Update_Process(entity);
                    }
                }
                if (UID == null) //edit
                {
                    ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                    PageTitleText = "User Management > Add New";
                }
                else
                {
                    ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                    PageTitleText = "User Management > Edit";
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

        private void Create_Process(User entity)
        {
            try
            {
                bool accessAudit = false;
                if (checkLocal && string.IsNullOrWhiteSpace(entity.Password))
                {
                    ErrorText = "Password cannot be empty.";
                    ModelState.AddModelError("password", ErrorText);
                }
                else if (userControl.isUserAvailable(Uname, entity.UserID))
                {
                    accessAudit = userControl.InsertUser(entity) && userControl.CreateData(entity, ClientIp);
                    ErrorText = accessAudit ? "Successfully Created. Please Wait for Approval." : "Failed to Create User.";
                    AuditLog.CreateAuditLog(entity.UserName + ": " + ErrorText, AuditCategory.System_User_Maintenance, ModuleLogAction.Create_UserMaintenance, _UserId, accessAudit, ClientIp);
                    PopUp("Alert!", ErrorText);
                }
                else
                {
                    ErrorText = "The Name already exist.";
                    ModelState.AddModelError("Uname", ErrorText);
                }
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "User Management > Add New";
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                HPLog.WriteLog(ex.Message, "ERROR");
            }
        }

        private void Update_Process(User entity)
        {
            try
            {
                bool accessAudit = false;
                string validate = userControl.IsValidatedToInactive_User(entity.UserID);
                if (!(string.IsNullOrEmpty(validate) || Status))
                {
                    ErrorText = validate;
                    ModelState.AddModelError("Uname", ErrorText);
                }
                else
                {
                    accessAudit = userControl.EditData(entity, ClientIp);
                    ErrorText = accessAudit ? "Successfully Edited. Please Wait for Approval." : "Failed to Update.";
                    PopUp("Alert!", ErrorText);
                    AuditLog.CreateAuditLog(entity.UserName + ": " + ErrorText, AuditCategory.System_User_Maintenance, ModuleLogAction.Update_UserMaintenance, _UserId, accessAudit, ClientIp);
                    HttpContext.Session.Remove("EditRoleID");
                }
                ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                PageTitleText = "User Management > Edit";
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                HPLog.WriteLog(ex.Message, "ERROR");
            }
        }

        protected void EditCommand(string aID)
        {
            try
            {
                DataTable dt = userControl.GetUpdateUserCY(Guid.Parse(aID.ToString()));

                Uname = dt.Rows[0]["uName"].ToString();
                Fname = dt.Rows[0]["uFullName"].ToString();
                Email = dt.Rows[0]["Email"].ToString();
                RoleId = dt.Rows[0]["rID"].ToString();
                Remarks = dt.Rows[0]["uRemarks"].ToString();
                Status = Convert.ToInt32(dt.Rows[0]["uStatus"]) == 1;
                if (dt.Rows[0]["LocalUser"].ToString() != "")
                    checkLocal = Convert.ToInt32(dt.Rows[0]["LocalUser"]) == 1;
                UserId = aID;
                ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                PageTitleText = "User Management > Edit";
                ActiveVisible = true;
            }
            catch (Exception ex)
            {
                //Logger.LogToFile("User_Create_imgbtnEdit_Command.log", ex);
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                HPLog.WriteLog(ex.Message, "ERROR");
            }
        }
    }
}