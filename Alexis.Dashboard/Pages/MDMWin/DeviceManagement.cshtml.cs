using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Win.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMWin;

public class DeviceManagementModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private readonly WinDeviceController mdmController = new WinDeviceController();
    //private readonly WinCmdExecController cmdController = new WinCmdExecController();
    public List<WindowsDeviceInfoViewModel> Devices { get; set; }
    public string? ClientIp { get; set; }
    public string ErrorText { get; set; }
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
        checkAuthorization(ModuleLogAction.View_WinMDMDeviceManagement);
    }
    public IActionResult OnGet()
    {
        try
        {
            BindGrid();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void BindGrid()
    {
        DataTable data = mdmController.GetAllClientDevices();
        Devices = Common.DataHelper.ConvertDataTableToList<WindowsDeviceInfoViewModel>(data);
    }


    public IActionResult OnPostReboot(string ID, string Name)
    {
        try
        {
            bool accessAudit = WinCmdExecController.RebootNow(ID).Result;
            ErrorText = accessAudit ? $"Successfully sent reboot command to {Name}." : $"Failed to send reboot command to {Name}.";
            AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Windows, ModuleLogAction.Send_WinMDMReboot, _UserId, accessAudit, ClientIp);
            PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
        }
        catch (Exception ex)
        {
            ErrorText = $"Failed to send reboot command to {Name}.";

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            return new JsonResult(new { message = "Error" });
        }
        return new JsonResult(new { message = "Success" });
    }
}


public class WindowsDeviceInfoViewModel
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string OEM { get; set; }
    public string FwV { get; set; }
    public string SwV { get; set; }
    public string HwV { get; set; }
    public string RadioSwV { get; set; }
    public string OSPlatform { get; set; }
    public string Resolution { get; set; }
    public int TotalStorage { get; set; }
    public int TotalRAM { get; set; }
    public string WlanIPv4Address { get; set; }

}
