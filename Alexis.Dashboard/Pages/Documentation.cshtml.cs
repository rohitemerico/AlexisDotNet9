using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;

namespace Alexis.Dashboard.Pages
{
    public class DocumentationModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
    {
        public Guid ModuleId { get; set; }
        public IActionResult OnGet()
        {
            try
            {
                getUserDetails();
                if (accessLogin)
                {
                    return RedirectToPage("/Login");
                }
                checkAuthorization(ModuleLogAction.View_Resources);
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
    }
}
