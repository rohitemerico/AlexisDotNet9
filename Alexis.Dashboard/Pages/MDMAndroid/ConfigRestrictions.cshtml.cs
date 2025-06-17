using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMAndroid;

public class ConfigRestrictionsModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{


    public string PageTitleText { get; set; }

    public string? ClientIp { get; set; }
    [BindProperty]
    public string? DID { get; set; }
    [BindProperty]
    public string DEVICENAME { get; set; }
    [BindProperty]
    public string deviceMACAdd { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Latitude(ErrorMessage = "Invalid latitude. Must be between -90 and 90 with up to 6 decimal places.")]
    //[RegularExpression(@"^[-+]?(90(\.0{1,6})?|([0-8]?\d)(\.\d{1,6})?),\s*[-+]?(180(\.0{1,6})?|((1[0-7]\d)|([0-9]?\d{1,2}))(\.\d{1,6})?)$", ErrorMessage = "Please enter valid latitude.")]
    public string Latitude { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "This field is required.")]
    [Longitude(ErrorMessage = "Invalid longitude. Must be between -180 and 180 with up to 6 decimal places.")]
    //[RegularExpression(@"^[-+]?(90(\.0{1,6})?|([0-8]?\d)(\.\d{1,6})?),\s*[-+]?(180(\.0{1,6})?|((1[0-7]\d)|([0-9]?\d{1,2}))(\.\d{1,6})?)$", ErrorMessage = "Please enter valid longitude.")]
    public string Longitude { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please enter Radius.")]
    [RegularExpression(@"\d+", ErrorMessage = "Please enter numbers only.")]
    public string Radius { get; set; }
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
        checkAuthorization(ModuleLogAction.View_AndroidMDMDevices);
    }
    public IActionResult OnGet()
    {

        try
        {
            var session = httpContextAccessor.HttpContext.Session;

            if (session.GetString("DeviceMACID") != null) //edit
            {
                DID = session.GetString("DeviceMACID").ToString();
                EditCommand(DID);
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
    private void EditCommand(string DID)
    {
        DataTable device = AndroidMDMController.GetDeviceById(DID);
        deviceMACAdd = device.Rows[0]["deviceMACAdd"].ToString();
        DEVICENAME = device.Rows[0]["DEVICENAME"].ToString();
        Latitude = device.Rows[0]["Restriction_LATITUDE"].ToString();
        Longitude = device.Rows[0]["Restriction_LONGITUDE"].ToString();
        Radius = device.Rows[0]["Restriction_radius"].ToString();
    }
    public IActionResult OnPost()
    {
        bool accessAudit;
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (ModelState.IsValid)
            {
                var KID = session.GetString("DeviceMACID");

                accessAudit = AndroidMDMController.EditDeviceRestriction(DID, Latitude, Longitude, Radius);
                if (!accessAudit)
                {
                    ErrorText = $"Unable to update Geofence restriction configurations.";
                }
                else
                {
                    ErrorText = $"Geofence restriction configurations updated successfully.";
                }
                AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, ModuleLogAction.Update_AndroidMDMGeoFenceConfig, _UserId, accessAudit, ClientIp);
                PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("DeviceMACID");
        return new JsonResult(new { message = "Success" });
    }
}