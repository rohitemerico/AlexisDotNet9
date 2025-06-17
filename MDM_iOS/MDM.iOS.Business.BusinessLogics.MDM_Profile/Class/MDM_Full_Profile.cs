using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Class
{
    /// <summary>
    /// Stores all different MDM profile section entities as attributes such as 
    /// cellular, passcode, HTTP proxies, restriction profiles. 
    /// </summary>
    public class MDM_Full_Profile
    {
        public BankIslamEn.MDM_Profile_General mDM_Profile_General { get; set; }
        public List<BankIslamEn.MDM_Profile_General_BranchID> mDM_Profile_General_BranchID { get; set; }
        public MDM_Profile_Cellular mDM_Profile_Cellular { get; set; }
        public MDM_Profile_Passcode mDM_Profile_Passcode { get; set; }
        public MDM_HttpProxy mDM_Profile_HTTP { get; set; }

        public List<MDM_Profile_Restriction> mDM_Profile_Restriction_list { get; set; }
        public MDM_Profile_Restriction_Advance mDM_Profile_Restriction_Advance { get; set; }
        //apps gridview details store in mDM_Profile_Restriction_Advance

        public List<MDM_Profile_LDAP> mDM_Profile_LDAP_list { get; set; }
        public List<MDM_Profile_LDAP_SearchSettings> mDM_Profile_LDAP_SearchSettings_List { get; set; }

        public List<MDM_Profile_WIFI> mDM_Profile_WIFI_list { get; set; }
        public List<MDM_Profile_VPN> mDM_Profile_VPN_list { get; set; }

    }
}
