using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML
{
    public static class LDAP_XML
    {
        /// <summary>
        /// The helper function to generate the LDAP section XML to be included in the mobileconfig file. 
        /// </summary>
        /// <param name="my_list_LDAP"></param>
        /// <param name="my_list_LDAPSearch"></param>
        /// <returns></returns>
        public static string LDAP_XMLGenerator(List<MDM_Profile_LDAP> my_list_LDAP, List<MDM_Profile_LDAP_SearchSettings> my_list_LDAPSearch)
        {
            string ret = string.Empty;
            try
            {

                foreach (MDM_Profile_LDAP my_LDAP in my_list_LDAP)
                {
                    ret += @" <dict>
                               <key>LDAPAccountDescription</key>
                               <string>" + my_LDAP.AccountDescription + @"</string>
                               <key>LDAPAccountHostName</key>
                               <string>" + my_LDAP.AccountHostname + @"</string>
                               <key>LDAPAccountPassword</key>
                               <string>" + my_LDAP.AccountPassword + @"</string>
                               <key>LDAPAccountUseSSL</key>
                               <" + (my_LDAP.UseSSL != null ? "true" : "false") + @"/>
                               <key>LDAPAccountUserName</key>
                               <string>" + (my_LDAP.AccountUsername != null ? my_LDAP.AccountUsername.ToLower() : null) + @"</string>";


                    if (Guid.Parse(my_list_LDAPSearch.First().Profile_LDAP_ID.ToString()) != Guid.Empty)
                    {
                        foreach (MDM_Profile_LDAP_SearchSettings my_LDAP_Search in my_list_LDAPSearch)
                        {
                            if (my_LDAP_Search.Profile_LDAP_ID == my_LDAP.Profile_LDAP_ID)
                            {

                                ret += @"<key>LDAPSearchSetting</key>
                               <array><dict>
                              <key>LDAPSearchSettingDescription</key>
                                     <string>" + my_LDAP_Search.Description + @"</string>
                                     <key>LDAPSearchSettingScope</key>
                                     <string>LDAPSearchSettingScope" + my_LDAP_Search.Scope + @"</string>
                                     <key>LDAPSearchSettingSearchBase</key>
                                     <string>" + my_LDAP_Search.SearchBase + @"</string>
                                    </dict></array>";
                            }
                        }
                    }

                    Guid Key = Guid.NewGuid();
                    ret += @"
                        	<key>PayloadDescription</key>
			<string>Configures an LDAP account</string>
			<key>PayloadDisplayName</key>
			<string>LDAP</string>
			<key>PayloadIdentifier</key>
			<string>com.apple.ldap.account." + Key + @"</string>
			<key>PayloadType</key>
			<string>com.apple.ldap.account</string>
			<key>PayloadUUID</key>
			<string>" + Key + @"</string>
			<key>PayloadVersion</key>
			<integer>1</integer></dict>";


                }

            }
            catch (Exception ex)
            {
                ret = @"ERROR";
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ex);
            }


            return ret;
        }

    }
}
