using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexis.Dashboard.Controller;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Alexis.Dashboard.Pages;

public class LoginModel : PageModel
{

    [BindProperty]
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string? ErrorMessage { get; set; }


    private readonly UserRoleController _userRole;

    public LoginModel()
    {
        _userRole = new UserRoleController();
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        bool allow = false;
        string username = Username.ToUpper().Trim();
        string pass = Password.Trim();


        try
        {
            // Active Directory Authentication (if implemented)
            if (!allow)
            {
                // allow = UserRoleADManager.IsADAuthenticate(username, pass);
            }

            // Local user authentication from database
            if (!allow)
            {
                allow = _userRole.AuthenticateLocalUser(username, pass);
            }

            if (allow)
            {
                Guid userId = _userRole.getUserID(username);
                HttpContext.Session.SetString("lFlag", _userRole.getLoginFlag(userId).ToString());
                HttpContext.Session.SetString("TroubleShoot", _userRole.getTroubleshoot(userId).ToString());
                if (_userRole.setLogin(HttpContext.Session.Id, userId))
                {
                    DataTable userDetails = _userRole.getUserDetails(userId);
                    HttpContext.Session.SetString("User_Det", JsonConvert.SerializeObject(userDetails));
                    return Redirect("~/Home");
                }
                else
                {
                    ErrorMessage = "Login Failed";
                }
            }
            else
            {
                ErrorMessage = "Incorrect Login ID / Password";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error occurred while logging in.";
            Logger.LogToFile("Alexis", nameof(LoginModel), nameof(OnPost), ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }

        return Page();

    }
}
