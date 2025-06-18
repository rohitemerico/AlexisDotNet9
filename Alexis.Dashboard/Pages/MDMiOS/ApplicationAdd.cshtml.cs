using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;
using Alexis.Common;
using Alexis.Dashboard.Helper;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogic.Common;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_App;
using MDM.iOS.Common.Data.Component;
using MDM.iOS.Entities.Dashboard;
using MDM.iOS.Entities.MDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Alexis.Dashboard.Pages.MDMiOS;

public class ApplicationAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    public static MDM_AppBase My_FMDM_AppBase = MDM_AppFactory.Create("");

    private static string uploadedFile = string.Empty; //example-00.ipa
    //private static string uploadedFileExt = string.Empty; //.ipa
    private static string uploadedFileNameNoExt = string.Empty; //example-00
    private static string strVersion = string.Empty; //from ipa file
    private static string strIdentify = string.Empty; //from ipa file
    private static string strIconName = string.Empty;


    //private static string MainAppFolder = string.Empty;
    private static string strDomainPath = string.Empty;
    private static string strFilePath = string.Empty;
    private static string strFilePath_Temp = string.Empty;
    private static string strInfoPlistPath = string.Empty;
    private static string strIconPath_Act = string.Empty;
    private static string strIconPath_Temp = string.Empty;
    private static string strIconPath = string.Empty;




    //private static string strIcon_Path_Temp = strIconPath_Temp + strIconName;
    //private static string strIcon_Path = strIconPath_Act + strIconName;
    //private static string strIcon_WebPath_Temp = $"~/{new DirectoryInfo(strIconPath_Temp).Name}/" + strIconName;
    //private static string strIcon_WebPath = $"~/{new DirectoryInfo(strIconPath_Act).Name}/" + strIconName;


    private DateTime expDate;

    public string ErrorText { get; set; }
    public string? ClientIp { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Version is required.")]
    public string Version { get; set; }
    [BindProperty]

    [Required(ErrorMessage = "Please select .ipa file to upload.")]
    [AllowedExtensions(new string[] { ".ipa" })]
    public IFormFile Upload { get; set; }
    public string ImageUrl { get; private set; }

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
        checkAuthorization(ModuleLogAction.View_iOSMDMApplicationManagement);
        //MainAppFolder = FileManager.GetLocalStoragePath(StorageType.iOS_Apps);
        strDomainPath = ConfigHelper.APPDomainPath;
        //strFilePath_Act = MainAppFolder;
        strFilePath_Temp = CreatedDirIfNotExist(ConfigHelper.APPPathTmp);
        strFilePath = CreatedDirIfNotExist(ConfigHelper.APPPathOri);
        strInfoPlistPath = CreatedDirIfNotExist(ConfigHelper.APPInfoPlist);
        strIconPath = CreatedDirIfNotExist(ConfigHelper.APPIconPath);
        strIconPath_Temp = CreatedDirIfNotExist(ConfigHelper.APPIconPath_Temp);
    }

    public IActionResult OnGet()
    {
        try
        {

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
            if (ModelState.IsValid)
            {

                String fileExtension = Path.GetExtension(Upload.FileName).ToLower();
                String[] allowedExtensions = { ".ipa" };
                if (allowedExtensions.Any(e1 => e1 == fileExtension))
                {
                    uploadedFile = Upload.FileName;
                    //uploadedFileExt = Path.GetExtension(Upload.FileName);
                    uploadedFileNameNoExt = Path.GetFileNameWithoutExtension(Upload.FileName);
                    En_MDMApp my_MDM_AppBase = new En_MDMApp()
                    {
                        name = uploadedFile
                    };

                    bool fileExists = My_FMDM_AppBase.CheckNameExists(my_MDM_AppBase);
                    if (fileExists)
                    {
                        ErrorText = uploadedFile + " already exists.";
                        ModelState.AddModelError("Upload", ErrorText);
                    }
                    else
                    {
                        if (Directory.Exists(Path.Combine(strFilePath_Temp, uploadedFileNameNoExt)))
                            Directory.Delete(Path.Combine(strFilePath_Temp, uploadedFileNameNoExt), true);

                        // Create a file directory for the newly uploaded ipa file 
                        Directory.CreateDirectory(Path.Combine(strFilePath_Temp, uploadedFileNameNoExt));

                        SaveUploadToFile(Path.Combine(strFilePath_Temp, uploadedFileNameNoExt, uploadedFile), Upload);

                        string zipFilePath = Path.Combine(strFilePath_Temp, uploadedFileNameNoExt, uploadedFile);

                        using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                // Extract iTunesArtwork
                                if (entry.FullName.Contains("iTunesArtwork"))
                                {
                                    Guid PK = Guid.NewGuid();
                                    string outputFileName = PK + ".png";
                                    string destinationPath = Path.Combine(strInfoPlistPath, entry.FullName);

                                    // Ensure directory exists
                                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                                    // Extract file
                                    entry.ExtractToFile(destinationPath, true);

                                    // Rename file
                                    if (!string.IsNullOrEmpty(outputFileName))
                                    {
                                        string strIconPathFull_Temp = Path.Combine(strIconPath_Temp, outputFileName);
                                        Directory.CreateDirectory(strIconPath_Temp);
                                        System.IO.File.Move(destinationPath, strIconPathFull_Temp);
                                        ImageUrl = strIconPath_Temp;
                                    }
                                }

                                // Extract Info.plist (exclude Base.lproj)
                                else if (entry.FullName.Contains("Info.plist") && !entry.FullName.Contains("Base.lproj"))
                                {
                                    string plistPath = Path.Combine(strInfoPlistPath, entry.FullName);
                                    Directory.CreateDirectory(Path.GetDirectoryName(plistPath));
                                    entry.ExtractToFile(plistPath, true);

                                    Dictionary<string, object> dict = (Dictionary<string, object>)PlistParseCS.readPlist(plistPath);
                                    if (dict.ContainsKey("CFBundleIdentifier"))
                                    {
                                        strIdentify = dict["CFBundleIdentifier"].ToString();
                                    }
                                    if (dict.ContainsKey("CFBundleShortVersionString"))
                                    {
                                        strVersion = dict["CFBundleShortVersionString"].ToString();
                                        Version = strVersion;
                                    }
                                }

                                // Extract embedded.mobileprovision
                                else if (entry.FullName.Contains("embedded.mobileprovision"))
                                {
                                    string provPath = Path.Combine(strInfoPlistPath, entry.FullName);
                                    Directory.CreateDirectory(Path.GetDirectoryName(provPath));
                                    entry.ExtractToFile(provPath, true);

                                    using (StreamReader sr = new StreamReader(provPath))
                                    {
                                        string q = sr.ReadToEnd();
                                        string[] a = Regex.Split(q, "ExpirationDate");
                                        if (a.Length > 1)
                                        {
                                            expDate = Convert.ToDateTime(a[1].Substring(14, 20));
                                        }
                                    }
                                }
                            }
                        }

                        //using (ZipFile zip = ZipFile.Read(zipFilePath))
                        //{
                        //    foreach (ZipEntry entry in zip)
                        //    {
                        //        // Extract iTunesArtwork
                        //        if (entry.FileName.Contains("iTunesArtwork"))
                        //        {
                        //            Guid PK = Guid.NewGuid();
                        //            entry.Extract(strInfoPlistPath, ExtractExistingFileAction.OverwriteSilently);
                        //            strIconName = PK + ".png";
                        //            if (strIconName != null)
                        //            {
                        //                string strIconPathFull_Temp = Path.Combine(strIconPath_Temp, strIconName);
                        //                System.IO.File.Move(Path.Combine(strInfoPlistPath, entry.FileName), strIconPathFull_Temp);
                        //            }
                        //            ImageUrl = strIconPath_Temp;
                        //        }

                        //        // Extract Info.plist
                        //        else if (entry.FileName.Contains("Info.plist") && !entry.FileName.Contains("Base.lproj"))
                        //        {
                        //            entry.Extract(strInfoPlistPath, ExtractExistingFileAction.OverwriteSilently);
                        //            Dictionary<string, object> dict = (Dictionary<string, object>)PlistParseCS.readPlist(Path.Combine(strInfoPlistPath, entry.FileName));
                        //            if (dict.ContainsKey("CFBundleIdentifier"))
                        //            {
                        //                //inside static strIdentifier
                        //                strIdentify = dict["CFBundleIdentifier"].ToString();
                        //            }
                        //            if (dict.ContainsKey("CFBundleShortVersionString"))
                        //            {
                        //                //inside static strVersion
                        //                strVersion = dict["CFBundleShortVersionString"].ToString();
                        //                Version = strVersion.ToString();
                        //            }
                        //        }

                        //        // Extract embedded.mobileprovision
                        //        else if (entry.FileName.Contains("embedded.mobileprovision"))
                        //        {

                        //            entry.Extract(strInfoPlistPath, ExtractExistingFileAction.OverwriteSilently);
                        //            using (StreamReader sr = new StreamReader(strInfoPlistPath + entry.FileName))
                        //            {
                        //                string q = sr.ReadToEnd();
                        //                string[] a = Regex.Split(q, "ExpirationDate");
                        //                expDate = Convert.ToDateTime(a[1].Substring(14, 20));
                        //            }
                        //        }
                        //    }
                        //}

                        using (XmlWriter writer = XmlWriter.Create(Path.Combine(strFilePath_Temp, uploadedFileNameNoExt, uploadedFile.Replace(".ipa", "") + ".plist")))
                        {
                            writer.WriteStartDocument();
                            writer.WriteStartElement("plist");
                            writer.WriteStartElement("dict");
                            writer.WriteElementString("key", "items");
                            writer.WriteStartElement("array");
                            writer.WriteStartElement("dict");
                            writer.WriteElementString("key", "assets");
                            writer.WriteStartElement("array");
                            writer.WriteStartElement("dict");
                            writer.WriteElementString("key", "kind");
                            writer.WriteElementString("string", "software-package");
                            writer.WriteElementString("key", "url");
                            writer.WriteElementString("string", Path.Combine(strDomainPath, uploadedFileNameNoExt, uploadedFile));
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteElementString("key", "metadata");
                            writer.WriteStartElement("dict");
                            writer.WriteElementString("key", "bundle-identifier");
                            writer.WriteElementString("string", strIdentify);
                            writer.WriteElementString("key", "bundle-version");
                            writer.WriteElementString("string", Version);
                            writer.WriteElementString("key", "kind");
                            writer.WriteElementString("string", "software");
                            writer.WriteElementString("key", "title");
                            writer.WriteElementString("string", uploadedFileNameNoExt);
                            writer.WriteElementString("key", "ManagementFlags");
                            writer.WriteElementString("integer", "1");
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                        }

                        if (strIconName.Trim() != "")
                        {
                            System.IO.File.Move(Path.Combine(strIconPath_Temp, strIconName), Path.Combine(strIconPath, strIconName));
                        }
                        if (Directory.Exists(Path.Combine(strFilePath, uploadedFileNameNoExt)))
                            Directory.Delete(Path.Combine(strFilePath, uploadedFileNameNoExt), true);
                        Directory.Move(Path.Combine(strFilePath_Temp, uploadedFileNameNoExt), Path.Combine(strFilePath, uploadedFileNameNoExt));

                        #region Save the file to diff server

                        //File_PassSoapClient my_PassSoap = new File_PassSoapClient();

                        ////// BRYAN - changing from AlexisDashboard.File_Pass.En_MDM_FilePass to EN_MDM_FilePass  
                        //En_MDM_FilePass my_En_MDM_FilePass_App = FileToByteArray(uploadedFile, (strFilePath_Act + uploadedFileNameNoExt + @"/" + uploadedFile), uploadedFileNameNoExt);

                        //En_MDM_FilePass my_En_MDM_FilePass_Plist = FileToByteArray(uploadedFile.Replace(".ipa", ".plist"), (strFilePath_Act + uploadedFileNameNoExt + @"/" + uploadedFile.Replace(".ipa", ".plist")), uploadedFileNameNoExt);


                        //boolError = my_PassSoap.AppAndPlist_Pass(my_En_MDM_FilePass_App);
                        //if (!boolError)
                        //{
                        //    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "AppAndPlist_Pass", "APP", JsonConvert.SerializeObject(my_En_MDM_FilePass_App));
                        //}

                        //boolError = my_PassSoap.AppAndPlist_Pass(my_En_MDM_FilePass_Plist);
                        //if (!boolError)
                        //{
                        //    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "AppAndPlist_Pass", "Plist", JsonConvert.SerializeObject(my_En_MDM_FilePass_Plist));
                        //}

                        #endregion
                        En_MDMApp entity = new En_MDMApp();
                        entity.appId = Guid.NewGuid();
                        entity.name = uploadedFile;
                        entity.desc = Description;
                        entity.version = Version;
                        entity.status = 2;
                        entity.path = Path.Combine(strDomainPath, uploadedFile.Replace(".ipa", ""), uploadedFile.Replace(".ipa", "") + ".plist");
                        entity.identifier = strIdentify;
                        entity.icon_FilePath = Path.Combine(strIconPath_Act, strIconName);
                        entity.expDate = expDate;
                        entity.createdDate = DateTime.Now;
                        entity.createdBy = _UserId;
                        DbEditing my_DbEditing = new DbEditing();
                        AssignTblMDMAPPTABLE(my_DbEditing, entity);
                        accessAudit = My_FMDM_AppBase.AppInsertIntoDb(entity) && DbEditingUpdater.insertEditTable(my_DbEditing);
                        string action_desc = $"Upload new application '{uploadedFile}'";
                        ErrorText = accessAudit ? $"{action_desc} successful!" : $"{action_desc} failed!";
                        AuditLog.CreateAuditLog(action_desc, AuditCategory.MDM_iOS, ModuleLogAction.Create_iOSMDMApp, _UserId, accessAudit, ClientIp);
                        PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
                    }
                }
                else
                {
                    ErrorText = "The file could not be uploaded. Only *.ipa files are allowed.";
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

    private void AssignTblMDMAPPTABLE(DbEditing ret, En_MDMApp my_En_MDMApp)
    {
        try
        {
            //string strDomainPath = ConfigHelper.APPDomainPath;

            //DataTable det = HttpContext.Current.Session["Client_Detail"] as DataTable;
            DataTable dt = My_FMDM_AppBase.GetMDM_APP("where APPID = '" + my_En_MDMApp.appId + "'");

            ret.EditDate = DateTime.Now;
            ret.EditedBy = _UserId;
            ret.EditorName = UserDetails.Rows[0]["uname"].ToString();

            ret.Mid = Module.GetModuleId(ModuleLogAction.Create_iOSMDMAppUpload);
            ret.ModuleName = "Application Create";
            ret.Tblid = my_En_MDMApp.appId;
            ret.TblStatus = 2;

            ret.Tables = new List<MDM.iOS.Entities.Dashboard.Table>();
            MDM.iOS.Entities.Dashboard.Table tb = new MDM.iOS.Entities.Dashboard.Table();
            tb.IsTableWithStatus = true;
            tb.MainTableName = "MDM_APP";
            tb.TblId = ret.Tblid;
            tb.TblIdName = "APPID";
            tb.TblStatusName = "STATUS";
            tb.ToDelete = false;

            tb.Rows = new List<rows>();
            rows r = new rows();
            r.Columns = new List<columns>();
            r.RowColName = "APPID";
            r.RowId = tb.TblId;

            columns name = new columns();
            name.ColumnName = "NAME";
            name.ColumnType = "NVARCHAR";
            name.NewDescriptionText = "Name:";
            if (uploadedFile == string.Empty)
            {
                name.NewDescriptionValue = my_En_MDMApp.name;
            }

            else
            {
                name.NewDescriptionValue = uploadedFile;
            }
            name.NewValue = uploadedFile;
            name.ToDisplay = true;


            columns desc = new columns();
            desc.ColumnName = "[DESC]";
            desc.ColumnType = "NVARCHAR";
            desc.NewDescriptionText = "Desc:";
            desc.NewDescriptionValue = Description;
            desc.NewValue = Description;
            desc.ToDisplay = true;

            columns version = new columns();
            version.ColumnName = "VERSION";
            version.ColumnType = "NVARCHAR";
            version.NewDescriptionText = "Version:";
            version.NewDescriptionValue = Version;
            version.NewValue = Version;
            version.ToDisplay = true;

            columns status = new columns();
            status.ColumnName = "STATUS";
            status.ColumnType = "INT";
            status.NewValue = 1;
            status.ToDisplay = false;

            columns iconPath = new columns();
            iconPath.ColumnName = "ICON_FILEPATH";
            iconPath.ColumnType = "NVARCHAR";
            if (strIconPath_Act == string.Empty)
            {
                iconPath.NewValue = my_En_MDMApp.icon_FilePath;
            }
            else
            {
                iconPath.NewValue = strIconPath_Act;
            }
            iconPath.ToDisplay = false;

            columns path = new columns();
            path.ColumnName = "PATH";
            path.ColumnType = "NVARCHAR";
            path.NewValue = strDomainPath + uploadedFile.Replace(".ipa", "") + "/" + uploadedFile.Replace(".ipa", "") + ".plist";
            path.ToDisplay = false;


            columns Identifier = new columns();
            Identifier.ColumnName = "IDENTIFIER";
            Identifier.ColumnType = "NVARCHAR";
            Identifier.NewDescriptionText = "Identifier:";
            if (strIdentify == string.Empty)
            {
                Identifier.NewDescriptionValue = my_En_MDMApp.identifier;
            }
            else
            {
                Identifier.NewDescriptionValue = strIdentify;
            }
            Identifier.NewValue = strIdentify;
            Identifier.ToDisplay = true;



            columns eDate = new columns();
            eDate.ColumnName = "ExpirationDate";
            eDate.ColumnType = "DATETIME";
            eDate.NewValue = my_En_MDMApp.expDate;
            eDate.ToDisplay = false;


            r.Columns.Add(name);
            r.Columns.Add(desc);
            r.Columns.Add(version);
            r.Columns.Add(status);
            r.Columns.Add(iconPath);
            r.Columns.Add(path);
            r.Columns.Add(Identifier);
            r.Columns.Add(eDate);
            tb.Rows.Add(r);
            ret.Tables.Add(tb);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                 ex);
        }
    }
    //public static En_MDM_FilePass FileToByteArray(string fileNameWithExt, string filePath, string PlistCategory)
    //{

    //    En_MDM_FilePass my_MDM_FilePass = new En_MDM_FilePass();

    //    my_MDM_FilePass.plist_Category = PlistCategory;
    //    my_MDM_FilePass.fileNameWithExtension = fileNameWithExt;
    //    my_MDM_FilePass.fileContent = System.IO.File.ReadAllBytes(filePath);
    //    return my_MDM_FilePass;
    //}




}
