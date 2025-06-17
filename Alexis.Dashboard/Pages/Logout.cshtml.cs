using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages;

public class LogoutModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
    }

    public IActionResult OnGet()
    {
        UserRoleController userRole = new UserRoleController();
        userRole.setLogout(_UserId);
        var session = httpContextAccessor.HttpContext.Session;
        session.Clear();
        return Redirect("~/Login");
    }
}
