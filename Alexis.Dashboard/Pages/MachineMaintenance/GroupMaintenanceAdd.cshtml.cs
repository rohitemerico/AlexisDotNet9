using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexis.Common;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class GroupMaintenanceAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }
    public bool ActiveVisible { get; set; } = false;
    public string? ClientIp { get; set; }

    [BindProperty]
    public string? KID { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please Enter Template Name.")]
    public string KDesc { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please choose from the Hopper Template.")]
    public string KHopperID { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please choose from the Document Template.")]
    public string KDocumentID { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please choose from the Alert Template.")]
    public string KAlertID { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Please choose from the Business Operating Hour.")]
    public string KBusinessHourID { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please choose from the Advertisement Template.")]
    public List<string> KAdvTempleteID { get; set; }
    [BindProperty]
    public IFormFile? Upload { get; set; }
    [BindProperty]
    public string? Remarks { get; set; }
    [BindProperty]
    public bool Status { get; set; }
    [BindProperty]
    public string? ImageUrl { get; set; }
    [BindProperty]
    public string? AlternateText { get; set; }
    [BindProperty]
    public string? BackgroundText { get; set; }
    public List<SelectListItem> Hoppers { get; set; }
    public List<SelectListItem> Documents { get; set; }
    public List<SelectListItem> Alertlist { get; set; }
    public List<SelectListItem> OperatingHours { get; set; }
    public List<SelectListItem> AdvTemplates { get; set; }
    public string ErrorText { get; set; }
    public string linkPath { get; set; }
    public string fileName { get; set; }

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
        checkAuthorization(ModuleLogAction.Create_Group_Template);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("EditGroupID") != null) //edit
            {
                KID = session.GetString("EditGroupID").ToString();
                EditCommand(KID);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Group > Add New";
            }
            BindDropDownList();
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
                var KID = session.GetString("EditGroupID");
                if ((Upload == null || Upload.Length == 0) && KID == null)
                {
                    ErrorText = "No File been upload.";
                    ModelState.AddModelError("Upload", ErrorText);
                }
                else if (GroupMaintenanceController.isGroupAvailable(KDesc, Guid.TryParse(KID, out var guid) ? guid : (Guid?)null))
                {
                    if (Upload != null)
                    {
                        string fileName1 = Path.GetFileNameWithoutExtension(Upload.FileName);
                        string format = Path.GetExtension(Upload.FileName);
                        string SystemFileName = Generate.GenerateUniqueID() + format;
                        string fileFullName = Path.GetFileName(Upload.FileName);

                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ConfigHelper.AdvPathTmp);
                        if (!Directory.Exists(fullPath))
                            Directory.CreateDirectory(fullPath);
                        string SavePath = Path.Combine(fullPath, SystemFileName);
                        string LinkPath = Path.Combine(ConfigHelper.AdvPathTmp, SystemFileName);

                        switch (format.ToUpper().ToString())
                        {
                            case ".JPG":
                                format = "img";
                                break;
                            case ".JPEG":
                                format = "img";
                                break;
                            case ".GIF":
                                format = "img";
                                break;
                            case ".PNG":
                                format = "img";
                                break;
                            case ".BMP":
                                format = "img";
                                break;
                            case ".MOV":
                                format = "ERR";
                                break;
                            case ".MP4":
                                format = "ERR";
                                break;
                            case ".M4V":
                                format = "ERR";
                                break;
                            default:
                                format = "ERR";
                                break;
                        }

                        if (format == "ERR")
                        {
                            ErrorText = "The file format is invalid!";
                            ModelState.AddModelError("Upload", ErrorText);
                        }
                        else if (format != "img")
                        {
                            ErrorText = "Error for validation";
                            ModelState.AddModelError("Upload", ErrorText);
                        }
                        else
                        {
                            using (var stream = new FileStream(SavePath, FileMode.Create))
                            {
                                Upload.CopyTo(stream);
                            }

                            linkPath = LinkPath;
                            fileName = fileFullName;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(ErrorText))
                    {
                        MAdvertisement entityAdv = new MAdvertisement();
                        entityAdv.AID = AlternateText == null ? Guid.NewGuid() : Guid.Parse(AlternateText);
                        entityAdv.AName = fileName == null ? BackgroundText : fileName;
                        entityAdv.ADesc = "For Background Image!";
                        entityAdv.ADuration = 0;
                        entityAdv.ADirectory = linkPath == null ? ImageUrl : linkPath;
                        entityAdv.AIsBackgroundIMG = true;

                        entityAdv.Status = Status ? 1 : 2;
                        entityAdv.CreatedBy = _UserId;
                        entityAdv.ApprovedBy = _UserId;
                        entityAdv.DeclineBy = _UserId;
                        entityAdv.UpdatedBy = _UserId;
                        entityAdv.CreatedDate = DateTime.Now;
                        entityAdv.ApprovedDate = DateTime.Now;
                        entityAdv.DeclineDate = DateTime.Now;
                        entityAdv.UpdatedDate = DateTime.Now;

                        MGroup entity = new MGroup();
                        entity.KID = KID == null ? Guid.NewGuid() : Guid.Parse(KID.ToString());
                        entity.KDesc = KDesc;
                        entity.KHopperID = Guid.Parse(KHopperID);
                        entity.KDocumentID = Guid.Parse(KDocumentID);
                        entity.KAlertID = Guid.Parse(KAlertID);
                        entity.KBusinessHourID = Guid.Parse(KBusinessHourID);

                        var selectedItem1 = Hoppers.FirstOrDefault(x => x.Value == KHopperID);
                        if (selectedItem1 != null)
                        {
                            entity.KHopperDesc = selectedItem1.Text;
                        }
                        var selectedItem2 = Documents.FirstOrDefault(x => x.Value == KDocumentID);
                        if (selectedItem2 != null)
                        {
                            entity.KDocumentDesc = selectedItem2.Text;
                        }
                        var selectedItem3 = Alertlist.FirstOrDefault(x => x.Value == KAlertID);
                        if (selectedItem3 != null)
                        {
                            entity.KAlertDesc = selectedItem3.Text;
                        }
                        var selectedItem4 = OperatingHours.FirstOrDefault(x => x.Value == KBusinessHourID);
                        if (selectedItem4 != null)
                        {
                            entity.KBusinessHourDesc = selectedItem4.Text;
                        }
                        entity.KScreenBackground = entityAdv;
                        entity.AdvIds = KAdvTempleteID;
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
                else
                {
                    ErrorText = "The name is already exist.";
                    ModelState.AddModelError("KDesc", ErrorText);
                }
            }
            if (session.GetString("EditGroupID") != null)
            {
                PageTitleText = "Machine Group > Edit";
                ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                ActiveVisible = true;
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Group > Add New";
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void Create_Process(MGroup entity)
    {

        bool accessAudit = GroupMaintenanceController.createGroup(entity, ClientIp) && new GroupMaintenanceController().CreateData(entity, ClientIp);
        ErrorText = accessAudit ? "Successfully Created. Please Wait for Approval." : "Failed to create " + entity.KDesc;
        AuditLog.CreateAuditLog(entity.KDesc + ": " + ErrorText, AuditCategory.Kiosk_Maintenance, ModuleLogAction.Create_Group_Template, _UserId, accessAudit, ClientIp);
        PopUp("Alert!", ErrorText);
        ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
        PageTitleText = "Machine Group > Add New";
    }

    private void Update_Process(MGroup entity)
    {
        bool accessAudit = new KioskCreateMaintenanceController().EditData(entity, ClientIp);
        ErrorText = accessAudit ? "Successfully updated machine with serial " + KDesc + ". Please Wait for Approval." : "Failed to update machine with serial " + KDesc;
        AuditLog.CreateAuditLog(entity.KDesc + ": " + ErrorText, AuditCategory.Kiosk_Maintenance, ModuleLogAction.Update_Group_Template, _UserId, accessAudit, ClientIp);
        PopUp("Alert!", ErrorText);
        HttpContext.Session.Remove("EditGroupID");
        ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
        PageTitleText = "Machine Management > Edit";
    }

    private void EditCommand(string KID)
    {
        try
        {
            ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
            PageTitleText = "Machine Group > Edit";
            ActiveVisible = true;
            DataTable detail = GroupMaintenanceController.getAssignedGroupsByID(Guid.Parse(KID));

            KDesc = detail.Rows[0]["description"].ToString();
            KHopperID = detail.Rows[0]["hID"].ToString();
            KDocumentID = detail.Rows[0]["dID"].ToString();
            KAlertID = detail.Rows[0]["alertID"].ToString();
            KBusinessHourID = detail.Rows[0]["bID"].ToString();

            Remarks = detail.Rows[0]["kRemarks"].ToString();
            Status = Convert.ToBoolean(Convert.ToInt32(detail.Rows[0]["kStatus"].ToString()));
            KAdvTempleteID = detail.AsEnumerable().Select(row => row.Field<Guid>("advertID").ToString()).ToList();
            ImageUrl = detail.Rows[0]["aDirectory"].ToString();
            AlternateText = detail.Rows[0]["KSCREENBACKGROUND"].ToString();
            BackgroundText = detail.Rows[0]["backgroundImage"].ToString();
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
        HttpContext.Session.Remove("EditGroupID");
        return new JsonResult(new { message = "Success" });
    }

    private void BindDropDownList()
    {
        DataTable data1 = GroupMaintenanceController.bindHopper();
        List<HopperMaintenanceViewModel> list1 = Common.DataHelper.ConvertDataTableToList<HopperMaintenanceViewModel>(data1);
        Hoppers = [new SelectListItem { Text = "Please Select One", Value = "" }];
        foreach (HopperMaintenanceViewModel item in list1)
        {
            Hoppers.Add(new SelectListItem { Text = item.hDesc, Value = item.hID });
        }

        DataTable data2 = GroupMaintenanceController.bindDoccument();
        List<KioskDocTypeViewModel> list2 = Common.DataHelper.ConvertDataTableToList<KioskDocTypeViewModel>(data2);
        Documents = [new SelectListItem { Text = "Please Select One", Value = "" }];
        foreach (KioskDocTypeViewModel item in list2)
        {
            Documents.Add(new SelectListItem { Text = item.dDesc, Value = item.dID });
        }

        DataTable data3 = GroupMaintenanceController.bindAlert();
        List<AlertViewModel> list3 = Common.DataHelper.ConvertDataTableToList<AlertViewModel>(data3);
        Alertlist = [new SelectListItem { Text = "Please Select One", Value = "" }];
        foreach (AlertViewModel item in list3)
        {
            Alertlist.Add(new SelectListItem { Text = item.aDesc, Value = item.aID });
        }

        DataTable data4 = GroupMaintenanceController.bindOperatingHour();
        List<BusinessMaintenanceViewModel> list4 = Common.DataHelper.ConvertDataTableToList<BusinessMaintenanceViewModel>(data4);
        OperatingHours = [new SelectListItem { Text = "Please Select One", Value = "" }];
        foreach (BusinessMaintenanceViewModel item in list4)
        {
            OperatingHours.Add(new SelectListItem { Text = item.bDesc, Value = item.bId });
        }

        DataTable data5 = GroupMaintenanceController.bindAdvertisement();
        List<Advertisement> list5 = Common.DataHelper.ConvertDataTableToList<Advertisement>(data5);
        AdvTemplates = [];
        //AdvTemplates = [new SelectListItem { Text = "Please Select One", Value = "" }];
        foreach (Advertisement item in list5)
        {
            AdvTemplates.Add(new SelectListItem { Text = item.adesc, Value = item.aid.ToString() });
        }
    }

}

public class Advertisement
{
    public Guid aid { get; set; }

    public string adesc { get; set; }
}