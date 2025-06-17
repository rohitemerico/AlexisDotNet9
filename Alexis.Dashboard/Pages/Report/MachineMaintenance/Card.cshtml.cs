using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.Report.MachineMaintenance;

public class CardModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private readonly CardMaintenanceControl _cardControl = new();
    public List<CardViewModel> Cards { get; set; }
    public string ExportFileName => $"MachineMaintenance_Cards_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.View_MachineMaintenanceCard);
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

    public IActionResult OnPostClear()
    {
        return RedirectToPage();
    }

    private void LoadData()
    {
        DataTable data = _cardControl.getCardList();
        Cards = Common.DataHelper.ConvertDataTableToList<CardViewModel>(data);
    }
}