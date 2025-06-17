using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMiOS;

public class ProfileManagementModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public static MDM_ProfileBase My_FMDM_ProfileBase = MDM_ProfileFactory.Create("");

    public List<iOSMDMProfileViewModel> Profiles { get; set; }
    public string? ClientIp { get; set; }

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
        checkAuthorization(ModuleLogAction.View_iOSMDMProfileManagement);
    }

    public IActionResult OnGet()
    {
        try
        {
            BindGridData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void BindGridData()
    {
        DataTable data = My_FMDM_ProfileBase.GetAllProfile();
        Profiles = Common.DataHelper.ConvertDataTableToList<iOSMDMProfileViewModel>(data);
        foreach (var item in Profiles)
        {
            if ((item.Status == "Edited") || (item.Status == "Pending"))
            {
                item.Visible = false;
            }
        }
    }

    public IActionResult OnPostEdit(string id)
    {
        if (Guid.Parse(id) == Guid.Empty)
        {
            return new JsonResult(new { message = "Invalid ID." });
        }
        HttpContext.Session.SetString("MDMiOSProfileID", id);
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnGetAdd()
    {
        HttpContext.Session.Remove("MDMiOSProfileID");
        HttpContext.Session.Remove("MDMWiFiData");
        HttpContext.Session.Remove("MDMVPNData");
        HttpContext.Session.Remove("MDMRestrictionAppData");
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostBranchList(string ProfileID)
    {
        List<string> branches = new List<string>();
        try
        {
            DataTable data = My_FMDM_ProfileBase.GetBranchByProfileId(Guid.Parse(ProfileID));
            branches = data.AsEnumerable().Select(row => row["bdesc"]?.ToString()).Where(value => !string.IsNullOrEmpty(value)).ToList();
        }
        catch (Exception ex)
        {

        }
        return Partial("_BranchesPartial", branches);
    }
}