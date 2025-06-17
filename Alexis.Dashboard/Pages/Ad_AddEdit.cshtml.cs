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
using Microsoft.CodeAnalysis;

namespace Alexis.Dashboard.Pages;

public class Ad_AddEditModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    protected AdvertisementController controller = new AdvertisementController();
    public bool DisplayVideo { get; set; }

    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }
    public bool ActiveVisible { get; set; } = false;
    public string? ClientIp { get; set; }

    [BindProperty]
    public string? aID { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "The Description cannot be empty.")]
    public string aDesc { get; set; }

    [BindProperty]
    public IFormFile? Upload { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please enter the duration!")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter numbers only.")]
    public string aDuration { get; set; }

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
    [BindProperty]
    public string? FileName { get; set; }
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
        checkAuthorization(ModuleLogAction.Create_AdvertisementManagement);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("EditAdsID") != null) //edit
            {
                aID = session.GetString("EditAdsID").ToString();
                EditCommand(aID);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Advertisement > Add New";
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
            if (ModelState.IsValid)
            {
                var KID = session.GetString("EditAdsID");
                if ((Upload == null || Upload.Length == 0) && KID == null)
                {
                    ErrorText = "No File been upload.";
                    ModelState.AddModelError("Upload", ErrorText);
                }
                else
                {
                    if (Upload != null)
                    {
                        //string fileName1 = Path.GetFileNameWithoutExtension(Upload.FileName);
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
                            case ".WEBM":
                                format = "vid";
                                break;
                            case ".MP4":
                                format = "vid";
                                break;
                            case ".M4V":
                                format = "vid";
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
                        else if (!new[] { "img", "vid" }.Contains(format))
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
                        MAdvertisement entity = new MAdvertisement();
                        entity.AID = KID == null ? Guid.NewGuid() : Guid.Parse(KID);
                        entity.AName = fileName == null ? FileName : fileName;
                        entity.ADesc = aDesc;
                        entity.ADuration = Convert.ToInt32(aDuration);
                        entity.ADirectory = linkPath == null ? ImageUrl : linkPath;
                        entity.AIsBackgroundIMG = false;
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
                if (session.GetString("EditAdsID") != null)
                {
                    PageTitleText = "Advertisement > Edit";
                    ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                }
                else
                {
                    ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                    PageTitleText = "Advertisement > Add New";
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

    private void Create_Process(MAdvertisement entity)
    {

        bool accessAudit = controller.insertAdvert(entity) && controller.CreateData(entity, ClientIp);
        ErrorText = accessAudit ? "Successfully Created. Please Wait for Approval." : "Failed to create.";
        AuditLog.CreateAuditLog(entity.AName + ": " + ErrorText, AuditCategory.Agent_Maintenance, ModuleLogAction.Create_AdvertisementManagement, _UserId, accessAudit, ClientIp);
        PopUp("Alert!", ErrorText);
        ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
        PageTitleText = "Advertisement > Add New";
    }

    private void Update_Process(MAdvertisement entity)
    {
        bool accessAudit = new KioskCreateMaintenanceController().EditData(entity, ClientIp);
        ErrorText = accessAudit ? "Successfully Edited. Please Wait for Approval." : "Failed to Update.";
        AuditLog.CreateAuditLog(entity.AName + ": " + ErrorText, AuditCategory.Agent_Maintenance, ModuleLogAction.Update_AdvertisementManagement, _UserId, accessAudit, ClientIp);
        PopUp("Alert!", ErrorText);
        HttpContext.Session.Remove("EditAdsID");
        ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
        PageTitleText = "Advertisement > Edit";
    }


    private void EditCommand(string KID)
    {
        try
        {
            ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
            PageTitleText = "Advertisement > Edit";
            ActiveVisible = true;
            DataTable dt = controller.getAdvertisementById(Guid.Parse(KID.ToString()));
            if (dt.Rows.Count != 0)
            {
                aDesc = dt.Rows[0]["adesc"].ToString();
                aDuration = dt.Rows[0]["aduration"].ToString();
                Remarks = dt.Rows[0]["aremarks"].ToString();
                Status = Convert.ToInt32(dt.Rows[0]["aStatus"]) == 1 ? true : false;
                FileName = dt.Rows[0]["aname"].ToString();
            }
            if (!string.IsNullOrEmpty(dt.Rows[0]["ARELATIVEPATHURL"].ToString()))
            {
                string fileType = "";
                string fileName = FileName.ToUpper();

                if (fileName.Contains(".JPG"))
                    fileType = "img";
                else if (fileName.Contains(".JPEG"))
                    fileType = "img";
                else if (fileName.Contains(".GIF"))
                    fileType = "img";
                else if (fileName.Contains(".PNG"))
                    fileType = "img";
                else if (fileName.Contains(".BMP"))
                    fileType = "img";
                else if (fileName.Contains(".WEBM"))
                    fileType = "vid";
                else if (fileName.Contains(".MP4"))
                    fileType = "vid";
                else if (fileName.Contains(".M4V"))
                    fileType = "vid";

                if (fileType == "img")
                {
                    ImageUrl = dt.Rows[0]["ARELATIVEPATHURL"].ToString();
                }
                else
                {
                    DisplayVideo = true;
                    ImageUrl = dt.Rows[0]["ARELATIVEPATHURL"].ToString();
                }
            }
            else
            {
                ImageUrl = "~/Content/img/3_User%20Maintenance/NoImage.jpg";
                DisplayVideo = false;
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("EditAdsID");
        return new JsonResult(new { message = "Success" });
    }

}
