using Alexis.Common;

namespace Dashboard.Common.Business.Component;

public enum StorageType
{
    Advertisement,
    Temp,
    Android_Groups, //store group QR code
    Android_Profiles, //store xml files (a.k.a restrictions)
    Android_Apps, //store apk files 
    iOS_Profiles, //store plist files (a.k.a restrictions)
    iOS_Apps, //store .ipa, .plist, icon, etc.
    iOS_Enroll //store QR images
}

public class FileManager
{


    /// <summary>
    /// Get the local path (e.g. C:\VS Workspace\..) pointing to the directory (dir)  
    /// of the storage type (e.g. MDM profiles, advertisement media, etc.)
    /// Note: Directory only, not file path.
    /// </summary>
    /// <param name="storageType"></param>
    /// <returns></returns>
    public static string GetLocalStoragePath(StorageType storageType)
    {
        string rootPath = ConfigHelper.LocalPath_DataFolder; //see web.config file
        string targetPath = "";

        switch (storageType)
        {
            case StorageType.Advertisement: targetPath = Path.Combine(rootPath, ConfigHelper.LocalDir_Advertisement); break;
            case StorageType.Android_Groups: targetPath = Path.Combine(rootPath, ConfigHelper.LocalDir_AndroidGroups); break;
            case StorageType.Android_Apps: targetPath = Path.Combine(rootPath, ConfigHelper.LocalDir_AndroidApps); break;
            case StorageType.Android_Profiles: targetPath = Path.Combine(rootPath, ConfigHelper.LocalDir_AndroidProfiles); break;
            case StorageType.iOS_Profiles: targetPath = Path.Combine(rootPath, ConfigHelper.iOSProfilesFolder); break;
            case StorageType.iOS_Apps: targetPath = Path.Combine(rootPath, ConfigHelper.iOSAppsFolder); break;
            case StorageType.iOS_Enroll: targetPath = Path.Combine(rootPath, ConfigHelper.iOSEnrollmentFolder); break;
            default: targetPath = null; break;
        }

        if (targetPath != null)
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

        return targetPath;
    }


    /// <summary>
    /// Map relative URL (e.g. /Content/<path>) 
    /// to absolute (WebRoot) path (e.g. C:\Users\username\_repos\Sol\Proj\Content\<path>).
    /// Storage type = (e.g. MDM profiles, advertisement media, etc.).
    /// Note: Directory only, not file path.
    /// </summary>
    /// <param name="storageType"></param>
    /// <returns></returns>
    public static string GetStorageWebRootPath(StorageType storageType)
    {
        string pathToMap = GetStorageRelativeURL(storageType);
        string targetPath = "";

        if (targetPath != null)
        {
            //targetPath = Path.Combine(_env.WebRootPath, pathToMap.TrimStart('~', '/'));
            targetPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", pathToMap);
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);
        }

        return targetPath;
    }


    /// <summary>
    /// Get the relative URL (e.g. /Content/<path>) .
    /// Storage type = (e.g. MDM profiles, advertisement media, etc.).
    /// Note: Directory only, not file path.
    /// </summary>
    /// <param name="storageType"></param>
    /// <returns></returns>
    public static string GetStorageRelativeURL(StorageType storageType)
    {
        string targetPath = "";  //usually in "Content" Folder with all the static files

        switch (storageType)
        {
            case StorageType.Temp: targetPath = ConfigHelper.WebRootRelativeURLPath_Temp; break;
            case StorageType.Advertisement: targetPath = ConfigHelper.WebRootRelativeURLPath_Advertisement; break;
            case StorageType.Android_Groups: targetPath = ConfigHelper.WebRootRelativeURLPath_AndroidGroups; break;
            case StorageType.Android_Apps: targetPath = ConfigHelper.WebRootRelativeURLPath_AndroidApps; break;
            case StorageType.Android_Profiles: targetPath = ConfigHelper.WebRootRelativeURLPath_AndroidProfiles; break;
            default: targetPath = null; break;
        }

        return targetPath;
    }
}
