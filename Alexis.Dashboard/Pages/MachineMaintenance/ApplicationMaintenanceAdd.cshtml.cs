using System.ComponentModel.DataAnnotations;
using Alexis.Common;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class ApplicationMaintenanceAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public string PageTitleText { get; set; }
    public string ErrorText { get; set; }
    public string? ClientIp { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Version is required.")]
    public string Version { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "File is required.")]
    public IFormFile Upload { get; set; }
    [BindProperty]
    public string? Remarks { get; set; }
    [BindProperty]
    public bool Pilot { get; set; }

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
        checkAuthorization(ModuleLogAction.View_Application);
    }

    public IActionResult OnGet()
    {
        try
        {
            PageTitleText = "Machine Application > Add New";
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
            var path = ConfigHelper.FPath;
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            bool fileOK = false;
            String fileExtension = System.IO.Path.GetExtension(Upload.FileName).ToLower();
            String[] allowedExtensions = { ".zip", ".exe" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                }
            }
            if (KioskVersionMaintenance.isDuplicatedVersion(Version, Pilot))
            {
                ErrorText = "Version " + Version + " is already exist in " + (Pilot ? "Agent Application." : "Machine.");
                ModelState.AddModelError("Version", ErrorText);
            }
            else
            {
                if (fileOK)
                {
                    string filename = Path.GetFileName(Upload.FileName);
                    var filePath = Path.Combine(fullPath, Path.GetFileName(Upload.FileName));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Upload.CopyTo(stream);
                    }

                    byte[] MyByte = System.IO.File.ReadAllBytes(filePath);
                    string bytes = Convert.ToBase64String(MyByte);
                    int onepart = Convert.ToInt32(ConfigHelper.OnePart);
                    int indicator = bytes.Length / onepart;

                    var xx = bytes.Length;

                    int startindex = 0;
                    int endindex = onepart;

                    MFirm entity = new MFirm();
                    entity.SysID = Guid.NewGuid();
                    entity.FPath = Path.Combine(path, filename);
                    entity.CountDL = indicator;
                    entity.Ver = Version;
                    entity.FSize = MyByte.Length;
                    entity.AgentFlag = Pilot;
                    entity.Remarks = Remarks;
                    entity.Status = 0;
                    entity.CreatedDate = DateTime.Now;

                    bool accessAudit = true;
                    if (KioskVersionMaintenance.InsertTblFirmware(entity))
                    {
                        string data = string.Empty;

                        for (int x = 0; x <= indicator; x++)
                        {

                            if (x == indicator)
                            {
                                data = bytes.Substring(startindex, bytes.Length - startindex);
                            }
                            else
                            {
                                data = bytes.Substring(startindex, onepart);
                            }

                            entity.DataPack.SysID = Guid.NewGuid();
                            entity.DataPack.FirmwareID = entity.SysID;
                            entity.DataPack.Indicator = x;
                            entity.DataPack.Data = data;

                            startindex = (onepart * (x + 1));

                            if (KioskVersionMaintenance.InsertTblFirmData(entity) && new KioskVersionMaintenance().CreateData(entity, ClientIp))
                            {
                                ErrorText = "Upload status: File uploaded!";
                                PopUp("Alert!", ErrorText);
                            }
                            else
                                accessAudit = false;
                        }
                    }
                    else
                        accessAudit = false;
                    AuditLog.CreateAuditLog(filename + ": " + entity.Ver, AuditCategory.Kiosk_Maintenance, ModuleLogAction.Update_Application, _UserId, accessAudit, ClientIp);

                }
                else
                {
                    ErrorText = "Upload status: The file could not be uploaded. The following error occured: File extension invalid (Only accept .zip and .exe)";
                    ModelState.AddModelError("Upload", ErrorText);
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
}