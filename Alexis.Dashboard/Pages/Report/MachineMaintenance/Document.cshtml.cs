using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.MachineMaintenance;

public class DocumentModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private DocumentSettingController documentControl = new DocumentSettingController();
    public List<KioskDocTypeViewModel> Docs { get; set; }
    public string ExportFileName { get => $"MachineMaintenance_Document_{DateTime.Now.ToString("yyyyMMdd")}"; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_MachineMaintenanceDocument);
    }

    public IActionResult OnGet()
    {
        try
        {
            LoadData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void LoadData()
    {
        DataTable data = documentControl.getDocSettingAll();
        DataTable data1 = data.DefaultView.ToTable(true, "did", "dDesc", "dCreatedDate", "CreatedBy", "docStatus", "dupdateddate", "updatedby");
        DataTable data2 = data.DefaultView.ToTable(true, "did", "REF_DID", "dName", "SWALLOW", "PRINT");
        Docs = Common.DataHelper.ConvertDataTableToList<KioskDocTypeViewModel>(data1);
        List<DocTypeViewModel> DocTypes = Common.DataHelper.ConvertDataTableToList<DocTypeViewModel>(data2);
        foreach (var item in Docs)
        {
            item.Components = DocTypes.Where(a => a.dID == item.dID).ToList();
        }
    }
}
