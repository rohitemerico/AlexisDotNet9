using System.ComponentModel.DataAnnotations;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Helper;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Alexis.Dashboard.Pages;

public class ResetPasswordModel(IOptions<SmtpSettings> smtpOptions) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Cannot be empty.")]
    [DataType(DataType.Password)]
    public required string NewPassword { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Cannot be empty.")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "The New Password and Confirm Password do not match.")]
    public required string ConfirmPassword { get; set; }

    public string? ErrorMessage { get; set; }


    private readonly SmtpSettings _smtpSettings = smtpOptions.Value;

    public IActionResult OnGet()
    {
        try
        {
            string? val1 = Request.Query["param1"];
            if (!string.IsNullOrEmpty(val1) && Guid.TryParse(val1, out Guid g))
            {
                HttpContext.Session.SetString("NewPasswordGuid", g.ToString());
                ErrorMessage = string.Empty;
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
            }
            else
            {
                ErrorMessage = "Bad value";
            }
        }
        catch
        {
            // Optionally log error
            ErrorMessage = "Bad value";
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        string? guid = HttpContext.Session.GetString("NewPasswordGuid");

        if (string.IsNullOrWhiteSpace(guid))
        {
            ErrorMessage = "Session expired or invalid.";
            return Page();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        UserRoleController userRole = new UserRoleController();
        string? email = userRole.GetEmailUpdateForgottenPasswordForLocalUser(guid);

        if (!string.IsNullOrWhiteSpace(email))
        {
            try
            {
                email = new System.Net.Mail.MailAddress(email).Address;
                bool updated = userRole.UpdateForgottenPasswordForLocalUser(guid, NewPassword);
                if (updated)
                {
                    HttpContext.Session.Remove("NewPasswordGuid");
                    ErrorMessage = "The new password is accepted.";
                    string htmlBody = "<div>You have changed the password.";
                    string subject = "Password was updated";
                    string cc = "";
                    bool isSuccessful = Generate.SendEmail(email, cc, _smtpSettings.Username, subject, htmlBody, _smtpSettings.Password, _smtpSettings.Host, _smtpSettings.EnableSsl, _smtpSettings.Port);

                }
                else
                {
                    ErrorMessage = "Failed to update password.";
                }
            }
            catch
            {
                ErrorMessage = "Invalid email address.";
            }
        }
        else
        {
            ErrorMessage = "User not found.";
        }
        return Page();
    }
}
