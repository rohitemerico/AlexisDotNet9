using System.Configuration;
using System.Net;
using System.Text;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML
{
    /// <summary>
    /// The helper function to generate the enrollment section XML to be included in the mobileconfig file. [Old implementation : Currently this is not used ]
    /// </summary>
    public class ProfileEnroll_XML
    {
        // CARE -- This part's App Settings will be modified in the new app code. 
        /// <summary>
        /// These parts are only called in the generate profile dashboard page. 
        /// </summary>
        protected static string Wc_PayloadDisplayName = ConfigurationManager.AppSettings.Get("PayloadDisplayName").Trim();// WebConfig SetUp-PayloadDisplayName
        protected static string Wc_Organization_Name = ConfigurationManager.AppSettings.Get("Organization_Name").Trim();// WebConfig SetUp-Organization_Name
        protected static string Wc_Server_CerName = ConfigurationManager.AppSettings.Get("Server_CerName").Trim();// WebConfig SetUp-Server_CerName
        protected static string Wc_APNS_CerName = ConfigurationManager.AppSettings.Get("APNS_CerName").Trim();// WebConfig SetUp-APNS_CerName
        protected static string Wc_Subject_CN = ConfigurationManager.AppSettings.Get("Subject_CN").Trim();// WebConfig SetUp-Subject_CN
        protected static string Wc_NDES_Link = ConfigurationManager.AppSettings.Get("NDES_Link").Trim();// WebConfig SetUp-NDES_Link
        protected static string Wc_CheckIn_Link = ConfigurationManager.AppSettings.Get("CheckIn_Link").Trim();// WebConfig CheckIn_Link
        protected static string Wc_Server_Link = ConfigurationManager.AppSettings.Get("Server_Link").Trim();// WebConfig SetUp-Server_Link

        public static string ProfileEnroll_XMLGenerator(MDM_Profile_General main)
        {
            string ret = string.Empty;
            try
            {
                ret += @"<?xml version='1.0' encoding='UTF-8'?>
                                <!DOCTYPE plist PUBLIC '-//Apple//DTD PLIST 1.0//EN''http://www.apple.com/DTDs/PropertyList-1.0.dtd'>
                                <plist version='1.0'>
                                <dict>
	                                <key>ConsentText</key>
	                                <dict>
		                                <key>default</key>
		                                <string>" + main.ConsentMessage + @"</string>
	                                </dict>";



                if (main.AutomaticallyRemoveProfile.ToUpper() == "AFTER INTERVAL" && (main.AutomaticallyRemoveProfile_Days != null || main.AutomaticallyRemoveProfile_Hours != null))
                {

                    int sec = 0;

                    if (main.AutomaticallyRemoveProfile_Days != string.Empty)
                    {

                        sec += Int32.Parse(main.AutomaticallyRemoveProfile_Days) * 60 * 60 * 24; // 60 sec for 1 min, 60 min for 1 hours,24 hours for one day
                    }

                    if (main.AutomaticallyRemoveProfile_Hours != string.Empty)
                    {
                        sec += Int32.Parse(main.AutomaticallyRemoveProfile_Hours) * 60 * 60; // 60 sec for 1 min, 60 min for 1 hours
                    }

                    ret += @"<key>DurationUntilRemoval</key>
                            <integer>" + sec.ToString() + @"</integer>";
                }

                //Automatically remove profile with 
                //Remarks it first, further study on WithAuthorization and Never if needed
                //                if (main.Security.ToUpper() == "WITH AUTHORIZATION")
                //                {
                //                    ret += @"<key>HasRemovalPasscode</key>
                //                  <true/>";
                //                }
                //                else if (main.Security.ToUpper() == "ALWAYS" || main.AutomaticallyRemoveProfile.ToUpper() == "NEVER")
                //                //else if (main.AutomaticallyRemoveProfile.ToUpper() == "NEVER")
                //                {
                ret += @"<key>HasRemovalPasscode</key>
<false/>"; // 'Always' Profile
           //}
           //else
           //{
           //    ret = @"ERROR, Invalid Automatically Type";
           //    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
           //                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
           //                 System.Reflection.MethodBase.GetCurrentMethod().Name,
           //                 ',', "Error of Automatically Type", "Automatically Type=" + main.AutomaticallyRemoveProfile + "");

                //    return ret;
                //}

                //   ret += @"<key>PayloadContent</key>
                //<array>";
                //Paylload Content
                ret += @"<key>PayloadContent</key>
                                         <array>";
                //                if (main.Security.ToUpper() != null)
                //                {
                //                    if (main.Security.ToUpper() == "WITH AUTHORIZATION" && main.Security != null)
                //                    {

                //                        if (main.AuthorizationPassword != null)
                //                        {
                //                            ret += @"<dict>
                //                                                   <key>PayloadDescription</key>
                //                                                   <string>Configures a password for profile removal</string>
                //                                                   <key>PayloadDisplayName</key>
                //                                                   <string>Profile Removal</string>
                //                                                   <key>PayloadIdentifier</key>
                //                                                   <string>com.apple.profileRemovalPassword." + Guid.NewGuid() + @"</string>
                //                                                   <key>PayloadType</key>
                //                                                   <string>com.apple.profileRemovalPassword</string>
                //                                                   <key>PayloadUUID</key>
                //                                                   <string>" + Guid.NewGuid() + @"</string>
                //                                                   <key>PayloadVersion</key>
                //                                                   <integer>1</integer>
                //                                                   <key>RemovalPassword</key>
                //                                                   <string>" + main.AuthorizationPassword + @"</string>
                //                                                  </dict>";
                //                        }
                //                        else
                //                        {
                //                            ret = @"ERROR, Invalid with Authorization Password";
                //                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                //                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                //                                         System.Reflection.MethodBase.GetCurrentMethod().Name,
                //                                         ',', "Error of Security Type", "Security Type is null", "Security Type =" + main.Security + "");

                //                            return ret;
                //                        }

                //    }

                //}


                ret += IniFile_StreamReader(ConfigurationManager.AppSettings.Get("ProfileEnrollment_CertificateRoot_Plist"));
                ret += IniFile_StreamReader(ConfigurationManager.AppSettings.Get("ProfileEnrollment_CertificateIme_Plist"));

                ret += " @@@@@@@@ ";// Replace for more MDM dictionary


                ret += @"<dict>
			                    <key>PayloadContent</key>
			                    <dict>
				                    <key>Challenge</key>
				                    <string>" + "#@ChallengeKey#@" + @"</string>
				                    <key>Key Type</key>
				                    <string>RSA</string>
				                    <key>Key Usage</key>
				                    <integer>0</integer>
				                    <key>Keysize</key>
				                    <integer>2048</integer>
				                    <key>Name</key>
				                    <string>" + Wc_Server_CerName + @"</string>
				                    <key>Retries</key>
				                    <integer>3</integer>
				                    <key>RetryDelay</key>
				                    <integer>10</integer>
				                    <key>Subject</key>
				                    <array>
					                    <array>
						                    <array>
							                    <string>CN</string>
							                    <string>" + Wc_Subject_CN + @"</string>
						                    </array>
					                    </array>
				                    </array>
				                    <key>URL</key>
				                    <string>" + Wc_NDES_Link + @"</string>
			                    </dict>
			                    <key>PayloadDescription</key>
			                    <string>Configures SCEP</string>
			                    <key>PayloadDisplayName</key>
			                    <string>SCEP (" + Wc_Server_CerName + @")</string>
			                    <key>PayloadIdentifier</key>
			                    <string>" + Wc_APNS_CerName + @".scep4</string>
			                    <key>PayloadOrganization</key>
			                    <string>" + Wc_Organization_Name + @"</string>
			                    <key>PayloadType</key>
			                    <string>com.apple.security.scep</string>
			                    <key>PayloadUUID</key>
			                    <string>C0353C66-44F0-4776-B674-A599BB622143</string>
			                    <key>PayloadVersion</key>
			                    <integer>1</integer>
		                    </dict>
		                    <dict>
			                    <key>AccessRights</key>
			                    <integer>8191</integer>
			                    <key>CheckInURL</key>
			                    <string>" + Wc_CheckIn_Link + @"</string>
			                    <key>CheckOutWhenRemoved</key>
			                    <true/>
			                    <key>IdentityCertificateUUID</key>
			                    <string>C0353C66-44F0-4776-B674-A599BB622143</string>
			                    <key>PayloadDescription</key>
			                    <string>Configures MobileDeviceManagement.</string>
			                    <key>PayloadIdentifier</key>
			                    <string>" + Wc_APNS_CerName + @".mdm5</string>
			                    <key>PayloadOrganization</key>
			                    <string>" + Wc_Organization_Name + @"</string>
			                    <key>PayloadType</key>
			                    <string>com.apple.mdm</string>
			                    <key>PayloadUUID</key>
			                    <string>52F27C1D-A5E4-4EE0-BFC3-5A147B471DBE</string>
			                    <key>PayloadVersion</key>
			                    <integer>1</integer>
			                    <key>ServerURL</key>
			                    <string>" + Wc_Server_Link + @"</string>
			                    <key>SignMessage</key>
			                    <true/>
			                    <key>Topic</key>
			                    <string>" + Wc_APNS_CerName + @"</string>
			                    <key>UseDevelopmentAPNS</key>
			                    <false/>
		                    </dict>
	                    </array>";

                if (main.Description != null)
                {
                    ret += @"<key>PayloadDescription</key>
	                    <string>" + main.Description + @"</string>";

                }
                ret += @"<key>PayloadDisplayName</key>
	                    <string>" + Wc_PayloadDisplayName + @"</string>
	                    <key>PayloadIdentifier</key>
	                    <string>" + Wc_APNS_CerName + @"</string>
	                    <key>PayloadOrganization</key>
	                    <string>" + Wc_Organization_Name + @"</string>";
                // Remarks it first, further study on WithAuthorization and Never if needed
                //                if (main.Security.ToUpper() == "WITH AUTHORIZATION" || main.Security.ToUpper() == "NEVER")
                //                {
                //                    ret += @"<key>PayloadRemovalDisallowed</key>
                //<true/>";
                //                }
                //                else if (main.Security.ToUpper() == "ALWAYS")
                //                {
                ret += @"<key>PayloadRemovalDisallowed</key>
<false/>"; //Profile Always
                //}
                //else
                //{
                //    ret = @"ERROR,Security Type is null";
                //    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                //     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                //     System.Reflection.MethodBase.GetCurrentMethod().Name,
                //     ',', "Error of Security Type", "Security Type is null", "Security Type =" + main.Security + "");

                //    return ret;
                //}

                ret += @"
	                    <key>PayloadType</key>
	                    <string>Configuration</string>
	                    <key>PayloadUUID</key>
	                    <string>A4DA6FF1-7AEC-450F-ACC5-663287BD6AF5</string>
	                    <key>PayloadVersion</key>
	                    <integer>1</integer>";


                if (main.AutomaticallyRemoveProfile.ToUpper() == "ON DATE" && main.AutomaticallyRemoveProfile_Date != null)
                {

                    string time_UTC = DateTime.UtcNow.ToLongTimeString();
                    string date_picker = Convert.ToDateTime(main.AutomaticallyRemoveProfile_Date).ToLongDateString();
                    string dt_UTCformat = DateTime.SpecifyKind(Convert.ToDateTime(date_picker + " " + time_UTC), DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm:ssZ");

                    ret += @"<key>RemovalDate</key>
                                     <date>" + dt_UTCformat + @"</date>";
                }

                ret += @"</dict>
                    </plist>";
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


        protected static string ChallengeKey_Generator()
        {
            string ret = string.Empty;
            try
            {
                WebRequest req = HttpWebRequest.Create(ConfigurationManager.AppSettings.Get("ChallengeKey_Url"));
                req.Credentials = new NetworkCredential(ConfigurationManager.AppSettings.Get("ChallengeKey_UserName"), ConfigurationManager.AppSettings.Get("ChallengeKey_Password"));


                string source;
                string convert;
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    source = reader.ReadToEnd();


                }

                convert = source.Replace(" </B> <P> This password can be used only once and will expire within 60 minutes. <P> Each enrollment requires a new challenge password. You can refresh this web page to obtain a new challenge password. </P> <P ID=locPageDesc> For more information see  <A HREF=http://go.microsoft.com/fwlink/?LinkId=67852>Using Network Device Enrollment Service </A>. </P> <P></Font></Body></HTML>\0", "");


                ret = convert.Substring(convert.Length - Convert.ToInt32(ConfigurationManager.AppSettings.Get("ChallangeKeySize").ToString()));
                Console.WriteLine("Successful Create the Challenge Key");
            }
            catch (Exception ex)
            {

            }

            return ret;

        }

        // This part failed in the staging server : Path related issues. 
        protected static string IniFile_StreamReader(string Filepath)
        {
            string ret = string.Empty;
            try
            {
                StreamReader iniReader = new StreamReader(Filepath);
                StringBuilder Context = new StringBuilder();
                if (iniReader.Peek() != 0)
                {
                    while (!iniReader.EndOfStream)
                    {
                        //string[] tmpdata = iniReader.ReadLine().Split(',');
                        Context.AppendLine(iniReader.ReadLine());

                    }
                }
                iniReader.Dispose();
                iniReader.Close();
                ret = Context.ToString();
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
