using System.Data;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_UI
{
    public class ProfileUIDataProvider
    {
        /// <summary>
        /// Retrieves the data from database based on different profiling sections
        /// </summary>
        /// <param name="Parameter"></param>
        /// <param name="ProfileID"></param>
        /// <param name="SubProfileID"></param>
        /// <returns></returns>
        public static DataTable GetEditData(string Parameter, Guid ProfileID, Guid SubProfileID)
        {

            DataTable ret = new DataTable();
            try
            {
                switch (Parameter)
                {
                    case "General":
                        MDM_Profile_General General = new MDM_Profile_General();
                        General.Profile_ID = ProfileID;
                        ret = General_Function.General_SelectAll(General);
                        break;
                    case "General_BranchID":
                        MDM_Profile_General_BranchID bid = new MDM_Profile_General_BranchID();
                        bid.Profile_ID = ProfileID;
                        ret = General_Function.General_Branch_SelectAll(bid);
                        break;
                    case "Cellular":
                        MDM_Profile_Cellular cell = new MDM_Profile_Cellular();
                        cell.Profile_ID = ProfileID;
                        ret = Cellular_Function.Cellular_SelectAll(cell);
                        break;
                    case "Passcode":
                        MDM_Profile_Passcode Passcode = new MDM_Profile_Passcode();
                        Passcode.Profile_ID = ProfileID;
                        ret = Passcode_Function.Passcode_SelectAll(Passcode);
                        break;
                    case "Restriction":
                        MDM_Profile_Restriction Restriction = new MDM_Profile_Restriction();
                        Restriction.Profile_ID = ProfileID;
                        ret = Restriction_Function.Restriction_SelectAll(Restriction);
                        break;
                    case "Restriction_Advance":
                        MDM_Profile_Restriction_Advance advance = new MDM_Profile_Restriction_Advance();
                        advance.Profile_ID = ProfileID;
                        ret = Restriction_Function.RestrictionMenu_App_Advance_SelectAll(advance);
                        break;
                    case "VPN":
                        MDM_Profile_VPN vpn = new MDM_Profile_VPN();
                        vpn.Profile_ID = ProfileID;
                        ret = VPN_Function.VPN_SelectAll(vpn);
                        break;
                    case "Wifi":
                        MDM_Profile_WIFI wifi = new MDM_Profile_WIFI();
                        wifi.Profile_ID = ProfileID;
                        ret = WIFI_Function.Wifi_SelectAll(wifi);
                        break;
                    //case "LDAP":
                    //    MDM_Profile_LDAP LDAP = new MDM_Profile_LDAP();
                    //    LDAP.Profile_ID = ProfileID;
                    //    ret = LDAP_Function.LDAP_SelectAll(LDAP);
                    //    break;
                    //case "LDAPSS":
                    //    MDM_Profile_LDAP_SearchSettings SS = new MDM_Profile_LDAP_SearchSettings();
                    //    SS.Profile_LDAP_ID = SubProfileID;
                    //    ret = LDAP_Function.LDAP_SearchSettings_SelectAll(SS);
                    //    break;
                    case "HTTP":
                        MDM_HttpProxy http = new MDM_HttpProxy();
                        http.Profile_ID = ProfileID;
                        ret = HTTPProxy_Function.HTTP_SelectAll(http);
                        break;
                }

            }

            catch (Exception ex)
            {


            }

            return ret;

        }


    }
}
