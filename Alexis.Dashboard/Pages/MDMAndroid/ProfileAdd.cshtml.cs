using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.Android.Business.BusinessLogics.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alexis.Dashboard.Pages.MDMAndroid;

public class ProfileAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public string PageTitleText { get; set; }

    public string? ClientIp { get; set; }
    [BindProperty]
    public string? PRID { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Profile name required.")]
    public string ProfileName { get; set; }
    [BindProperty]
    public List<ProfileRestrictionsViewModel> ProfileRestrictions { get; set; }
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
        checkAuthorization(ModuleLogAction.Create_AndroidMDMProfile);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("MDMProfileID") != null)
            {
                PRID = session.GetString("MDMProfileID").ToString();
                EditCommand(PRID);
            }
            else
            {
                DataTable ProfileRestrictionTable = AndroidMDMController.getAllProfileRestrictionTemplateActive();
                ProfileRestrictions = Common.DataHelper.ConvertDataTableToList<ProfileRestrictionsViewModel>(ProfileRestrictionTable);
            }
            PageTitleText = PRID == null ? "Profile > Add New" : "Profile > Edit";
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
            var PRID = session.GetString("MDMProfileID");
            if (ModelState.IsValid)
            {
                bool accessAudit = true;
                //check for duplicates
                if (AndroidMDMController.IsProfileExists(ProfileName, PRID))
                {
                    string errorText = "Duplicated profile name is detected. Please use a different profile name.";
                    ModelState.AddModelError("ProfileName", errorText);
                    accessAudit = false;
                }
                else
                {
                    string localFilePath = Path.Combine(FileManager.GetLocalStoragePath(StorageType.Android_Profiles), ProfileName + ".xml");
                    // save profile to local path
                    if (accessAudit && SerializeProfileSave("Restriction", ProfileRestrictions, localFilePath))
                        accessAudit = true;
                    if (accessAudit)
                    {
                        string filePathInWebRoot = Path.Combine(FileManager.GetStorageWebRootPath(StorageType.Android_Profiles), ProfileName + ".xml");
                        string fileURL = Path.Combine("\\", FileManager.GetStorageRelativeURL(StorageType.Android_Profiles), ProfileName + ".xml").Replace("\\", "/");

                        // save profile to webRoot path
                        System.IO.File.Copy(localFilePath, filePathInWebRoot, overwrite: true);
                        // save the data to db
                        Dictionary<string, bool> dictChecked = ProfileRestrictions.ToDictionary(m => m.RESTRICTION_ID, m => m.ACTIVE);
                        var result = AndroidMDMController.saveProfileRestrictionsByID(PRID, ProfileName, fileURL, dictChecked, _UserId.ToString());
                        if (result)
                        {
                            ErrorText = PRID == null ? "Successfully added new profile!" : "Successfully updated profile!";
                        }
                        else
                        {
                            ErrorText = PRID == null ? "Fail to add new profile!" : "Fail to update profile!";
                        }
                    }
                    else
                    {
                        ErrorText = PRID == null ? "Fail to add new profile!" : "Fail to update profile!";
                    }
                    PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
                    AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_Android, PRID == null ? ModuleLogAction.Create_AndroidMDMProfile : ModuleLogAction.Update_AndroidMDMProfile, _UserId, accessAudit, ClientIp);
                }
            }
            PageTitleText = PRID == null ? "Profile > Add New" : "Profile > Edit";
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void EditCommand(string Id)
    {
        try
        {
            PageTitleText = "Profile > Edit";
            var MDMProfileTable = AndroidMDMController.getProfileByID(Id);
            if (MDMProfileTable != null)
            {
                ProfileName = MDMProfileTable.Rows[0]["Profile_Name"] as string;
            }
            DataTable ProfileRestrictionTable = AndroidMDMController.getAllProfileRestrictionsByID(Id);
            ProfileRestrictions = Common.DataHelper.ConvertDataTableToList<ProfileRestrictionsViewModel>(ProfileRestrictionTable);
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
        HttpContext.Session.Remove("MDMProfileID");
        return new JsonResult(new { message = "Success" });
    }
    private static bool SerializeProfileSave(string rootName, List<ProfileRestrictionsViewModel> dataNodeValue, string filepath)
    {
        try
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.AppendChild(xDoc.CreateElement(rootName));
            foreach (var node in dataNodeValue)
            {
                XmlNode xmlNode = xDoc.CreateNode(XmlNodeType.Element, node.DESCRIPTION, null);
                xmlNode.InnerText = Convert.ToInt32(node.ACTIVE).ToString();
                xDoc.DocumentElement.AppendChild(xmlNode);
            }
            xDoc.Save(filepath);
        }
        catch
        {
            return false;
        }
        return true;
    }
}