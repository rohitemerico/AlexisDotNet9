using System.ComponentModel.DataAnnotations;
using Alexis.Dashboard.Controller;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alexis.Dashboard.Pages;

public class RegisterModel : PageModel
{
    UserRoleController userRole = new UserRoleController();
    [BindProperty]
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email Address is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [BindProperty]
    public bool ConsentCheck { get; set; }

    public string? ErrorMessage { get; set; }
    public string? ClientIp { get; set; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var forwardedHeader = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        ClientIp = forwardedHeader ?? HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        string username = Username.ToUpper();
        string email = Email.Trim().ToLower();

        try
        {
            //verifyuser
            if (!userRole.isUserAvailable(username) && !userRole.isUserLoginEmailAvailable(username, email))
            {
                //Register user in oracledb
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(email) && IsEmailValidEmail(email))
                {
                    Guid userid = Guid.NewGuid();
                    var entity = new User()
                    {
                        UserID = userid,
                        UserName = username,
                        UserFullName = username,
                        CreatedDate = DateTime.Now,
                        CreatedBy = userid,
                        Troubleshoot = true,
                        RoleID = new Guid("8d72ac7b-96d4-481e-a895-9a54aa75f876") /* Default role */,
                        Password = Password,
                        Email = email,
                        LocalUser = true

                    };
                    bool accessAudit = userRole.InsertUser(entity) && userRole.CreateData(entity, ClientIp);
                    if (accessAudit)
                    {
                        ErrorMessage = "Account registered successfully.. Please Wait for Approval.";
                        Username = string.Empty;
                        Email = string.Empty;
                        ModelState.Clear();
                    }
                    else
                    {
                        ErrorMessage = "Failed to register account. Please try again";
                    }
                }
                else
                {
                    ErrorMessage = "Failed to register account. Please try again";
                }
            }
            else
            {
                ErrorMessage = "Account already exist";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error";
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
    public bool IsEmailValidEmail(string emailAddress)
    {
        try
        {
            System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailAddress);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
