using System.Data;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogic.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.Enrollment;

public class iOSModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private static ReportingBase iOS_MDM_ReportingBase = ReportingFactory.Create("");
    public string ExportFileName { get => $"iOS_Enrollment_{DateTime.Now.ToString("yyyyMMdd")}"; }
    public List<iOSMDMDeviceInfoViewModel> Devices { get; set; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_EnrollmentiOS);
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
