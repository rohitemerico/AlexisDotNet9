using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using Alexis.Common;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;

namespace Alexis.Dashboard.Pages.MDMAndroid;

public class DeviceGroupModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public List<DeviceGroupViewModel> Groups { get; set; }
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
        checkAuthorization(ModuleLogAction.View_AndroidMDMDeviceGroup);
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
        DataTable data = AndroidMDMController.getAllMDMDeviceGroup();
        Groups = Common.DataHelper.ConvertDataTableToList<DeviceGroupViewModel>(data);
    }

    public IActionResult OnPostEdit(string id)
    {
        if (Guid.Parse(id) == Guid.Empty)
        {
            return new JsonResult(new { message = "Invalid ID." });
        }
        HttpContext.Session.SetString("DeviceGroupID", id);
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnGetAdd()
    {
        HttpContext.Session.Remove("DeviceGroupID");
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostView(string id)
    {
        if (Guid.Parse(id) == Guid.Empty)
        {
            return new JsonResult(new { message = "Invalid ID." });
        }
        HttpContext.Session.SetString("DeviceGroupID", id);

        string sUrlInPlain = "ServerIP=" + ConfigHelper.AndroidMDMServerIP;
        sUrlInPlain += "|Port=" + ConfigHelper.AndroidMDMServerPort;
        sUrlInPlain += "|DeviceGroup=" + id;
        sUrlInPlain += "|EnrollURL=" + ConfigHelper.AndroidMDMEnrollURL;

        string sEncryptedURL = Base64Encode(sUrlInPlain);


        var writer = new BarcodeWriter<Bitmap> { Format = BarcodeFormat.QR_CODE, Options = new QrCodeEncodingOptions { Width = 300, Height = 300 }, Renderer = new BitmapRenderer() };

        Bitmap barcodeBitmap = writer.Write(sEncryptedURL);

        string path = Path.Combine(FileManager.GetStorageWebRootPath(StorageType.Android_Groups), id.ToString() + ".jpg");
        string relativePath = Path.Combine(FileManager.GetStorageRelativeURL(StorageType.Android_Groups), id.ToString() + ".jpg");

        using (MemoryStream memory = new MemoryStream())
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                byte[] bytes = memory.ToArray();
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        var groupRow = AndroidMDMController.getAllMDMDeviceGroup().AsEnumerable().SingleOrDefault(gg => (string)gg["GID"] == id);

        return new JsonResult(
            new
            {
                message = "Success",
                caption = "QR",
                ImageUrl = relativePath,
                GroupName = groupRow["GROUPNAME"] as string,
                GroupDescription = groupRow["GROUPDESC"] as string
            }
        );
    }

    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }
}
