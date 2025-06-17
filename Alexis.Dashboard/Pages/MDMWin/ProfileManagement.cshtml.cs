using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Win.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMWin;

public class ProfileManagementModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private WinProfileController profController = new WinProfileController();
    public string? ClientIp { get; set; }

    public List<ProfileInfo> Profiles { get; set; }
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
        checkAuthorization(ModuleLogAction.View_WinMDMProfileManagement);
    }
    public IActionResult OnGet()
    {
        try
        {
            BindData();
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
        DataTable data = profController.GetAllMdmProfiles();
        Profiles = Common.DataHelper.ConvertDataTableToList<ProfileInfo>(data);
    }
}


public class ProfileInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Version { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? EditedDate { get; set; }
}
