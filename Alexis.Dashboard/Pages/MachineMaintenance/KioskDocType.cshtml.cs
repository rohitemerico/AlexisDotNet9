using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class KioskDocTypeModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private DocumentSettingController documentControl = new DocumentSettingController();

    public List<KioskDocTypeViewModel> DocTypes { get; set; }
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
        checkAuthorization(ModuleLogAction.View_DocType_Template);
    }

    public IActionResult OnGet()
    {
        try
        {
            BindGridData();
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

    private void BindGridData()
    {
        DataTable data = documentControl.getDocTypeAll();
        DocTypes = Common.DataHelper.ConvertDataTableToList<KioskDocTypeViewModel>(data);
        foreach (var item in DocTypes)
        {
            if (!Maker || userControl.IsEditingDataExist(Guid.Parse(item.dID)) || (item.txtStatus == "Pending"))
            {
                item.Visible = false;
            }
        }
    }

    public IActionResult OnPostEdit(string id)
    {
        if (Guid.Parse(id) == Guid.Empty)
        {
            return new JsonResult(new { message = "Invalid Alert ID." });
        }
        HttpContext.Session.SetString("EditDocTypeID", id);
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnGetAdd()
    {
        HttpContext.Session.Remove("EditDocTypeID");
        return new JsonResult(new { message = "Success" });
    }
}
