using System.ComponentModel.DataAnnotations;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Helper;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Alexis.Dashboard.Pages;

public class ForgotPasswordModel(IOptions<SmtpSettings> smtpOptions) : PageModel
{

    [BindProperty]
    [Required(ErrorMessage = "Login ID is required")]
    public string Username { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Enter valid Email ID.")]
    public string Email { get; set; }

    public string? ErrorMessage { get; set; }


    private readonly SmtpSettings _smtpSettings = smtpOptions.Value;

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        string username = Username.Trim().ToUpper();
        string email = Email.Trim().ToLower();
        try
        {
            // validation of the email
            email = new System.Net.Mail.MailAddress(email).Address;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email))
            {
                UserRoleController userRole = new UserRoleController();
                if (!userRole.isUserLoginEmailAvailable(username, email))
                {
                    ErrorMessage = "The entered data are incorrect";
                }
                else
                {

                    string guidString = userRole.GetGuidForgotPasswordLocalUser(username, email);
                    // assuming notes is 1 string without extra info. Set it to LOCALUSER table row.
                    if (!string.IsNullOrEmpty(guidString))
                    {

                        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                        var link = $"{baseUrl}/ResetPassword?param1={guidString}";
                        string htmlBody = "<div>In order to change the old password, you have 15 minutes to click the " +
                            "<a href=\"" + link + "\">link</a></div>";

                        string cc = "";//not used
                        string subject = "Forgot Password";

                        bool isSuccessful = Generate.SendEmail(email, cc, _smtpSettings.Username, subject, htmlBody, _smtpSettings.Password, _smtpSettings.Host, _smtpSettings.EnableSsl, _smtpSettings.Port);

                        if (isSuccessful)
                        {
                            ErrorMessage = "Email has been sent successfully!";
                            Username = string.Empty;
                            Email = string.Empty;
                        }
                        else
                            ErrorMessage = "Failed to send email!";
                    }
                    else
                    {
                        ErrorMessage = "The entered data are incorrect";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
        ModelState.Clear();

        return Page();
    }
}