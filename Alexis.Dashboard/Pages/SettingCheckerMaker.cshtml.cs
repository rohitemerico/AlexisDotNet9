using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages;

public class SettingCheckerMakerModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    protected MasterSettingController msController = new MasterSettingController();
    public string? ClientIp { get; set; }
    public List<SettingsViewModel> Settings { get; set; }
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
        checkAuthorization(ModuleLogAction.View_SettingCheckerMaker);
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

    public void BindGridData()
    {
        string currentUser = UserDetails.Rows[0]["uName"].ToString();
        Guid Moduleid = Guid.Empty;
        DataTable dt = msController.GetDTMasterSettings();
        DataTable ret = new DataTable();
        ret.Columns.Add("mID", typeof(Guid));
        ret.Columns.Add("PWORD", typeof(string));
        ret.Rows.Add(Module.GetModuleId(ModuleLogAction.View_MasterSetting).ToString(), dt.Rows[0]["PWORD"].ToString());
        Settings = Common.DataHelper.ConvertDataTableToList<SettingsViewModel>(ret);
        Moduleid = Module.GetModuleId(ModuleLogAction.Approve_MasterSetting);
        foreach (SettingsViewModel item in Settings)
        {
            bool editing = new GlobalController().IsEditingDataExist(item.mID);
            string editor = new GlobalController().GetEditorIfEditingDataExist(item.mID);
            if (Checker)
            {
                if ((item.bStatus == "Pending") && item.CreatedBy != UserDetails.Rows[0]["uName"].ToString())
                {
                    item.AllowView = true;
                    item.AllowApprove = true;
                    item.AllowReject = true;
                }
                else if (editing && (editor != UserDetails.Rows[0]["uName"].ToString()))
                {
                    item.AllowView = true;
                }
            }
            if (item.bStatus == "Active" && editing && (editor == UserDetails.Rows[0]["uName"].ToString()))
            {
                item.AllowView = true;
                item.AllowDecline = true;
            }
        }

    }
}


public class SettingsViewModel
{
    public Guid mID { get; set; }
    public string PWORD { get; set; }
    public string bStatus { get; set; }
    public string CreatedBy { get; set; }
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
}