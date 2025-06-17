using System.Data;
using System.Reflection;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogic.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.AppInstallation;

public class iOSDevicesModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private static ReportingBase iOS_MDM_ReportingBase = ReportingFactory.Create("");
    public string ExportFileName { get => $"iOS_Devices_{DateTime.Now:yyyyMMdd}"; }

    public string? ErrorText { get; set; }

    public List<iOSMDMDeviceInfoViewModel> Devices { get; set; }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_AppDevicesiOS);
    }

    public IActionResult OnGet()
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod()?.DeclaringType?.Name, MethodBase.GetCurrentMethod()?.Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void BindData()
    {
        DataTable DataTable1 = iOS_MDM_ReportingBase.GetAllDevices_Report();
        DataTable1.Columns.Add("CertListing", typeof(string));
        for (int i = 0; i < DataTable1.Rows.Count; i++)
        {
            var certListing = iOS_MDM_ReportingBase.GetDeviceCert_Report(DataTable1.Rows[i]["MachineUDID"].ToString());
            string certListing_str = string.Join("<br/><br/>", certListing.Rows.OfType<DataRow>().Select(x => string.Join("/", x.ItemArray)));
            DataTable1.Rows[i]["CertListing"] = certListing_str;
        }
        Devices = Common.DataHelper.ConvertDataTableToList<iOSMDMDeviceInfoViewModel>(DataTable1);
    }
}