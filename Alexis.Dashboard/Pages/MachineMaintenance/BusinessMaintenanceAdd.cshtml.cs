using System.ComponentModel.DataAnnotations;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Telerik.SvgIcons;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class BusinessMaintenanceAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "The Branch Name cannot be empty.")]
    [RegularExpression(@"^[ A-Za-z0-9_@.,-]*$", ErrorMessage = "Please enter valid character only.")]
    public string BranchName { get; set; }
    [BindProperty]
    public bool Monday { get; set; }
    [BindProperty]
    public bool Tuesday { get; set; }
    [BindProperty]
    public bool Wednesday { get; set; }
    [BindProperty]
    public bool Thursday { get; set; }
    [BindProperty]
    public bool Friday { get; set; }
    [BindProperty]
    public bool Saturday { get; set; }
    [BindProperty]
    public bool Sunday { get; set; }
    [BindProperty]
    public bool ChkStatus { get; set; }
    [BindProperty]
    public string Start { get; set; } = "08:00"; // Default time
    [BindProperty]
    public string End { get; set; } = "17:00"; // Default time
    [BindProperty]
    public string? BusinessId { get; set; }
    [BindProperty]
    public string? Remarks { get; set; }
    public string? LabelInfo { get; set; }
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
        checkAuthorization(ModuleLogAction.Create_BusinessHour_Template);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("EditBusinessID") != null) //edit
            {
                BusinessId = session.GetString("EditBusinessID").ToString();
                EditCommand(BusinessId);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Business Operation > Add New";
            }
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

    public IActionResult OnPost()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (ModelState.IsValid)
            {
                var BID = session.GetString("EditBusinessID");
                MBizHour item = new MBizHour();
                item.Bid = string.IsNullOrEmpty(BID) ? Guid.NewGuid() : Guid.Parse(BID);
                item.TemplateName = BranchName;
                item.Monday = Monday;
                item.Tuesday = Tuesday;
                item.Wednesday = Wednesday;
                item.Thursday = Thursday;
                item.Friday = Friday;
                item.Saturday = Saturday;
                item.Sunday = Sunday;

                item.Endtime = TimeSpan.Parse(End);
                item.Starttime = TimeSpan.Parse(Start);

                item.Remarks = Remarks;
                item.Status = ChkStatus ? 1 : 2;

                item.CreatedBy = _UserId;
                item.UpdatedBy = _UserId;

                item.CreatedDate = DateTime.Now;
                item.UpdatedDate = DateTime.Now;

                if (BID is null)
                {
                    CreateNew(item);
                }
                else
                {
                    UpdateOld(item);
                }
            }
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
    private void EditCommand(string BusinessId)
    {
        try
        {
            ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
            PageTitleText = "Machine Business Operation > Edit";
            MBizHour item = new MBizHour();
            item = BusinessHourMaintenanceController.getBusinessOperating(Guid.Parse(BusinessId));

            //update description
            BranchName = item.TemplateName;

            //update sliders 
            Monday = item.Monday;
            Tuesday = item.Tuesday;
            Wednesday = item.Wednesday;
            Thursday = item.Thursday;
            Friday = item.Friday;
            Saturday = item.Saturday;
            Sunday = item.Sunday;
            //update operating hours
            string[] tmpstartTime = item.Starttime.ToString().Split(':');
            string[] tmpendTime = item.Endtime.ToString().Split(':');

            Start = tmpstartTime[0].ToString() + ":" + tmpstartTime[1].ToString();
            End = tmpendTime[0].ToString() + ":" + tmpendTime[1].ToString();
            Remarks = item.Remarks;
            ChkStatus = item.Status == 1 ? true : false;
            if (item.Status == 2) //pending
            {
                LabelInfo = "Note: Cannot edit. Status is already pending.";
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("EditBusinessID");
        return new JsonResult(new { message = "Success" });
    }

    private void CreateNew(MBizHour item)
    {
        try
        {

            if (BusinessHourMaintenanceController.isTemplateValid(item.TemplateName, item.Bid))
            {

                bool accessAudit = false;
                if (BusinessHourMaintenanceController.CreateBusinessHour(item, ClientIp) && new BusinessHourMaintenanceController().CreateData(item, ClientIp))
                {
                    ErrorText = "Successfully Created. Please Wait for Approval.";
                    accessAudit = true;
                }
                else
                    ErrorText = "Failed to create " + BranchName;
                AuditLog.CreateAuditLog(item.TemplateName + ": " + ErrorText, AuditCategory.Kiosk_Maintenance, ModuleLogAction.Create_BusinessHour_Template, _UserId, accessAudit, ClientIp);
                PopUp("Alert!", ErrorText);
            }
            else
            {
                ErrorText = "Branch Name already exist.";
                ModelState.AddModelError("BranchName", ErrorText);
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Business Operation > Add New";
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    private void UpdateOld(MBizHour item)
    {
        try
        {
            bool accessAudit = false;
            if (BusinessHourMaintenanceController.isTemplateValid(item.TemplateName, item.Bid))
            {
                if (new BusinessHourMaintenanceController().EditData(item, ClientIp))
                {
                    ErrorText = "Successfully Edited! Please Wait for Approval.";
                    accessAudit = true;
                }
                else
                    ErrorText = "Failed to update " + item.TemplateName;

                AuditLog.CreateAuditLog(item.TemplateName + ": " + ErrorText, AuditCategory.Kiosk_Maintenance, ModuleLogAction.Update_BusinessHour_Template, _UserId, accessAudit, ClientIp);
                PopUp("Alert!", ErrorText);

                HttpContext.Session.Remove("EditBusinessID");
            }
            else
            {
                ErrorText = "Branch Name already exist.";
                ModelState.AddModelError("BranchName", ErrorText);
                ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                PageTitleText = "Machine Business Operation > Edit";
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }
}
