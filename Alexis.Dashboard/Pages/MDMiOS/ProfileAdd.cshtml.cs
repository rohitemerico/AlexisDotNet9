using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using Alexis.Common;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Profile;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.Class;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Controller;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alexis.Dashboard.Pages.MDMiOS;

public class ProfileAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{

    public static MDM_ProfileBase My_FMDM_ProfileBase = MDM_ProfileFactory.Create("");


    public string? ClientIp { get; set; }
    [BindProperty]
    public string? ProfileID { get; set; }
    [BindProperty]
    public string? AcceptCookies { get; set; }
    public List<SelectListItem> AcceptCookiesOptions { get; set; }
    [BindProperty]
    public string? RestrictAppsUsage { get; set; }
    public List<SelectListItem> RestrictAppsUsageOptions { get; set; }
    [BindProperty]
    public string? APNType { get; set; }
    public List<SelectListItem> APNTypeOptions { get; set; }
    [BindProperty]
    public string? DAAT { get; set; }
    public List<SelectListItem> DAATOptions { get; set; }
    public List<BranchViewModel> BranchList { get; set; }
    [BindProperty]
    public List<string> BranchIds { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Profile Name is required.")]
    public string Name { get; set; }
    [BindProperty]
    public string? Identifier { get; set; }
    [BindProperty]
    public string? HiddenFieldMasterID { get; set; }
    [BindProperty]
    public string? HiddenFieldcProfileID { get; set; }
    [BindProperty]
    public string? MinPass { get; set; }
    [BindProperty]
    public string? MinCC { get; set; }
    [BindProperty]
    public string? MaxPassAge { get; set; }
    [BindProperty]
    public string? MaximumAutoLock { get; set; }
    [BindProperty]
    public string? PassHistory { get; set; }
    [BindProperty]
    public string? MaxGPeriod { get; set; }
    [BindProperty]
    public string? MaxAttem { get; set; }
    [BindProperty]
    public bool ckbCheckTogglePasscodeSettings { get; set; }
    [BindProperty]
    public bool ckbRequireAlphanumericValue { get; set; }
    [BindProperty]
    public bool ckbAllowSimpleValue { get; set; }
    [BindProperty]
    public bool ckbNotRequiredCell { get; set; }
    [BindProperty]
    public string? DeAPN { get; set; }
    [BindProperty]
    public string? DAUN { get; set; }
    [BindProperty]
    public string? DAPass { get; set; }
    [BindProperty]
    public string? DataAN { get; set; }
    [BindProperty]
    public string? DataAAT { get; set; }
    [BindProperty]
    public string? DataAPass { get; set; }
    [BindProperty]
    public string? DataAUserName { get; set; }
    [BindProperty]
    public string? DataPS { get; set; }
    [BindProperty]
    public bool ckbCheckAllFunctionality { get; set; }
    [BindProperty]
    public bool ckbNotRequiredWifi { get; set; }
    [BindProperty]
    public bool ckbNotRequiredVPN { get; set; }
    [BindProperty]
    public List<FunctionalityViewModel> Functionalities { get; set; }
    [BindProperty]
    public List<AppsRestrictionViewModel> Apps { get; set; }

    public List<WiFiViewModel> WiFiList { get; set; }

    public List<VPNViewModel> VPNList { get; set; }
    public string ErrorText { get; set; }
    public string PageTitleText { get; set; }
    [BindProperty]
    public bool ckbSelectall { get; set; }
    [BindProperty]
    public List<RestrictionApp> RestrictionApps { get; set; }
    [BindProperty]
    public int? EditIndex { get; set; }
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
        checkAuthorization(ModuleLogAction.View_iOSMDMProfileManagement);
    }
    public IActionResult OnGet()
    {
        try
        {
            BindDropdown();
            DataTable dtFun = Restriction_Function.RestrictionMenu_Functionality_SelectAll();
            Functionalities = DataHelper.ConvertDataTableToList<FunctionalityViewModel>(dtFun);
            DataTable dtApps = Restriction_Function.RestrictionMenu_App_SelectAll();
            Apps = DataHelper.ConvertDataTableToList<AppsRestrictionViewModel>(dtApps);
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("MDMiOSProfileID") != null) //edit
            {
                ProfileID = session.GetString("MDMiOSProfileID").ToString();
                EditCommand(ProfileID);
            }
            else
            {
                Functionalities.ForEach(f => f.IsCheck = true);
                Apps.ForEach(a => a.IsCheck = true);
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }
    private void BindDropdown()
    {
        AcceptCookiesOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "Never", Text = "Never" },
            new SelectListItem { Value = "Fromcurrentwebsiteonly", Text = "From current website only" },
            new SelectListItem { Value = "FromwebsitesIvisit", Text = "From websites I visit" },
            new SelectListItem { Value = "Always", Text = "Always" }
        };

        RestrictAppsUsageOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "AllowAllApps", Text = "Allow all apps" },
            new SelectListItem { Value = "DoNotAllowSomeApps", Text = "Do not allow some apps" },
            new SelectListItem { Value = "OnlyAllowSomeApps", Text = "Only allow some apps" }
        };

        APNTypeOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "default", Text = "Default APN" },
            new SelectListItem { Value = "Data", Text = "Data APN" },
            new SelectListItem { Value = "DD", Text = "Default and Data APNs" }
        };
        DAATOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "PAP", Text = "PAP" },
            new SelectListItem { Value = "CHAP", Text = "CHAP" }
        };

        UserManageDefault userManageDefault = new UserManageDefault();
        DataTable data1 = userManageDefault.getBranches(new List<Params>());
        BranchList = Common.DataHelper.ConvertDataTableToList<BranchViewModel>(data1);
    }
    public IActionResult OnPost()
    {
        try
        {
            bool accessAudit = false;
            var session = httpContextAccessor.HttpContext.Session;
            ProfileID = session.GetString("MDMiOSProfileID")?.ToString();
            if (ModelState.IsValid)
            {

                MDM_Full_Profile main = new MDM_Full_Profile();

                List<MDM_Profile_Restriction> re = new List<MDM_Profile_Restriction>();
                MDM_Profile_Restriction pr = new MDM_Profile_Restriction();
                MDM_Profile_General general = new MDM_Profile_General();
                List<MDM_Profile_General_BranchID> gen_Branch = new List<MDM_Profile_General_BranchID>();
                MDM_Profile_General_BranchID branch = new MDM_Profile_General_BranchID();
                MDM_Profile_Passcode passcode = new MDM_Profile_Passcode();
                List<MDM_Profile_WIFI> wifi = new List<MDM_Profile_WIFI>();
                MDM_Profile_WIFI w = new MDM_Profile_WIFI();
                List<MDM_Profile_VPN> vpn = new List<MDM_Profile_VPN>();
                MDM_Profile_VPN v = new MDM_Profile_VPN();
                List<MDM_Profile_LDAP> ldapList = new List<MDM_Profile_LDAP>();
                MDM_Profile_LDAP ld = new MDM_Profile_LDAP();
                List<MDM_Profile_LDAP_SearchSettings> searchsettings = new List<MDM_Profile_LDAP_SearchSettings>();
                MDM_Profile_LDAP_SearchSettings ss = new MDM_Profile_LDAP_SearchSettings();
                MDM_Profile_Cellular cell = new MDM_Profile_Cellular();
                MDM_Profile_Restriction_Advance ad = new MDM_Profile_Restriction_Advance();
                MDM_HttpProxy proxy = new MDM_HttpProxy();

                GetGeneralSettings(general, gen_Branch);
                GetPasscode(passcode);
                GetFunctionality(re, pr);
                GetAppsSetting(re, pr, ad);
                GetWifi(wifi, w);
                GetVPN(vpn, v);
                GetCellular(cell);

                main.mDM_Profile_General = general;
                main.mDM_Profile_General_BranchID = gen_Branch;
                main.mDM_Profile_Passcode = passcode;
                main.mDM_Profile_Restriction_list = re;
                main.mDM_Profile_Restriction_Advance = ad;
                main.mDM_Profile_WIFI_list = wifi;
                main.mDM_Profile_VPN_list = vpn;
                main.mDM_Profile_Cellular = cell;

                accessAudit = ProfileController.Insert_All(main);

                if (accessAudit)
                {
                    ErrorText = ProfileID == null ? "Successfully added new profile!" : "Successfully updated profile!";
                }
                else
                {
                    ErrorText = ProfileID == null ? "Fail to add new profile!" : "Fail to update profile!";
                }
                PopUp(accessAudit ? "Success!" : "Fail!", ErrorText);
                AuditLog.CreateAuditLog(ErrorText, AuditCategory.MDM_iOS, ProfileID == null ? ModuleLogAction.Create_iOSMDMProfile : ModuleLogAction.Update_iOSMDMProfile, _UserId, accessAudit, ClientIp);
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        BindDropdown();
        return Page();
    }
    public IActionResult OnPostAddWiFi(WiFiViewModel model)
    {
        List<WiFiViewModel> wifiList = new List<WiFiViewModel>();
        var session = httpContextAccessor.HttpContext.Session;
        if (session.GetString("MDMWiFiData") != null)
        {
            var wifiDataJson = session.GetString("MDMWiFiData");
            if (!string.IsNullOrEmpty(wifiDataJson))
            {
                wifiList = JsonSerializer.Deserialize<List<WiFiViewModel>>(wifiDataJson);
            }
        }
        if (wifiList.Where(a => a.Profile_WIFI_ID == model.Profile_WIFI_ID).Any())
        {
            wifiList.RemoveAll(a => a.Profile_WIFI_ID == model.Profile_WIFI_ID);
            wifiList.Add(model);
        }
        else
        {
            wifiList.Add(model);
        }
        HttpContext.Session.SetString("MDMWiFiData", JsonSerializer.Serialize(wifiList));
        return Partial("_WiFiListPartial", wifiList);
    }
    public IActionResult OnPostAddVPN(VPNViewModel model)
    {
        List<VPNViewModel> vpnList = new List<VPNViewModel>();
        var session = httpContextAccessor.HttpContext.Session;
        if (session.GetString("MDMVPNData") != null)
        {
            var vpnDataJson = session.GetString("MDMVPNData");
            if (!string.IsNullOrEmpty(vpnDataJson))
            {
                vpnList = JsonSerializer.Deserialize<List<VPNViewModel>>(vpnDataJson);
            }
        }
        if (vpnList.Where(a => a.Profile_VPN_ID == model.Profile_VPN_ID).Any())
        {
            vpnList.RemoveAll(a => a.Profile_VPN_ID == model.Profile_VPN_ID);
            vpnList.Add(model);
        }
        else
        {
            vpnList.Add(model);
        }
        HttpContext.Session.SetString("MDMVPNData", JsonSerializer.Serialize(vpnList));
        return Partial("_VPNListPartial", vpnList);
    }

    public IActionResult OnPostLoadAddWiFi(string Profile_WIFI_ID)
    {
        List<WiFiViewModel> wifiList = new List<WiFiViewModel>();
        var session = httpContextAccessor.HttpContext.Session;
        if (session.GetString("MDMWiFiData") != null)
        {
            var wifiDataJson = session.GetString("MDMWiFiData");
            if (!string.IsNullOrEmpty(wifiDataJson))
            {
                wifiList = JsonSerializer.Deserialize<List<WiFiViewModel>>(wifiDataJson);
            }
        }
        WiFiViewModel model;
        if (string.IsNullOrEmpty(Profile_WIFI_ID))
        {
            model = new WiFiViewModel();
            model.Profile_WIFI_ID = Guid.NewGuid();
            model.WhiteListApps = new List<string>();
        }
        else
        {
            model = wifiList.Where(a => a.Profile_WIFI_ID == Guid.Parse(Profile_WIFI_ID)).FirstOrDefault();
        }
        return Partial("_WiFiAddPartial", model);
    }

    public IActionResult OnPostRestrictionAppAdd(string AppName)
    {
        List<RestrictionApp> restrictionAppList = new List<RestrictionApp>();
        var session = httpContextAccessor.HttpContext.Session;
        if (session.GetString("MDMRestrictionAppData") != null)
        {
            var restrictionAppDataJson = session.GetString("MDMRestrictionAppData");
            if (!string.IsNullOrEmpty(restrictionAppDataJson))
            {
                restrictionAppList = JsonSerializer.Deserialize<List<RestrictionApp>>(restrictionAppDataJson);
            }
        }
        RestrictionApp model;
        model = restrictionAppList.FirstOrDefault(a => string.Equals(a.ResApp?.Trim(), AppName?.Trim(), StringComparison.OrdinalIgnoreCase));
        if (model == null)
        {
            model = new RestrictionApp();
            model.ResApp = AppName;
            restrictionAppList.Add(model);
        }
        HttpContext.Session.SetString("MDMRestrictionAppData", JsonSerializer.Serialize(restrictionAppList));
        return Partial("_RestrictionApps", restrictionAppList);
    }

    public IActionResult OnPostRestrictionAppEdit(string Original, string AppName)
    {
        List<RestrictionApp> restrictionAppList = new List<RestrictionApp>();
        var session = httpContextAccessor.HttpContext.Session;
        if (session.GetString("MDMRestrictionAppData") != null)
        {
            var restrictionAppDataJson = session.GetString("MDMRestrictionAppData");
            if (!string.IsNullOrEmpty(restrictionAppDataJson))
            {
                restrictionAppList = JsonSerializer.Deserialize<List<RestrictionApp>>(restrictionAppDataJson);
            }
        }
        RestrictionApp model;
        model = restrictionAppList.FirstOrDefault(a => string.Equals(a.ResApp?.Trim(), Original?.Trim(), StringComparison.OrdinalIgnoreCase));
        if (model != null)
        {
            restrictionAppList.Remove(model);
        }
        model = new RestrictionApp();
        model.ResApp = AppName;
        restrictionAppList.Add(model);

        HttpContext.Session.SetString("MDMRestrictionAppData", JsonSerializer.Serialize(restrictionAppList));
        return Partial("_RestrictionApps", restrictionAppList);
    }

    public IActionResult OnPostLoadAddVPN(string Profile_VPN_ID)
    {
        List<VPNViewModel> vpnList = new List<VPNViewModel>();
        var session = httpContextAccessor.HttpContext.Session;
        if (session.GetString("MDMVPNData") != null)
        {
            var vpnDataJson = session.GetString("MDMVPNData");
            if (!string.IsNullOrEmpty(vpnDataJson))
            {
                vpnList = JsonSerializer.Deserialize<List<VPNViewModel>>(vpnDataJson);
            }
        }
        VPNViewModel model;
        if (string.IsNullOrEmpty(Profile_VPN_ID))
        {
            model = new VPNViewModel();
            model.Profile_VPN_ID = Guid.NewGuid();
        }
        else
        {
            model = vpnList.Where(a => a.Profile_VPN_ID == Guid.Parse(Profile_VPN_ID)).FirstOrDefault();
        }
        return Partial("_VPNAddPartial", model);
    }

    private void GetGeneralSettings(MDM_Profile_General general, List<MDM_Profile_General_BranchID> genBranchID)
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            //status = 0 then pending , status = 1 then Active, status = 2 then Inactive, status =3 then Edited, status = 4 then no show.
            if (session.GetString("MDMiOSProfileID") == null)
            {
                general.Profile_ID = Guid.NewGuid();
                general.CProfileId = general.Profile_ID;
                general.pStatus = 0;
                HiddenFieldcProfileID = general.Profile_ID.ToString();
                general.LastUpdateBy = Guid.Empty;
                general.CreatedDate = DateTime.Now;
                general.CreatedBy = _UserId;

            }
            else
            {
                ProfileID = session.GetString("MDMiOSProfileID").ToString();
                general.Profile_ID = Guid.NewGuid();
                general.CProfileId = Guid.Parse(ProfileID.ToString());
                general.LastUpdateDate = DateTime.Now;
                general.pStatus = 3;
                general.LastUpdateBy = _UserId;
                MDM_Profile_General my_general = new MDM_Profile_General();
                my_general.Profile_ID = Guid.Parse(general.CProfileId.ToString());
                DataTable General_ret = General_Function.General_SelectAll(my_general);
                general.CreatedDate = Convert.ToDateTime(General_ret.Rows[0]["CreatedDate"].ToString());
                general.CreatedBy = Guid.Parse(General_ret.Rows[0]["CreatedBy"]?.ToString());
                HiddenFieldcProfileID = ProfileID.ToString();

                MDM_Profile_General general_oldData = new MDM_Profile_General();
                general_oldData.Profile_ID = Guid.Parse(ProfileID.ToString());
                general_oldData.pStatus = 5;
                My_FMDM_ProfileBase.UpdateProfileGeneralByUpdateType(general_oldData, "UPDATEOLDDATASTATUS");
            }
            HiddenFieldMasterID = general.Profile_ID.ToString();
            general.Name = Name;
            general.Identifier = Identifier;
            if (string.IsNullOrEmpty(general.Identifier))
            {
                general.Identifier = null;
            }
            general.Security = "Always";

            #region branchid
            for (int i = 0; i < BranchIds.Count; i++)
            {
                MDM_Profile_General_BranchID bid = new MDM_Profile_General_BranchID();
                bid.cProfile_ID = Guid.Parse(HiddenFieldcProfileID);
                bid.Branch_ID = Guid.Parse(BranchIds[i]);
                bid.IID = Guid.NewGuid();
                bid.Profile_ID = general.Profile_ID;
                genBranchID.Add(bid);
            }
            #endregion
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void GetPasscode(MDM_Profile_Passcode passcode)
    {

        try
        {

            if (!ckbCheckTogglePasscodeSettings)
            {
                passcode.Profile_ID = Guid.Parse(HiddenFieldMasterID);
                passcode.AllowSimpleValue = ckbAllowSimpleValue.ToString();
                passcode.Requirealphanumericvalue = ckbRequireAlphanumericValue.ToString();
                if (!string.IsNullOrEmpty(MinPass))
                {
                    passcode.MinimumPasscodeLength = MinPass;
                }
                if (!string.IsNullOrEmpty(MinCC))
                {
                    passcode.MinimumNumberOfComplexCharacters = MinCC;
                }

                if (!string.IsNullOrEmpty(MaxPassAge))
                {
                    passcode.MaximumPasscodeAge = MaxPassAge;
                }
                if (!string.IsNullOrEmpty(MaximumAutoLock))
                {
                    passcode.MaximumAutoLock = MaximumAutoLock;
                }
                if (!string.IsNullOrEmpty(PassHistory))
                {
                    passcode.PasscodeHistory = PassHistory;
                }
                if (!string.IsNullOrEmpty(MaxGPeriod))
                {
                    passcode.MaximumGracePeriod = MaxGPeriod;
                }
                if (!string.IsNullOrEmpty(MaxAttem))
                {
                    passcode.MaximumNumberOfFailedAttempts = MaxAttem;
                }
            }
            else
            {
                passcode = new MDM_Profile_Passcode();
                passcode.Profile_ID = Guid.Empty;

            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);

        }
    }
    private void GetCellular(MDM_Profile_Cellular cell)
    {
        try
        {
            if (!ckbNotRequiredCell)
            {
                cell.Profile_ID = Guid.Parse(HiddenFieldMasterID);
                cell.ConfiguredAPNType = APNType switch
                {
                    "default" => "Default APN",
                    "Data" => "Data APN",
                    _ => "Default and Data APNs"
                };

                if (cell.ConfiguredAPNType == "Default APN")
                {
                    cell.DefaultAPN_Name = DeAPN;
                    if (string.IsNullOrEmpty(cell.DefaultAPN_Name))
                    {
                        cell.DefaultAPN_Name = null;
                    }
                    cell.DefaultAPN_AuthenticationType = DAAT;
                    cell.DefaultAPN_UserName = DAUN;
                    if (string.IsNullOrEmpty(cell.DefaultAPN_UserName))
                    {
                        cell.DefaultAPN_UserName = null;
                    }
                    cell.DefaultAPN_Password = DAPass;
                    if (string.IsNullOrEmpty(cell.DefaultAPN_Password))
                    {
                        cell.DefaultAPN_Password = null;
                    }
                }
                else if (cell.ConfiguredAPNType == "Data APN")
                {

                    cell.DataAPN_Name = DataAN;
                    if (string.IsNullOrEmpty(cell.DataAPN_Name))
                    {
                        cell.DataAPN_Name = null;
                    }
                    cell.DataAPN_AuthenticationType = DataAAT;
                    cell.DataAPN_Password = DataAPass;
                    if (string.IsNullOrEmpty(cell.DataAPN_Password))
                    {
                        cell.DataAPN_Password = null;
                    }
                    cell.DataAPN_UserName = DataAUserName;
                    if (string.IsNullOrEmpty(cell.DataAPN_UserName))
                    {
                        cell.DataAPN_UserName = null;
                    }
                    cell.DataAPN_ProxyServer = DataPS;
                    if (string.IsNullOrEmpty(cell.DataAPN_ProxyServer))
                    {
                        cell.DataAPN_ProxyServer = null;
                    }
                }
                else if (cell.ConfiguredAPNType == "Default and Data APNs")
                {

                    cell.DefaultAPN_Name = DeAPN;
                    if (string.IsNullOrEmpty(cell.DefaultAPN_Name))
                    {
                        cell.DefaultAPN_Name = null;
                    }
                    cell.DefaultAPN_AuthenticationType = DAAT;
                    cell.DefaultAPN_UserName = DAUN;
                    if (string.IsNullOrEmpty(cell.DefaultAPN_UserName))
                    {
                        cell.DefaultAPN_UserName = null;
                    }
                    cell.DefaultAPN_Password = DAPass;
                    if (string.IsNullOrEmpty(cell.DefaultAPN_Password))
                    {
                        cell.DefaultAPN_Password = null;
                    }
                    cell.DataAPN_Name = DataAN;
                    if (string.IsNullOrEmpty(cell.DataAPN_Name))
                    {
                        cell.DataAPN_Name = null;
                    }
                    cell.DataAPN_AuthenticationType = DataAAT;
                    cell.DataAPN_Password = DataAPass;
                    if (string.IsNullOrEmpty(cell.DataAPN_Password))
                    {
                        cell.DataAPN_Password = null;
                    }
                    cell.DataAPN_UserName = DataAUserName;
                    if (string.IsNullOrEmpty(cell.DataAPN_UserName))
                    {
                        cell.DataAPN_UserName = null;
                    }
                    cell.DataAPN_ProxyServer = DataPS;
                    if (string.IsNullOrEmpty(cell.DataAPN_ProxyServer))
                    {
                        cell.DataAPN_ProxyServer = null;
                    }
                }
            }
            else
            {
                cell = new MDM_Profile_Cellular();
                cell.Profile_ID = Guid.Empty;
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void GetFunctionality(List<MDM_Profile_Restriction> re, MDM_Profile_Restriction pr)
    {
        try
        {
            if (!ckbCheckAllFunctionality)
            {
                foreach (var item in Functionalities)
                {
                    pr = new MDM_Profile_Restriction();
                    pr.RestrictionName = item.RestrictionName;
                    if (pr.RestrictionName == "safariAllowPopups")
                    {
                        if (!item.IsCheck)
                        {
                            pr.IsCheck = true;
                        }
                        else
                        {
                            pr.IsCheck = false;
                        }
                    }
                    else
                    {
                        pr.IsCheck = item.IsCheck;

                    }
                    pr.Profile_ID = Guid.Parse(HiddenFieldMasterID);
                    pr.RID = Guid.Parse(item.RID.ToString());
                    pr.ID = Guid.NewGuid();
                    pr.Queue = Convert.ToInt32(item.Queue);
                    re.Add(pr);
                }
            }
            else
            {
                pr = new MDM_Profile_Restriction();
                pr.Profile_ID = Guid.Empty;
                re.Add(pr);
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void GetAppsSetting(List<MDM_Profile_Restriction> re, MDM_Profile_Restriction pr, MDM_Profile_Restriction_Advance ad)
    {
        try
        {
            if (!ckbCheckAllFunctionality)
            {
                foreach (var item in Apps)
                {
                    pr = new MDM_Profile_Restriction();
                    pr.Profile_ID = Guid.Parse(HiddenFieldMasterID);
                    pr.RID = Guid.Parse(item.RID.ToString());
                    pr.IsCheck = item.IsCheck;
                    pr.RestrictionName = item.RestrictionName;
                    pr.Queue = Convert.ToInt32(item.Queue);
                    pr.ID = Guid.NewGuid();
                    re.Add(pr);
                }
                if (RestrictAppsUsage.ToUpper() != "ALLOWALLAPPS")
                {
                    List<RestrictionApp> RestrictionAppData = new List<RestrictionApp>();
                    var session = httpContextAccessor.HttpContext.Session;
                    if (session.GetString("MDMRestrictionAppData") != null)
                    {
                        var RestrictionAppJson = session.GetString("MDMRestrictionAppData");
                        if (!string.IsNullOrEmpty(RestrictionAppJson))
                        {
                            RestrictionAppData = JsonSerializer.Deserialize<List<RestrictionApp>>(RestrictionAppJson);
                        }
                    }
                    string list = string.Empty;
                    for (int i = 0; i < RestrictionAppData.Count; i++)
                    {
                        if (i + 1 == RestrictionAppData.Count)
                        {
                            list += RestrictionAppData[i].ResApp;
                        }
                        else
                        {
                            list += RestrictionAppData[i].ResApp + ", ";
                        }
                    }
                    ad.Profile_ID = pr.Profile_ID;
                    ad.App_Identify = list;
                }
                ad.RestrictAppUsage = RestrictAppsUsage;
                ad.AcceptCookies = AcceptCookies;
            }
            else
            {
                ad.Profile_ID = Guid.Empty;
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void GetWifi(List<MDM_Profile_WIFI> wifiList, MDM_Profile_WIFI w)
    {
        try
        {
            if (!ckbNotRequiredWifi)
            {
                List<WiFiViewModel> wifiListData = new List<WiFiViewModel>();
                var session = httpContextAccessor.HttpContext.Session;
                if (session.GetString("MDMWiFiData") != null)
                {
                    var wifiDataJson = session.GetString("MDMWiFiData");
                    if (!string.IsNullOrEmpty(wifiDataJson))
                    {
                        wifiListData = JsonSerializer.Deserialize<List<WiFiViewModel>>(wifiDataJson);
                    }
                }

                foreach (var item in wifiListData)
                {
                    w = new MDM_Profile_WIFI();
                    w.Profile_ID = Guid.Parse(HiddenFieldMasterID);
                    w.Profile_WIFI_ID = Guid.NewGuid();
                    w.ServiceSetIdentifier = item.ServiceSetIdentifier?.ToString();
                    if (string.IsNullOrEmpty(w.ServiceSetIdentifier))
                    {
                        w.ServiceSetIdentifier = null;
                    }
                    w.HiddenNetwork = item.HiddenNetwork.ToString();
                    w.AutoJoin = item.AutoJoin.ToString();
                    w.DisableCaptiveNetworkDetection = item.DisableCaptiveNetworkDetection.ToString();
                    w.ProxySetup = item.ProxySetup?.ToString();
                    if (string.IsNullOrEmpty(w.ProxySetup))
                    {
                        w.ProxySetup = null;
                    }
                    w.ServerPort = item.ProxyServerPort?.ToString();
                    if (string.IsNullOrEmpty(w.ServerPort))
                    {
                        w.ServerPort = null;
                    }
                    w.ServerIPAddress = item.ProxyServer?.ToString();
                    if (string.IsNullOrEmpty(w.ServerIPAddress))
                    {
                        w.ServerIPAddress = null;
                    }
                    w.ServerProxyURL = item.ProxyURL?.ToString();
                    if (string.IsNullOrEmpty(w.ServerProxyURL))
                    {
                        w.ServerProxyURL = null;
                    }
                    w.Username = item.Username?.ToString();
                    if (string.IsNullOrEmpty(w.Username))
                    {
                        w.Username = null;
                    }
                    w.Password = item.ProxyPassword?.ToString();
                    if (string.IsNullOrEmpty(w.Password))
                    {
                        w.Password = null;
                    }
                    w.SecurityType = item.SecurityType.ToString();
                    w.SecurityTypePassword = item.SecurityPassword?.ToString();
                    if (string.IsNullOrEmpty(w.SecurityTypePassword))
                    {
                        w.SecurityTypePassword = null;
                    }
                    w.NetworkType = item.NetworkType.ToString();
                    w.FastLaneQosMarking = item.FastLaneQosmarking?.ToString();
                    if (string.IsNullOrEmpty(w.FastLaneQosMarking))
                    {
                        w.FastLaneQosMarking = null;
                    }
                    w.EnableQosMarking = item.EnableQos?.ToString();
                    if (string.IsNullOrEmpty(w.EnableQosMarking))
                    {
                        w.EnableQosMarking = null;
                    }
                    w.WhitelistAppleAudioVideoCalling = item.Whitelist?.ToString();
                    if (string.IsNullOrEmpty(w.WhitelistAppleAudioVideoCalling))
                    {
                        w.WhitelistAppleAudioVideoCalling = null;
                    }
                    w.App_Identifity = item.App_Identifity?.ToString();
                    if (string.IsNullOrEmpty(w.App_Identifity))
                    {
                        w.App_Identifity = null;
                    }
                    wifiList.Add(w);
                }
            }
            else
            {

                w = new MDM_Profile_WIFI();
                w.Profile_ID = Guid.Empty;
                wifiList.Add(w);
            }
        }

        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void GetVPN(List<MDM_Profile_VPN> vpn, MDM_Profile_VPN v)
    {

        try
        {
            if (!ckbNotRequiredVPN)
            {
                List<VPNViewModel> vpnListData = new List<VPNViewModel>();
                var session = httpContextAccessor.HttpContext.Session;
                if (session.GetString("MDMVPNData") != null)
                {
                    var vpnDataJson = session.GetString("MDMVPNData");
                    if (!string.IsNullOrEmpty(vpnDataJson))
                    {
                        vpnListData = JsonSerializer.Deserialize<List<VPNViewModel>>(vpnDataJson);
                    }
                }


                if (vpnListData != null)
                {
                    if (vpnListData.Count > 0)
                    {
                        foreach (var item in vpnListData)
                        {
                            if (item.ConnectionType == "IPsec")
                            {
                                v = new MDM_Profile_VPN();
                                v.Profile_ID = Guid.Parse(HiddenFieldMasterID);
                                v.Profile_VPN_ID = Guid.NewGuid();
                                v.ConnectionName = item.ConnectionName;
                                v.ConnectionType = "IPSEC";
                                v.IPSEC_Server = item.IPSEC_SERVER?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_Server))
                                {
                                    v.IPSEC_Server = null;
                                }
                                v.IPSEC_Account = item.IPSEC_Account?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_Account))
                                {
                                    v.IPSEC_Account = null;
                                }
                                v.IPSEC_Account_Password = item.IPSEC_Account_Password?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_Account_Password))
                                {
                                    v.IPSEC_Account_Password = null;
                                }
                                v.IPSEC_MachineAuthentication = item.IPSEC_MachineAuthentication?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_MachineAuthentication))
                                {
                                    v.IPSEC_MachineAuthentication = null;
                                }
                                v.IPSEC_GroupName = item.IPSEC_GroupName?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_GroupName))
                                {
                                    v.IPSEC_GroupName = null;
                                }
                                v.IPSEC_SharedSecret = item.IPSEC_SharedSecret?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_SharedSecret))
                                {
                                    v.IPSEC_SharedSecret = null;
                                }
                                v.IPSEC_PromptForPassword = item.IPSEC_PromptForPassword.ToString();
                                v.IPSEC_UseHybridAuthentication = item.IPSEC_UseHybridAuthentication?.ToString();
                                v.IPSEC_ProxySetup = item.IPSEC_ProxySetup?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_ProxySetup))
                                {
                                    v.IPSEC_ProxySetup = null;
                                }
                                v.IPSEC_Proxy_Server = item.IPSEC_Proxy_Server?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_Proxy_Server))
                                {
                                    v.IPSEC_Proxy_Server = null;
                                }
                                v.IPSEC_Proxy_Port = item.IPSEC_Proxy_Port?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_Proxy_Port))
                                {
                                    v.IPSEC_Proxy_Port = null;
                                }
                                v.IPSEC_Authentication = item.IPSEC_Authentication?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_Authentication))
                                {
                                    v.IPSEC_Authentication = null;
                                }
                                v.IPSEC_ProxyServerURL = item.IPSEC_ProxyServerURL?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_ProxyServerURL))
                                {
                                    v.IPSEC_ProxyServerURL = null;
                                }
                                v.IPSEC_Password = item.IPSEC_Password?.ToString();
                                if (string.IsNullOrEmpty(v.IPSEC_Password))
                                {
                                    v.IPSEC_Password = null;
                                }
                                vpn.Add(v);
                            }
                            else
                            {
                                v = new MDM_Profile_VPN();
                                v.Profile_ID = Guid.Parse(HiddenFieldMasterID);
                                v.Profile_VPN_ID = Guid.NewGuid();
                                v.ConnectionName = item.ConnectionName.ToString();
                                v.ConnectionType = "L2TP";
                                v.L2TP_Server = item.L2TP_Server?.ToString();
                                if (string.IsNullOrEmpty(v.L2TP_Server))
                                {
                                    v.L2TP_Server = null;
                                }
                                v.L2TP_Account = item.L2TP_Account?.ToString();
                                if (string.IsNullOrEmpty(v.L2TP_Account))
                                {
                                    v.L2TP_Account = null;
                                }
                                if (item.L2TP_UserAuthentication_RSASecurID?.ToString() == "RSASecureID")
                                {
                                    v.L2TP_UserAuthentication_RSASecurID = "RSASecureID";
                                    v.L2TP_UserAuthentication_Password = null;
                                }
                                else
                                {
                                    v.L2TP_UserAuthentication_RSASecurID = "Password";
                                    v.L2TP_UserAuthentication_Password = item.L2TP_UserAuthentication_Password?.ToString();
                                    if (string.IsNullOrEmpty(v.L2TP_UserAuthentication_Password))
                                    {
                                        v.L2TP_UserAuthentication_Password = null;
                                    }
                                }
                                v.L2TP_MachineAuthentication = item.L2TP_MachineAuthentication?.ToString();
                                v.L2TP_GroupName = item.L2TP_GroupName?.ToString();
                                if (string.IsNullOrEmpty(v.L2TP_GroupName))
                                {
                                    v.L2TP_GroupName = null;
                                }
                                v.L2TP_PromptForPassword = item.L2TP_PromptForPassword?.ToString();
                                if (!string.IsNullOrEmpty(v.L2TP_GroupName))
                                {
                                    v.L2TP_UseHybridAuthentication = item.L2TP_UseHybridAuthentication?.ToString();
                                }
                                else
                                {
                                    v.L2TP_UseHybridAuthentication = "false";

                                }
                                v.L2TP_SharedSecret = item.L2TP_SharedSecret?.ToString();
                                if (string.IsNullOrEmpty(v.L2TP_SharedSecret))
                                {
                                    v.L2TP_SharedSecret = null;
                                }
                                v.L2TP_SendAllTrafficeThroughVPN = item.L2TP_SendAllTrafficeThroughVPN?.ToString();
                                v.L2TP_ProxySetup = item.L2TP_ProxySetup?.ToString();
                                if (v.L2TP_ProxySetup == "Manual")
                                {
                                    v.L2TP_ProxySetup_Server = item.L2TP_ProxySetup_Server?.ToString();
                                    if (string.IsNullOrEmpty(v.L2TP_ProxySetup_Server))
                                    {
                                        v.L2TP_ProxySetup_Server = null;
                                    }
                                    v.L2TP_ProxySetup_Port = item.L2TP_ProxySetup_Port?.ToString();
                                    if (string.IsNullOrEmpty(v.L2TP_ProxySetup_Port))
                                    {
                                        v.L2TP_ProxySetup_Port = null;
                                    }
                                    v.L2TP_ProxySetup_Authentication = item.L2TP_ProxySetup_Authentication?.ToString();
                                    if (string.IsNullOrEmpty(v.L2TP_ProxySetup_Authentication))
                                    {
                                        v.L2TP_ProxySetup_Authentication = null;
                                    }
                                    v.L2TP_ProxySetup_Password = item.L2TP_ProxySetup_Password?.ToString();
                                    if (string.IsNullOrEmpty(v.L2TP_ProxySetup_Password))
                                    {
                                        v.L2TP_ProxySetup_Password = null;
                                    }
                                }
                                else if (v.L2TP_ProxySetup == "Automatic")
                                {
                                    v.L2TP_ProxySetupURL = item.L2TP_ProxySetupURL?.ToString();
                                    if (string.IsNullOrEmpty(v.L2TP_ProxySetupURL))
                                    {
                                        v.L2TP_ProxySetupURL = null;
                                    }
                                }
                                vpn.Add(v);
                            }
                        }
                    }
                }
                else
                {
                    v = new MDM_Profile_VPN();
                    v.Profile_ID = Guid.Empty;
                    vpn.Add(v);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void EditCommand(string Id)
    {
        try
        {
            MDM_Profile mp = new MDM_Profile();
            mp.Profile_ID = Guid.Parse(Id.ToString());
            #region general
            DataTable General = ProfileUIDataProvider.GetEditData("General", mp.Profile_ID, Guid.Empty);
            if (General.Rows.Count > 0)
            {
                RetrieveBackGeneralData(General);
            }
            #endregion

            #region passcode
            DataTable Passcode = ProfileUIDataProvider.GetEditData("Passcode", mp.Profile_ID, Guid.Empty);
            if (Passcode.Rows.Count > 0)
            {
                RetrieveBackPasscodeData(Passcode);
                foreach (var item in Functionalities)
                {
                    if (item.RestrictionName == "allowPasscodeModification" || item.RestrictionName == "allowFingerprintForUnlock")
                    {
                        item.IsCheck = true;
                    }
                }
            }
            else
            {
                ckbCheckTogglePasscodeSettings = true;
            }
            #endregion

            #region restriction
            DataTable Restriction = ProfileUIDataProvider.GetEditData("Restriction", mp.Profile_ID, Guid.Empty);
            if (Restriction.Rows.Count > 0)
            {
                RetrieveBackRestrictionFunctionality(Restriction);

                RetrieveBackRestrictionApps(Restriction);

                DataTable Restriction_ad = ProfileUIDataProvider.GetEditData("Restriction_Advance", mp.Profile_ID, Guid.Empty);
                if (Restriction_ad.Rows.Count > 0)
                {
                    RetrieveBackRestrictionAppsAdvance(Restriction_ad);
                }
            }
            else
            {
                ckbCheckAllFunctionality = true;
            }

            #endregion

            #region Cellular
            DataTable Cellular = ProfileUIDataProvider.GetEditData("Cellular", mp.Profile_ID, Guid.Empty);
            if (Cellular.Rows.Count > 0)
            {
                RetrieveBackCellular(Cellular);
            }
            else
            {
                ckbNotRequiredCell = true;
            }
            #endregion

            #region Wifi
            DataTable Wifi = ProfileUIDataProvider.GetEditData("Wifi", mp.Profile_ID, Guid.Empty);
            if (Wifi.Rows.Count > 0)
            {
                RetrieveBackWifiData(Wifi);
            }
            else
            {
                ckbNotRequiredWifi = true;
            }
            #endregion

            #region VPN
            DataTable VPN = ProfileUIDataProvider.GetEditData("VPN", mp.Profile_ID, Guid.Empty);
            if (VPN.Rows.Count > 0)
            {
                RetrieveBackVPNData(VPN);
            }
            else
            {
                ckbNotRequiredVPN = true;
            }
            #endregion
            DataTable allProfileBranchIDs = ProfileUIDataProvider.GetEditData("General_BranchID", mp.Profile_ID, Guid.Empty);
            for (int i = 0; i < allProfileBranchIDs.Rows.Count; i++)
            {
                string itemId = allProfileBranchIDs.Rows[i]["Branch_ID"].ToString();
                BranchViewModel itemToCheck = BranchList.Where(a => a.BID == Guid.Parse(itemId)).FirstOrDefault();
                // within the full list of branches, only tick the boxes that are checked beforehand [from the datatable "branch"]
                itemToCheck.chk = true;
            }
            if (allProfileBranchIDs.Rows.Count == BranchList.Count)
            {
                ckbSelectall = true;
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }
    private void RetrieveBackGeneralData(DataTable ret)
    {
        try
        {
            Name = ret.Rows[0]["Name"].ToString();
            Identifier = ret.Rows[0]["Identifier"].ToString();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void RetrieveBackPasscodeData(DataTable ret)
    {
        try
        {
            ckbAllowSimpleValue = Convert.ToBoolean(ret.Rows[0]["AllowSimpleValue"].ToString());
            ckbRequireAlphanumericValue = Convert.ToBoolean(ret.Rows[0]["Requirealphanumericvalue"].ToString());
            MinPass = ret.Rows[0]["MinimumPasscodeLength"].ToString();
            MinCC = ret.Rows[0]["MinimumNumberOfComplexCharacters"].ToString();
            MaxPassAge = ret.Rows[0]["MaximumPasscodeAge"].ToString();
            MaximumAutoLock = ret.Rows[0]["MaximumAutoLock"].ToString();
            PassHistory = ret.Rows[0]["PasscodeHistory"].ToString();
            MaxGPeriod = ret.Rows[0]["MaximumGracePeriod"].ToString();
            MaxAttem = ret.Rows[0]["MaximumNumberOfFailedAttempts"].ToString();
        }

        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void RetrieveBackRestrictionFunctionality(DataTable ret)
    {
        try
        {

            for (int item = 0; item < Functionalities.Count; item++)
            {
                for (int r = 0; r < ret.Rows.Count; r++)
                {
                    Guid datakeyRID = Guid.Parse(Functionalities[item].RID.ToString());
                    if (Functionalities[item].RestrictionName == "safariAllowPopups")
                    {
                        if (ret.Rows[r]["ischeck"].ToString().ToUpper() == "TRUE")
                        {
                            Functionalities[item].IsCheck = false;
                        }
                        else
                        {
                            Functionalities[item].IsCheck = true;
                        }

                    }
                    else if (Guid.Parse(ret.Rows[r]["RID"].ToString()) == datakeyRID)
                    {
                        Functionalities[item].IsCheck = Convert.ToBoolean(ret.Rows[r]["ischeck"].ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void RetrieveBackRestrictionApps(DataTable ret)
    {
        try
        {

            for (int a = 0; a < Apps.Count; a++)
            {
                for (int r = 0; r < ret.Rows.Count; r++)
                {
                    if (Guid.Parse(ret.Rows[r]["RID"].ToString()) == Guid.Parse(Apps[a].RID.ToString()))
                    {
                        Apps[a].IsCheck = Convert.ToBoolean(ret.Rows[r]["ischeck"].ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void RetrieveBackRestrictionAppsAdvance(DataTable ret)
    {
        try
        {
            string resapp = ret.Rows[0]["RestrictAppUsage"].ToString();
            RestrictAppsUsage = resapp;
            string cookies = ret.Rows[0]["AcceptCookies"].ToString();
            AcceptCookies = cookies;
            if (RestrictAppsUsage == "AllowAllApps")
            {
                //gvRestrictionApp.DataSource = null;
                //gvRestrictionApp.DataBind();
            }
            else
            {
                string[] list = ret.Rows[0]["App_Identify"].ToString().Split(',');
                RestrictionApps = [.. list.Select(s => new RestrictionApp { ResApp = s })];
                HttpContext.Session.SetString("MDMRestrictionAppData", JsonSerializer.Serialize(RestrictionApps));
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void RetrieveBackCellular(DataTable ret)
    {
        try
        {
            string conAPN = ret.Rows[0]["ConfiguredAPNType"].ToString();
            APNType = conAPN switch
            {
                "Default APN" => "default",
                "Data APN" => "Data",
                "Default and Data APNs" => "DD"
            };
            if (APNType == "Data")
            {
                DataAN = ret.Rows[0]["DataAPN_Name"].ToString();
                string dataapnAT = ret.Rows[0]["DataAPN_AuthenticationType"].ToString();
                DataAAT = dataapnAT;
                DataAUserName = ret.Rows[0]["DataAPN_UserName"].ToString();
                DataAPass = ret.Rows[0]["DataAPN_Password"].ToString();
                DataPS = ret.Rows[0]["DataAPN_ProxyServer"].ToString();
            }
            else if (APNType == "DD")
            {

                DeAPN = ret.Rows[0]["DefaultAPN_Name"].ToString();
                string defaultAPNType = ret.Rows[0]["DefaultAPN_AuthenticationType"].ToString();
                DAAT = defaultAPNType;
                DAUN = ret.Rows[0]["DefaultAPN_UserName"].ToString();
                DAPass = ret.Rows[0]["DefaultAPN_Password"].ToString();
                DataAN = ret.Rows[0]["DataAPN_Name"].ToString();
                string dataapnAT = ret.Rows[0]["DataAPN_AuthenticationType"].ToString();
                DataAAT = dataapnAT;
                DataAUserName = ret.Rows[0]["DataAPN_UserName"].ToString();
                DataAPass = ret.Rows[0]["DataAPN_Password"].ToString();
                DataPS = ret.Rows[0]["DataAPN_ProxyServer"].ToString();
            }
            else if (APNType == "default")
            {
                DeAPN = ret.Rows[0]["DefaultAPN_Name"].ToString();
                string defaultAPNType = ret.Rows[0]["DefaultAPN_AuthenticationType"].ToString();
                DAAT = defaultAPNType;
                DAUN = ret.Rows[0]["DefaultAPN_UserName"].ToString();
                DAPass = ret.Rows[0]["DefaultAPN_Password"].ToString();
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void RetrieveBackWifiData(DataTable ret)
    {
        try
        {
            List<WiFiViewModel> wifiList = DataHelper.ConvertDataTableToList<WiFiViewModel>(ret);
            foreach (var item in wifiList)
            {
                if (item.App_Identifity != null)
                {
                    item.WhiteListApps = item.App_Identifity.Split(',').Select(item => item.Trim()).Where(item => !string.IsNullOrEmpty(item)).ToList();
                }
            }
            var session = httpContextAccessor.HttpContext.Session;
            HttpContext.Session.SetString("MDMWiFiData", JsonSerializer.Serialize(wifiList));
            WiFiList = wifiList;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
    private void RetrieveBackVPNData(DataTable ret)
    {
        try
        {
            List<VPNViewModel> vpnList = DataHelper.ConvertDataTableToList<VPNViewModel>(ret);
            var session = httpContextAccessor.HttpContext.Session;
            HttpContext.Session.SetString("MDMVPNData", JsonSerializer.Serialize(vpnList));
            foreach (var item in vpnList)
            {
                if (item.L2TP_UserAuthentication_RSASecurID?.ToString() == "true")
                {
                    item.L2TP_UserAuthentication_RSASecurID = "RSASecurID";
                }
            }
            VPNList = vpnList;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
}
public class RestrictionApp
{
    public string ResApp { get; set; }
}
public class WiFiViewModel
{
    public Guid Profile_WIFI_ID { get; set; }
    public string? ServiceSetIdentifier { get; set; }
    public string? HiddenNetwork { get; set; }
    public string? AutoJoin { get; set; }
    public string? DisableCaptiveNetworkDetection { get; set; }
    public string? ProxySetup { get; set; }
    public string? ProxyServer { get; set; }
    public string? ProxyServerPort { get; set; }
    public string? Username { get; set; }
    public string? ProxyPassword { get; set; }
    public string? ProxyURL { get; set; }
    public string? PacUnreachable { get; set; }
    public string? SecurityType { get; set; }
    public string? SecurityPassword { get; set; }
    public string? NetworkType { get; set; }
    public string? FastLaneQosmarking { get; set; }
    public string? EnableQos { get; set; }
    public string? Whitelist { get; set; }
    public string? App_Identifity { get; set; }
    public List<string> WhiteListApps { get; set; }
}


public class VPNViewModel
{
    public Guid Profile_VPN_ID { get; set; }
    public Guid Profile_ID { get; set; }
    public string? ConnectionName { get; set; }
    public string? ConnectionType { get; set; }
    public string? IPSEC_SERVER { get; set; }
    public string? IPSEC_Account { get; set; }
    public string? IPSEC_Account_Password { get; set; }
    public string? IPSEC_MachineAuthentication { get; set; }
    public string? IPSEC_GroupName { get; set; }
    public string? IPSEC_SharedSecret { get; set; }
    public string? IPSEC_UseHybridAuthentication { get; set; }
    public string? IPSEC_PromptForPassword { get; set; }
    public string? IPSEC_ProxySetup { get; set; }
    public string? IPSEC_Proxy_Server { get; set; }
    public string? IPSEC_Proxy_Port { get; set; }
    public string? IPSEC_Authentication { get; set; }
    public string? IPSEC_ProxyServerURL { get; set; }
    public string? IPSEC_Password { get; set; }
    public string? L2TP_Server { get; set; }
    public string? L2TP_Account { get; set; }
    public string? L2TP_UserAuthentication_RSASecurID { get; set; }
    public string? L2TP_UserAuthentication_Password { get; set; }
    public string? L2TP_SendAllTrafficeThroughVPN { get; set; }
    public string? L2TP_MachineAuthentication { get; set; }
    public string? L2TP_GroupName { get; set; }
    public string? L2TP_SharedSecret { get; set; }
    public string? L2TP_UseHybridAuthentication { get; set; }
    public string? L2TP_PromptForPassword { get; set; }
    public string? L2TP_ProxySetup { get; set; }
    public string? L2TP_ProxySetup_Server { get; set; }
    public string? L2TP_ProxySetup_Port { get; set; }
    public string? L2TP_ProxySetupURL { get; set; }
    public string? L2TP_ProxySetup_Authentication { get; set; }
    public string? L2TP_ProxySetup_Password { get; set; }
}



public class FunctionalityViewModel
{
    public Guid RID { get; set; }
    public string? RestrictionName { get; set; }
    public string? RestrictionDesc { get; set; }
    public int Queue { get; set; }
    public int Active { get; set; }
    public int RGroup { get; set; }
    public int GroupHeader { get; set; }
    public int NumberType { get; set; }
    public int Partation { get; set; }
    public bool IsCheck { get; set; }
}

public class AppsRestrictionViewModel
{
    public Guid RID { get; set; }
    public string? RestrictionName { get; set; }
    public string? RestrictionDesc { get; set; }
    public int Queue { get; set; }
    public int Active { get; set; }
    public int RGroup { get; set; }
    public int GroupHeader { get; set; }
    public int NumberType { get; set; }
    public int Partation { get; set; }
    public bool IsCheck { get; set; }
}