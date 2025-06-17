using Microsoft.Extensions.Configuration;

namespace Alexis.Common;

public static class ConfigHelper
{
    public static IConfiguration Configuration { get; }
    static ConfigHelper()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static string GetConnectionString(string connection)
    {
        return Configuration.GetConnectionString(connection);
    }
    public static string? LogDirectory => Configuration["LogDirectory"];
    public static string? DestinationIP => Configuration["DestinationIP"];
    public static string? FPath => Configuration["FPath"];
    public static string? AdvPathTmp => Configuration["AdvPathTmp"];
    public static string? IisPathTmp => Configuration["IisPathTmp"];
    public static string? OnePart => Configuration["OnePart"];

    public static string? AndroidMDMServerIP => Configuration["AndroidMDMServerIP"];
    public static string? AndroidMDMServerPort => Configuration["AndroidMDMServerPort"];
    public static string? AndroidMDMEnrollURL => Configuration["AndroidMDMEnrollURL"];
    public static string? AndroidMDMX => Configuration["AndroidMDMX"];
    public static string? WindowsMDMBaseURL => Configuration["WindowsMDMBaseURL"];

    public static string? WebRootRelativeURLPath_Temp => Configuration["WebRootRelativeURLPath_Temp"];
    public static string? WebRootRelativeURLPath_Advertisement => Configuration["WebRootRelativeURLPath_Advertisement"];
    public static string? WebRootRelativeURLPath_AndroidGroups => Configuration["WebRootRelativeURLPath_AndroidGroups"];
    public static string? WebRootRelativeURLPath_AndroidApps => Configuration["WebRootRelativeURLPath_AndroidApps"];
    public static string? WebRootRelativeURLPath_AndroidProfiles => Configuration["WebRootRelativeURLPath_AndroidProfiles"];
    public static string? LocalPath_DataFolder => Configuration["LocalPath_DataFolder"];
    public static string? LocalDir_Advertisement => Configuration["LocalDir_Advertisement"];
    public static string? LocalDir_AndroidGroups => Configuration["LocalDir_AndroidGroups"];
    public static string? LocalDir_AndroidApps => Configuration["LocalDir_AndroidApps"];
    public static string? LocalDir_AndroidProfiles => Configuration["LocalDir_AndroidProfiles"];

    public static string? APPPathTmp => Configuration["APPPathTmp"];
    public static string? APPInfoPlist => Configuration["APPInfoPlist"];
    public static string? APPPathOri => Configuration["APPPathOri"];
    public static string? APPDomainPath => Configuration["APPDomainPath"];
    public static string? APPIconPath_Temp => Configuration["APPIconPath_Temp"];
    public static string? APPIconPath => Configuration["APPIconPath"];
    public static string? Profile_APNS_Path => Configuration["Profile_APNS_Path"];

    public static string? APPIcon_ProjectPath_Temp => Configuration["APPIcon_ProjectPath_Temp"];
    public static string? APPIcon_ProjectPath => Configuration["APPIcon_ProjectPath"];
    public static string? MQueue => Configuration["MQueue"];
    public static string? MQFirstPrior => Configuration["MQFirstPrior"];
    public static string? MQSecondPrior => Configuration["MQSecondPrior"];


    public static string? PortalURL => Configuration["PortalURL"];

    public static string? MultiTenancyAPI => Configuration["MultiTenancyAPI"];
    public static string? iOSProfilesFolder => Configuration["iOSProfilesFolder"];
    public static string? iOSAppsFolder => Configuration["iOSAppsFolder"];
    public static string? iOSEnrollmentFolder => Configuration["iOSEnrollmentFolder"];





}
