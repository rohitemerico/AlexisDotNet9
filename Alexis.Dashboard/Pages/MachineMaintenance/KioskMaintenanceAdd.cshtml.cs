using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Telerik.SvgIcons;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class KioskMaintenanceAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }
    public string ErrorText { get; set; }
    public bool ActiveVisible { get; set; } = false;
    public string? ClientIp { get; set; }
    public List<SelectListItem> Groups { get; set; }

    [BindProperty]
    public string? KID { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please enter Machine Name.")]
    [RegularExpression(@"^[ A-Za-z0-9_@.,-]*$", ErrorMessage = "Please enter valid character only.")]
    public string MachineName { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please enter Machine Serial.")]
    [RegularExpression(@"^[ A-Za-z0-9_@.,-]*$", ErrorMessage = "Please enter valid character only.")]
    public string MachineSerial { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please enter Address.")]
    public string Address { get; set; }
    [BindProperty]
    [RegularExpression(@"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$", ErrorMessage = "Please enter valid latitude.")]
    public string Latitude { get; set; }
    [BindProperty]
    [RegularExpression(@"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$", ErrorMessage = "Please enter valid longitude.")]
    public string Longitude { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please enter Kiosk ID.")]
    [RegularExpression(@"^[ A-Za-z0-9_@.,-]*$", ErrorMessage = "Please enter valid character only.")]
    public string KioskID { get; set; }
    [BindProperty]
    [RegularExpression(@"^\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b", ErrorMessage = "Invalid IP Address Format.")]
    [Required(ErrorMessage = "Please enter IP Address.")]
    public string IP { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please enter Port Number.")]
    [RegularExpression(@"\d+", ErrorMessage = "Please enter numbers only.")]
    public string Port { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please enter Station ID.")]
    [RegularExpression(@"\d+", ErrorMessage = "Please enter numbers only.")]
    public string StationID { get; set; }
    [Required(ErrorMessage = "Please select Group Template.")]
    [BindProperty]
    public string GroupTemplate { get; set; }
    [BindProperty]
    public string? Remarks { get; set; }
    [BindProperty]
    public bool Status { get; set; }

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
        checkAuthorization(ModuleLogAction.Create_Kiosk);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            BindDropDownList();
            if (session.GetString("EditMachineCreateID") != null) //edit
            {
                KID = session.GetString("EditMachineCreateID").ToString();
                ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                PageTitleText = "Machine Management > Edit";
                EditCommand(KID);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Management > Add New";
            }

        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
    public IActionResult OnPost()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            BindDropDownList();
            if (ModelState.IsValid)
            {
                var KID = session.GetString("EditMachineCreateID");
                System.Data.DataTable dt = KioskCreateMaintenanceController.bindMachine();
                if (dt.Rows.Count > 8 && KID is null)
                {
                    ErrorText = "Kiosk License Exceed.";
                    ModelState.AddModelError("GroupTemplate", ErrorText);
                }
                else
                {

                    MKiosk entity = new MKiosk();
                    entity.MachineID = KID == null ? Guid.NewGuid() : Guid.Parse(KID.ToString());
                    entity.MachineDescription = MachineName;
                    entity.MachineSerial = MachineSerial;
                    entity.MachineAddress = Address;
                    entity.MachineLatitude = Latitude;
                    entity.MachineLongtitude = Longitude;
                    entity.MachineGroupID = Guid.Parse(GroupTemplate);
                    entity.MachineKioskID = KioskID;
                    entity.MachineStationID = StationID;
                    var selectedItem = Groups.FirstOrDefault(x => x.Value == GroupTemplate);
                    if (selectedItem != null)
                    {
                        entity.MachineGroupDesc = selectedItem.Text;
                    }
                    entity.MacIP = IP;
                    entity.MacPort = Port;
                    entity.Remarks = Remarks;
                    entity.Status = Status ? 1 : 2;
                    entity.CreatedBy = _UserId;
                    entity.ApprovedBy = _UserId;
                    entity.DeclineBy = _UserId;
                    entity.UpdatedBy = _UserId;
                    entity.CreatedDate = DateTime.Now;
                    entity.ApprovedDate = DateTime.Now;
                    entity.DeclineDate = DateTime.Now;
                    entity.UpdatedDate = DateTime.Now;

                    if (KID == null)
                    {
                        Create_Process(entity);
                    }
                    else
                    {
                        Update_Process(entity);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
    private void Create_Process(MKiosk entity)
    {
        if (KioskCreateMaintenanceController.isMachineAvailable(entity.MachineSerial))
        {
            if (KioskCreateMaintenanceController.isKioskIDAvailable(entity.MachineKioskID))
            {
                bool accessAudit = false;
                if (KioskCreateMaintenanceController.CreateKiosk(entity, ClientIp) && new KioskCreateMaintenanceController().CreateData(entity, ClientIp))
                {
                    ErrorText = "Successfully Created. Please Wait for Approval.";
                    accessAudit = true;
                }
                else
                {
                    ErrorText = "Failed to create " + entity.MachineDescription;
                }
                AuditLog.CreateAuditLog(entity.MachineSerial + ": " + ErrorText, AuditCategory.Kiosk_Maintenance, ModuleLogAction.Create_Kiosk, _UserId, accessAudit, ClientIp);
                PopUp("Alert!", ErrorText);
            }
            else
            {
                ErrorText = "Duplicated Kiosk ID.";
                ModelState.AddModelError("KioskID", ErrorText);
            }
        }
        else
        {
            ErrorText = "Machine Already Exist";
            ModelState.AddModelError("MachineSerial", ErrorText);
        }
        ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
        PageTitleText = "Machine Management > Add New";
    }

    private void Update_Process(MKiosk entity)
    {
        bool accessAudit = false;
        if (new KioskCreateMaintenanceController().EditData(entity, ClientIp))
        {
            ErrorText = "Successfully updated machine with serial " + MachineSerial + ". Please Wait for Approval.";
            accessAudit = true;
        }
        else
        {
            ErrorText = "Failed to update machine with serial " + MachineSerial;
        }
        AuditLog.CreateAuditLog(entity.MachineSerial + ": " + ErrorText, AuditCategory.Kiosk_Maintenance, ModuleLogAction.Update_Kiosk, _UserId, accessAudit, ClientIp);
        PopUp("Alert!", ErrorText);
        HttpContext.Session.Remove("EditMachineCreateID");
        ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
        PageTitleText = "Machine Management > Edit";
    }

    private void BindDropDownList()
    {
        System.Data.DataTable data = KioskCreateMaintenanceController.bindGroup();
        List<GroupMaintenanceViewModel> list = Common.DataHelper.ConvertDataTableToList<GroupMaintenanceViewModel>(data);
        Groups = [new SelectListItem { Text = "Please Select One", Value = "" }];
        foreach (GroupMaintenanceViewModel group in list)
        {
            Groups.Add(new SelectListItem { Text = group.kdesc, Value = group.kId });
        }
    }

    private void EditCommand(string KID)
    {
        try
        {
            string mid = KID;
            string HiddenOldValue = KioskCreateMaintenanceController.getMachineItems(Guid.Parse(mid));
            string[] tmp = Regex.Split(HiddenOldValue, "#SPLIT#");
            MachineName = tmp[3].ToString();
            KioskID = tmp[4].ToString();
            StationID = tmp[5].ToString();
            MachineSerial = tmp[6].ToString();
            Address = tmp[8].ToString();
            Latitude = tmp[9].ToString();
            Longitude = tmp[10].ToString();
            Status = (tmp[1].ToString() == "Active") ? true : false;
            IP = tmp[19].ToString();
            Port = tmp[20].ToString();
            Remarks = tmp[21].ToString();
            string GroupTemplateText = tmp[14].ToString();
            var selectedItem = Groups.FirstOrDefault(x => x.Text == GroupTemplateText);
            if (selectedItem != null)
            {
                GroupTemplate = selectedItem.Value;
            }
            ActiveVisible = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("EditMachineCreateID");
        return new JsonResult(new { message = "Success" });
    }
}
