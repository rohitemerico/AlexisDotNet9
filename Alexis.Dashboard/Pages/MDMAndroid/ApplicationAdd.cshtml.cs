using System.ComponentModel.DataAnnotations;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Alexis.Dashboard.Pages.MDMAndroid;
public class ApplicationAddModel(IHttpContextAccessor httpContextAccessor, ApkReader apkReader, IWebHostEnvironment env) : BasePageModel(httpContextAccessor)
{
    private readonly ApkReader _apkReader = apkReader;
    private readonly IWebHostEnvironment _env = env;
    public string PageTitleText { get; set; }
    public string ErrorText { get; set; }
    public string? ClientIp { get; set; }

    private static string strFilePath_Temp = string.Empty;
    private static string uploadedFileNameNoExt = string.Empty;
    private static string uploadedFile = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Application Name is required.")]
    public string ApplicationName { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Version is required.")]
    public string Version { get; set; }
    [BindProperty]

    [Required(ErrorMessage = "Application File is required.")]
    public IFormFile Upload { get; set; }

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
        checkAuthorization(ModuleLogAction.Create_AndroidMDMAppUpload);
        strFilePath_Temp = FileManager.GetStorageWebRootPath(StorageType.Temp);
    }

    public IActionResult OnGet()
    {
        try
        {
            PageTitleText = "Application Management > Add New";
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
            bool accessAudit = false;
            String fileExtension = Path.GetExtension(Upload.FileName).ToLower();
            String[] allowedExtensions = { ".apk" };

            if (allowedExtensions.Any(e1 => e1 == fileExtension))
            {
                string packageName = string.Empty;
                string filename = Path.GetFileName(Upload.FileName);
                string localFilePath = FileManager.GetLocalStoragePath(StorageType.Android_Apps);
                string filePathInWebRoot = FileManager.GetStorageWebRootPath(StorageType.Android_Apps);
                string fileURL = Path.Combine("\\", FileManager.GetStorageRelativeURL(StorageType.Android_Apps), filename).Replace("\\", "/");
                uploadedFileNameNoExt = Path.GetFileNameWithoutExtension(Upload.FileName);
                uploadedFile = Upload.FileName;
                try
                {
                    var filePath = Path.Combine(_env.WebRootPath, "uploads", Upload.FileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Upload.CopyTo(stream);
                    }
                    ApkInfo Info = _apkReader.GetFileInfo(filePath);
                    packageName = Info.PackageName;

                    //if (!Directory.Exists(localFilePath)) Directory.CreateDirectory(localFilePath);
                    if (!Directory.Exists(filePathInWebRoot)) Directory.CreateDirectory(filePathInWebRoot);
                    //SaveUploadToFile(Path.Combine(localFilePath, filename), Upload);
                    SaveUploadToFile(Path.Combine(filePathInWebRoot, filename), Upload);

                    accessAudit = AndroidMDMController.AppAddEntry(ApplicationName, packageName, Version, fileURL, "Active");
                    ErrorText = accessAudit ? "Successfully added new application!" : "Fail to add new application!";
                }
                catch (Exception e)
                {
                    ErrorText = "Fail to add new application!";
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                }
                AuditLog.CreateAuditLog(filename + ": " + Version, AuditCategory.MDM_Android, ModuleLogAction.Create_Application, _UserId, accessAudit, ClientIp);
                PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
            }
            else
            {
                ErrorText = "The file could not be uploaded. Only *.apk files are allowed.";
                ModelState.AddModelError("Upload", ErrorText);
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
